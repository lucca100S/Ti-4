using UnityEditor;
using UnityEngine;

namespace ArthurRoque.Tools.Project 
{
    public class FolderGeneratorTool
    {
        public static readonly string[] foldersToCreate = new string[]
        {
        "_Project",
            "_Project/Art",
                "_Project/Art/Models",
                "_Project/Art/Textures",
                "_Project/Art/Sprites",
                "_Project/Art/VFX",
                    "_Project/Art/VFX/Models",
                    "_Project/Art/VFX/Textures",
                    "_Project/Art/VFX/Graphs",
                "_Project/Art/Placeholder",
            "_Project/Animations",
                "_Project/Animations/Player",
                "_Project/Animations/Enemies",
                "_Project/Animations/UI",
            "_Project/Audio",
                "_Project/Audio/SFX",
                "_Project/Audio/BGM",
                "_Project/Audio/VO",
                "_Project/Audio/Mixers",
            "_Project/Materials",
            "_Project/Prefabs",
            "_Project/Resources",
            "_Project/Scenes",
                "_Project/Scenes/Debug",
                "_Project/Scenes/Private",
            "_Project/Scripts",
                "_Project/Scripts/Core",
                "_Project/Scripts/Gameplay",
                    "_Project/Scripts/Gameplay/Player",
                    "_Project/Scripts/Gameplay/Enemies",
                    "_Project/Scripts/Gameplay/Items",
                "_Project/Scripts/Systems",
                    "_Project/Scripts/Systems/Audio",
                    "_Project/Scripts/Systems/UI",
                    "_Project/Scripts/Systems/SaveLoad",
                "_Project/Scripts/Utils",
                "_Project/ScriptableObjects",
            "_Project/Docs",
                "_Project/Docs/01_Brainstorming",
                    "_Project/Docs/01_Brainstorming/README.md",
                "_Project/Docs/02_Moodboard",
                    "_Project/Docs/02_Moodboard/README.md",
                "_Project/Docs/03_Pitch",
                    "_Project/Docs/03_Pitch/README.md",
                "_Project/Docs/04_GameDesignDocument",
                    "_Project/Docs/04_GameDesignDocument/README.md",
                "_Project/Docs/05_SoundDesignDocument",
                    "_Project/Docs/05_SoundDesignDocument/README.md",
                "_Project/Docs/06_ArtDesignDocument",
                    "_Project/Docs/06_ArtDesignDocument/README.md",
                "_Project/Docs/07_TechnicalDesignDocument",
                    "_Project/Docs/07_TechnicalDesignDocument/README.md",
                "_Project/Docs/08_Diagrams",
                    "_Project/Docs/08_Diagrams/Structural",
                        "_Project/Docs/08_Diagrams/Structural/Packages",
                            "_Project/Docs/08_Diagrams/Structural/Packages/README.md",
                        "_Project/Docs/08_Diagrams/Structural/UML",
                            "_Project/Docs/08_Diagrams/Structural/UML/README.md",
                    "_Project/Docs/08_Diagrams/Dynamic",
                        "_Project/Docs/08_Diagrams/Dynamic/UserFlows",
                            "_Project/Docs/08_Diagrams/Dynamic/UserFlows/README.md",
                        "_Project/Docs/08_Diagrams/Dynamic/UseCases",
                            "_Project/Docs/08_Diagrams/Dynamic/UseCases/README.md",
                "_Project/Docs/09_Guidelines",
                    "_Project/Docs/09_Guidelines/README.md",
            "_Project/Shaders",
        "Plugins",
        "ThirdParty",
        "Editor",
            "Editor/Arthur Roque",
                "Editor/Arthur Roque/Project",
                "Editor/Arthur Roque/Audio"
        };

        [MenuItem("Tools/Arthur Roque/Project/Create Project Folder Structure")]
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
}

