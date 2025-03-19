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

    internal Vector3 ToVector()
    {
        // Each direction to vector
        return direction switch
        {
            Direction.Forward => Vector3.forward,
            Direction.Backward => -Vector3.forward,
            Direction.Left => -Vector3.right,
            Direction.Right => Vector3.right,
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}
