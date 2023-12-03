using System.Linq;
using CraftingSizeAndSpeed.Model;
using Jotunn.Managers;

namespace CraftingSizeAndSpeed;

public static class Registry
{
    private const string BeehiveName = "Beehive";
    private const string CookingStationName = "CookingStation";
    private const string FermenterName = "Fermenter";
    private const string SmelterName = "Smelter";

    private static ConfigObject<BeehiveModel, Beehive> Beehives;
    private static ConfigObject<CookingStationModel, CookingStation> CookingStations;
    private static ConfigObject<FermenterModel, Fermenter> Fermenters;
    private static ConfigObject<SmelterModel, Smelter> Smelters;

    public static void Initialize()
    {
        Beehives = new ConfigObject<BeehiveModel, Beehive>(BeehiveName);
        CookingStations = new ConfigObject<CookingStationModel, CookingStation>(CookingStationName);
        Fermenters = new ConfigObject<FermenterModel, Fermenter>(FermenterName);
        Smelters = new ConfigObject<SmelterModel, Smelter>(SmelterName);

        Beehives.ConfigureAll();
        CookingStations.ConfigureAll();
        Fermenters.ConfigureAll();
        Smelters.ConfigureAll();
    }

    public static void LoadFromGameWriteFile()
    {
        ConfigFileAccess.WriteFile(ConfigFileAccess.GetFileNameAndPath(BeehiveName, isDefaultFile: true),
            ConfigFileAccess.Serialize(PrefabManager.Cache.GetPrefabs(typeof(Beehive)).ToDictionary(
                kv => kv.Key,
                kv => BeehiveModel.From((Beehive)kv.Value)
            )));
        ConfigFileAccess.WriteFile(ConfigFileAccess.GetFileNameAndPath(CookingStationName, isDefaultFile: true),
            ConfigFileAccess.Serialize(PrefabManager.Cache.GetPrefabs(typeof(CookingStation)).ToDictionary(
                kv => kv.Key,
                kv => CookingStationModel.From((CookingStation)kv.Value)
            )));
        ConfigFileAccess.WriteFile(ConfigFileAccess.GetFileNameAndPath(FermenterName, isDefaultFile: true),
            ConfigFileAccess.Serialize(PrefabManager.Cache.GetPrefabs(typeof(Fermenter)).ToDictionary(
                kv => kv.Key,
                kv => FermenterModel.From((Fermenter)kv.Value)
            )));
        ConfigFileAccess.WriteFile(ConfigFileAccess.GetFileNameAndPath(SmelterName, isDefaultFile: true),
            ConfigFileAccess.Serialize(PrefabManager.Cache.GetPrefabs(typeof(Smelter)).ToDictionary(
                kv => kv.Key,
                kv => SmelterModel.From((Smelter)kv.Value)
            )));
    }
}