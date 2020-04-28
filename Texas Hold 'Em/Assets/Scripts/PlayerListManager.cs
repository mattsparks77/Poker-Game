using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListManager : MonoBehaviour
{
    public Transform[] alternateTransforms;

    public Color originalPanelColor;
    public static PlayerListManager instance;
    public ScrollRect scrollView;
    public GameObject prefab;
    public GameObject prefabAlternate;

    public GameObject alternateUIContainer;
    public GameObject listUIContainer;

    public static Dictionary<int, PlayerListObject> players = new Dictionary<int, PlayerListObject>(); // players client id : list element
    public static Dictionary<int, PlayerListObject> playersAlternate = new Dictionary<int, PlayerListObject>(); // players client id : list element
    public static Dictionary<string, Sprite> customPlayerIcons = new Dictionary<string, Sprite>(); // dictionary of custom player icons where username is the key.

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    /// <summary>
    /// Disables blind ui for both alternate and normal UI.
    /// </summary>
    public void DisableAllBlindUI()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players.ContainsKey(i))
            {
                players[i].bigBlindChip.SetActive(false);
                players[i].smallBlindChip.SetActive(false);
            }

            if (playersAlternate.ContainsKey(i))
            {
                playersAlternate[i].bigBlindChip.SetActive(false);
                playersAlternate[i].smallBlindChip.SetActive(false);
            }

        }
    }

    public void EnableBlindUI(int bigBlindIndex, int smallBlindIndex)
    {
        DisableAllBlindUI();

        players[GameManager.tableIndexPlayerLookup[bigBlindIndex].id].bigBlindChip.SetActive(true);
        playersAlternate[GameManager.tableIndexPlayerLookup[bigBlindIndex].id].bigBlindChip.SetActive(true);

        players[GameManager.tableIndexPlayerLookup[smallBlindIndex].id].smallBlindChip.SetActive(true);
        playersAlternate[GameManager.tableIndexPlayerLookup[smallBlindIndex].id].smallBlindChip.SetActive(true);


    }

    public static void CreatePlayerList()
    {
   
        //Vector3 offset = new Vector3(-45f, -690f, 0);
        Vector3 offset2 = new Vector3(0f, 0f, 0); // 0,5,0
        Vector3 horizontalOffset = new Vector3(0f, 0f, 0); // 20.0.0
        int i = 0;
        foreach (PlayerManager p in GameManager.players.Values)
        {
           
            if (!players.ContainsKey(p.id))
            {
                PlayerListObject _playerListObject;
                PlayerListObject _playerListObjectAlternate;

                //string card1 = CardDealer.instance.SuitValueToStringConverter(p.cards[0].suitValue, p.cards[0].value);
                //string card2 = CardDealer.instance.SuitValueToStringConverter(p.cards[1].suitValue, p.cards[1].value);
                _playerListObject = Instantiate(instance.prefab, instance.scrollView.content.transform).GetComponent<PlayerListObject>();
                _playerListObject.gameObject.transform.SetParent(instance.scrollView.content.transform, false);
                //_playerListObject.transform.position = _playerListObject.transform.position - offset;
                //_playerListObject.transform.position = _playerListObject.transform.position + (offset2 * i);
                //_playerListObject.transform.position = _playerListObject.transform.position + horizontalOffset;
                _playerListObject.Init(p, p.username, p.chipTotal, false);
                
                _playerListObjectAlternate = Instantiate(instance.prefabAlternate, instance.alternateTransforms[p.tableIndex]).GetComponent<PlayerListObject>();
                _playerListObjectAlternate.gameObject.transform.SetParent(instance.alternateTransforms[p.tableIndex], false);
                _playerListObjectAlternate.Init(p, p.username, p.chipTotal, true);



                //PlayerListObject _player = new PlayerListObject(p.username, p.chipTotal.ToString(), CardManager.instance.cardGameObjects[card1].sprite, CardManager.instance.cardGameObjects[card2].sprite);

                if (customPlayerIcons.ContainsKey(p.username))
                {
                    _playerListObject.playerIcon.sprite = customPlayerIcons[p.username];
                    _playerListObjectAlternate.playerIcon.sprite = customPlayerIcons[p.username];
                }
                players.Add(p.id, _playerListObject);
                playersAlternate.Add(p.id, _playerListObjectAlternate);
            }
            
            i++;
        }
    }

    public static void UpdatePlayerListAlternateUI()
    {
        foreach (PlayerListObject p in playersAlternate.Values)
        {
            // checks for if client id is still in game if not then it deletes the ui
            if (!GameManager.players.ContainsKey(p.player.id))
            {
                playersAlternate.Remove(p.player.id);
                Destroy(p);
            }
            else
            {
                p.UpdateCardSprites();
                p.HideCards();
                p.UpdateChipTotal(p.player.chipTotal);
            }
            if (p.player.id == Client.instance.myId)
            {
                p.RevealCards();
            }

        }
    }


    public static void UpdatePlayerListValues()
    {
        foreach (PlayerListObject p in players.Values)
        {
            // checks for if client id is still in game if not then it deletes the ui
            if (!GameManager.players.ContainsKey(p.player.id))
            {
                players.Remove(p.player.id);
                Destroy(p);
            }
            else
            {
                p.UpdateCardSprites();
                p.HideCards();
                p.UpdateChipTotal(p.player.chipTotal);
            }
            if (GameState.instance.currentTurnIndex == p.player.tableIndex)
            {
                p.panel.color = Color.green;
            }
            else
            {
                p.panel.color = instance.originalPanelColor;
            }
           
            

        }
    }


    public static void RevealCards()
    {
        foreach (PlayerManager p in GameState.playersAtTable)
        {
            if (!p.isFolding)
            {
                players[p.id].RevealCards();
                playersAlternate[p.id].RevealCards();

            }
        }
    }


}
