using Player.Movement;
using UnityEngine;
using Lugu.Console;

namespace Player
{
    public class PlayerCheats : MonoBehaviour
    {
        private static PlayerController controller;
        private static PlayerSpawnpoint spawnpoint;
        private static PlayerMovement movement;

        #region Properties

        private static PlayerController Controller { get { if (controller == null) { controller = GameObject.FindAnyObjectByType<PlayerController>(); } return controller; } }
        private static PlayerSpawnpoint Spawnpoint { get { if (spawnpoint == null) { spawnpoint = GameObject.FindAnyObjectByType<PlayerSpawnpoint>(); } return spawnpoint; } }
        private static PlayerMovement Movement { get { if (movement == null) { movement = GameObject.FindAnyObjectByType<PlayerMovement>(); } return movement; } }
        #endregion

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

        [DebugMethod("spawn_reset", "Resets SpawnPoint to original position")]
        public static void SetReset()
        {
            Spawnpoint.ResetSpawnPoint();
        }

        [DebugMethod("change_sens", "Changes the sensibility of the camera")]
        public static void ChangeSensitivity(float sensitivity)
        {
            Controller.ChangeCameraSensitivity(sensitivity);
        }

    }
}
