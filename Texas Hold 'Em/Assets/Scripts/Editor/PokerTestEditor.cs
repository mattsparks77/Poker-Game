using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PokerTest))]
public class PokerTestEditor : Editor
{
    PokerTest pt;
    CardManager cm;

    private void Awake()
    {
        pt = GameObject.FindGameObjectWithTag("TestHand").GetComponent<PokerTest>();
        cm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CardManager>();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //n = EditorGUILayout.IntField("Initial Hands:", n);

        //if (GUILayout.Button("Test Create Hand"))
        //{
        //   // pt.CreateTestHand();

        //}
        //if (GUILayout.Button("Test Straight Flush"))
        //{
        //    cm.TestStraightFlush();
        //}
        //if (GUILayout.Button("Test Full House"))
        //{
        //    cm.TestFullHouse();
        //}
        //if (GUILayout.Button("Test Straight"))
        //{
        //    cm.TestStraight();
        //}

        //if (GUILayout.Button("Test TwoPairs"))
        //{
        //    cm.TestTwoPairs();
        //}
        //if (GUILayout.Button("Test Royal Flush"))
        //{
        //    cm.CreateRoyalFlush();
        //}

        if (GUILayout.Button("Create x Number of Hands"))
        {
            //pt.CreateHandObjects(pt.HandsToCreate,pt.CardsInHand);
        }
        if (GUILayout.Button("Reset Deck()"))
        {
            cm.TestReshuffle();
        }
    }
}

