using UnityEngine;
public class Collectible : MonoBehaviour
{
    [SerializeField] private int _score = 10;
    [SerializeField] private IntEventChannel _scoreChannel;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        _scoreChannel.Invoke(_score);
        Destroy(gameObject);
    }
}