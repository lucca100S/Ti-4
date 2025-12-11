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

        private static PlayerController controller;
        private static PlayerSpawnpoint spawnpoint;
        private static PlayerSpawnpoint Spawnpoint { get { if (spawnpoint == null) { spawnpoint = GameObject.FindAnyObjectByType<PlayerSpawnpoint>(); } return spawnpoint; } }
        private static PlayerController Controller { get { if (controller == null) { controller = GameObject.FindAnyObjectByType<PlayerController>(); } return controller; } }

        public static void Teleport(int point, PlayerSpawnpoint spawnpoint) =>
            spawnpoint?.TeleportPoint(point);

        public static void GoToSpawn(PlayerSpawnpoint spawnpoint) =>
            spawnpoint?.ReturnToSpawnpoint();

        public static void SetSpawn(PlayerSpawnpoint spawnpoint, Player.PlayerController controller) =>
            spawnpoint?.SetSpawnPoint(controller.transform.position);


        public static void ResetSpawn(PlayerSpawnpoint spawnpoint) =>
            spawnpoint?.ResetSpawnPoint();

        public static void ChangeSensitivity(Player.PlayerController controller, float sens) =>
            controller?.ChangeCameraSensitivity(sens);

        public static void GoToScene(string sceneName)
        {
            SaveHandler.SaveGameSettings(FindAnyObjectByType<SaveHandler>().gameSettingsData);
            SceneManager.LoadScene(sceneName);   
        }

        [DebugMethod("tp", "Teleports to points in a array")]
        public static void Teleport(int point)
        {
            Spawnpoint.TeleportPoint(point);
        }

        [DebugMethod("spawn", "Teleports to SpawnPoint")]
        public static void GoToSpawn()
        {
            Spawnpoint.ReturnToSpawnpoint();
        }

        [DebugMethod("spawn_set", "Sets SpawnPoint to current position")]
        public static void SetSpawn()
        {
            Spawnpoint.SetSpawnPoint(Controller.transform.position);
        }

    }
}


