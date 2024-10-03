using UnityEngine;

public abstract  class Perception : MonoBehaviour
{
    public bool visualDebug = true;

    protected virtual void Initialize() { }

    protected virtual void UpdatePerception() { }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        UpdatePerception();
    }

}
