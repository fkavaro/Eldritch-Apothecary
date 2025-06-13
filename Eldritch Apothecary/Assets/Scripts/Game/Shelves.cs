using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shelves : MonoBehaviour
{
    [Header("Bar")]
    public Slider supplyBar;
    public Gradient colorGradient;
    public Image fill;
    public TextMeshProUGUI valueText;

    [Header("Amounts")]
    public int capacity = 100;
    public int currentAmount;
    public int lackingAmount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        supplyBar.maxValue = capacity;

        // Calculate 80% of capacity
        float fractionCapacity = capacity * 0.8f;
        // Randomly generate a number between fractionCapacity and capacity + 1
        currentAmount = (int)Random.Range(fractionCapacity, capacity + 1);

        // Set supply bar value to currenValue
        supplyBar.value = currentAmount;
        fill.color = colorGradient.Evaluate(supplyBar.normalizedValue);
        valueText.text = currentAmount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // Supply bar isn't updated
        if (supplyBar.value != currentAmount)
        {
            // Set supply bar value to currenValue
            supplyBar.value = currentAmount;
            fill.color = colorGradient.Evaluate(supplyBar.normalizedValue);
            valueText.text = currentAmount.ToString();
        }

        lackingAmount = capacity - currentAmount;
    }

    /// <summary>
    /// Returns true if the amount can be taken.
    /// </summary>
    internal bool CanTake(int amount)
    {
        // Enough supplying
        if (currentAmount >= amount && currentAmount > 0)
            return true;
        else // Not enough supplying
            return false;
    }

    /// <summary>
    /// Takes the specified amount from the shelves if possible.
    /// </summary>
    public bool Take(int amount)
    {
        if (CanTake(amount))
        {
            currentAmount -= amount;
            return true;
        }
        else
            return false;
    }

    public bool TakeRandom(int maxAmout)
    {
        return Take(Random.Range(1, maxAmout + 1));
    }

    /// <summary>
    /// Resupply the shelves to its maximum value.
    /// </summary>
    public int Replenish(int supplyAmount)
    {
        currentAmount += supplyAmount;

        // Max amount surpassed
        if (currentAmount > capacity)
        {
            // Return the excess
            supplyAmount = currentAmount - capacity;
            currentAmount = capacity;
        }
        else
            // All supplies were replenished
            supplyAmount = 0;

        return supplyAmount;
    }

    public bool IsFull()
    {
        return currentAmount >= capacity;
    }
}