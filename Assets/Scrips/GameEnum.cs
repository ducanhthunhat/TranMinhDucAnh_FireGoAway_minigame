using System;

[Flags] // Cho phép cộng dồn các hướng (Ví dụ: Up | Right)
public enum Direction
{
    None  = 0,
    Up    = 1,  // 0001
    Right = 2,  // 0010
    Down  = 4,  // 0100
    Left  = 8   // 1000
}

// Class hỗ trợ tính toán hướng (Helper)
public static class DirectionExtensions
{
    // Hàm lấy hướng đối diện (để check khớp nối)
    // VD: Tôi có lỗ bên Phải -> Ông bên cạnh phải có lỗ bên Trái
    public static Direction GetOpposite(this Direction dir)
    {
        switch (dir)
        {
            case Direction.Up: return Direction.Down;
            case Direction.Right: return Direction.Left;
            case Direction.Down: return Direction.Up;
            case Direction.Left: return Direction.Right;
            default: return Direction.None;
        }
    }

    // Hàm xoay hướng logic theo chiều kim đồng hồ
    public static Direction RotateRight(this Direction dir)
    {
        if (dir == Direction.None) return Direction.None;
        
        int d = (int)dir;
        d = d << 1; // Dịch bit sang trái (1 -> 2 -> 4 -> 8)
        if (d > 8) d = 1; // Nếu quá 8 (Left) thì quay về 1 (Up)
        
        return (Direction)d;
    }
}