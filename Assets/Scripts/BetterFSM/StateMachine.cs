using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
   class StateNode
    {
        public IState State { get;  }
        public HashSet<ITransition> Transitions { get; }


        public StateNode(IState state)
        {
            Transitions = new HashSet<ITransition>();
            State = state;
        }


        public void AddTransition(IState to, IPredicate condition)
        {
            Transitions.Add(new Transition(to, condition));
        }
    }



    private StateNode _currentState;
    private Dictionary<Type, StateNode> _nodes = new();

    //Helper Methods


    private StateNode GetorAddNode(IState state)
    {
        var node = _nodes.GetValueOrDefault(state.GetType());
        if (node == null)
        {
            node = new StateNode(state);
            _nodes.Add(state.GetType(), node);
        }
        return node;
    }

    public void AddTransition(IState from, IState to, IPredicate condition)
    {
        GetorAddNode(from).AddTransition(to, condition);
    }

    private ITransition GetTransition()
    {
        foreach (var transition in _currentState.Transitions)
        {
            if (transition.Condition.Evaluate())
            {
                return transition;
            }
        }
        return null;
    }


    public void SetState(IState state)
    {
        _currentState = _nodes[state.GetType()];
        _currentState.State?.OnEnter();
    }


    private void ChangeState(IState state)
    {
        if (state == _currentState.State) return;

        var previousState = _currentState.State;
        var nextState = _nodes[state.GetType()].State;

        previousState?.OnExit();
        nextState?.OnEnter();
        _currentState = _nodes[state.GetType()];
    }

    public void Update()
    {
        var transition = GetTransition();
        if (transition != null)
        {
            ChangeState(transition.To);
        }

        _currentState.State?.Update();
    }

    public void FixedUpdate()
    {
        _currentState.State?.FixedUpdate();
    }
}
