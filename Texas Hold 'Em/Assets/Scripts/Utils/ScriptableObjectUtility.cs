using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;

public static class ScriptableObjectUtility
{
    // allows easy creation of cards and custom cards through an editor script
#if UNITY_EDITOR
    static readonly string path = "Assets/CardObjects/";
    static readonly string prefabPath = "Assets/LittleGamesAssetsPack/Prefabs/Cards";

    public static CardScriptableObject CreateCard(string name, int value, string suit, int suitVal, int count, GameObject prefab)
    {
        CardScriptableObject o = ScriptableObject.CreateInstance<CardScriptableObject>();
        o.InitCardObject(name, value, suit, suitVal, prefab);
        o.path = AssetDatabase.GenerateUniqueAssetPath(path + "/Card_ " + name + ".asset");

        return o;
    }

    public static void GenerateTextFileFromCardAssets()
    {
        string sAssetFolderPath = prefabPath;
        // Construct the system path of the asset folder 
        string sDataPath = Application.dataPath;
        string sFolderPath = sDataPath.Substring(0, sDataPath.Length - 6) + sAssetFolderPath;
        // get the system file paths of all the files in the asset folder
        string[] aFilePaths = Directory.GetFiles(sFolderPath);
        // enumerate through the list of files loading the assets they represent and getting their type

        string writePath = "Assets/Resources/loadCards.txt";
        foreach (string sFilePath in aFilePaths)
        {
            string sAssetPath = sFilePath.Substring(sDataPath.Length - 6);
            Object objAsset = AssetDatabase.LoadAssetAtPath(sAssetPath, typeof(Object));

            if (objAsset != null)
            {

                string cardName = objAsset.name;
                Debug.Log("Adding card to .txt.. " + cardName);

                File.AppendAllText(writePath, cardName + "\n");
            }
        }

     }


       public static void GenerateAllCardScriptableObjects()
        {


            string sAssetFolderPath = prefabPath;
            // Construct the system path of the asset folder 
            string sDataPath = Application.dataPath;
            string sFolderPath = sDataPath.Substring(0, sDataPath.Length - 6) + sAssetFolderPath;
            // get the system file paths of all the files in the asset folder
            string[] aFilePaths = Directory.GetFiles(sFolderPath);
            // enumerate through the list of files loading the assets they represent and getting their type

            foreach (string sFilePath in aFilePaths)
            {
                string sAssetPath = sFilePath.Substring(sDataPath.Length - 6);
                //Debug.Log(sAssetPath);
                Object objAsset = AssetDatabase.LoadAssetAtPath(sAssetPath, typeof(Object));
                int val;
                string suit;
                int suitVal;
                if (objAsset != null)
                {
                    string cardName = objAsset.name;

                    Debug.Log(objAsset.name);
                    //checks if second char is a 0 if it is then its a 10 card edge case
                    if (cardName[1] == '0') // sets val for 10
                    {
                        val = 10;
                    }
                    else if (!char.IsNumber(cardName[0])) //sets val for jack,queen, king, ace
                    {
                        val = ConvertRoyaltyToValue(cardName[0]);

                    }
                    else
                    {
                        val = int.Parse(cardName[0].ToString());
                    }

                    suit = DeriveSuitFromCardString(cardName);
                    suitVal = DeriveSuitValFromCardString(cardName);
                    CreateAsset<CardScriptableObject>(cardName, val, suit, suitVal, 4, (UnityEngine.GameObject)objAsset);
                }



            }

            AssetDatabase.ImportAsset("Assets/Resources/loadCards.txt");
            var asset = Resources.Load("loadCards");


        }

        public static int ConvertRoyaltyToValue(char c)
        {
            if (c == 'J') return 11;
            if (c == 'Q') return 12;
            if (c == 'K') return 13;
            if (c == 'A') return 1;
            return -1;

        }

        public static string DeriveSuitFromCardString(string s)
        {
            int index = s.IndexOf('f'); // finds index of the only f in the sequence 8ofSpades 'f' in 'of'
            char suit = s[index + 1]; //gets first char after f to obtain the suit

            return suit.ToString();
        }

        public static int DeriveSuitValFromCardString(string s)
        {
            int index = s.IndexOf('f'); // finds index of the only f in the sequence 8ofSpades 'f' in 'of'
            char suit = s[index + 1]; //gets first char after f to obtain the suit
      
            if (suit == 'C') { return 0; }
            if (suit == 'D') { return 1; }
            if (suit == 'H') { return 2; }
            if (suit == 'S') { return 3; }
            return -1;

        }

    public static void CreateAsset<Card>(string name, int value, string suit, int suitVal, int count, GameObject prefab) where Card : ScriptableObject
        {
            Debug.Log(Selection.activeObject);
            var asset = CreateCard(prefab.name, value, suit, suitVal, count, prefab);

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/Card_" + prefab.name + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);
            asset.name = prefab.name;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
#endif
}
