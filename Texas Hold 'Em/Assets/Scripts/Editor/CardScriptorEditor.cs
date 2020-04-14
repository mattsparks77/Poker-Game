//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(CardGenerator))]
//public class CardScriptorEditor : Editor
//{

//    public GameObject pprefab;

//    public int val;

//    public string suit;
//    public string cname;


    
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        pprefab = (GameObject)EditorGUILayout.ObjectField(pprefab, typeof(GameObject), true);

//        val = EditorGUILayout.IntField("Card Value:", val);

//        cname = EditorGUILayout.TextField("Card Name:", cname);
//        suit = EditorGUILayout.TextField("Card Suit:", suit);

//        if (GUILayout.Button("Test Deal Card"))
//        {

//        }
//        if (GUILayout.Button("Generate Card"))
//        {
//            CardGenerator.CreateCardSciptObject(cname, val, suit, 0, pprefab);
//        }

//        if (GUILayout.Button("Generate All Cards"))
//        {
//            CardGenerator.GenerateAllCards();
//        }

//        if (GUILayout.Button("Test Create Deck"))
//        {
            
//        }
//    }
//}

