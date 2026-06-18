using EnvDTE;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WinterbiteStudios.CodeRomba.Helpers;
using WinterbiteStudios.CodeRomba.Logic.Cleaning;
using WinterbiteStudios.CodeRomba.Properties;
using WinterbiteStudios.CodeRomba.UI.Dialogs.CleanupProgress;

namespace WinterbiteStudios.CodeRomba.Integration.Commands
{
    /// <summary>
    /// A command that provides for cleaning up code in all documents.
    /// </summary>
    internal sealed class CleanupAllCodeCommand : BaseCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupAllCodeCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal CleanupAllCodeCommand(CodeRombaPackage package)
            : base(package, PackageGuids.GuidCodeRombaMenuSet, PackageIds.CmdIDCodeRombaCleanupAllCode)
        {
            CodeCleanupAvailabilityLogic = CodeCleanupAvailabilityLogic.GetInstance(Package);
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static CleanupAllCodeCommand Instance { get; private set; }

        /// <summary>
        /// Gets the list of all project items.
        /// </summary>
        private IEnumerable<ProjectItem> AllProjectItems
            => SolutionHelper.GetAllItemsInSolution<ProjectItem>(Package.IDE.Solution).Where(x => CodeCleanupAvailabilityLogic.CanCleanupProjectItem(x));

        /// <summary>
        /// Gets or sets the code cleanup availability logic.
        /// </summary>
        private CodeCleanupAvailabilityLogic CodeCleanupAvailabilityLogic { get; set; }

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeRombaPackage package)
        {
            Instance = new CleanupAllCodeCommand(package);
            await package.SettingsMonitor.WatchAsync(s => s.Feature_CleanupAllCode, Instance.SwitchAsync);
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

            if (!CodeCleanupAvailabilityLogic.IsCleanupEnvironmentAvailable())
            {
                MessageBox.Show(Resources.CleanupCannotRunWhileDebugging,
                                Resources.CodeRombaCleanupAllCode,
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (MessageBox.Show(Resources.AreYouReadyForCodeRombaToCleanEverythingInTheSolution,
                                     Resources.CodeRombaConfirmationForCleanupAllCode,
                                     MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No)
                         == MessageBoxResult.Yes)
            {
                using (new ActiveDocumentRestorer(Package))
                {
                    var viewModel = new CleanupProgressViewModel(Package, AllProjectItems);
                    var window = new CleanupProgressWindow { DataContext = viewModel };

                    window.ShowModal();
                }
            }
        }
    }
}