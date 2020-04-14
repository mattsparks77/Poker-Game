using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour
{
    public CardManager cm;
    private void Start()
    {
        cm = GetComponent<CardManager>();
    }
    //public static void TestDealCard()
    //{
    //    Card c = cm.cd.DealCard();
    //}
    public static void CreateCardSciptObject(string name, int value, string suit, int count, GameObject prefab)
    {
        Debug.Log("Card is being created!! WOO!!");
        //ScriptableObjectUtility.CreateAsset<Card>(name, value, suit, count, prefab);
    }


    public static void GenerateAllCards()
    {
        Debug.Log("All Cards are trying to be created!! WOO!!");
        //ScriptableObjectUtility.GenerateAllCardScriptableObjects();
    }
}
