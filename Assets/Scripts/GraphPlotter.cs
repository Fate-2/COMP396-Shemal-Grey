using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphPlotter : MonoBehaviour
{
	[SerializeField] private GameObject pointPrefab;
	[SerializeField, Range(10, 200)] private int resolution = 100; // How many points in graph
	[SerializeField] private float scale = 1f;
	void Start()
	{
		for (int i = 0; i < resolution; i++)
		{
			float x = X_Formula2(i);
			float y = Y_SinFormula(x);
			Vector3 position = new Vector3(x, y, 0);
			Instantiate(pointPrefab, position, Quaternion.identity, transform);
		}
	}

	private float X_Formula(int step)
	{
		return step / (float)resolution;
	}
	private float X_Formula2(int step)
	{
		return (step + 0.5f) * (2f / resolution) - 1f;
	}

	private float Y_Formula(float x)
	{
		return x * x;
	}
	private float Y_QuadFormula(float x)
	{
		return x * x * scale;
	}
	private float Y_SinFormula(float x)
	{
		return Mathf.Sin(x * Mathf.PI * 2) * scale;
	}
}
