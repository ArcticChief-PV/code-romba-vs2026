using EnvDTE;
using System.Threading.Tasks;
using WinterbiteStudios.CodeRomba.Helpers;
using WinterbiteStudios.CodeRomba.Properties;

namespace WinterbiteStudios.CodeRomba.Integration.Commands
{
    /// <summary>
    /// A command that provides for collapsing nodes in the solution explorer tool window.
    /// </summary>
    internal sealed class CollapseAllSolutionExplorerCommand : BaseCommand
    {
        /// <summary>
        /// A flag indicating if waiting to execute.
        /// </summary>
        private bool _isWaitingToExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollapseAllSolutionExplorerCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal CollapseAllSolutionExplorerCommand(CodeRombaPackage package)
            : base(package, PackageGuids.GuidCodeRombaMenuSet, PackageIds.CmdIDCodeRombaCollapseAllSolutionExplorer)
        {
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static CollapseAllSolutionExplorerCommand Instance { get; private set; }

        /// <summary>
        /// Gets the top level (solution) UI hierarchy item.
        /// </summary>
        private UIHierarchyItem TopUIHierarchyItem => UIHierarchyHelper.GetTopUIHierarchyItem(Package);

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeRombaPackage package)
        {
            Instance = new CollapseAllSolutionExplorerCommand(package);
            await package.SettingsMonitor.WatchAsync(s => s.Feature_CollapseAllSolutionExplorer, Instance.SwitchAsync);
        }

        /// <summary>
        /// Called when a solution is opened.
        /// </summary>
        internal void OnSolutionOpened()
        {
            if (!Settings.Default.Collapsing_CollapseSolutionWhenOpened) return;

            var topItem = TopUIHierarchyItem;
            if (topItem == null || topItem.UIHierarchyItems.Count == 0)
            {
                _isWaitingToExecute = true;
            }
            else
            {
                OnExecute();
            }
        }

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            Enabled = Package.IDE.Solution.IsOpen;

            if (Enabled && _isWaitingToExecute)
            {
                OnExecute();
            }
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            _isWaitingToExecute = false;

            var topItem = TopUIHierarchyItem;

            if (topItem != null && UIHierarchyHelper.HasExpandedChildren(topItem))
            {
                UIHierarchyHelper.CollapseRecursively(TopUIHierarchyItem);
            }
        }
    }
}