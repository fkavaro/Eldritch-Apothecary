using UnityEngine;
using System.Collections.Generic;

public class Shop
{
    // Dictionary of shop stands and their ocupant
    readonly Dictionary<Transform, Client> shopStands = new();
    readonly object shopLock = new();

    // Constructor given list
    public Shop(List<Transform> shopStands)
    {
        foreach (Transform stand in shopStands)
        {
            this.shopStands.Add(stand, null);
        }
    }

    public Vector3 RandomStand(Client client)
    {
        lock (shopLock)
        {
            List<Transform> availableStands = new();

            // Find available shop stands
            foreach (KeyValuePair<Transform, Client> stand in shopStands)
            {
                if (stand.Value == null)
                    availableStands.Add(stand.Key);
            }

            if (availableStands.Count > 0)
            {
                // Select a random stand
                Transform randomStand = availableStands[Random.Range(0, availableStands.Count)];
                // Set client target to shop stand
                //client.SetTarget(randomStand.position);
                // Set shop stand as ocupied
                shopStands[randomStand] = client;
                return randomStand.position;
            }
            else
            {
                // No available shop stands
                return Vector3.zero;
            }

        }
    }

    public void Leave(Client client)
    {
        lock (shopLock)
        {
            // Liberate shop stand, accessed by value
            foreach (KeyValuePair<Transform, Client> stand in shopStands)
            {
                if (stand.Value == client)
                {
                    // Set shop stand as available
                    shopStands[stand.Key] = null;
                    break;
                }
            }

        }
    }
}
