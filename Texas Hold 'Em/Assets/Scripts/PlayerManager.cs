using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public int tableIndex = -1;

    public bool isPlayingHand;

    public bool isFolding;
    public bool isCheckCalling;
    public bool isRaising;
    public int raiseAmount = 0;
    public int amountInPot = 0;
    public int lastBet = 0;
    public bool receivedCards = false;
    public int chipTotal;

    public GameObject UsernameUI;
    public List<CardScriptableObject> cards;
    public MeshRenderer model;
    public PlayerUI playerUI;
    public Animator animator;

    public HandPlaceholder handPlaceHolder;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        cards = new List<CardScriptableObject>();
        handPlaceHolder = GetComponentInChildren<HandPlaceholder>();

    }

    public void SetChips(int _chips)
    {
        chipTotal = _chips;
    }

    public void AddCardToHand(int _suit, int _rank)
    {
        if (cards.Count >= 2)
        {
            return;
        }
        //cards.Add(c);
        CardScriptableObject c = CardDealer.instance.DealCard(_suit, _rank, handPlaceHolder.placeholders[handPlaceHolder.nextEmptySpot]);
        handPlaceHolder.nextEmptySpot += 1;
        cards.Add(c);
        if (cards.Count >= 2)
        {
            receivedCards = true;
            PlayerListManager.players[id].UpdateCardSprites();

        }
        if (handPlaceHolder.nextEmptySpot > 2)
        {
            
            handPlaceHolder.Reset();
        }

    }

    public void MakeBet(int _chips)
    {
        chipTotal -= _chips;
    }

    public void AddChips(int _chips)
    {
        chipTotal += _chips;
    }

    public void SubtractChips(int _chips)
    {
        chipTotal -= _chips;
    }


    public void Reset()
    {
        cards.Clear();
        handPlaceHolder.nextEmptySpot = 0;
        amountInPot = 0;
        raiseAmount = 0;
        lastBet = 0;
        receivedCards = false;
    }

    public void SetAnimator(string anim)
    {
        animator.Play(anim);
    }


}
