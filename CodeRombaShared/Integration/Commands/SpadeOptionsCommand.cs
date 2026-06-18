using WinterbiteStudios.CodeRomba.UI.Dialogs.Options;
using WinterbiteStudios.CodeRomba.UI.Dialogs.Options.Digging;
using System.Threading.Tasks;

namespace WinterbiteStudios.CodeRomba.Integration.Commands
{
    /// <summary>
    /// A command that provides for launching the CodeRomba Options to the Spade page.
    /// </summary>
    internal sealed class SpadeOptionsCommand : BaseCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeOptionsCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeOptionsCommand(CodeRombaPackage package)
            : base(package, PackageGuids.GuidCodeRombaMenuSet, PackageIds.CmdIDCodeRombaSpadeOptions)
        {
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static SpadeOptionsCommand Instance { get; private set; }

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeRombaPackage package)
        {
            Instance = new SpadeOptionsCommand(package);
            await Instance.SwitchAsync(on: true);
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            new OptionsWindow { DataContext = new OptionsViewModel(Package, typeof(DiggingViewModel)) }.ShowModal();
        }
    }
}