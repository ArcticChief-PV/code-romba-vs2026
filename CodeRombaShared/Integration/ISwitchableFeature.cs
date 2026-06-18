using System.Threading.Tasks;

namespace WinterbiteStudios.CodeRomba.Integration
{
    internal interface ISwitchableFeature
    {
        Task SwitchAsync(bool on);
    }
}