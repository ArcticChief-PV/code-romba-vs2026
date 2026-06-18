using EnvDTE;
using System;
using WinterbiteStudios.CodeRomba.Helpers;
using WinterbiteStudios.CodeRomba.Properties;

namespace WinterbiteStudios.CodeRomba.Logic.Cleaning
{
    /// <summary>
    /// A class for encapsulating the logic of inserting whitespace.
    /// </summary>
    internal class InsertWhitespaceLogic
    {
        #region Fields

        private readonly CodeRombaPackage _package;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="InsertWhitespaceLogic" /> class.
        /// </summary>
        private static InsertWhitespaceLogic _instance;

        /// <summary>
        /// Gets an instance of the <see cref="InsertWhitespaceLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="InsertWhitespaceLogic" /> class.</returns>
        internal static InsertWhitespaceLogic GetInstance(CodeRombaPackage package)
        {
            return _instance ?? (_instance = new InsertWhitespaceLogic(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertWhitespaceLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private InsertWhitespaceLogic(CodeRombaPackage package)
        {
            _package = package;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Inserts a single blank space before a self-closing angle bracket.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void InsertBlankSpaceBeforeSelfClosingAngleBracket(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_InsertBlankSpaceBeforeSelfClosingAngleBrackets) return;

            const string pattern = @"([^ \t])/>";
            const string replacement = @"$1 />";

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// Insert the trailing newline to the end of the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void InsertEOFTrailingNewLine(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_InsertEndOfFileTrailingNewLine) return;

            EditPoint cursor = textDocument.EndPoint.CreateEditPoint();

            if (cursor.AtEndOfDocument && !cursor.AtStartOfLine)
            {
                cursor.Insert(Environment.NewLine);
            }
        }

        #endregion Methods
    }
}