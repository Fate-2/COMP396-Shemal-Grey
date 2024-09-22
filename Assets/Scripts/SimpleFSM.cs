using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public enum EnemySimpleFSMStates
{
    Patrol,
    Chase,
    Attack,
    FleeToHQ
}

public class SimpleFSM : MonoBehaviour
{
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
    [SerializeField] private int _index = 0;

    private void Start()
    {
        _index = 0;
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
        Vector3 playerPosInRelationToGuard = _player.transform.position - transform.position;
        float distanceToPlayer = playerPosInRelationToGuard.magnitude;
        Vector3 directionToPlayer = playerPosInRelationToGuard / distanceToPlayer;

        // Check distance to HQ and flee if within range
        if (Vector3.Distance(transform.position, _HQ.transform.position) < _fleeDistanceFromHQ)
        {
            TransitionToState(EnemySimpleFSMStates.FleeToHQ);
            return; // Exit early to avoid executing patrol logic
        }

        Vector3 destination = _patrollingLocations[_index].position;
        _agent.SetDestination(destination);

        if (Vector3.Distance(transform.position, destination) < 2.0f)
        {
            _index = (_index + 1) % _patrollingLocations.Count;
        }

        _isInFront = Vector3.Dot(transform.forward, directionToPlayer) > _fieldOfView;
        if (_isInFront && distanceToPlayer < _distanceToChase)
        {
            TransitionToState(EnemySimpleFSMStates.Chase);
        }
        if (_isInFront && distanceToPlayer < _distanceToAttack)
        {
            TransitionToState(EnemySimpleFSMStates.Attack);
        }
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
        if (Vector3.Distance(transform.position, _player.transform.position) > _distanceToChase)
        {
            TransitionToState(EnemySimpleFSMStates.Patrol);
        }
        if (Vector3.Distance(transform.position, _player.transform.position) < _distanceToAttack)
        {
            TransitionToState(EnemySimpleFSMStates.Attack);
        }
    }

    private void Attack()
    {
        Debug.Log("You are lucky, I have no arms...");
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
