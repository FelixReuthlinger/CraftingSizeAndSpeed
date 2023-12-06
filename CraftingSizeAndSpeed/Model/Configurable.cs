using System.Collections.Generic;
using System.IO;
using BepInEx;
using JetBrains.Annotations;
using Jotunn.Managers;
using ServerSync;
using UnityEngine;
using Logger = Jotunn.Logger;

namespace CraftingSizeAndSpeed.Model;

public abstract class Configurable<GameType> where GameType : MonoBehaviour
{
    [UsedImplicitly]
    public abstract void Configure(GameType original);
}

public class ConfigObject<ConfiguredType, GameType>
    where ConfiguredType : Configurable<GameType>
    where GameType : MonoBehaviour
{
    private readonly string ObjectFile;
    private readonly CustomSyncedValue<string> ObjectFilesContent;
    private Dictionary<string, ConfiguredType> Value;

    public ConfigObject(string objectName)
    {
        ObjectFile = ConfigFileAccess.GetFileNameAndPath(objectName);
        ObjectFilesContent = new(CraftingSizeAndSpeedPlugin.ConfigSync, $"{objectName}Content",
            ConfigFileAccess.ReadFile(ObjectFile));
        ObjectFilesContent.ValueChanged += OnValueChanged;
        OnValueChanged();
        FileSystemWatcher watcher = new(Paths.ConfigPath, Path.GetFileName(ObjectFile));
        watcher.Changed += OnFileChanged;
        watcher.Created += OnFileChanged;
        watcher.Renamed += OnFileChanged;
        watcher.IncludeSubdirectories = true;
        watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
        watcher.EnableRaisingEvents = true;
    }

    private void OnFileChanged(object _, FileSystemEventArgs __)
    {
        ObjectFilesContent.Value = ConfigFileAccess.ReadFile(ObjectFile);
        OnValueChanged();
    }

    private void OnValueChanged()
    {
        if (ObjectFilesContent is { Value: not null } && ObjectFilesContent.Value != "")
        {
            Value = ConfigFileAccess.Deserialize<Dictionary<string, ConfiguredType>>(ObjectFile,
                ObjectFilesContent.Value);
            Logger.LogInfo($"loaded:\n{ConfigFileAccess.Serialize(Value)}");
        }
    }

    public void ConfigureAll()
    {
        if (Value != null)
            foreach (var config in Value)
            {
                GameObject gameObject = PrefabManager.Instance.GetPrefab(config.Key);
                if (!gameObject) continue;
                if (gameObject.TryGetComponent(out GameType plant)) config.Value.Configure(plant);
            }
    }
}