#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

/// <summary>
/// Editor tool responsible for populating AudioId values in the project.
/// Placeholder class — implement codegen tooling as needed.
/// </summary>
public static class CodeGenAudioIdGenerator
{
    private const string OutputPath = "Assets/_Project/Audio/Generated/AudioId.cs";
    private const string SearchFolder = "Assets/_Project/Audio/";

    [MenuItem("Tools/Audio/Regenerar AudioId Enum")]
    public static void Generate()
    {
        var guids = AssetDatabase.FindAssets("t:AudioSO", new[] { SearchFolder });
        var assets = guids.Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<AudioSO>)
            .Where(a => a != null)
            .ToList();

        if (assets.Count == 0)
        {
            Debug.LogWarning("Nenhum AudioSO encontrado em " + SearchFolder);
            return;
        }

        string[] enumEntries = assets
            .Select(a => a.name.Replace(" ", "_").Replace("-", "_"))
            .Prepend("None")
            .Distinct()
            .ToArray();

        string enumBody = string.Join(",\n    ", enumEntries);
        string output = $@"// AUTO-GENERATED FILE - DO NOT EDIT MANUALLY
// Generated on {DateTime.Now:yyyy-MM-dd HH:mm:ss}

public enum AudioId
{{
    {enumBody}
}}";

        Directory.CreateDirectory(Path.GetDirectoryName(OutputPath)!);
        File.WriteAllText(OutputPath, output);
        AssetDatabase.Refresh();

        Debug.Log($"AudioId enum regenerado com {enumEntries.Length} entradas em: {OutputPath}");
    }
}
#endif