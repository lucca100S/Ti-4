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

        [DebugMethod("tp")]
        public static void Teleport(int point)
        {
            Spawnpoint.TeleportPoint(point);
        }

        [DebugMethod("spawn")]
        public static void GoToSpawn()
        {
            Spawnpoint.ReturnToSpawnpoint();
        }

        [DebugMethod("spawn_set")]
        public static void SetSpawn()
        {
            Spawnpoint.SetSpawnPoint(Controller.transform.position);
        }

        [DebugMethod("spawn_reset")]
        public static void SetReset()
        {
            Spawnpoint.ResetSpawnPoint();
        }

    }
}
