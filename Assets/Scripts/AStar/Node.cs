using System;
using UnityEngine;

[System.Serializable]
public class Node : IComparable
{
    public float nodeTotalCost, estimateCost;

    public bool Obstacle;

    public Node parent;

    public Vector3 position;


    public Node(Vector3 position)
    {
        estimateCost = 0.0f;
        nodeTotalCost = 1.0f;
        Obstacle = false;
        parent = null;
        this.position = position;
    }


    public void MarkAsObstacle()
    {
        Obstacle = true;
    }
    public int CompareTo(object obj)
    {
        Node node = (Node)obj;
        if (estimateCost < node.estimateCost)
        {
            return -1; // this node is priority on cost effectiveness
        }
        if (estimateCost > node.estimateCost)
        {
            return 1; // this node is not priority
        }
        return 0; // Either way, both nodes cost the same
    }
}
