using UnityEditor;
using UnityEngine;

public class FolderGeneratorTool
{
    private static readonly string[] foldersToCreate = new string[]
    {
        "_Project",
        "_Project/Art",
        "_Project/Art/Placeholder",
        "_Project/Art/Models",
        "_Project/Art/Textures",
        "_Project/Art/Sprites",
        "_Project/Art/VFX",
        "_Project/Audio",
        "_Project/Materials",
        "_Project/Prefabs",
        "_Project/Resources",
        "_Project/Scenes",
        "_Project/Scenes/Debug",
        "_Project/Scenes/Private",
        "_Project/Scripts",
        "_Project/Scripts/Core",
        "_Project/Scripts/Player",
        "_Project/Scripts/Enemies",
        "_Project/Scripts/UI",
        "_Project/Scripts/Systems",
        "_Project/Scripts/Utils",
        "_Project/ScriptableObjects",
        "_Project/Docs",
        "_Project/Docs/GDD",
        "_Project/Docs/TechDocs",
        "_Project/Docs/Guidelines",
        "_Project/Docs/QA",
        "_Project/Docs/QA/TestCases",
        "_Project/Docs/QA/Reports",
        "_Project/Docs/QA/Checklists",
        "_Project/Shaders",
        "_Project/VFX",
        "_Project/VFX/Models",
        "_Project/VFX/Textures",
        "Plugins",
        "ThirdParty",
        "Editor"
    };

    [MenuItem("Tools/Arthur Roque/Create Project Folder Structure")]
    public static void CreateFolders()
    {
        foreach (string folder in foldersToCreate)
        {
            if (!AssetDatabase.IsValidFolder("Assets/" + folder))
            {
                // Para criar subpastas, precisamos do caminho pai e da nova pasta
                string parent = "Assets";
                string[] parts = folder.Split('/');
                for (int i = 0; i < parts.Length; i++)
                {
                    string currentPath = parent + "/" + parts[i];
                    if (!AssetDatabase.IsValidFolder(currentPath))
                    {
                        AssetDatabase.CreateFolder(parent, parts[i]);
                    }
                    parent = currentPath;
                }
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Estrutura de pastas criada com sucesso!");
    }
}
