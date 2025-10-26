using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DiceManager : MonoBehaviour
{
    public Button rollButton;
    public Text diceResultText; // optional, shows the number
    public int lastRoll { get; private set; }
    public bool hasRolled { get; private set; }

    public void RollDice()
    {
        StartCoroutine(RollAnimation());
    }

    private IEnumerator RollAnimation()
    {
        rollButton.interactable = false;
        hasRolled = false;

        // Fake rolling animation
        for (int i = 0; i < 10; i++)
        {
            int tempRoll = Random.Range(1, 7);
            diceResultText.text = tempRoll.ToString();
            yield return new WaitForSeconds(0.05f);
        }

        // Final result
        lastRoll = Random.Range(1, 7);
        diceResultText.text = lastRoll.ToString();

        hasRolled = true;
        rollButton.interactable = true;

        Debug.Log("Rolled a " + lastRoll);
    }
}
