using UnityEngine;
public class Stamina : MonoBehaviour
{
    [SerializeField] private int _maxStamina = 100;
    [SerializeField] private FloatEventChannel _staminaChannel;
    private int _currentStamina;
    private void Awake()
    {
        _currentStamina = _maxStamina;
    }
    private void Start()
    {
        PublishStaminaPercentage();
    }
    public void UpdateStamina(int value)
    {
        _currentStamina = value;
        PublishStaminaPercentage();
    }
    private void PublishStaminaPercentage()
    {
        if (_staminaChannel != null)
        {
            _staminaChannel.Invoke(_currentStamina / (float)_maxStamina);
        }
    }
}