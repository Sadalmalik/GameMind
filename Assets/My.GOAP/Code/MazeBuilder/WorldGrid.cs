using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class WorldGrid : MonoBehaviour
{
	public Vector2Int size;
	public float      cellSize;

	public List<Cell>   AllCells   { get; } = new List<Cell>();
	public List<Border> AllBorders { get; } = new List<Border>();

	public Cell[,] Grid { get; private set; }

	void Awake()
	{
		BuildGrid();
	}

	void Update()
	{
	}

	private void BuildGrid()
	{
		AllCells.Clear();
		AllBorders.Clear();
		Grid = new Cell[size.x, size.y];

		for (int y = 0; y < size.y; y++)
		for (int x = 0; x < size.x; x++)
		{
			var cell = Create<Cell>(transform);
			cell.name                    = $"Cell [{x}, {y}]";
			cell.position                = new Vector2Int(x, y);
			cell.transform.localPosition = new Vector3(cell.position.x, -0.5f, cell.position.y) * cellSize;
			cell.view                    = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cell.view.transform.SetParent(cell.transform, false);
			cell.view.transform.localScale = cellSize * Vector3.one;

			Grid[x, y] = cell;
			AllCells.Add(cell);
		}

		for (int y = 0; y <= size.y; y++)
		for (int x = 0; x <= size.x; x++)
		{
			var cell = GetCell(x, y);
			var left = GetCell(x - 1, y);
			var back = GetCell(x, y - 1);

			if (cell || left)
			{
				var horBorder = Create<Border>(transform);

				horBorder.name                    = $"Border [{x - 1}-{x}, {y}]";
				horBorder.forwardDirection        = true;
				horBorder.transform.localPosition = new Vector3(x - 0.5f, 0, y) * cellSize;
				horBorder.view                    = GameObject.CreatePrimitive(PrimitiveType.Cube);
				horBorder.view.transform.SetParent(horBorder.transform, false);
				horBorder.view.transform.localScale = new Vector3(cellSize * 0.1f, cellSize, cellSize);

				horBorder.leftOrForward = left;
				horBorder.rightOrBack   = cell;
				if (left) left.right = horBorder;
				if (cell) cell.left  = horBorder;

				AllBorders.Add(horBorder);
			}

			if (back || cell)
			{
				var verBorder = Create<Border>(transform);

				verBorder.name                    = $"Border [{x}, {y - 1}-{y}]";
				verBorder.forwardDirection        = false;
				verBorder.transform.localPosition = new Vector3(x, 0, y - 0.5f) * cellSize;
				verBorder.view                    = GameObject.CreatePrimitive(PrimitiveType.Cube);
				verBorder.view.transform.SetParent(verBorder.transform, false);
				verBorder.view.transform.localScale = new Vector3(cellSize, cellSize, cellSize * 0.1f);

				verBorder.leftOrForward = cell;
				verBorder.rightOrBack   = back;
				if (cell) cell.back    = verBorder;
				if (back) back.forward = verBorder;

				AllBorders.Add(verBorder);
			}
		}
	}

	public Cell GetCell(int x, int y)
	{
		if (x < 0
		 || x >= size.x
		 || y < 0
		 || y >= size.y)
			return null;
		return Grid[x, y];
	}

	public static T Create<T>(Transform parent) where T : Component
	{
		var o = new GameObject();
		o.transform.SetParent(parent);
		return o.AddComponent<T>();
	}
}