using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DiceRoller : MonoBehaviour
{
    public Button rollButton;
    public TextMeshProUGUI diceResultText;
    public event Action<int> OnDiceRolled; // So other scripts (like PlayerMover) can listen

    private bool canRoll = true;

    private void Start()
    {
        rollButton.onClick.AddListener(RollDice);
        diceResultText.text = "Click Roll to start!";
    }

    private void RollDice()
    {
        if (!canRoll) return;

        int result = UnityEngine.Random.Range(1, 7); // 1–6 inclusive
        diceResultText.text = "🎲 You rolled: " + result;

        OnDiceRolled?.Invoke(result);

        canRoll = false; // Prevent rolling twice in one turn
    }

    public void EnableRoll(bool state)
    {
        canRoll = state;
        rollButton.interactable = state;
    }

}