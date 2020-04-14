using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    const int NUM_SEATS_ALLOWED = 10;
    public static GameState instance;

    public int currentBet;
    public int amountInPot;

    public int currentTurnIndex;

    public int smallBlindIndex;
    public int bigBlindIndex;
    public int dealerIndex;

   // public static int playersAtTable;
    public int playersLeft;
    public bool gameStarted = false;

    public static List<PlayerManager> playersAtTable = new List<PlayerManager>();

    public static List<Card> communityCards = new List<Card>();

    public static Dictionary<int, Seat> seats = new Dictionary<int, Seat>();


    public static void Initialize()
    {
        for (int i = 0; i < NUM_SEATS_ALLOWED; i++) playersAtTable.Add(null);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Initialize();
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object.");
            Destroy(this);
        }
    }


    public static void UpdateGameState()
    {
        //to do set values in variables above to values received from the server. May also need to add a game state class to the server.. 
    }
}
