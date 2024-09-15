using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVectors : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    [SerializeField] private Vector3 v1;

    [SerializeField] private Vector3 v2;

    [SerializeField] private Vector3 v3;

    [SerializeField] private Vector3 v4;




    void Start()
    {
        var point1 = Instantiate(prefab, v1, Quaternion.identity);
        var point2 = Instantiate(prefab, v2, Quaternion.identity);
        var point3 = Instantiate(prefab, v3, Quaternion.identity);
        var point4 = Instantiate(prefab, v4, Quaternion.identity);
        var distance = Vector3.Distance(point1.transform.position, point2.transform.position);
        var dotProduct = Vector3.Dot(point1.transform.position, point2.transform.position);
        var distance1 = Vector3.Distance(point3.transform.position, point4.transform.position);
        var dotProduct1 = Vector3.Dot(point3.transform.position, point4.transform.position);
        Debug.Log($"dot = {dotProduct} {dotProduct1}|| distance = {distance} {distance1}");
        if (distance < 10 && dotProduct < 50)
        {
            Debug.Log($"Target in range, ATTACK!!");
        }

    }


}
