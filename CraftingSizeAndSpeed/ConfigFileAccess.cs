using System;
using System.IO;
using BepInEx;
using Jotunn;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CraftingSizeAndSpeed;

internal static class ConfigFileAccess
{
    private static readonly IDeserializer Deserializer = new DeserializerBuilder()
        .WithNamingConvention(PascalCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

    private static readonly ISerializer Serializer =
        new SerializerBuilder().WithNamingConvention(PascalCaseNamingConvention.Instance).Build();

    internal static string GetFileNameAndPath(string configObjectName, bool isDefaultFile = false)
    {
        string fileName = isDefaultFile
            ? $"{CraftingSizeAndSpeedPlugin.PluginGuid}.Default.{configObjectName}.yaml"
            : $"{CraftingSizeAndSpeedPlugin.PluginGuid}.{configObjectName}.yaml";
        return Path.Combine(Paths.ConfigPath, fileName);
    }

    internal static string ReadFile(string fileNameAndPath)
    {
        if (File.Exists(fileNameAndPath))
        {
            Logger.LogInfo($"reading from file '{fileNameAndPath}'");
            return File.ReadAllText(fileNameAndPath);
        }
        Logger.LogInfo($"file '{fileNameAndPath}' does not exist, skipping to load this config type");
        return "";
    }

    internal static void WriteFile(string fileNameAndPath, string fileContent)
    {
        File.WriteAllText(fileNameAndPath, fileContent);
        Logger.LogInfo($"file '{fileNameAndPath}' successfully written");
    }

    internal static T Deserialize<T>(string fileNameAndPath, string fileContent) where T : class
    {
        try
        {
            return Deserializer.Deserialize<T>(fileContent);
        }
        catch (Exception e)
        {
            Logger.LogWarning(
                $"Unable to parse config file '{fileNameAndPath}' due to '{e.Message}' " +
                $"because of '{e.GetBaseException().Message}', \n{e.StackTrace}");
        }

        return null;
    }

    internal static string Serialize(object fileContent)
    {
        return Serializer.Serialize(fileContent);
    }
}