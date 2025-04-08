using System;
using UnityEngine;

public class Position : MonoBehaviour
{
    public enum Direction
    {
        Forward,
        Backward,
        Left,
        Right
    }
    public Direction direction;
    public bool occupied = false;
    readonly object posLock = new();

    public void SetOccupied(bool occupied)
    {
        lock (posLock)
        {
            this.occupied = occupied;
        }
    }

    public bool IsOccupied()
    {
        lock (posLock)
        {
            return occupied;
        }
    }

    /// <summary>
    /// Each direction to a vector in world coordinates.
    /// </summary>
    public Vector3 DirectionToVector()
    {
        return direction switch
        {
            Direction.Forward => new Vector3(0f, 0f, 0f),
            Direction.Backward => new Vector3(0f, -180f, 0f),
            Direction.Left => new Vector3(0f, -90f, 0f),
            Direction.Right => new Vector3(0f, 90f, 0f),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}
