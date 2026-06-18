using WinterbiteStudios.CodeRomba.Properties;

namespace WinterbiteStudios.CodeRomba.UI.Dialogs.Options.Cleaning
{
    /// <summary>
    /// The view model for cleaning options - a parent to more specific view models.
    /// </summary>
    public class CleaningParentViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleaningParentViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="activeSettings">The active settings.</param>
        public CleaningParentViewModel(CodeRombaPackage package, Settings activeSettings)
            : base(package, activeSettings)
        {
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header => Resources.Cleaning_n;

        #endregion Overrides of OptionsPageViewModel
    }
}