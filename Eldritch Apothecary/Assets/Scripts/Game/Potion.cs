using System;
using UnityEngine;
using TMPro;

public class Potion : MonoBehaviour
{
    [SerializeField] int turnNumber = -1;
    public GameObject model;
    public TextMeshProUGUI turnText;

    void Update()
    {
        turnText.text = turnNumber.ToString();

        // Isn't assigned
        if (turnNumber == -1)
        {
            model.SetActive(false);
            turnText.gameObject.SetActive(false);
        }
        // Is assigned
        else
        {
            model.SetActive(true);
            turnText.gameObject.SetActive(true);
        }
    }

    public bool IsAssigned()
    {
        return turnNumber >= 0;
    }

    public bool ThisNumber(int number)
    {
        return turnNumber == number && IsAssigned();
    }

    public int Take()
    {
        int number = turnNumber;
        turnNumber = -1;
        return number;
    }

    public void Assign(int number)
    {
        turnNumber = number;
    }
}