using System;

[Flags]
public enum Direction
{
    None = 0,
    Up = 1,  // 0001
    Right = 2,  // 0010
    Down = 4,  // 0100
    Left = 8   // 1000
}

public static class DirectionExtensions
{

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

    public static Direction RotateRight(this Direction dir)
    {
        if (dir == Direction.None) return Direction.None;

        int d = (int)dir;
        d = d << 1;
        if (d > 8) d = 1;

        return (Direction)d;
    }
}