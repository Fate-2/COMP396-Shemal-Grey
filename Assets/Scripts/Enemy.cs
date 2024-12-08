using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject[] _patrolPoints;

    public string Name;
    public float Speed;
    public int Damage;
    public int Health;
    public string AnimatonControllerPath;
 

    private StateMachine _stateMachine;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _patrolPoints = GameObject.FindGameObjectsWithTag("PatrolLocations");

    }


    private void Start()
    {
        _animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(AnimatonControllerPath);
        _stateMachine = new StateMachine();
        var patrolState = new PatrolState(_agent, _animator, _patrolPoints);
        var chaseState = new ChaseState(_agent, _player, _animator);


        _stateMachine.AddTransition(from: patrolState, to: chaseState, condition: new FuncPredicate(() =>
          Vector3.Distance(_player.transform.position, transform.position) < 5f));
        _stateMachine.AddTransition(from: chaseState, to: patrolState, condition: new FuncPredicate(() =>
        Vector3.Distance(a: _player.transform.position, b: transform.position) > 15f));

        _stateMachine.SetState(patrolState);
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    public class Builder
    {
        private string name = "Enemy";
        private float speed = 5f;
        private int damage = 10;
        private int health = 100;
        private string animatonControllerPath = string.Empty;
        private GameObject body;

        private bool needAudioSource = false;
        private AudioSourceSettings audioSettings;
        private bool needCollider = false;
        private ColliderSettings colliderSettings;
        public Builder WithCollider(ColliderSettings colliderSettings)
        {
            needCollider = true;
            this.colliderSettings = colliderSettings;
            return this;
        }
        public Builder WithAudioSource(AudioSourceSettings audioSettings)
        {
            needAudioSource = true;
            this.audioSettings = audioSettings;
            return this;
        }
        public Builder WithAnimation(string animatonControllerPath)
        {
            this.animatonControllerPath = animatonControllerPath;
            return this;
        }
        public Builder WithBody(GameObject body)
        {
            this.body = body;
            return this;
        }
        public Builder WithName(string name)
        {
            this.name = name;
            return this;
        }
        public Builder WithSpeed(float speed)
        {
            this.speed = speed;
            return this;
        }
        public Builder WithDamage(int damage)
        {
            this.damage = damage;
            return this;
        }
        public Builder WithHealth(int health)
        {
            this.health = health;
            return this;
        }
        public Enemy Build()
        {
            //Enemy enemy = new GameObject(name).AddComponent<Enemy>();
            Enemy enemy = GameObject.Instantiate(this.body).AddComponent<Enemy>();
            enemy.Name = name;
            enemy.Speed = speed;
            enemy.Damage = damage;
            enemy.Health = health;
            enemy.AnimatonControllerPath = animatonControllerPath;
            if (needAudioSource)
            {
                enemy.gameObject.AddComponent<GuardSounds>().GetAudioSourceSettings(audioSettings);
            }
            if (needCollider)
            {
                var collider = enemy.gameObject.AddComponent<CapsuleCollider>();
                collider.height = colliderSettings.ColliderHeight;
                collider.radius = colliderSettings.ColliderRadius;
                collider.center = new Vector3(0, colliderSettings.ColliderCenter, 0);
            }

            return enemy;
        }
    }
}
