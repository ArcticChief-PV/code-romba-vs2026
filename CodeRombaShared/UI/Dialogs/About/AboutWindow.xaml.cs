using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace WinterbiteStudios.CodeRomba.UI.Dialogs.About
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutWindow" /> class.
        /// </summary>
        public AboutWindow()
        {
            Application.ResourceAssembly = Assembly.GetExecutingAssembly();

            InitializeComponent();
        }

        #endregion Constructors

        #region Event Handlers

        /// <summary>
        /// Called when a key is pressed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// Called when an uncaptured left mouse button down event is received on the background.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.MouseButtonEventArgs" /> instance containing the
        /// event data.
        /// </param>
        private void OnMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// Called when the Website link is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.
        /// </param>
        private void OnWebsiteLinkClick(object sender, RoutedEventArgs e)
        {
            ShowInDevelopmentWarning("Website");
        }

        /// <summary>
        /// Called when the Visual Studio Marketplace link is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.
        /// </param>
        private void OnVisualStudioMarketplaceLinkClick(object sender, RoutedEventArgs e)
        {
            ShowInDevelopmentWarning("Visual Studio Marketplace listing");
        }

        /// <summary>
        /// Called when the GitHub link is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.
        /// </param>
        private void OnGitHubLinkClick(object sender, RoutedEventArgs e)
        {
            LaunchLink(@"https://github.com/ArcticChief-PV/code-romba-vs2026");
        }

        /// <summary>
        /// Called when the Twitter link is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.
        /// </param>
        private void OnTwitterLinkClick(object sender, RoutedEventArgs e)
        {
            ShowInDevelopmentWarning("Twitter/X account");
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Shows a warning that the requested resource is not available yet because CodeRomba is
        /// still in development.
        /// </summary>
        /// <param name="resource">The name of the resource the user attempted to open.</param>
        private static void ShowInDevelopmentWarning(string resource)
        {
            MessageBox.Show(
                $"The {resource} is not available yet.\n\nCodeRomba is still in development.",
                "CodeRomba - Still in development",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }

        /// <summary>
        /// Attempts to launch the specified link.
        /// </summary>
        /// <param name="link">The link.</param>
        private static void LaunchLink(string link)
        {
            try
            {
                Process.Start(link);
            }
            catch (Exception ex) when (ex is Win32Exception || ex is InvalidOperationException)
            {
                // Do nothing if no default application handler is associated with the link.
            }
        }

        #endregion Methods
    }
}