using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("Map Size")]
    public int width = 10;
    public int height = 10;

    private PipeCell[,] _grid; // Mảng quản lý toàn bộ ô

    private void Start()
    {
        InitializeGrid();

        // Đăng ký sự kiện: Cứ có ống nào xoay xong thì tính lại nước
        PipeCell.OnPipeRotated += RecalculateFlow;

        // Tính lần đầu khi vào game
        RecalculateFlow();
    }

    private void OnDestroy()
    {
        PipeCell.OnPipeRotated -= RecalculateFlow;
    }

    // 1. Khởi tạo bản đồ logic
    private void InitializeGrid()
    {
        _grid = new PipeCell[width, height];
        var allPipes = FindObjectsOfType<PipeCell>();

        foreach (var pipe in allPipes)
        {
            // Kiểm tra xem ống có nằm trong phạm vi map không
            if (pipe.x >= 0 && pipe.x < width && pipe.y >= 0 && pipe.y < height)
            {
                _grid[pipe.x, pipe.y] = pipe;
                Debug.Log($"Đã thêm ống {pipe.name} vào Grid[{pipe.x}, {pipe.y}]");
            }
            else
            {
                Debug.LogError($"Pipe ở ({pipe.x},{pipe.y}) nằm ngoài Map Size!");
            }
        }
    }

    // 2. Logic tính dòng chảy (Reset & FloodFill)
    private void RecalculateFlow()
    {
        // Bước A: Reset toàn bộ nước (trừ nguồn)
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

        // Bước B: Tìm Nguồn và bắt đầu loang nước
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

        // Bước C: Check Win
        CheckWinCondition();
    }

    // Thuật toán loang nước (Đệ quy)
    private void FloodFill(int x, int y)
    {
        PipeCell current = _grid[x, y];
        if (current == null) return;

        current.SetWaterState(true); // Đánh dấu có nước

        // Kiểm tra 4 hướng xung quanh
        CheckNeighbor(current, x, y + 1, Direction.Up);    // Trên
        CheckNeighbor(current, x + 1, y, Direction.Right); // Phải
        CheckNeighbor(current, x, y - 1, Direction.Down);  // Dưới
        CheckNeighbor(current, x - 1, y, Direction.Left);  // Trái
    }

    private void CheckNeighbor(PipeCell current, int nx, int ny, Direction dirToNeighbor)
    {
        // Check biên map
        if (nx < 0 || nx >= width || ny < 0 || ny >= height) return;

        PipeCell neighbor = _grid[nx, ny];

        // Nếu không có ống, hoặc đã có nước rồi -> Bỏ qua
        if (neighbor == null || neighbor.isWet) return;

        // --- LOGIC KHỚP LỖ (QUAN TRỌNG NHẤT) ---
        // 1. Tôi có lỗ hướng về phía Ông không?
        bool myOutput = current.currentConnections.HasFlag(dirToNeighbor);

        // 2. Ông có lỗ hướng về phía Tôi không? (Hướng ngược lại)
        bool neighborInput = neighbor.currentConnections.HasFlag(dirToNeighbor.GetOpposite());

        // Nếu cả 2 đều mở lỗ -> Thông nhau -> Nước chảy qua
        if (myOutput && neighborInput)
        {
            FloodFill(nx, ny);
        }
    }

    // 3. Kiểm tra điều kiện thắng
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
                    break; // Có 1 cái chưa có nước thì chưa thắng
                }
            }
        }

        if (hasEndPoint && allEndsConnected)
        {
            Debug.Log("<color=green>WIN! TẤT CẢ ĐIỂM CUỐI ĐÃ CÓ NƯỚC!</color>");
            // TODO: Hiện bảng Win UI ở đây
        }
    }
}