using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public InputField usernameField;
    public TMP_InputField _raiseAmount;
    public GameObject foldButton;
    public GameObject raiseButton;
    public GameObject checkCallButton;
    public GameObject raiseAmountTextInput;
    public GameObject allInButton;
    public TextMeshProUGUI chipTotal;
    public TextMeshProUGUI playerTurnText;

    public TextMeshProUGUI amountInPot;
    public TextMeshProUGUI currentBet;
    public TextMeshProUGUI currentTurnIndex;

    public GameObject playerUI;

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


    public void UpdateGameStateUI()
    {

        int amountToCall = GetAmountNeededToCall();

        amountInPot.text = $"Total Pot Size: ${GameState.instance.amountInPot} \n Your amount in pot: ${GameManager.players[Client.instance.myId].amountInPot}";

        currentBet.text = $"Current Table Bet: ${GameState.instance.currentBet} \nAmount needed to call: ${amountToCall}" ;

        currentTurnIndex.text = "Current Turn Index: " + GameState.instance.currentTurnIndex + "\nYour bet: " + GameManager.players[Client.instance.myId].raiseAmount;
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
        int toRaise = int.Parse(_raiseAmount.text);
        if (toRaise  > GameManager.players[Client.instance.myId].chipTotal || toRaise < 0)
        {
            _raiseAmount.text = "0";
        }
        
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
        //GameManager.players[Client.instance.myId].chipTotal -= GameState.instance.currentBet;
        if (GameManager.players[Client.instance.myId].chipTotal < GameState.instance.currentBet - GameManager.players[Client.instance.myId].amountInPot)
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
        int toRaise = int.Parse(_raiseAmount.text);
        int amountToNeededToCall = GetAmountNeededToCall();
        if (toRaise > GameManager.players[Client.instance.myId].chipTotal || toRaise < amountToNeededToCall)
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
        instance.chipTotal.text = "$" + GameManager.players[Client.instance.myId].chipTotal.ToString();
    }


    public void SetActionText(string _action, int _id)
    {
        string action = GameManager.players[_id].username + " has " + _action;
        instance.playerTurnText.text = action;
        StartCoroutine(ResetTextToTurnWithDelay(3.5f, _id));
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
        usernameField.interactable = false;
        Client.instance.ConnectToServer();
        //SetChipTotalText();
    }

   
}
