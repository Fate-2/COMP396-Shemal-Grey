using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyCommonData enemyData;
    void Start()
    {
        var NewEnemy = new Enemy.Builder()
            .WithName("Troll")
            .WithSpeed(7)
            .WithHealth(60)
            .WithDamage(15)
            .WithAnimation(enemyData.animControllerPath)
            .WithBody(enemyData.body)
            .WithAudioSource(enemyData.audioSourceSettings)
            .WithCollider(enemyData.colliderSettings)
            .Build();
    }
}