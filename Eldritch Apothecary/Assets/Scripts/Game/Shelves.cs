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

    [Header("Values")]
    public int maxValue = 100;
    public int currentValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        supplyBar.maxValue = maxValue;
        currentValue = maxValue;

        // Set supply bar value to currenValue
        supplyBar.value = currentValue;
        fill.color = colorGradient.Evaluate(supplyBar.normalizedValue);
        valueText.text = currentValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // Supply bar isn't updated
        if (supplyBar.value != currentValue)
        {
            // Set supply bar value to currenValue
            supplyBar.value = currentValue;
            fill.color = colorGradient.Evaluate(supplyBar.normalizedValue);
            valueText.text = currentValue.ToString();
        }
    }

    /// <summary>
    /// Returns true if the amount can be taken.
    /// </summary>
    internal bool CanTake(int amount)
    {
        // Enough supplying
        if (currentValue >= amount && currentValue > 0)
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
            currentValue -= amount;
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Resupply the shelves to its maximum value.
    /// </summary>
    public void Resupply()
    {
        currentValue = maxValue;
    }


}
