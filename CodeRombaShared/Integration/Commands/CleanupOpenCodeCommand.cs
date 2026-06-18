using EnvDTE;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinterbiteStudios.CodeRomba.Helpers;
using WinterbiteStudios.CodeRomba.Logic.Cleaning;
using WinterbiteStudios.CodeRomba.UI.Dialogs.CleanupProgress;

namespace WinterbiteStudios.CodeRomba.Integration.Commands
{
    /// <summary>
    /// A command that provides for cleaning up code in the open documents.
    /// </summary>
    internal sealed class CleanupOpenCodeCommand : BaseCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupOpenCodeCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal CleanupOpenCodeCommand(CodeRombaPackage package)
            : base(package, PackageGuids.GuidCodeRombaMenuSet, PackageIds.CmdIDCodeRombaCleanupOpenCode)
        {
            CodeCleanupAvailabilityLogic = CodeCleanupAvailabilityLogic.GetInstance(Package);
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static CleanupOpenCodeCommand Instance { get; private set; }

        /// <summary>
        /// Gets or sets the code cleanup availability logic.
        /// </summary>
        private CodeCleanupAvailabilityLogic CodeCleanupAvailabilityLogic { get; }

        /// <summary>
        /// Gets the list of open documents that are cleanup candidates.
        /// </summary>
        private IEnumerable<Document> OpenCleanableDocuments
            => OpenDocuments.Where(x => CodeCleanupAvailabilityLogic.CanCleanupDocument(x));

        /// <summary>
        /// Gets the list of open documents.
        /// </summary>
        private IEnumerable<Document> OpenDocuments
            => Package.IDE.Documents.OfType<Document>().Where(x => x.ActiveWindow != null);

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeRombaPackage package)
        {
            Instance = new CleanupOpenCodeCommand(package);
            await package.SettingsMonitor.WatchAsync(s => s.Feature_CleanupOpenCode, Instance.SwitchAsync);
        }

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            Enabled = OpenDocuments.Any();
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            using (new ActiveDocumentRestorer(Package))
            {
                var viewModel = new CleanupProgressViewModel(Package, OpenCleanableDocuments);
                var window = new CleanupProgressWindow { DataContext = viewModel };

                window.ShowModal();
            }
        }
    }
}