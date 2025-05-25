using System;
using UnityEngine;

public class Shelf : Spot
{
    Shelves _shelves;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get Shelves component from parent
        _shelves = transform.parent.GetComponent<Shelves>();
    }

    public bool Take(int amount)
    {
        return _shelves.Take(amount);
    }

    internal bool CanTake(int amount)
    {
        return _shelves.CanTake(amount);
    }
}
