using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAgent : MonoBehaviour
{
    public float Speed; // This will be randomized based on Min and Max speed from Manager
    private List<Transform> group = new();
    private bool _isTurning = false;

    private void Start()
    {
        Speed = Random.Range(FlockManager.Instance.MinSpeed, FlockManager.Instance.MaxSpeed);
    }

    private void Update()
    {
        _isTurning = !FlockManager.Instance.SwimLimits.Contains(transform.position);

        if (_isTurning)
        {
            Vector3 newDirection = FlockManager.Instance.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newDirection),
                FlockManager.Instance.RotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 40)
            {
                Speed = Random.Range(FlockManager.Instance.MinSpeed, FlockManager.Instance.MaxSpeed);
            }
            ApplyRules();
        }
        transform.Translate(0, 0, Speed * Time.deltaTime);
    }

    private void ApplyRules()
    {
        Vector3 groupCenter = Vector3.zero;
        Vector3 groupdAvoid = Vector3.zero;
        float groupSpeed = 0.01f; // Is to get the average speed of the group of fishes
        float neighbourDistance = 0f; // The max distance to consider another fish in the group
        int groupSize = 0; // The amount of fishes I have in close proximity
        group.Clear();

        foreach (GameObject fish in FlockManager.Instance.School)
        {
            if (fish == this.gameObject) continue;

            neighbourDistance = Vector3.Distance(fish.transform.position, transform.position);
            if (neighbourDistance >= FlockManager.Instance.NeighbourDistance) continue;

            groupCenter += fish.transform.position;
            groupSize++;
            group.Add(fish.transform);

            if (neighbourDistance < FlockManager.Instance.AvoidDistance)
            {
                groupdAvoid += transform.position - fish.transform.position;
            }

            FlockAgent fishAgent = fish.GetComponent<FlockAgent>();
            groupSpeed += fishAgent.Speed;
        }

        if (groupSize > 0)
        {
            groupCenter = groupCenter / groupSize;
            Speed = groupSpeed / groupSize; // Cohesion being applied
            if (Speed > FlockManager.Instance.MaxSpeed)
            {
                Speed = FlockManager.Instance.MaxSpeed;
            }

            Vector3 newDirection = (groupCenter + groupdAvoid) - transform.position; // Alignment and Separation being applied
            if (newDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newDirection),
                    FlockManager.Instance.RotationSpeed * Time.deltaTime);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Transform friend in group)
        {
            Gizmos.DrawLine(transform.position, friend.position);
        }
    }
}