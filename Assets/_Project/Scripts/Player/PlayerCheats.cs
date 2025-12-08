using Lugu.Console;
using UnityEngine.SceneManagement;
using UnityEngine;
namespace Player
{
    public class PlayerCheats : MonoBehaviour
    {
        public static readonly string level01Name = "Level1Entrega";
        public static readonly string level02Name = "LuguFase2";
        public static readonly string level03Name = "LuguFase3";

        [DebugMethod("tp", "Teleports to points in an array")]
        public static void Teleport(int point, PlayerSpawnpoint spawnpoint) =>
            spawnpoint?.TeleportPoint(point);

        [DebugMethod("spawn", "Teleports to SpawnPoint")]
        public static void GoToSpawn(PlayerSpawnpoint spawnpoint) =>
            spawnpoint?.ReturnToSpawnpoint();

        [DebugMethod("spawn_set", "Sets SpawnPoint to current position")]
        public static void SetSpawn(PlayerSpawnpoint spawnpoint, Player.PlayerController controller) =>
            spawnpoint?.SetSpawnPoint(controller.transform.position);

        [DebugMethod("spawn_reset", "Resets SpawnPoint to original position")]
        public static void ResetSpawn(PlayerSpawnpoint spawnpoint) =>
            spawnpoint?.ResetSpawnPoint();

        [DebugMethod("change_sens", "Changes camera sensitivity")]
        public static void ChangeSensitivity(Player.PlayerController controller, float sens) =>
            controller?.ChangeCameraSensitivity(sens);

        [DebugMethod("go_to_scene", "Changes scenes")]
        public static void GoToScene(string sceneName)
        {
            SaveHandler.SaveGameSettings(FindAnyObjectByType<SaveHandler>().gameSettingsData);
            SceneManager.LoadScene(sceneName);   
        }
            
    }
}


