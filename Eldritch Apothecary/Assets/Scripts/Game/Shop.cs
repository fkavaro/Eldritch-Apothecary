using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;

public class Shop
{
    // Dictionary of shop stands and their ocupant
    //readonly Dictionary<Position, Client> shopStands = new();
    readonly List<Spot> _shopStands = new();
    readonly object _shopLock = new();

    // Constructor given list
    public Shop(List<Spot> shopStands)
    {
        this._shopStands = shopStands;
        // foreach (Position stand in shopStands)
        // {
        //     this.shopStands.Add(stand, null);
        // }
    }

    public Spot RandomStand(Client client)
    {
        lock (_shopLock)
        {
            List<Spot> availableStands = new();

            // Find available shop stands
            foreach (Spot stand in _shopStands)
            {
                if (!stand.IsOccupied())
                    availableStands.Add(stand);
            }

            if (availableStands.Count > 0)
            {
                // Select a random stand
                Spot randomStand = availableStands[Random.Range(0, availableStands.Count)];
                // Set client target to shop stand
                //client.SetTarget(randomStand.position);
                // Set shop stand as ocupied
                //shopStands[randomStand] = client;
                randomStand.SetOccupied(true);
                return randomStand;
            }
            else
            {
                // No available shop stands
                return null;
            }

        }
    }
}
