using UnityEngine;
public class GameManager : Singleton<GameManager>
{
    public int Score { get; private set; }
    public void AddScore(int score)
    {
        Score += score;
    }
}