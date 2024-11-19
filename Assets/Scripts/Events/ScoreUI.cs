using System.Collections;
using TMPro;
using UnityEngine;
public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    private void Start()
    {
        UpdateScore();
    }
    public void UpdateScore()
    {
        StartCoroutine(UpdateScoreNextFrame());
    }
    private IEnumerator UpdateScoreNextFrame()
    {
        yield return null;
        _scoreText.text = GameManager.Instance.Score.ToString();
    }
}