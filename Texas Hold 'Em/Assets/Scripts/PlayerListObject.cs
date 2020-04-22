using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerListObject : MonoBehaviour
{

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

    public PlayerManager player;


    public void Init(PlayerManager _player, string _name, int _chipTotal, Sprite _card1 = null, Sprite _card2 = null, Sprite _playerSprite = null)
    {
        username.text = _name;
        player = _player;
        //card1Sprite = _card1;
        //card2Sprite = _card2;
        UpdateChipTotal(_chipTotal);
        HideCards();

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
        Debug.Log($"Suit Val 0: {player.cards[0].suitValue}");

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

    public void UpdateActionText(string _action)
    {
        action.text = $"{_action}";
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
