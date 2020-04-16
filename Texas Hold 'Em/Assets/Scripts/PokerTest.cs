using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerTest : MonoBehaviour
{
    public List<Hand> testHands;
    public CardManager cm;
    public GameManager gm;
    public List<CardScriptableObject> handCards;
    public int CardsInHand;
    public int HandsToCreate;
    public int playerIDs;
    public GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        cm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CardManager>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        testHands = new List<Hand>();
        cm.InitStart();
        CardsInHand = 7;
        playerIDs = 0;
   
    }

    //public Player CreatePlayer()
    //{
    //    Player p = new Player();
    //    GameObject o = new GameObject();
    //   // p = Instantiate(o).AddComponent<Player>();
    //    //p.gameObject.AddComponent<Hand>();
    //   // p.hand = new Hand();
    //   // p.hand.InitStart(p);
    //    int id = GeneratePlayerID();
    //    //p.gameObject.name = "Player " + id;
    //   // p.ID = id;

    //    return p;
    //}
    ////creates a new hand for the player
    //public void CreateHandForPlayer(int pID)
    //{
    //    GameObject o = new GameObject();
    //    Hand h = Instantiate(o).AddComponent<Hand>();
    //    h.PlayerID = GeneratePlayerID();
    //    Debug.Log(h.PlayerID);
    //}

    //public void CreateHandObjects(int n, int numCards)
    //{
    //    for (int i = 0; i < n; i++)
    //    {
    //        testHands.Add(cm.TestDealHand(numCards));
    //    }
    //}

    //public int GeneratePlayerID()
    //{
    //    return playerIDs++;
    //}

    //public void CreateTestHand(Hand h)
    //{
    //    //for (int i =0; i <7; i++)
    //    //{
    //    //    Card c = cm.TestDeal();
    //    //    h.cards.Add(c); // adds in 7 cards to hand
    //    //}
    //    //handCards = h.cards;
    //    //h.AddHandtoPokerHandInfo();
    //    //NumHands++;
    //}
}
