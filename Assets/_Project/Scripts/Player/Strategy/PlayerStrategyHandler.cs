using System.Collections.Generic;
using UnityEngine;

namespace Player.Strategy
{
    public class PlayerStrategyHandler : MonoBehaviour
    {
        public enum Strategy
        { 
            Solid,
            Mud
        }

        [SerializeField] private List<PlayerStrategyScriptable> _playerStrategies = new List<PlayerStrategyScriptable>();

        #region Properties

        public List<PlayerStrategyScriptable> playerStrategies
        {
            get { return _playerStrategies; }
            private set { _playerStrategies = value; }
        }

        #endregion

        public PlayerStrategyScriptable ChangeStrategy(Strategy strategy)
        {
            return ChangeStrategy((int)strategy);
        }

        public PlayerStrategyScriptable ChangeStrategy(int strategy)
        {
            return _playerStrategies[strategy];
        }
    }
}
