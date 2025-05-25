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
    public float currentValue;
    float _newValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        supplyBar.maxValue = maxValue;
        currentValue = maxValue;

        _newValue = maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        // If the current value is greater than the new value
        if (currentValue > _newValue)
            // Reduce it by one second each time
            currentValue -= Time.deltaTime;
        // If current value is less than the new value
        else if (currentValue < _newValue)
            // Increase it by one second each time
            currentValue += Time.deltaTime;

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
    public bool Take(int amount)
    {
        // Enough supplying
        if (_newValue >= amount && _newValue > 0)
        {
            _newValue -= amount;
            return true;
        }
        else // Not enough supplying
        {
            return false;
        }
    }

    /// <summary>
    /// Resupply the shelves to its maximum value.
    /// </summary>
    public void Resupply()
    {
        _newValue = maxValue;
    }
}
