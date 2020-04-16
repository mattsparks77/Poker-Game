using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Hand : MonoBehaviour
{
    public List<CardScriptableObject> cards; // includes the cards out on the table
    public PokerHandInfo handInfo;
    public int PlayerID;
    public PlayerManager player;

    public void InitStart(PlayerManager p)
    {
        player = p;
        cards = new List<CardScriptableObject>();
        handInfo = new PokerHandInfo();
        handInfo.InitPokerHandInfo();
        
    }


    public void InitStart()
    {
        cards = new List<CardScriptableObject>();
        handInfo = new PokerHandInfo();
        handInfo.InitPokerHandInfo();

    }


    public void AddCard(CardScriptableObject c)
    {
        cards.Add(c);
        SortCards();
        handInfo.AddSingleCardToDicts(c);
    }

    public void AddSingleCardToPokerHandInfo(CardScriptableObject c)
    {
        handInfo.AddSingleCardToDicts(c);
    }

    public void CalculateRanks()
    {
        SortCards();

        LogicSystem.FindPairs(handInfo);
        LogicSystem.FindTwoPairs(handInfo);
        LogicSystem.FindTriples(handInfo);
        LogicSystem.FindQuads(handInfo);
        LogicSystem.FindFlush(handInfo);
        LogicSystem.FindStraight(this);
        LogicSystem.FindStraightFlushOrRoyalFlush(this);
        LogicSystem.FindFullHouse(handInfo);

        handInfo.PrintRanks();
    }



    public void AddHandtoPokerHandInfo()
    {

        //cards = cards.OrderBy(o => o.value).ToList(); // sorts cards before adding to dict
        handInfo.AddCardsToDicts(cards);

        CalculateRanks();

        //to do straight flush and royal flush

        //foreach (KeyValuePair<string, List<List<Card>>> kv in handInfo.MyRank){
        //    string s = "";
        //    foreach (List<Card> l in kv.Value)
        //    {
        //        s += l[0].ToString()+ ", ";
        //    }
        //    Debug.Log(kv.Key + ": " + s);
        //}
        //Debug.Log(handInfo.MyRank);



    }

    public void SortCards()
    {
        cards = cards.OrderBy(o => o.value).ToList(); // sorts cards before adding to dict
    }


    public void PrintCards()
    {
        foreach (CardScriptableObject c in cards)
        {
            Debug.Log(c.name);
        }
    }

    public void PrintCards(List<CardScriptableObject> c)
    {
        foreach (CardScriptableObject cd in c)
        {
            Debug.Log(cd.name);
        }
    }
}

public class PokerHandInfo
{
    public Dictionary<string, List<CardScriptableObject>> SuitTotals;
    public Dictionary<int, List<CardScriptableObject>> CardTotals; // dict of card value mapped to a sorted dict of num occurences in hand

    public SortedDictionary<string, List<List<CardScriptableObject>>> MyRank;
    public List<int> cardInHandValues;

    public int[] InitCardValues = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
    public string[] InitSuitStrings = { "C", "H", "D", "S" };
    public string[] InitRankStrings = {"highCard", "pairs", "twoPairs", "triples","straight", "flush", "fullHouse", "quads", "straightFlush", "royalFlush" };


    public List<int> cardVals;
    public List<string> suitStrings;

    public List<CardScriptableObject> hand;


    public void InitPokerHandInfo()
    {
        cardVals = new List<int>(InitCardValues);
        suitStrings = new List<string>(InitSuitStrings);
        cardInHandValues = new List<int>();

        SuitTotals = new Dictionary<string, List<CardScriptableObject>>();
        CardTotals = new Dictionary<int, List<CardScriptableObject>>();
        MyRank = new SortedDictionary<string, List<List<CardScriptableObject>>>();

        InitializeDictionaryCardLists();
    }


    public void InitializeDictionaryCardLists() // call once for each player
    {
        foreach (int i in cardVals)
        {
            List<CardScriptableObject> c = new List<CardScriptableObject>();
            CardTotals[i] = c;
        }

        foreach (string s in suitStrings)
        {
            List<CardScriptableObject> c = new List<CardScriptableObject>();
            SuitTotals[s] = c;
        }

        foreach (string s in InitRankStrings)
        {
            List<List<CardScriptableObject>> c = new List<List<CardScriptableObject>>();
            MyRank[s] = c;
        }

    }


    public void AddCardsToDicts(List<CardScriptableObject> h)
    {

        foreach (CardScriptableObject c in h)
        { 
            SuitTotals[c.suit].Add(c);
            cardInHandValues.Add(c.value); // adds in card values for high value comparison
            
            CardTotals[c.value].Add(c);

        }
    }

    public void AddSingleCardToDicts(CardScriptableObject c)
    {
        SuitTotals[c.suit].Add(c);
        cardInHandValues.Add(c.value); // adds in card values for high value comparison
        CardTotals[c.value].Add(c);
              
    }

    public void ClearDicts()
    {
        foreach (KeyValuePair<int, List<CardScriptableObject>> i in CardTotals)
        {
            i.Value.Clear();
        }
        foreach (KeyValuePair<string, List<CardScriptableObject>> i in SuitTotals)
        {
            i.Value.Clear();
        }
        foreach (var k in MyRank.Keys)
        {
            MyRank[k].Clear();
        }
    }

    ~PokerHandInfo()
    {
        foreach (CardScriptableObject c in hand)
        {
            GameObject.Destroy(c.prefab.gameObject);
        }
        
    }

    public void InitializeHandWithCards()
    {

    }

    public void PrintRanks()
    {
        string s = "";

        foreach (var v in MyRank)
        {
            s += v.Key + ": ";
            foreach (List<CardScriptableObject> c in v.Value)
            {
                foreach(CardScriptableObject cc in c)
                {
                    s += cc.name + ", ";
                }
            }
            s += "\n";

           

        }
        Debug.Log(s);
    }

}
