using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public virtual void OnEnter()
    {
        Debug.Log($"{GetType().Name} Enter");
        enabled = true;
    }

    public virtual void OnExit()
    {
        Debug.Log($"{GetType().Name} Exit");
        enabled = false;
    }

    public virtual void OnKeep() { }
}

public class Transition
{
    public Type From { get; }
    public Type To { get; }
    public Func<State, bool> Condition { get; }
    private readonly Action<State, State> action;

    public Transition(Type from, Type to, Func<State, bool> condition = null, Action<State, State> action = null)
    {
        From = from;
        To = to;
        Condition = condition ?? (s => true); // default: sempre true
        this.action = action;
    }

    public bool CanTransition(State current) => Condition(current);

    public void Execute(State current, State next)
    {
        current.OnExit();
        next.OnEnter();
        action?.Invoke(current, next);

        Debug.Log($"Transitioned from {current.GetType().Name} to {next.GetType().Name}");
    }
}

public class StateMachine : MonoBehaviour
{
    private Dictionary<Type, List<Transition>> transitions = new();
    private State currentState;

    public void SetInitialState(State state)
    {
        currentState = state;
        currentState.OnEnter();
    }

    public void AddTransition<Current, Next>(Func<State, bool> condition = null, Action<State, State> onTransition = null)
        where Current : State
        where Next : State
    {
        var from = typeof(Current);
        var to = typeof(Next);

        if (!transitions.ContainsKey(from))
            transitions[from] = new List<Transition>();

        transitions[from].Add(new Transition(from, to, condition, onTransition));
    }

    public void TransitionTo(Type toType)
    {
        if (!transitions.TryGetValue(currentState.GetType(), out var list))
        {
            Debug.LogWarning($"No transitions from {currentState.GetType().Name}");
            return;
        }

        // Escolhe a primeira transição que atende a condição e que tenha o tipo To desejado
        var transition = list.Find(t => t.To == toType && t.CanTransition(currentState));
        if (transition == null)
        {
            Debug.LogWarning($"No valid transition from {currentState.GetType().Name} to {toType.Name}");
            return;
        }

        var nextState = GetComponent(toType) as State;
        if (nextState == null)
            nextState = gameObject.AddComponent(toType) as State;

        transition.Execute(currentState, nextState);
        currentState = nextState;
    }

    // Genérico para facilitar
    public void TransitionTo<Next>() where Next : State => TransitionTo(typeof(Next));

    private void Update()
    {
        currentState?.OnKeep();
    }
}

public class MainMenu : State { }
public class HUD : State { }
public class PauseMenu : State { }
