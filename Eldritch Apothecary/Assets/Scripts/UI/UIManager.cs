using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Simulation speed")]
    public Slider simulationSpeedBar;
    public TextMeshProUGUI simulationSpeedText;
    [Header("Sorcerer turn system")]
    public TextMeshProUGUI sorcererGeneratedTurnsText;
    public TextMeshProUGUI sorcererCurrentTurnText;
    [Header("Alchemist turn system")]
    public TextMeshProUGUI alchemistGeneratedTurnsText;
    public TextMeshProUGUI alchemistCurrentTurnText;
    [Header("Supplies bars")]
    public Slider sorcererSuppliesBar;
    public Slider alchemistSuppliesBar;
    public Slider shopSuppliesBar;

    // Update is called once per frame
    void Update()
    {
        ApothecaryManager.Instance.simSpeed = simulationSpeedBar.value;
        simulationSpeedText.text = simulationSpeedBar.value.ToString("F1");

        sorcererGeneratedTurnsText.text = ApothecaryManager.Instance.generatedSorcererTurns.ToString();
        sorcererCurrentTurnText.text = ApothecaryManager.Instance.currentSorcererTurn.ToString();

        alchemistGeneratedTurnsText.text = ApothecaryManager.Instance.generatedAlchemistTurns.ToString();
        alchemistCurrentTurnText.text = ApothecaryManager.Instance.currentAlchemistTurn.ToString();

        sorcererSuppliesBar.value = 1f - ApothecaryManager.Instance.normalisedSorcererLack;
        alchemistSuppliesBar.value = 1f - ApothecaryManager.Instance.normalisedAlchemistLack;
        shopSuppliesBar.value = 1f - ApothecaryManager.Instance.normalisedShopLack;
    }
}
