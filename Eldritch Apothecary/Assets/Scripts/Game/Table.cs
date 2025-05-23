using System;
using UnityEngine;

public class Table : MonoBehaviour
{
    // Event triggered when object enters the trigger area
    public event Action<GameObject> AnnoyingOnTable;

    // Event triggered when object exits the trigger area
    public event Action<GameObject> AnnoyingOffTable;

    // The specific GameObject to watch for
    public GameObject annoyingCreature;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == annoyingCreature)
        {
            Debug.Log("Annoying creature over " + name + "!!");
            AnnoyingOnTable?.Invoke(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == annoyingCreature)
        {
            Debug.Log("Annoying creature off " + name);
            AnnoyingOffTable?.Invoke(other.gameObject);
        }
    }
}

