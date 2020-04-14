using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class LogicSystem // best 5 card
{
    bool hasRoyalFlush = false;
    bool hasStraightFlush = false;
    bool hasQuads = false;
    bool hasFullHouse = false;
    bool hasFlush = false;
    bool hasStraight = false;
    bool hasTrips = false;
    bool hasTwoPair = false;
    bool hasPair = false;
    bool hasOnlyHighCard = false;

    int highCardValue = 0;
    int highPair1Value = 0;
    int highPair2Value = 0;
    int highPair3Value = 0;
    int highTripValue = 0;
    int highTrip1Value = 0;
    int highQuadValue = 0;



    struct BitBoolValue
    {
        public BitArray bools { get; set; }
        public int highValue { get; set; }

    }
    public void CheckHand(Hand h)
    {
        BitBoolValue retData = new BitBoolValue();
        retData.bools = new BitArray(10);
        int numCards = h.cards.Count;

        switch (numCards)
        {
            case 2:
                if (h.handInfo.CardTotals[h.cards[0].value].Count == 2) // checks for pocket hand
                {
                    hasPair = true;
                    highPair1Value = h.cards[0].value;
                }
                else
                {
                    highCardValue = Mathf.Max(h.cards[0].value, h.cards[1].value);
                    hasOnlyHighCard = true;
                }
                break;


            case 3:
                foreach (CardScriptableObject c in h.cards)
                {
                    if (h.handInfo.CardTotals[c.value].Count == 2)
                    {

                    }
                    else if (h.handInfo.CardTotals[c.value].Count == 3)
                    {
                        hasTrips = true;
                    }
                }
                break;
        }
        foreach (KeyValuePair<int, List<CardScriptableObject>> item in h.handInfo.CardTotals)
        {
            if (item.Value.Count >= 2) // checks how many cards are in the dictionary for each card value 1-13
            {
                hasPair = true;
                highPair1Value = item.Key;
            }
        }

        foreach (KeyValuePair<string, List<CardScriptableObject>> item in h.handInfo.SuitTotals)
        {
            if (item.Value.Count >= 5) // checks how many cards are in the dictionary for each card value 1-13
            {
                hasFlush = true;


            }
        }
    }


    public static bool FindPairs(PokerHandInfo p) // returns true if any pairs (including more than one) are found and adds into hand info.
    {
        bool anyPairs = false;
        foreach (int key in p.CardTotals.Keys)
        {
            if (p.CardTotals[key].Count == 2 && !p.MyRank["pairs"].Contains(p.CardTotals[key]))
            {
                p.MyRank["pairs"].Add(p.CardTotals[key]);
                anyPairs = true;
            }
        }
        return anyPairs;
    }

    public static bool FindTwoPairs(PokerHandInfo p)
    {
        if (p.MyRank["pairs"].Count == 2)
        {
            p.MyRank["twoPairs"].Add(p.MyRank["pairs"][0]);
            p.MyRank["twoPairs"].Add(p.MyRank["pairs"][1]);
            return true;

        }
        else if (p.MyRank["pairs"].Count > 2)
        {
            p.MyRank["twoPairs"] = FindMaxTwoPairInCards(p.MyRank["pairs"]);
            return true;
        }
        return false;
    }


    public static bool FindTriples(PokerHandInfo p) // returns true if any pairs (including more than one) are found and adds into hand info.
    {
        bool anyTrips = false;
        foreach (int key in p.CardTotals.Keys)
        {
            if (p.CardTotals[key].Count == 3 && !p.MyRank["triples"].Contains(p.CardTotals[key]))
            {
                p.MyRank["triples"].Add(p.CardTotals[key]);
                anyTrips = true;
            }
        }
        return anyTrips;
    }

    public static bool FindQuads(PokerHandInfo p) // returns true if any pairs (including more than one) are found and adds into hand info.
    {
        bool any = false;
        foreach (int key in p.CardTotals.Keys)
        {
            if (p.CardTotals[key].Count == 4 && !p.MyRank["quads"].Contains(p.CardTotals[key]))
            {
                p.MyRank["quads"].Add(p.CardTotals[key]);
                any = true;
            }
        }
        return any;
    }


    public static bool FindUnusedHighCards(PokerHandInfo p) // to be written
    {
        return false;
    }

    public static List<CardScriptableObject> FindMaxRankInCards(List<List<CardScriptableObject>> cl) // takes in list of list of cards which returns the highest ranking list of cards with that list
    {
        int maxValuePair = 0;
        List<CardScriptableObject> maxPair = new List<CardScriptableObject>();
        foreach (List<CardScriptableObject> cards in cl)
        {

            if (cards[0].value > maxValuePair)
            {
                maxValuePair = cards[0].value;
                maxPair = cards;
            }
        }
        return maxPair;
    }


    public static List<List<CardScriptableObject>> FindMaxTwoPairInCards(List<List<CardScriptableObject>> cl) // takes in list of list of cards which returns the highest ranking list of cards with that list
    {
        int maxValuePair = 0;
        int nextMax = 0;
        List<CardScriptableObject> tempMax = new List<CardScriptableObject>();
        List<CardScriptableObject> tempNextMax = new List<CardScriptableObject>();

        List<List<CardScriptableObject>> maxPair = new List<List<CardScriptableObject>>();
        foreach (List<CardScriptableObject> cards in cl)
        {

            if (cards[0].value > maxValuePair)
            {
                tempNextMax = tempMax;
                tempMax = cards;
                nextMax = maxValuePair;
                maxValuePair = cards[0].value;
            }
        }
        maxPair.Add(tempMax);
        maxPair.Add(tempNextMax);

        return maxPair;
    }


    public static CardScriptableObject FindMaxRankInHand(List<CardScriptableObject> cl) // takes in list of cards and returns hgihest ranking card
    {
        int maxValue = 0;
        CardScriptableObject maxCard = null;
        foreach (CardScriptableObject cards in cl)
        {

            if (cards.value > maxValue)
            {
                maxValue = cards.value;
                maxCard = cards;
            }
        }
        return maxCard;
    }


    public static bool FindFullHouse(PokerHandInfo p) // returns true if any pairs (including more than one) are found and adds into hand info must be called after triples and pairs.
    {
        bool anyPairs = false;
        if (p.MyRank["pairs"].Count > 0 && p.MyRank["triples"].Count > 0)
        {
            anyPairs = true;
            if (p.MyRank["pairs"].Count > 1)
            {
                p.MyRank["fullHouse"].Add(FindMaxRankInCards(p.MyRank["pairs"]));
            }
            else if (p.MyRank["triples"].Count > 1)
            {
                p.MyRank["fullHouse"].Add(FindMaxRankInCards(p.MyRank["triples"]));
            }
            else
            {
                p.MyRank["fullHouse"].Add(p.MyRank["pairs"][0]);
                p.MyRank["fullHouse"].Add(p.MyRank["triples"][0]);

            }


        }
        return anyPairs;

    }


    public static bool FindFlush(PokerHandInfo p) // need to handle checking for highest card in event of tie
    {
        bool hasFlushy = false;
        foreach (List<CardScriptableObject> c in p.SuitTotals.Values)
        {
            if (c.Count >= 5) // checksfor flush if more than 5 cards are in same suit then returns true
            {
                //List<Card> sc;
                //sc = c.OrderBy(o => o.value).ToList();
                // sc.Reverse();// sorts cards before adding to dict

                //p.MyRank["flush"].Add(sc.GetRange(0,5));
                p.MyRank["flush"].Add(c);

                hasFlushy = true;
            }
        }
        return hasFlushy;
    }

    public static bool FindStraightFlushOrRoyalFlush(Hand h) // not very scalable since it checks based on hand sizes being 5, 6, or 7
    {

        if (h.handInfo.MyRank["flush"].Count > 0)
        {
            foreach (List<CardScriptableObject> lc in h.handInfo.MyRank["flush"])
            {
                List<CardScriptableObject> sc;
                sc = lc.OrderBy(o => o.value).ToList();
                sc.Reverse();
                //need to check  A K 5 4 3 2

                if (sc[0].value == 14 && sc[4].value == 10) // checks for royal flush
                {
                    h.handInfo.MyRank["royalFlush"].Add(sc.GetRange(0, 5));
                    return true;
                }
                else if (sc[0].value == sc[4].value + 4)
                {
                    h.handInfo.MyRank["straightFlush"].Add(sc.GetRange(0, 5));
                    return true;

                }
                else if (sc.Count > 5 && sc[1].value == sc[5].value + 4)
                {
                    h.handInfo.MyRank["straightFlush"].Add(sc.GetRange(1, 5));
                    return true;

                }
                else if (sc.Count > 6 && sc[2].value == sc[6].value + 4)
                {
                    h.handInfo.MyRank["straightFlush"].Add(sc.GetRange(2, 5));
                    return true;

                }
                else if (sc[0].value == 14 && sc[sc.Count - 1].value == sc[sc.Count - 4].value - 3) // checks for A k 5 4 3 2
                {
                    List<CardScriptableObject> c = new List<CardScriptableObject>
                    {
                        sc[0],
                        sc[sc.Count - 1],
                        sc[sc.Count - 2],
                        sc[sc.Count - 3],
                        sc[sc.Count - 4]
                    };

                    h.handInfo.MyRank["straightFlush"].Add(c);
                    return true;

                }

            }
        }
        return false;

    }

    public static bool FindRoyalFlush(Hand h)
    {
        if (h.handInfo.MyRank["straightFlush"].Count > 0)
        {
            if (h.handInfo.MyRank["straightFlush"][0][0].value == 10 && h.handInfo.MyRank["straightFlush"][0][4].value == 14) // checks first and last value of straight to see if its royal
            {
                h.handInfo.MyRank["royalFlush"].Add(h.handInfo.MyRank["straightFlush"][0]);
                return true;
            }
        }
        return false;
    }


    public static bool FindStraight(Hand h)
    {

        List<CardScriptableObject> sc;
        sc = h.cards.OrderBy(o => o.value).ToList();
        sc.Reverse();

        sc = sc.GroupBy(x => x.value).Select(x => x.First()).ToList();
        //DEBUGGING
        //h.PrintCards(sc);
        //if (sc.Count >= 5)
        //{
        //    Debug.Log(sc[0].value);
        //    Debug.Log(sc[4].value);

        //    Debug.Log(sc[sc.Count - 1]);
        //    Debug.Log(sc[sc.Count - 4]);
        //}


        if (sc.Count >= 5 && sc[0].value == sc[4].value + 4) // if count is >=5
        {
            h.handInfo.MyRank["straight"].Add(sc.GetRange(0, 5));
            return true;

        }
        else if (sc.Count > 5 && sc[1].value == sc[5].value + 4) //if count is 6
        {
            h.handInfo.MyRank["straight"].Add(sc.GetRange(1, 5));
            return true;

        }
        else if (sc.Count > 6 && sc[2].value == sc[6].value + 4) // if count is 7
        {
            h.handInfo.MyRank["straight"].Add(sc.GetRange(2, 5));
            return true;

        }
        else if (sc.Count >=5 && sc[0].value == 14 && sc[sc.Count - 1].value == sc[sc.Count - 4].value - 3) // checks for A k 5 4 3 2
        {
            List<CardScriptableObject> c = new List<CardScriptableObject>
            {
                sc[0],
                sc[sc.Count - 1],
                sc[sc.Count - 2],
                sc[sc.Count - 3],
                sc[sc.Count - 4]
            };
            h.handInfo.MyRank["straight"].Add(c);
            return true;

        }
        return false;
    }

   






    public class LogicResults
    {
        public int highCard;




    }
}