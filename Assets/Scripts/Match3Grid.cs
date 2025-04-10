using Unity.VisualScripting;
using UnityEngine;

public static class Match3GridConfig
{
    public const int Width = 10;
    public const int Height = 8;
    public const float CellSize = 1f;
    public const float PivotX = -4f;
    public const float PivotY = -4f;
}
public enum GridPoint
{
    Edge,
    Center
}

public class Match3Grid<T> where T : MonoBehaviour
{
    public int width;
    public int height;
    public float cellSize;
    public Vector3 pivot;
    public T[,] gridArray;

    public Match3Grid(int width, int height, float cellSize, Vector3 pivot)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.pivot = pivot;
        this.gridArray = new T[width, height];
    }

    public Vector3 GridToWorld(int x, int y, GridPoint gridPoint)
    {
        if (gridPoint == GridPoint.Edge)
        {
            return new Vector3(x, y, 0) * cellSize + pivot;
        }
        else
        {
            return new Vector3(x, y, 0) * cellSize + pivot + new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0);
        }
    }

    public Vector2Int? WorldToGrid(Vector3 worldPosition)
    {
        if (worldPosition.x > width * cellSize + pivot.x
            || worldPosition.y > height * cellSize + pivot.y
            || worldPosition.x < pivot.x
            || worldPosition.y < pivot.y)
        {
            return null;
        }

        Vector3 gridPosition = (worldPosition - pivot) / cellSize;
        var x = Mathf.FloorToInt(gridPosition.x);
        var y = Mathf.FloorToInt(gridPosition.y);
        return new Vector2Int(x, y);
    }

    //public void DrawDebugLines()
    //{
    //    for (int x = 0; x < width; x++)
    //    {
    //        for (int y = 0; y < height; y++)
    //        {
    //            Debug.DrawLine(GridToWorld(x, y, GridPoint.Edge), GridToWorld(x, y + 1, GridPoint.Edge), Color.white, 100f);
    //            Debug.DrawLine(GridToWorld(x, y, GridPoint.Edge), GridToWorld(x + 1, y, GridPoint.Edge), Color.white, 100f);
    //        }
    //    }
    //    Debug.DrawLine(GridToWorld(0, height, GridPoint.Edge), GridToWorld(width, height, GridPoint.Edge), Color.white, 100f);
    //    Debug.DrawLine(GridToWorld(width, 0, GridPoint.Edge), GridToWorld(width, height, GridPoint.Edge), Color.white, 100f);
    //}

    public bool IsAdjacent(Vector2Int posA, Vector2Int posB)
    {
        int deltaX = (posA.x - posB.x >= 0) ? (posA.x - posB.x) : -(posA.x - posB.x);   // Abs
        int deltaY = (posA.y - posB.y >= 0) ? (posA.y - posB.y) : -(posA.y - posB.y);   // Abs

        return (deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1);
    }
    public bool IsValidPos(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public bool IsValidPos(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }

    public void Swap(Vector2Int targetA, Vector2Int targetB)
    {
        if (!gridArray[targetA.x, targetA.y] || !gridArray[targetB.x, targetB.y]) return;
        
        var tempA = gridArray[targetA.x, targetA.y];
        var tempAPos = gridArray[targetA.x, targetA.y].transform.position;

        var tempB = gridArray[targetB.x, targetB.y];
        var tempBPos = gridArray[targetB.x, targetB.y].transform.position;

        gridArray[targetA.x, targetA.y] = tempB;
        gridArray[targetB.x, targetB.y] = tempA;

        gridArray[targetA.x, targetA.y].transform.position = tempAPos;
        gridArray[targetB.x, targetB.y].transform.position = tempBPos;
    }

}
