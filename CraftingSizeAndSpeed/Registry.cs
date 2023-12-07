using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

    private static Dictionary<string, T> GetType<T>()
    {
        return PrefabManager.Cache.GetPrefabs(typeof(T))
            .GroupBy(kv => Regex.Replace(kv.Key, @"\(Clone\)", "").Trim())
            .ToDictionary(
                group => group.Key,
                group => (T)Convert.ChangeType(group.First().Value, typeof(T))
            );
    }

    public static void LoadFromGameWriteFile()
    {
        ConfigFileAccess.WriteFile(ConfigFileAccess.GetFileNameAndPath(BeehiveName, isDefaultFile: true),
            ConfigFileAccess.Serialize(GetType<Beehive>()
                .ToDictionary(kv => kv.Key, kv => BeehiveModel.From(kv.Value))));
        ConfigFileAccess.WriteFile(ConfigFileAccess.GetFileNameAndPath(CookingStationName, isDefaultFile: true),
            ConfigFileAccess.Serialize(GetType<CookingStation>()
                .ToDictionary(kv => kv.Key, kv => CookingStationModel.From(kv.Value))));
        ConfigFileAccess.WriteFile(ConfigFileAccess.GetFileNameAndPath(FermenterName, isDefaultFile: true),
            ConfigFileAccess.Serialize(GetType<Fermenter>()
                .ToDictionary(kv => kv.Key, kv => FermenterModel.From(kv.Value))));
        ConfigFileAccess.WriteFile(ConfigFileAccess.GetFileNameAndPath(SmelterName, isDefaultFile: true),
            ConfigFileAccess.Serialize(GetType<Smelter>()
                .ToDictionary(kv => kv.Key, kv => SmelterModel.From(kv.Value))));
    }
}