using Lugu.Utils;
using Player;
using Player.Movement;
using Player.Strategy;
using System;
using UnityEngine;


public class ActionsManager : Singleton<ActionsManager>
{
    #region Player
    public Action<IState> OnFormChanged;
    public Action OnPlayerJumped;
    public Action OnPlayerFall;
    public Action OnPlayerLanded;

    #endregion
}
