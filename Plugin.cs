using BepInEx;
using BepInEx.IL2CPP;
using CustomGameModes;
using HarmonyLib;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace FreeForAllGameMode
{
    [BepInPlugin($"lammas123.ffa{MyPluginInfo.PLUGIN_NAME}", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency("lammas123.CustomGameModes")]
    public class SequencedDropGameMode : BasePlugin
    {
        internal static string PluginPath;

        public override void Load()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            PluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)[6..];

            CustomGameModes.Api.RegisterCustomGameMode(new CustomGameModeFFA());

            Log.LogInfo($"Loaded [{MyPluginInfo.PLUGIN_NAME} {MyPluginInfo.PLUGIN_VERSION}]");
        }
    }
}