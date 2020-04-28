﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject secondPlayerUI;
    public GameObject backgroundPanel;

    public GameObject startMenu;
    public InputField usernameField;
    public Slider _raiseAmount;
    public GameObject foldButton;
    public GameObject raiseButton;
    public GameObject checkCallButton;
    public TextMeshProUGUI pauseButtonText;

    public GameObject nextRoundCountdownObject;
    public TextMeshProUGUI checkCallButtonText;
    public GameObject raiseAmountTextInput;
    public GameObject allInButton;
    public TextMeshProUGUI chipTotal;
    public TextMeshProUGUI playerTurnText;
    public TextMeshProUGUI totalPotText;
    public TextMeshProUGUI updateCountdownUIObject;


    public int _raiseAmnt { get { return int.Parse(raiseAmountTextInput.GetComponentInChildren<TextMeshProUGUI>().text); } set { _raiseAmnt = int.Parse(raiseAmountTextInput.GetComponentInChildren<TextMeshProUGUI>().text); } }


    public TextMeshProUGUI amountInPot;
    public TextMeshProUGUI currentBet;
    public TextMeshProUGUI currentTurnIndex;

    public GameObject playerUI;
    public int prefabId = 0;

    public bool isPaused;
    public int countdownTimer = 5;

    public bool toggle2D = false;

    public PlayerManager playerManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object.");
            Destroy(this);
        }
    }


    public void TogglePause()
    {
        isPaused = !isPaused;
        ClientSend.PlayerPause();
        if (isPaused)
        {
            pauseButtonText.text = "PLAY";
        }

    }


    #region Alternate UI
    /// <summary>
    /// Toggles alternate ui, may need to add disabling of other UI and rearranging
    /// </summary>
    public void Toggle2DMode()
    {
        toggle2D = !toggle2D;
        //if (toggle2D)
        //{
        //    Cursor.visible = true;
        //}
        //else
        //{
        //    Cursor.visible = false;
        //}
        //secondPlayerUI.SetActive(toggle2D);
        if (toggle2D)
        {
            PlayerListManager.instance.listUIContainer.SetActive(false);

            PlayerListManager.instance.alternateUIContainer.SetActive(true);
            CommunityCards.instance.communityCardsUIObject.transform.position = CommunityCards.instance.communityCardsAlternateUIObject.transform.position;
            //CommunityCards.instance.communityCardsUIObject.transform.SetParent(CommunityCards.instance.communityCardsAlternateUIObject.transform, false);

        }
        else
        {
            PlayerListManager.instance.alternateUIContainer.SetActive(false);
            PlayerListManager.instance.listUIContainer.SetActive(true);

            CommunityCards.instance.communityCardsUIObject.transform.position = CommunityCards.instance.originalPosition.transform.position;
            //CommunityCards.instance.communityCardsUIObject.transform.SetParent(CommunityCards.instance.originalPosition.transform, false);

        }

        backgroundPanel.SetActive(toggle2D);

    }

    internal void UpdateCountdownUI(bool isPaused)
    {
        if (isPaused)
        {
            updateCountdownUIObject.text = $"PAUSED";
        }
        else
        {
            updateCountdownUIObject.text = $"Next round in.. {countdownTimer}";

        }
    }

    internal IEnumerator RoundCountdown()
    {
        while (!isPaused && countdownTimer >= -1)
        {
            UpdateCountdownUI(false);
            yield return new WaitForSeconds(1f);
            countdownTimer--;
        }
        if (isPaused)
        {
            UpdateCountdownUI(true);

        }

    }

    internal void StartNextRoundCountdown()
    {
        StartCoroutine(RoundCountdown());
    }

    #endregion
    public void UpdateGameStateUI()
    {

        int amountToCall = GetAmountNeededToCall();
        if (amountToCall > 0)
        {
            checkCallButtonText.text = "Call $" + amountToCall;
        }
        else
        {
            checkCallButtonText.text = "Check";

        }
        totalPotText.text = $"Total Pot: ${GameState.instance.amountInPot}";
        amountInPot.text = $"Total Pot: ${GameState.instance.amountInPot} \nChips You Bet: ${GameManager.players[Client.instance.myId].amountInPot} \nYour Last Bet: ${GameManager.players[Client.instance.myId]?.lastBet}";

        currentBet.text = $"Current Bet: ${GameState.instance.currentBet} \nAmount to Call: ${amountToCall}" ;

        //string name = "Your";
        //if (!IsTurn())
        //{
        //    name = GameState.playersAtTable[GameState.instance.currentTurnIndex]?.username + "'s";
        //}
        //currentTurnIndex.text = name  + " Turn at Position " + GameState.instance.currentTurnIndex+ "! ";
    }

   

    public void FixedUpdate()
    {

        // needs fixing
        //if (GameState.gameStarted && GameState.currentTurnIndex == GameManager.players[Client.instance.myId].tableIndex)
        //{
        //    ToggleTurnUI(true);
        //}
        //else { ToggleTurnUI(false); }
    }

    public void SendStartRoundToServer()
    {
        playerUI.SetActive(true);
        GameState.instance.gameStarted = true;
        ClientSend.SendStartRound();
        //PlayerListManager.CreatePlayerList();
    }

    public void SendFoldToServer()
    {
        GameManager.players[Client.instance.myId].isPlayingHand = false;
        playerUI.SetActive(false);
        //GameState.playersAtTable.Remove(GameManager.players[Client.instance.myId]);
        ClientSend.PlayerAction(Client.instance.myId, isFolding: true);
    }

    public void SendSeatToServer(Seat _seat)
    {
        if (_seat.isOccupied)
        {
            Debug.Log("Error seat is occupied cannot sit down here.");
            return;
        }
        GameManager.players[Client.instance.myId].tableIndex = _seat.positionAtTable;
        ClientSend.PlayerTablePosition(Client.instance.myId, _seat.positionAtTable);
        _seat.DisplayJoinButton(false);
        _seat.isOccupied = true;
    }

    public void AssertRaiseAmount()
    {
        int toRaise = (int)_raiseAmount.value;
        //if (toRaise  > GameManager.players[Client.instance.myId].chipTotal || toRaise < 0)
        //{
        //    _raiseAmount.text = "0";
        //}
        
    }

    /// <summary>
    /// Add in button for client to push to reset table or start new round.
    /// </summary>
    public void SendResetTable()
    {

    }
    //needs to fix subtracting chips for calling a bet
    public void SendCheckCallToServer()
    {
        int callAmount = GameState.instance.currentBet - GameManager.players[Client.instance.myId].amountInPot;
        //GameManager.players[Client.instance.myId].chipTotal -= GameState.instance.currentBet;
        if (GameManager.players[Client.instance.myId].chipTotal < callAmount && GameManager.players[Client.instance.myId].chipTotal - callAmount > 0)
        {
            Debug.Log($"Error placing bet of not enough funds or too low of bet.");
            return;
        }
        ClientSend.PlayerAction(Client.instance.myId, isCheckCalling: true);
        SetChipTotalText();
    }


    public int GetAmountNeededToCall()
    {
        if (GameState.instance.currentBet == 0)
        {
            return 0;
        }
        return GameState.instance.highestPlayerAmountInPot - GameManager.players[Client.instance.myId].amountInPot;
    }
    // useful for accessing current players data ... GameManager.players[Client.instance.myId]
    public void SendRaiseAmountToServer()
    {
        int toRaise = (int)_raiseAmount.value;
       
        int amountToNeededToCall = GetAmountNeededToCall();
        // checks if bet is higher than money the player has or if it is less than the amount needed to call but also check if the player is all in so if they are they are allowed to bet.
        if ((toRaise > GameManager.players[Client.instance.myId].chipTotal || toRaise < amountToNeededToCall) && GameManager.players[Client.instance.myId].chipTotal - toRaise > 0) 
        {
            Debug.Log($"Error placing bet of ${toRaise} not enough funds or too low of bet.");
            return;
        }
        if (toRaise == amountToNeededToCall)
        {
            ClientSend.PlayerAction(Client.instance.myId, isCheckCalling: true);

        }
        else
        {
            ClientSend.PlayerAction(Client.instance.myId, isRaising: true, raiseAmount: toRaise);
        }
       
    }

    public void SendAllInToServer()
    {
        int toRaise = GameManager.players[Client.instance.myId].chipTotal;
        ClientSend.PlayerAction(Client.instance.myId, isRaising: true, raiseAmount: toRaise);
       

    }

    public void CheckIsTurn()
    {
        SetChipTotalText();
        if (GameState.instance.currentTurnIndex == GameManager.players[Client.instance.myId].tableIndex)
        {
            ToggleTurnUI(true);
        }
        else
        {
            ToggleTurnUI(false);
        }
    }

    public bool IsTurn()
    {
        return GameState.instance.currentTurnIndex == GameManager.players[Client.instance.myId].tableIndex;
    }


    public void ToggleTurnUI(bool _set)
    {
        playerUI.SetActive(_set);
    }
    

    public void SetChipTotalText()
    {
        if (!chipTotal) { return; }
        Debug.Log($"[SETCHIPTOTALTEXT] ${GameManager.players[Client.instance.myId].chipTotal.ToString()}");
        instance.chipTotal.text = "$" + GameManager.players[Client.instance.myId].chipTotal.ToString();
    }


    public void SetActionText(string _action, int _id)
    {
        string action = GameManager.players[_id].username + " has " + _action;
        instance.playerTurnText.text = action;
        //StartCoroutine(ResetTextToTurnWithDelay(3.5f, _id));
    }


    public IEnumerator ResetTextToTurnWithDelay(float delay, int _id)
    {
        yield return new WaitForSeconds(delay);
        SetPlayerTurnText(_id);
    }


    public void SetPlayerTurnText(int _id)
    {
        string name = "";
        if (IsTurn())
        {
            name = "Your turn!!";
        }
        else
        {
            name = GameManager.players[_id]?.username + "'s turn!";
        }


        instance.playerTurnText.text = name;
    }


    public void ConnectToServer()
    {

        startMenu.SetActive(false);
        //playerUI.SetActive(true);
        usernameField.interactable = false;
        Client.instance.ConnectToServer();
        //SetChipTotalText();
    }

   
}
