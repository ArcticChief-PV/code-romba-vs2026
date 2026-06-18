using EnvDTE;
using System.Threading.Tasks;
using WinterbiteStudios.CodeRomba.Helpers;
using WinterbiteStudios.CodeRomba.Properties;

namespace WinterbiteStudios.CodeRomba.Integration.Commands
{
    /// <summary>
    /// A command that provides for joining lines together.
    /// </summary>
    internal sealed class JoinLinesCommand : BaseCommand
    {
        private readonly UndoTransactionHelper _undoTransactionHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinLinesCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal JoinLinesCommand(CodeRombaPackage package)
            : base(package, PackageGuids.GuidCodeRombaMenuSet, PackageIds.CmdIDCodeRombaJoinLines)
        {
            _undoTransactionHelper = new UndoTransactionHelper(package, Resources.CodeRombaJoin);
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static JoinLinesCommand Instance { get; private set; }

        /// <summary>
        /// Gets the active text document, otherwise null.
        /// </summary>
        private TextDocument ActiveTextDocument => Package.ActiveDocument?.GetTextDocument();

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeRombaPackage package)
        {
            Instance = new JoinLinesCommand(package);
            await package.SettingsMonitor.WatchAsync(s => s.Feature_JoinLines, Instance.SwitchAsync);
        }

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            Enabled = ActiveTextDocument != null;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            var activeTextDocument = ActiveTextDocument;
            if (activeTextDocument != null)
            {
                var textSelection = activeTextDocument.Selection;
                if (textSelection != null)
                {
                    _undoTransactionHelper.Run(() => JoinText(textSelection));
                }
            }
        }

        /// <summary>
        /// Joins the text within the specified text selection.
        /// </summary>
        /// <param name="textSelection">The text selection.</param>
        private void JoinText(TextSelection textSelection)
        {
            // If the selection has no length, try to pick up the next line.
            if (textSelection.IsEmpty)
            {
                textSelection.LineDown(true);
                textSelection.EndOfLine(true);
            }

            const string pattern = @"[ \t]*\r?\n[ \t]*";
            const string replacement = @" ";

            // Substitute all new lines (and optional surrounding whitespace) with a single space.
            TextDocumentHelper.SubstituteAllStringMatches(textSelection, pattern, replacement);

            // Move the cursor forward, clearing the selection.
            textSelection.CharRight();
        }
    }
}