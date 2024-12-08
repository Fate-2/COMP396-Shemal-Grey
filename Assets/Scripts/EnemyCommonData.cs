using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Create Enemy", fileName = "Enemy Data")]
public class EnemyCommonData : ScriptableObject
{
    public string animControllerPath;
    public GameObject body;
    public AudioSourceSettings audioSourceSettings;
    public ColliderSettings colliderSettings;
}
[System.Serializable]
public struct AudioSourceSettings
{
    public bool AudioSourcePlayOnAwake;
    public float AudioSourceSpatialBlend; // Change to 3d if 1 or 2d if 0
    public float AudioSourceMinDistance; // Min Distance to be heard if 3D
    public float AudioSourceMaxDistance; // Max Distance to be head if 3D
    public AudioRolloffMode AudioRolloffMode; // Change the mode to either Logarithmic or Linear
    public AudioClip audioClip;
}
[System.Serializable]
public struct ColliderSettings
{
    public float ColliderCenter;
    public float ColliderRadius;
    public float ColliderHeight;
}