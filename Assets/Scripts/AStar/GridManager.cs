using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;

public class GridManager : MonoBehaviour
{

	private static GridManager s_instance;
	public static GridManager Instance
	{
		get
		{
			if (s_instance != null) { return s_instance; }
			s_instance = FindObjectOfType(typeof(GridManager)) as GridManager;
			if (s_instance == null)
			{
				Debug.Log("Warning, add a GridManager into the scene");
			}
			return s_instance;
		}
	}



	public int NumOfRows, NumOfColumns;
	public float GridCellSize;
	public bool ShowGrid = true, ShowObstacleBlocks = true;
	private Vector3 _origin = new Vector3(0, 0, 0);
	public Vector3 Origin { get { return _origin; } }
	[SerializeField] private GameObject[] _obstacles;
	public Node[,] Nodes { get; set; }
	private void Awake()
	{
		_obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		CalculateObstacles();
	}
	// Helper Methods
	public int GetRow(int index) => index / NumOfColumns;
	public int GetColumn(int index) => index % NumOfColumns;
	public int GetGridIndex(Vector3 position)
	{
		if (!IsBounds(position))
		{
			return -1;
		}
		position -= Origin;
		int col = (int)(position.x / GridCellSize);
		int row = (int)(position.z / GridCellSize);
		return row * NumOfColumns + col;
	}
	public bool IsBounds(Vector3 position)
	{
		float width = NumOfColumns * GridCellSize;
		float height = NumOfRows * GridCellSize;
		return position.x >= Origin.x && position.x <= Origin.x + width &&
				position.z <= Origin.z + height && position.z >= Origin.z;
	}
	public Vector3 GetGridCellCenter(int index)
	{
		Vector3 cellPosition = GetGridCellPosition(index);
		cellPosition.x += (GridCellSize / 2.0f);
		cellPosition.z += (GridCellSize / 2.0f);
		return cellPosition;
	}
	public Vector3 GetGridCellPosition(int index)
	{
		int row = GetRow(index);
		int col = GetColumn(index);
		float xPosInGrid = col * GridCellSize;
		float zPosInGrid = row * GridCellSize;
		return Origin + new Vector3(xPosInGrid, 0, zPosInGrid);
	}
	private void CalculateObstacles()
	{
		Nodes = new Node[NumOfColumns, NumOfRows];
		int index = 0;
		for (int row = 0; row < NumOfRows; row++)
		{
			for (int col = 0; col < NumOfColumns; col++)
			{
				Vector3 cellPosition = GetGridCellCenter(index);
				Node node = new Node(cellPosition);
				Nodes[col, row] = node;
				index++;
			}
		}
		if (_obstacles != null && _obstacles.Length > 0)
		{
			foreach (GameObject obstacle in _obstacles)
			{
				int indexCell = GetGridIndex(obstacle.transform.position);
				int col = GetColumn(indexCell);
				int row = GetRow(indexCell);
				Nodes[col, row].MarkAsObstacle();
				// TODO check if problems happens with osbtacles here.
			}
		}
	}
	public void GetNeighbours(Node node, ArrayList neighbours)
	{
		Vector3 neighbourPosition = node.position;
		int neighbourIndex = GetGridIndex(neighbourPosition);
		int row = GetRow(neighbourIndex);
		int col = GetColumn(neighbourIndex);
		// Bottom neighbour
		int leftNodeRow = row - 1;
		int leftNodeColumn = col;
		AssignNeighbour(leftNodeRow, leftNodeColumn, neighbours);

		// Top neighbour
		leftNodeRow = row + 1;
		leftNodeColumn = col;
		AssignNeighbour(leftNodeRow, leftNodeColumn, neighbours);
		// Left neighbour
		leftNodeRow = row;
		leftNodeColumn = col - 1;
		AssignNeighbour(leftNodeRow, leftNodeColumn, neighbours);

		// Right neighbour
		leftNodeRow = row;
		leftNodeColumn = col + 1;
		AssignNeighbour(leftNodeRow, leftNodeColumn, neighbours);
	}
	private void AssignNeighbour(int row, int col, ArrayList neighbours)
	{
		// If row and col is between the correct values, then act
		if (row != -1 && col != -1 &&
			row < NumOfRows && col < NumOfColumns)
		{
			Node nodeToAdd = Nodes[col, row];
			// If node is not an Obstacle, then add into the neighbours
			if (!nodeToAdd.Obstacle)
			{
				neighbours.Add(nodeToAdd);
			}
		}
	}
	private void OnDrawGizmos()
	{
		if (ShowGrid)
		{
			DebugDrawGrid(transform.position, NumOfRows, NumOfColumns,
						GridCellSize, Color.blue);
		}
		Gizmos.DrawSphere(transform.position, 0.5f);
		if (ShowObstacleBlocks)
		{
			Vector3 cellSize = new Vector3(GridCellSize, 1.0f, GridCellSize);
			if (_obstacles != null && _obstacles.Length > 0)
			{
				foreach (GameObject obstacle in _obstacles)
				{
					Gizmos.DrawCube(GetGridCellCenter(
						GetGridIndex(obstacle.transform.position)), cellSize);
				}
			}
		}
	}
	public void DebugDrawGrid(Vector3 origin, int numRows, int numCols, float cellSize, Color color)
	{
		float width = numCols * cellSize;
		float height = numRows * cellSize;
		for (int row = 0; row <= numRows; row++)
		{
			Vector3 startPos = origin + row * cellSize * new Vector3(0.0f, 0.0f, 1.0f);
			Vector3 endPos = startPos + width * new Vector3(1.0f, 0.0f, 0.0f);
			Debug.DrawLine(startPos, endPos, Color.cyan);
		}
		for (int col = 0; col <= numCols; col++)
		{
			Vector3 startPos = origin + col * cellSize * new Vector3(1.0f, 0.0f, 0.0f);
			Vector3 endPos = startPos + height * new Vector3(0.0f, 0.0f, 1.0f);
			Debug.DrawLine(startPos, endPos, Color.green);
		}
	}
}

