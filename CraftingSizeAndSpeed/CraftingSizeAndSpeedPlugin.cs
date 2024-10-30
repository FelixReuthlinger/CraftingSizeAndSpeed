using BepInEx;
using BepInEx.Configuration;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using ServerSync;

namespace CraftingSizeAndSpeed
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class CraftingSizeAndSpeedPlugin : BaseUnityPlugin
    {
        internal const string PluginAuthor = "FixItFelix";
        internal const string PluginGuid = PluginAuthor + "." + PluginName;
        internal const string PluginName = "CraftingSizeAndSpeed";
        internal const string PluginVersion = "1.0.5";

        private static CraftingSizeAndSpeedPlugin _instance = null!;

        internal static readonly ConfigSync ConfigSync = new(PluginGuid)
        {
            DisplayName = PluginName,
            CurrentVersion = PluginVersion
        };

        private static ConfigEntry<bool> _configLocked = null!;
        
        private void Awake()
        {
            _instance = this;

            _configLocked = CreateConfig("1 - General", "Lock Configuration", true,
                "If 'true' and playing on a server, config can only be changed on server-side configuration, " +
                "clients cannot override");
            ConfigSync.AddLockingConfigEntry(_configLocked);
            
            PrefabManager.OnPrefabsRegistered += Registry.Initialize;
            CommandManager.Instance.AddConsoleCommand(new ConfigPrintController());
        }
        
        private static ConfigEntry<T> CreateConfig<T>(string group, string parameterName, T value,
            ConfigDescription description,
            bool synchronizedSetting = true)
        {
            ConfigEntry<T> configEntry = _instance.Config.Bind(group, parameterName, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        private static ConfigEntry<T> CreateConfig<T>(string group, string parameterName, T value, string description,
            bool synchronizedSetting = true) => CreateConfig(group, parameterName, value,
            new ConfigDescription(description), synchronizedSetting);
    }
    
    public class ConfigPrintController : ConsoleCommand
    {
        public override void Run(string[] args)
        {
            Registry.LoadFromGameWriteFile();
        }

        public override string Name => "crafting_configurtor_write_defaults";

        public override string Help =>
            "Write all prefabs loaded in-game into a YAML defaults config file inside the BepInEx config folder. " +
            "Can be used to create your own configs without creating everything by hand. " +
            "You will just need to rename the file and remove the '.Default'";
    }
}