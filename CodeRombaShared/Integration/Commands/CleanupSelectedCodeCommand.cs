using EnvDTE;
using WinterbiteStudios.CodeRomba.Helpers;
using WinterbiteStudios.CodeRomba.Logic.Cleaning;
using WinterbiteStudios.CodeRomba.UI.Dialogs.CleanupProgress;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WinterbiteStudios.CodeRomba.Integration.Commands
{
    /// <summary>
    /// A command that provides for cleaning up code in the selected documents.
    /// </summary>
    internal sealed class CleanupSelectedCodeCommand : BaseCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupSelectedCodeCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal CleanupSelectedCodeCommand(CodeRombaPackage package)
            : base(package, PackageGuids.GuidCodeRombaMenuSet, PackageIds.CmdIDCodeRombaCleanupSelectedCode)
        {
            CodeCleanupAvailabilityLogic = CodeCleanupAvailabilityLogic.GetInstance(Package);
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static CleanupSelectedCodeCommand Instance { get; private set; }

        /// <summary>
        /// Gets or sets the code cleanup availability logic.
        /// </summary>
        private CodeCleanupAvailabilityLogic CodeCleanupAvailabilityLogic { get; }

        /// <summary>
        /// Gets the list of selected project items.
        /// </summary>
        private IEnumerable<ProjectItem> SelectedProjectItems
            => SolutionHelper.GetSelectedProjectItemsRecursively(Package).Where(x => CodeCleanupAvailabilityLogic.CanCleanupProjectItem(x));

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeRombaPackage package)
        {
            Instance = new CleanupSelectedCodeCommand(package);
            await package.SettingsMonitor.WatchAsync(s => s.Feature_CleanupSelectedCode, Instance.SwitchAsync);
        }

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            Enabled = Package.IDE.Solution.IsOpen;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            using (new ActiveDocumentRestorer(Package))
            {
                var viewModel = new CleanupProgressViewModel(Package, SelectedProjectItems);
                var window = new CleanupProgressWindow { DataContext = viewModel };

                window.ShowModal();
            }
        }
    }
}