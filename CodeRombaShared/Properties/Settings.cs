using System.Configuration;
using WinterbiteStudios.CodeRomba.Helpers;

namespace WinterbiteStudios.CodeRomba.Properties
{
    /// <summary>
    /// This partial class instructs the <see cref="Settings"/> class to utilize the <see cref="CodeRombaSettingsProvider"/>.
    /// </summary>
    [SettingsProvider(typeof(CodeRombaSettingsProvider))]
    public sealed partial class Settings
    {
        /// <summary>
        /// Updates application settings to reflect a more recent installation of the application.
        /// </summary>
        public override void Upgrade()
        {
            var oldSettingsProvider = new LocalFileSettingsProvider();
            var oldPropertyValues = oldSettingsProvider.GetPropertyValues(Context, Properties);

            foreach (SettingsPropertyValue oldPropertyValue in oldPropertyValues)
            {
                if (!Equals(this[oldPropertyValue.Name], oldPropertyValue.PropertyValue))
                {
                    this[oldPropertyValue.Name] = oldPropertyValue.PropertyValue;
                }
            }
        }
    }
}