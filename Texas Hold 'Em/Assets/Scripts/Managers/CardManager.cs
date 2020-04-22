using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;


public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    public List<Card> communityCards;
    public List<CardScriptableObject> cardList;
    public Queue<CardScriptableObject> deck; // contains card SOs 
    public TextAsset txt;
    public string[] lines;

    public Dictionary<string, CardScriptableObject> cardGameObjects;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitStart();
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    public void InitStart()
    {
        deck = new Queue<CardScriptableObject>();

        communityCards = new List<Card>();
        cardList = new List<CardScriptableObject>();
        cardGameObjects = new Dictionary<string, CardScriptableObject>();
        LoadCardResourcesIntoGame();
        LoadCards();

    }

    public CardScriptableObject TestDealCard()
    {
        GameObject o = new GameObject();
        return CardDealer.instance.DealCard(3,4);
    }

    public Hand TestDealHand(int numCards)
    {
         return CardDealer.instance.DealHand(numCards);
    }

    public void TestReshuffle()
    {
        CardDealer.instance.ReShuffleCards();
    }

    // shuffles list of cards and then puts them into a queue
    public void CreateDeck()
    {
        cardList.ShuffleList<CardScriptableObject>();
        foreach (CardScriptableObject c in cardList)
        {
            deck.Enqueue(c);
        }
    }


    public CardScriptableObject SpawnCardByName(string n)
    {
        if (!lines.Contains(n))
        {
            //return null;
        }
        return Resources.Load<CardScriptableObject>("CardObjects/Card_" + n);
    }

    public void SpawnCardBySuitRank(int suit, int rank)
    {
        //if (!lines.Contains(n))
        //{
        //    //return null;
        //}
        //return Resources.Load<CardScriptableObject>("CardObjects/Card_" + n);
    }


    public void TestStraightFlush()
    {
        Hand h = CardDealer.instance.CreateNewHandObject();
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("AceofSpades"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("QueenofSpades"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("JackofSpades"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("5ofSpades"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("4ofSpades"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("3ofSpades"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("2ofSpades"));

        //h.SortCards();
        //h.AddHandtoPokerHandInfo();
        h.CalculateRanks();
    }

    public void TestStraight()
    {
        Hand h = CardDealer.instance.CreateNewHandObject();
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("AceofSpades"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("KingofSpades"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("JackofSpades"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("9ofClubs"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("10ofDiamonds"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("8ofHearts"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("7ofSpades"));

        //h.SortCards();
        //h.AddHandtoPokerHandInfo();
        h.CalculateRanks();
    }

    public void TestFullHouse()
    {
        Hand h = CardDealer.instance.CreateNewHandObject();
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("AceofSpades"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("QueenofSpades"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("QueenofHearts"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("QueenofClubs"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("AceofDiamonds"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("3ofHearts"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("2ofSpades"));

        //h.SortCards();
        //h.AddHandtoPokerHandInfo();
        h.CalculateRanks();
    }

    public void TestTwoPairs()
    {
        {
            Hand h = CardDealer.instance.CreateNewHandObject();
            CardDealer.instance.DealSpecificCard(h, SpawnCardByName("2ofSpades"));
            CardDealer.instance.DealSpecificCard(h, SpawnCardByName("QueenofSpades"));
            CardDealer.instance.DealSpecificCard(h, SpawnCardByName("QueenofHearts"));
            CardDealer.instance.DealSpecificCard(h, SpawnCardByName("3ofClubs"));
            CardDealer.instance.DealSpecificCard(h, SpawnCardByName("3ofDiamonds"));
            CardDealer.instance.DealSpecificCard(h, SpawnCardByName("JackofHearts"));
            CardDealer.instance.DealSpecificCard(h, SpawnCardByName("2ofSpades"));

            //h.SortCards();
            //h.AddHandtoPokerHandInfo();
            h.CalculateRanks();
        }
    }



    public void CreateRoyalFlush()
    {
        Hand h = CardDealer.instance.CreateNewHandObject();
        List<CardScriptableObject> cards = new List<CardScriptableObject>();
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("KingofSpades"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("QueenofSpades"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("JackofSpades"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("AceofSpades"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("10ofSpades"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("7ofClubs"));
        CardDealer.instance.DealSpecificCard(h, SpawnCardByName("6ofDiamonds"));
        h.CalculateRanks();

        //h.SortCards();
        //h.AddHandtoPokerHandInfo();
    }


    public void LoadCardResourcesIntoGame()
    {
        txt = (TextAsset)Resources.Load("loadCards", typeof(TextAsset));
        lines = Regex.Split(txt.text, "\n|\r|\r\n");
    }

    public void LoadCards()
    {
        foreach (string line in lines)
        {
            if (line == "") continue;
            CardScriptableObject c = Resources.Load<CardScriptableObject>("CardObjects/Card_" + line);
            string k = c.suitValue.ToString() + c.value.ToString();
            //Debug.Log($"Adding key as {k}");
            cardGameObjects.Add(k, c);
            cardList.Add(c);

        }
    }
}
