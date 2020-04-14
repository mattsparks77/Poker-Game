using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



//[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardScriptableObject : ScriptableObject
{
    private static CardScriptableObject card;
    // 1 hearts, 2 spades, 3 clubs, 4 diamonds.... 1-02 = 2 hearts 202 = 2 spades
    public int value; //
    public int suitValue;
    public string suit;
    public Card cardInstance;
    public GameObject prefab;

    public string path;
	public new string name;

    public CardScriptableObject()
    {
        card = this;
    }

    public void InitCardObject(string n, int value, string suit, int suitVal, GameObject pref)
    {
		card.name = n;
        card.prefab = pref;
       
        card.value = value;
        card.suit = suit;
        card.suitValue = suitVal;

        cardInstance = new Card(suitVal, value);
    }

    public void Print()
    {
        Debug.Log(name + ":" + name);
    }


    public static bool operator >(CardScriptableObject c1, CardScriptableObject c2)
    {
        return c1.value > c2.value;
    }



    public static bool operator <(CardScriptableObject c1, CardScriptableObject c2)
    {
        return c1.value < c2.value;
    }



    public static bool operator <=(CardScriptableObject c1, CardScriptableObject c2)
    {
        return c1.value <= c2.value;
    }


    public static bool operator >=(CardScriptableObject c1, CardScriptableObject c2)
    {
        return c1.value >= c2.value;
    }


    public static bool operator ==(CardScriptableObject c1, CardScriptableObject c2)
    {
        return c1.value == c2.value;
    }


    public static bool operator !=(CardScriptableObject c1, CardScriptableObject c2)
    {
        return c1.value != c2.value;
    }

    public override int GetHashCode()
    {
        
        
        int hash = 17;
        hash = hash * 23 + value;
        return hash;
        
    }

    public override bool Equals(object o)
    {
        if (o == null || GetType() != o.GetType())
        {
            return false;
        }
        var c = (CardScriptableObject)o;
        return value == c.value;
    }

    public void Start()
    {


    }
}

