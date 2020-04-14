using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rank
{
    TWO = 2, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN,
    JACK, QUEEN, KING, ACE
}

public enum Suit
{
    CLUB,
    DIAMOND,
    HEART,
    SPADE
}
public class Card
{
    public Rank rank;
    public Suit suit;

    public Card(Suit newSuit, Rank newRank)
    {
        suit = newSuit;
        rank = newRank;
    }

    public Card(int newSuit, int newRank)
    {
        suit = (Suit)newSuit;
        rank = (Rank)newRank;
    }
}
