using Lugu.Utils;
using Player;
using Player.Movement;
using Player.Strategy;
using System;
using UnityEngine;


public class ActionsManager : Singleton<ActionsManager>
{
    public Action<IState> OnFormChanged;
}
