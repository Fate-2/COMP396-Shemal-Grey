using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections;

public enum EnemySimpleFSMStates
{
    Patrol,
    Looking,
    Chase,
    Attack,
    FleeToHQ
}

[RequireComponent(typeof(NavMeshAgent))]

public class SimpleFSM : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private EnemySimpleFSMStates _currentState;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _patrolArea;
    [SerializeField] private List<Transform> _patrollingLocations = new List<Transform>();
    [SerializeField] private GameObject _HQ;


    [Header("Guard Stats")]
    [SerializeField, Range(8f, 15f)]
    private float _distanceToChase = 10f;

    [SerializeField, Range(0.12f, 0.80f)]
    private float _fieldOfView = 0.28f;

    [SerializeField, Range(2f, 7f)]
    private float _distanceToAttack = 3f;

    [SerializeField, Range(2f, 10f)]
    private float _fleeDistanceFromHQ = 5f; // Distance to trigger fleeing to HQ

    [SerializeField] private bool _isInFront;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _walkSpeed = 3f, _runSPeed = 6f;
    [SerializeField] private int _index = 0;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _currentState = EnemySimpleFSMStates.Patrol;
        _player = GameObject.FindWithTag("Player");
        _patrollingLocations = _patrolArea.GetComponentsInChildren<Transform>().ToList();
    }

    private void Update()
    {
        FiniteStateMachineRunner();
    }

    private void FiniteStateMachineRunner()
    {
        switch (_currentState)
        {
            case EnemySimpleFSMStates.Patrol:
                Patrol();
                break;
            case EnemySimpleFSMStates.Chase:
                Chase();
                break;
            case EnemySimpleFSMStates.Attack:
                Attack();
                break;
            case EnemySimpleFSMStates.Looking:
                Looking();
                break;
            case EnemySimpleFSMStates.FleeToHQ:
                FleeToHQ();
                break;
            default:
                Debug.LogError("State in FSM not implemented");
                break;
        }
    }

    private void TransitionToState(EnemySimpleFSMStates state)
    {
        _currentState = state;
    }

    private void Patrol()
    {
        _animator.SetBool("isWalking", true);
        Vector3 playerPosInRelationToGuard = _player.transform.position - transform.position;
        float distanceToPlayer = playerPosInRelationToGuard.magnitude;
        Vector3 directionToPlayer = playerPosInRelationToGuard / distanceToPlayer;

       

        Vector3 destination = _patrollingLocations[_index].position;
        _agent.SetDestination(destination);
        _agent.speed = _walkSpeed;
        if (Vector3.Distance(transform.position, destination) < 2.0f)
        {
            TransitionToState(EnemySimpleFSMStates.Looking);
        }


        _isInFront = Vector3.Dot(transform.forward, directionToPlayer) > _fieldOfView;
        if (_isInFront && distanceToPlayer < _distanceToChase)
        {
            _animator.SetBool("isRunning", true);
            TransitionToState(EnemySimpleFSMStates.Chase);
        }


        // Check distance to HQ and flee if within range
        if (Vector3.Distance(transform.position, _HQ.transform.position) < _fleeDistanceFromHQ)
        {
            TransitionToState(EnemySimpleFSMStates.FleeToHQ);
            return; // Exit early to avoid executing patrol logic
        }
        
    }

    private void Looking()
    {
        if (!_animator.GetBool("isLooking"))
        {
            StartCoroutine("LookingAround");;
        }
        _animator.SetBool("isLooking", true);
    }
    private IEnumerator LookingAround()
    {
        yield return new WaitForSeconds(2f);
        _index = (_index + 1) % _patrollingLocations.Count;
        _animator.SetBool("isLooking", false);
        TransitionToState(EnemySimpleFSMStates.Patrol);
    }
    private void Chase()
    {
        // Check distance to HQ and flee if within range
        if (Vector3.Distance(transform.position, _HQ.transform.position) < _fleeDistanceFromHQ)
        {
           
            TransitionToState(EnemySimpleFSMStates.FleeToHQ);
            return; // Exit early to avoid executing chase logic
        }

        _agent.SetDestination(_player.transform.position);
        _agent.speed = _runSPeed;
        if (Vector3.Distance(transform.position, _player.transform.position) > _distanceToChase)
        {
            _animator.SetBool("isRunning", false);
            TransitionToState(EnemySimpleFSMStates.Patrol);
        }
        if (Vector3.Distance(transform.position, _player.transform.position) < _distanceToAttack)
        {
            _animator.SetBool("isAttacking", true);
            TransitionToState(EnemySimpleFSMStates.Attack);
        }
    }

    private void Attack()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) > _distanceToAttack)
        {
            _animator.SetBool("isAttacking", false);
            TransitionToState(EnemySimpleFSMStates.Chase);
        }
          
    }

    private void FleeToHQ()
    {
        // Set the destination to the HQ
        _agent.SetDestination(_HQ.transform.position);

        // Check if the enemy has reached HQ
        if (Vector3.Distance(transform.position, _HQ.transform.position) < 2.0f)
        {
            // Transition back to Patrol or another state as needed
            TransitionToState(EnemySimpleFSMStates.Patrol);
        }
    }
}
