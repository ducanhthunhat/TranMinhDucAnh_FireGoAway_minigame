using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("Map Size")]
    public int width = 10;
    public int height = 10;

    private PipeCell[,] _grid;
    public static event System.Action OnLevelWin;

    private void Start()
    {
        InitializeGrid();


        PipeCell.OnPipeRotated += RecalculateFlow;

        RecalculateFlow();
    }

    private void OnDestroy()
    {
        PipeCell.OnPipeRotated -= RecalculateFlow;
    }


    private void InitializeGrid()
    {
        _grid = new PipeCell[width, height];
        var allPipes = FindObjectsOfType<PipeCell>();

        foreach (var pipe in allPipes)
        {

            if (pipe.x >= 0 && pipe.x < width && pipe.y >= 0 && pipe.y < height)
            {
                _grid[pipe.x, pipe.y] = pipe;
            }
            else
            {
                Debug.LogError($"Pipe ở ({pipe.x},{pipe.y}) nằm ngoài Map Size!");
            }
        }
    }

    private void RecalculateFlow()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_grid[x, y] != null && !_grid[x, y].isSource)
                {
                    _grid[x, y].SetWaterState(false);
                }
            }
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_grid[x, y] != null && _grid[x, y].isSource)
                {
                    FloodFill(x, y);
                }
            }
        }

        CheckWinCondition();
    }

    // Thuật toán loang nước (Đệ quy)
    private void FloodFill(int x, int y)
    {
        PipeCell current = _grid[x, y];
        if (current == null) return;

        current.SetWaterState(true);

        // Kiểm tra 4 hướng xung quanh
        CheckNeighbor(current, x, y + 1, Direction.Up);
        CheckNeighbor(current, x + 1, y, Direction.Right);
        CheckNeighbor(current, x, y - 1, Direction.Down);
        CheckNeighbor(current, x - 1, y, Direction.Left);
    }

    private void CheckNeighbor(PipeCell current, int nx, int ny, Direction dirToNeighbor)
    {
        // Check biên map
        if (nx < 0 || nx >= width || ny < 0 || ny >= height) return;

        PipeCell neighbor = _grid[nx, ny];
        if (neighbor == null || neighbor.isWet) return;
        bool myOutput = current.currentConnections.HasFlag(dirToNeighbor);
        bool neighborInput = neighbor.currentConnections.HasFlag(dirToNeighbor.GetOpposite());
        if (myOutput && neighborInput)
        {
            FloodFill(nx, ny);
        }
    }

    //Kiểm tra điều kiện thắng
    private void CheckWinCondition()
    {
        bool allEndsConnected = true;
        bool hasEndPoint = false;

        foreach (var pipe in _grid)
        {
            if (pipe != null && pipe.isEndPoint)
            {
                hasEndPoint = true;
                if (!pipe.isWet)
                {
                    allEndsConnected = false;
                    break;
                }
            }
        }

        if (hasEndPoint && allEndsConnected)
        {
            OnLevelWin?.Invoke();
            UIManager.Instance.OpenUI<UIWinPanel>();
            UIManager.Instance.PauseGame();
        }
    }
}