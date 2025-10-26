using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem; // if you use Keyboard.current in Update

public class TurnManager : MonoBehaviour
{
    public PlayerMover[] players;   // All players in the game
    public DiceRoller diceRoller;  // recieve the dice results
    private int currentPlayerIndex = 0;
    public float turnDuration = 20f;
    private float timer;
    private bool turnActive = false;

    void Start()
    {
        // Disable all players first
        foreach (var player in players)
        {
            player.canMove = false;
        }
        // Start with player 0
        StartTurn();
    }

    void Update()
    {
        if (!turnActive) return;

        // Countdown timer
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            EndTurn();
        }
        
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            EndTurn();
        }
    }

    public void StartTurn()
    {
        PlayerMover currentPlayer = players[currentPlayerIndex];
        Debug.Log("Starting turn for: " + currentPlayer.name);

        currentPlayer.canMove = false; // wait until dice is rolled

        // Enable dice rolling
        diceRoller.EnableRoll(true);

        // Listen for dice roll event
        diceRoller.OnDiceRolled += currentPlayer.SetMovementPoints;

        // Reset and start timer
        timer = turnDuration;
        turnActive = true;
    }

    public void EndTurn()
    {
        PlayerMover currentPlayer = players[currentPlayerIndex];
        Debug.Log("Ending turn for: " + currentPlayer.name);

        currentPlayer.canMove = false;
        diceRoller.EnableRoll(false);
        diceRoller.diceResultText.text = "";

        // Stop listening to dice rolls
        diceRoller.OnDiceRolled -= currentPlayer.SetMovementPoints;

        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
        StartCoroutine(NextTurnDelay());
    }

    private IEnumerator NextTurnDelay()
    {
        turnActive = false;
        yield return new WaitForSeconds(1f);
        StartTurn();
    }
}
