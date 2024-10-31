using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : Singleton<FlockManager>
{
    public GameObject FishPrefab;
    [Range(5, 100)] public int AmountOfFish = 10;
    public List<GameObject> School = new(); // School is a collection of the same type of fish
    public Vector3 SpawnLimits = new Vector3(5, 5, 5);
    public Bounds SwimLimits;
    public int SwimLimitsSize = 2;

    [Header("Fish Settings")]
    [Range(0.1f, 1f)] public float MinSpeed = 0.3f;
    [Range(1f, 5f)] public float MaxSpeed = 1.6f;
    [Range(0.2f, 10.0f)] public float NeighbourDistance = 1f;
    [Range(0.5f, 2f)] public float AvoidDistance = 1f;
    [Range(1f, 5f)] public float RotationSpeed = 1f;

    private void Start()
    {
        SwimLimits = new Bounds(transform.position, SpawnLimits * SwimLimitsSize);
        for (int i = 0; i < AmountOfFish; i++)
        {
            Vector3 SpawnBuffer =
                new Vector3(
                    Random.Range(-SpawnLimits.x, SpawnLimits.x),
                    Random.Range(-SpawnLimits.y, SpawnLimits.y),
                    Random.Range(-SpawnLimits.z, SpawnLimits.z)
                );
            Vector3 SpawnLocation = transform.position + SpawnBuffer;
            School.Add(Instantiate(FishPrefab, SpawnLocation, Quaternion.identity));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(SwimLimits.center, SwimLimits.size);
    }
}