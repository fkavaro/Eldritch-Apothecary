using System;
using UnityEngine;

public class Table : MonoBehaviour
{
    /// <summary>
    /// Event triggered when object enters the trigger area
    /// </summary>
    public event Action<GameObject> AnnoyingOnTable;

    /// <summary>
    /// Event triggered when object exits the trigger area
    /// </summary>
    public event Action<GameObject> AnnoyingOffTable;

    /// <summary>
    /// The specific GameObject to watch for
    /// </summary>
    public GameObject annoyingCreature;

    /// <summary>
    /// Position where the creature will annoy
    /// </summary>
    public Transform annoyPosition;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == annoyingCreature)
        {
            //Debug.Log("Annoying creature over " + name + "!!");
            AnnoyingOnTable?.Invoke(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == annoyingCreature)
        {
            //Debug.Log("Annoying creature off " + name);
            AnnoyingOffTable?.Invoke(other.gameObject);
        }
    }
}

