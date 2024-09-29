using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : BaseState
{
    private GameObject _player;
    public ChaseState(NavMeshAgent agent, GameObject player, Animator animator) : base(agent, animator)
    {
        _player = player;
    }

    public override void OnEnter()
    {
        Debug.Log("TIME TO DIE!!!!!!");
        agent.speed = 4f;
        animator.Play(stateNameHash:Run);
    }


    public override void Update()
    {
        agent.SetDestination(_player.transform.position);
    }

    public override void OnExit()
    {
        Debug.Log("ehh chasing is to tiring lets go do something else");
    }

}