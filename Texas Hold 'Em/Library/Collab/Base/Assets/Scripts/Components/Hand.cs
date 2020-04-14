using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Hand : MonoBehaviour
{
    public List<Card> cards; // includes the cards out on the table
    public PokerHandInfo handInfo;


    public void InitStart()
    {
        cards = new List<Card>();
        handInfo = new PokerHandInfo();
        handInfo.InitPokerHandInfo();
    }


    public void AddHandtoPokerHandInfo()
    {

        //cards = cards.OrderBy(o => o.value).ToList(); // sorts cards before adding to dict
        SortCards();
        handInfo.AddCardsToDicts(cards);
        LogicSystem.FindStraight(this);
    
    }

    public void SortCards()
    {
        cards = cards.OrderBy(o => o.value).ToList(); // sorts cards before adding to dict
    }
}

public class PokerHandInfo
{
    public Dictionary<string, List<Card>> SuitTotals;
    public Dictionary<int, List<Card>> CardTotals; // dict of card value mapped to a sorted dict of num occurences in hand

    public SortedDictionary<string, List<List<Card>>> MyRank;
    public List<int> cardInHandValues;

    public int[] InitCardValues = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
    public string[] InitSuitStrings = { "C", "H", "D", "S" };
    public string[] InitRankStrings = {"highCard", "pairs", "triples","straight", "flush", "fullHouse", "quads", "straightFlush", "royalFlush" };


    public List<int> cardVals;
    public List<string> suitStrings;

    public List<Card> hand;


    public void InitPokerHandInfo()
    {
        cardVals = new List<int>(InitCardValues);
        suitStrings = new List<string>(InitSuitStrings);
        cardInHandValues = new List<int>();

        SuitTotals = new Dictionary<string, List<Card>>();
        CardTotals = new Dictionary<int, List<Card>>();
        MyRank = new SortedDictionary<string, List<List<Card>>>();

        InitializeDictionaryCardLists();
    }


    public void InitializeDictionaryCardLists() // call once for each player
    {
        foreach (int i in cardVals)
        {
            List<Card> c = new List<Card>();
            CardTotals[i] = c;
        }

        foreach (string s in suitStrings)
        {
            List<Card> c = new List<Card>();
            SuitTotals[s] = c;
        }

        foreach (string s in InitRankStrings)
        {
            List<List<Card>> c = new List<List<Card>>();
            MyRank[s] = c;
        }

    }


    public void AddCardsToDicts(List<Card> h)
    {

        foreach (Card c in h)
        { 
            SuitTotals[c.suit].Add(c);
            cardInHandValues.Add(c.value); // adds in card values for high value comparison
            
            CardTotals[c.value].Add(c);

        }
    }

    public void AddSingleCardToDicts(Card c)
    {
        SuitTotals[c.suit].Add(c);
        cardInHandValues.Add(c.value); // adds in card values for high value comparison
        CardTotals[c.value].Add(c);
              
    }

    public void ClearDicts()
    {
        foreach (KeyValuePair<int, List<Card>> i in CardTotals)
        {
            i.Value.Clear();
        }
        foreach (KeyValuePair<string, List<Card>> i in SuitTotals)
        {
            i.Value.Clear();
        }
        foreach (var k in MyRank.Keys)
        {
            MyRank[k].Clear();
        }
    }

    public void DestroyHand()
    {
        
    }

    public void InitializeHandWithCards()
    {

    }

    public void Print()
    {
        foreach (var v in CardTotals)
        {
            Debug.Log("       Key: " + v.Key);
            for (int i = 0; i < v.Value.Count ; i ++)
            {
                Debug.Log(v.Value[i]);
            }
           

        }
    }
}
