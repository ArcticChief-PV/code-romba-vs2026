using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using WinterbiteStudios.CodeRomba.Properties;

namespace WinterbiteStudios.CodeRomba.Helpers
{
    /// <summary>
    /// A helper class for writing messages to a CodeRomba output window pane.
    /// </summary>
    internal static class OutputWindowHelper
    {
        #region Fields

        private static IVsOutputWindowPane _CodeRombaOutputWindowPane;

        #endregion Fields

        #region Properties

        private static IVsOutputWindowPane CodeRombaOutputWindowPane =>
            _CodeRombaOutputWindowPane ?? (_CodeRombaOutputWindowPane = GetCodeRombaOutputWindowPane());

        #endregion Properties

        #region Methods

        /// <summary>
        /// Writes the specified diagnostic line to the CodeRomba output pane, but only if diagnostics are enabled.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">An optional exception that was handled.</param>
        internal static void DiagnosticWriteLine(string message, Exception ex = null)
        {
            if (!Settings.Default.General_DiagnosticsMode) return;

            if (ex != null)
            {
                message += $": {ex}";
            }

            WriteLine(Resources.Diagnostic, message);
        }

        /// <summary>
        /// Writes the specified exception line to the CodeRomba output pane.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The exception that was handled.</param>
        internal static void ExceptionWriteLine(string message, Exception ex)
        {
            var exceptionMessage = $"{message}: {ex}";

            WriteLine(Resources.HandledException, exceptionMessage);
        }

        /// <summary>
        /// Writes the specified warning line to the CodeRomba output pane.
        /// </summary>
        /// <param name="message">The message.</param>
        internal static void WarningWriteLine(string message)
        {
            WriteLine(Resources.Warning, message);
        }

        /// <summary>
        /// Attempts to create and retrieve the CodeRomba output window pane.
        /// </summary>
        /// <returns>The CodeRomba output window pane, otherwise null.</returns>
        private static IVsOutputWindowPane GetCodeRombaOutputWindowPane()
        {
            if (!(Package.GetGlobalService(typeof(SVsOutputWindow)) is IVsOutputWindow outputWindow))
            {
                return null;
            }

            Guid outputPaneGuid = new Guid(PackageGuids.GuidCodeRombaOutputPane.ToByteArray());

            outputWindow.CreatePane(ref outputPaneGuid, "CodeRomba", 1, 1);
            outputWindow.GetPane(ref outputPaneGuid, out IVsOutputWindowPane windowPane);

            return windowPane;
        }

        /// <summary>
        /// Writes the specified line to the CodeRomba output pane.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="message">The message.</param>
        private static void WriteLine(string category, string message)
        {
            var outputWindowPane = CodeRombaOutputWindowPane;
            if (outputWindowPane != null)
            {
                string outputMessage = $"[CodeRomba {category} {DateTime.Now.ToString("hh:mm:ss tt")}] {message}{Environment.NewLine}";

                outputWindowPane.OutputString(outputMessage);
            }
        }

        #endregion Methods
    }
}