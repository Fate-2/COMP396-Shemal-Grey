using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVectors : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Vector3[] _vectors;
    [SerializeField] private GameObject[] _points;

    [Header("PlayerMovement")]
    [SerializeField, Range(3, 15)]
    private float _speed = 5f;
    private Rigidbody _rigidbody;
    private float dotProduct = 0f;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _points = new GameObject[_vectors.Length];

        for (int i = 0; i < _vectors.Length; i++)
        {
            _points[i] = Instantiate(prefab, _vectors[i], Quaternion.identity);
            _points[i].name = $"Clone of Point {i}";
        }
        
        WhatAreVectors();
    }

    private void WhatAreVectors()
    {
        Vector3 vector1 = new Vector3(1, 3, 5);
        // Debug.Log($"{vector1}");
        Vector3 vector2 = new Vector3(3, -5, 7);
        // Debug.Log($"{vector2}");
        Vector3 v1plusv2 = vector1 + vector2;
        // Debug.Log($"{v1plusv2}");
        Vector3 v1minusv2 = vector1 - vector2;
        // Debug.Log($"{v1minusv2}");
        Vector3 v2minusv1 = vector2 - vector1;
        // Debug.Log($"{v2minusv1}");
        Vector3 v1times4 = vector1 * 4;
        // Debug.Log($"{v1times4}");
        
        // Debug.Log($"Vector1 Magnitude = {vector1.magnitude}");

        Vector3 forward = transform.forward;    // Vector3 (0, 0, 1)
        Vector3 back = transform.forward * -1;  // Vector3 (0, 0, -1)
        Vector3 up = transform.up;              // Vector3 (0, 1, 0)
        Vector3 down = transform.up * -1;       // Vector3 (0, -1, 0)
        Vector3 right = transform.right;        // Vector3 (1, 0, 0)
        Vector3 left = transform.right * -1;    // Vector3 (-1, 0, 0)
        
        // Few most used methods for Vectors
        // Vector3.Distance
        float distanceToVector = Vector3.Distance(transform.position, vector1);
        
        //Debug.Log($"Distance {distanceToVector}");
        
        // Vector3.Lerp - Requires 2 vectors and a float from 0 to 1
        Vector3 lerpFromVector = Vector3.Lerp(transform.position, vector2, 0.30f);
        //Debug.Log($"lerp = {lerpFromVector}");
        
        // Dot Product
        Vector3 thisToVector2 = vector2 - transform.position;
        Debug.Log($"thisToV2 {thisToVector2} normalized {Vector3.Normalize(thisToVector2)}");
        float vectorDot = Vector3.Dot(forward, Vector3.Normalize(vector2 - transform.position));
        Debug.Log($"Dot Prod {vectorDot}");
    }

    private void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(inputX, 0, inputZ);
        Vector3 movement = input * _speed; // Replace constant with speed variable

        _rigidbody.linearVelocity = movement;

        for (int index = 0; index < _points.Length; index++)
        {
            dotProduct = Vector3.Dot(transform.forward,
                Vector3.Normalize(_points[index].transform.position - transform.position));
            Debug.DrawLine(transform.position, _points[index].transform.position, 
                (dotProduct < 0.20) ? Color.red : Color.green, 0.5f);
        }
    }
}
