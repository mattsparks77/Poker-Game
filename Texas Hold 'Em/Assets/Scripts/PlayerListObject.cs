using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ActionPosition{ LEFT, RIGHT, TOP, BOTTOM}

public class PlayerListObject : MonoBehaviour
{
    public Image panel;
    public Image playerIcon;
    public Image playerCard1;
    public Image playerCard2;

    public Sprite playerSprite;
    public Sprite card1Sprite;
    public Sprite card2Sprite;
    public Sprite cardBack;

    public TextMeshProUGUI username;
    public TextMeshProUGUI chipTotal;
    public TextMeshProUGUI action;

    public GameObject smallBlindChip;
    public GameObject bigBlindChip;

    public PlayerManager player;

    public Transform[] actionPositions;
    public bool isAlternate = false;


    public void Init(PlayerManager _player, string _name, int _chipTotal, bool _isAlternate, Sprite _card1 = null, Sprite _card2 = null, Sprite _playerSprite = null)
    {
        username.text = _name;
        if (_player.id == Client.instance.myId)
        {
            username.color = Color.yellow;
            username.text = "You";
        }
        isAlternate = _isAlternate;
        player = _player;
        //card1Sprite = _card1;
        //card2Sprite = _card2;
        UpdateChipTotal(_chipTotal);
        HideCards();
        if (isAlternate)
        {
            AdjustActionTextLocation();
        }

    }



    public void AdjustActionTextLocation()
    {
        if (player.id == Client.instance.myId)
        {
            //action.gameObject.transform.position = actionPositions[(int)ActionPosition.TOP].transform.position;
        }
        else
        {
            if (1 <= player.tableIndex && player.tableIndex <= 2)
            {
                action.gameObject.transform.position = actionPositions[(int)ActionPosition.RIGHT].transform.position;
            }
            if (3 <= player.tableIndex && player.tableIndex <= 4)
            {
                action.gameObject.transform.position = actionPositions[(int)ActionPosition.BOTTOM].transform.position;
            }
            if (5 <= player.tableIndex && player.tableIndex <= 6)
            {
                action.gameObject.transform.position = actionPositions[(int)ActionPosition.LEFT].transform.position;
            }
            if (0 == player.tableIndex || player.tableIndex == 7)
            {
                action.gameObject.transform.position = actionPositions[(int)ActionPosition.TOP].transform.position;
            }
        }
    }

    public void UpdateCardSprites(string card1, string card2)
    {
       // string card1 = CardDealer.instance.SuitValueToStringConverter(player.cards[0].suitValue, player.cards[0].value);
        //string card2 = CardDealer.instance.SuitValueToStringConverter(player.cards[1].suitValue, player.cards[1].value);
        card1Sprite = CardManager.instance.cardGameObjects[card1].sprite;
        card2Sprite = CardManager.instance.cardGameObjects[card2].sprite;
    }

    public void UpdateCardSprites()
    {
        if (player == null) { return; }


        string card1 = CardDealer.instance.SuitValueToStringConverter(player.cards[0].suitValue, player.cards[0].value);
        string card2 = CardDealer.instance.SuitValueToStringConverter(player.cards[1].suitValue, player.cards[1].value);
        card1Sprite = CardManager.instance.cardGameObjects[card1].sprite;
        card2Sprite = CardManager.instance.cardGameObjects[card2].sprite;
    }

    public void UpdateChipTotal(int chips)
    {
        chipTotal.text = $"${chips.ToString()}";
    }

    public void UpdateChipTotal()
    {
        chipTotal.text = $"${player.chipTotal.ToString()}";
    }


    public void UpdateChipChange(string change, Color color, float delay)
    {
        chipTotal.color = color;
        chipTotal.text = change;
        StartCoroutine(ResetChipTotal(delay));
    }


    public IEnumerator AddResetDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    public void UpdateActionText(string _action)
    {

        action.text = $"{_action}";
    }

    public IEnumerator ResetChipTotal(float delay)
    {
        yield return new WaitForSeconds(delay);
        chipTotal.color = Color.white;
        UpdateChipTotal();

    }

    public IEnumerator ResetAction(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        UpdateActionText("");

    }

    public void RevealCards()
    {
        playerCard1.sprite = card1Sprite;
        playerCard2.sprite = card2Sprite;
    }

    public void HideCards()
    {
        playerCard1.sprite = cardBack;
        playerCard2.sprite = cardBack;
    }

}
