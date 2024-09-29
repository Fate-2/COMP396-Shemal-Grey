using UnityEngine;
using UnityEngine.AI;

public class BaseState : IState
{
    protected readonly Animator animator;
    protected readonly NavMeshAgent agent;

    protected static readonly int Walk = Animator.StringToHash("Walking");
    protected static readonly int Run = Animator.StringToHash("Running");

    protected BaseState(NavMeshAgent agent, Animator animator)
    {
        this.animator = animator;
        this.agent = agent;
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void FixedUpdate() { }
    public virtual void Update() { }
    

}
