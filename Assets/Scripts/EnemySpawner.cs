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
            .Build();
        //Instantiate(NewEnemy);
        var Goblin = new Enemy.Builder()
            .WithName("Goblin")
            .WithDamage(5)
            .WithSpeed(12)
            .Build();
    }
}