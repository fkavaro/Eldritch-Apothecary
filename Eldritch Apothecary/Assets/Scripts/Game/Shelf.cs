using System;
using UnityEngine;

public class Shelf : Spot
{
    public Shelves shelves;

    public int Amount => shelves.currentAmount;
    public int Capacity => shelves.capacity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // Get Shelves component from parent
        shelves = transform.parent.GetComponent<Shelves>();
    }

    public bool IsFull() => shelves.IsFull();
    public bool Take(int amount) => shelves.Take(amount);
    public bool TakeRandom(int maxAmount) => shelves.TakeRandom(maxAmount);
    public bool CanTake(int amount) => shelves.CanTake(amount);
    public int Replenish(int amount) => shelves.Replenish(amount);
}
