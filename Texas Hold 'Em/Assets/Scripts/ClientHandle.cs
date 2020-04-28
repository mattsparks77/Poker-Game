using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

/// <summary>
/// Handles data received from the server
/// </summary>
public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();


        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        // Now that we have the client's id, connect UDP
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
        

    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _prefabId = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _prefabId);
     
    }

    public static void ReceiveCardToCommunity(Packet _packet)
    {
        int suit = _packet.ReadInt();
        int rank = _packet.ReadInt();

        CardDealer.instance.AddCardToCommunity(suit, rank);
    }

    public static void RoundReset(Packet _packet)
    {
        UIManager.instance.nextRoundCountdownObject.gameObject.SetActive(false);
        UIManager.instance.countdownTimer = 5;

        UIManager.instance.isPaused = false;
        CardDealer.instance.DeleteCardsInPlay();
        CommunityCards.nextEmptySpot = 0;
        CardManager.instance.communityCards.Clear();
        GameState.playersAtTable.Clear();
        CommunityCards.instance.ResetCardUI();

        foreach (PlayerManager p in GameManager.players.Values)
        {
            p.Reset();
        }
        foreach (PlayerListObject p in PlayerListManager.players.Values)
        {
            p.ResetAction(1.0f);
        }
        
    }

    public static void ReceiveCard(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int suit = _packet.ReadInt();
        int rank = _packet.ReadInt();

        if (!GameManager.players.ContainsKey(_id))
        {
            return;
        }
        //Debug.Log($"Receiving Card.. ID: {_id}, Suit: {suit}, Rank: {rank}");
        GameManager.players[_id].AddCardToHand(suit, rank);
        
    }

    public static void RoundOver(Packet _packet)
    {
        UIManager.instance.nextRoundCountdownObject.gameObject.SetActive(true);

        //UIManager.instance.updateCountdownUIObject.gameObject.SetActive(true);

        UIManager.instance.StartNextRoundCountdown();

    }


    public static void PlayerPause(Packet _packet)
    {
        bool isPaused = _packet.ReadBool();
        UIManager.instance.isPaused = isPaused;
        if (!isPaused)
        {
            UIManager.instance.pauseButtonText.text = "PAUSE";
            UIManager.instance.StartNextRoundCountdown();
        }
        else
        {
            UIManager.instance.pauseButtonText.text = "PLAY";

        }
        //if (isPaused)
        //{
        //    UIManager.instance.isPaused = false;
        //}
        //else
        //{
        //    UIManager.instance.isPaused = true;
        //}


    }

    /// <summary>
    /// Sets chips for players Add, Subtract, Set
    /// </summary>
    /// <param name="_packet"></param>
    public static void SetChips(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _setAmount = _packet.ReadInt();
        int _subtractAmount = _packet.ReadInt();
        int _addAmount = _packet.ReadInt();
        bool _isBlinds = _packet.ReadBool();
        bool _isWinner = _packet.ReadBool();
        string winningHand = _packet.ReadString();

        if (!GameManager.players.ContainsKey(_id))
        {
            return;
        }
        GameManager.players[_id].AddChips(_addAmount);
        GameManager.players[_id].SubtractChips(_subtractAmount);
        if (_isWinner && _addAmount > 0)
        {
            PlayerListManager.players[_id].UpdateActionText($"WINNER! +${_addAmount} with {winningHand}");
            PlayerListManager.playersAlternate[_id].UpdateActionText($"WINNER! +${_addAmount} with {winningHand}");
            UIManager.instance.ToggleTurnUI(false);
        }
        if (_setAmount != 0)
        {
            GameManager.players[_id].SetChips(_setAmount);
        }
    
        if (_isBlinds)
        {
            GameManager.players[_id].amountInPot = _subtractAmount;
         

        }
        UIManager.instance.SetChipTotalText();
        SliderObject.UpdateMinMaxValues();

    }


    public static void PlayerAction(Packet _packet)
    {
        int _id = _packet.ReadInt();

        bool isFold = _packet.ReadBool();
        bool isCheckCall = _packet.ReadBool();
        bool isRaise = _packet.ReadBool();
        int raiseAmount = _packet.ReadInt();

        GameManager.players[_id].isFolding = isFold;
        GameManager.players[_id].isCheckCalling = isCheckCall;
        GameManager.players[_id].isRaising = isRaise;
        GameManager.players[_id].raiseAmount = raiseAmount;
        if (isFold)
        {
            //UIManager.instance.SetActionText("folded.", _id);
            PlayerListManager.players[_id].UpdateActionText("FOLD");
            PlayerListManager.playersAlternate[_id].UpdateActionText("FOLD");
            //UIManager.instance.playerTurnText.text = GameManager.players[_id].username + " folded.";
            Debug.Log($"[FOLDED] Received player action: Username: { GameManager.players[_id].username}, ID: {_id} , Action: Folded");


        }

        else if (isRaise)
        {
            //UIManager.instance.SetActionText("raised $" + raiseAmount, _id);
            PlayerListManager.players[_id].UpdateActionText("RAISE $" + raiseAmount);
            PlayerListManager.playersAlternate[_id].UpdateActionText("RAISE $" + raiseAmount);

            GameManager.players[_id].SubtractChips(raiseAmount);
            GameManager.players[_id].amountInPot += raiseAmount;
            GameManager.players[_id].lastBet = raiseAmount;
            GameState.instance.currentBet = GameManager.players[_id].amountInPot;
            GameState.instance.highestPlayerAmountInPot = GameManager.players[_id].amountInPot;

            Debug.Log($"[RAISE] Player {GameManager.players[_id].username} has: {GameManager.players[_id].chipTotal}");


        }

        else if (isCheckCall)
        {
            //UIManager.instance.SetActionText("checked.", _id);

            //UIManager.instance.playerTurnText.text = GameManager.players[_id].username + " checked.";
            int amountToCall = 0;
            amountToCall = GameState.instance.highestPlayerAmountInPot - GameManager.players[_id].amountInPot;
            Debug.Log($"[CHECK] Amount to call: {amountToCall}");

            if (amountToCall >= 0)
            {
                GameManager.players[_id].SubtractChips(amountToCall);
                GameManager.players[_id].amountInPot += amountToCall;
                GameState.instance.amountInPot += amountToCall;
                GameManager.players[_id].lastBet = amountToCall;
                PlayerListManager.players[_id].UpdateActionText("CALL");
                PlayerListManager.playersAlternate[_id].UpdateActionText("CALL");

            }
            else
            {
                PlayerListManager.players[_id].UpdateActionText("CHECK");
                PlayerListManager.playersAlternate[_id].UpdateActionText("CHECK");

            }

            Debug.Log($"[CHECK] Received player action: Username: { GameManager.players[_id].username}, ID: {_id} , Action: Check");

        }
        PlayerListManager.players[_id].UpdateChipTotal();

        SliderObject.UpdateMinMaxValues();

    }


    public static void PokerState(Packet _packet)
    {

        GameState.instance.currentBet = _packet.ReadInt();
        GameState.instance.amountInPot = _packet.ReadInt();
        GameState.instance.currentTurnIndex = _packet.ReadInt();
        int currentTurnId = _packet.ReadInt();
        GameState.instance.highestPlayerAmountInPot = _packet.ReadInt();
        bool roundStarted = _packet.ReadBool();
        bool roundOver = _packet.ReadBool();

        int smallBlindIndex = _packet.ReadInt();
        int bigBlindIndex = _packet.ReadInt();
        Debug.Log($"[PokerState] BigBlind Index: {bigBlindIndex}");
        Debug.Log($"[PokerState] SmallBlind Index: {smallBlindIndex}");



        //string winningHand = _packet.ReadString();
        


        //Debug.Log($"Current Bet: {GameState.instance.currentBet}");
        //Debug.Log($"Amount in Pot: {GameState.instance.amountInPot}");
        Debug.Log($"[PokerState] Current Turn Index: {GameState.instance.currentTurnIndex}");

        UIManager.instance.UpdateGameStateUI();
        SliderObject.UpdateMinMaxValues();

        if (roundOver)
        {
            PlayerListManager.RevealCards();
            return;
        }
        if (roundStarted)
        {
            PlayerListManager.CreatePlayerList();
            UIManager.instance.countdownTimer = 5;
            CommunityCards.instance.gameObject.SetActive(true);
            UIManager.instance.playerUI.SetActive(true);
            PlayerListManager.UpdatePlayerListValues();
            PlayerListManager.UpdatePlayerListAlternateUI();
            CommunityCards.instance.communityCardsUIObject.SetActive(true);
            if (smallBlindIndex != -1 &&  bigBlindIndex != -1)//smallBlindIndex != GameState.instance.smallBlindIndex && bigBlindIndex != GameState.instance.bigBlindIndex)
            {

                GameState.instance.smallBlindIndex = smallBlindIndex;

                GameState.instance.bigBlindIndex = bigBlindIndex;
                PlayerListManager.instance.EnableBlindUI(bigBlindIndex, smallBlindIndex);
            }
        }


        UIManager.instance.CheckIsTurn();
        //UIManager.instance.SetPlayerTurnText(currentTurnId);
        //GameManager.instance.SpawnPlayer(_id, _username);
    }

    public static void PlayerTablePosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _tableIndex = _packet.ReadInt();
        // GameState.Initialize();
        GameManager.players[_id].tableIndex = _tableIndex;
        GameManager.tableIndexPlayerLookup[GameManager.players[_id].tableIndex] = GameManager.players[_id];
        GameState.playersAtTable.Add(GameManager.players[_id]);
        //GameManager.players[_id].tableIndex = GameState.playersAtTable.IndexOf(GameManager.players[_id]);

        if (GameState.playersAtTable.IndexOf(GameManager.players[_id]) != _tableIndex)
        {
            Debug.Log($"{GameManager.players[_id].username} has an incorrect table index of {GameState.playersAtTable.IndexOf(GameManager.players[_id])} but should be {_tableIndex}!!");
           
        }
        //if (_tableIndex == -1)
        //{
        //    GameState.seats[GameManager.players[_id].tableIndex].isOccupied = false;
        //    GameState.seats[GameManager.players[_id].tableIndex].DisplayJoinButton(true);
        //    GameManager.players[_id].tableIndex = -1;
        //    GameState.playersAtTable.Remove(GameManager.players[_id]);

        //}
        //else
        //{
        //    //GameState.seats[GameManager.players[_id].tableIndex].isOccupied = true;
        //    //GameState.seats[GameManager.players[_id].tableIndex].DisplayJoinButton(false);
            
        //    GameManager.players[_id].tableIndex = _tableIndex;
        //    GameState.playersAtTable.Add(GameManager.players[_id]);
        //    if (GameState.playersAtTable.IndexOf(GameManager.players[_id]) != _tableIndex)
        //    {
        //        Debug.Log($"{GameManager.players[_id].username} has an incorrect table index of {GameState.playersAtTable.IndexOf(GameManager.players[_id])} but should be {_tableIndex}!!");
        //    }
        //}
        //}
        //if (GameState.playersAtTable[_tableIndex] != null)
        //{
        //    //If clients removes himself from the table then -1 is sent and client is removed from playersInGameList.
        //    if (_tableIndex == -1)
        //    {
        //        GameState.seats[GameManager.players[_id].tableIndex].isOccupied = false;
        //        GameState.seats[GameManager.players[_id].tableIndex].DisplayJoinButton(true);
        //        GameManager.players[_id].tableIndex = -1;
        //        GameState.playersAtTable.Remove(GameManager.players[_id]);

        //    }
        //    else
        //    {
        //        //GameState.seats[GameManager.players[_id].tableIndex].isOccupied = true;
        //        //GameState.seats[GameManager.players[_id].tableIndex].DisplayJoinButton(false);

        //        GameManager.players[_id].tableIndex = _tableIndex;
        //        GameState.playersAtTable[_tableIndex] = GameManager.players[_id];
        //    }
           
        
    }


    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        if (!GameManager.players.ContainsKey(_id)) { return; }
  
        GameManager.players[_id].transform.position = _position;

        if (GameManager.players[_id].transform.position != _position && _id != Client.instance.myId)
        {
            GameManager.players[_id].SetAnimator("walk");
        }


    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();
        if (!GameManager.players.ContainsKey(_id)) { return; }
        GameManager.players[_id].transform.rotation = _rotation;

    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        
        
        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
        PlayerListManager.UpdatePlayerListValues();
        PlayerListManager.UpdatePlayerListAlternateUI();
    }

    //public static void PlayerHealth(Packet _packet)
    //{
    //    int _id = _packet.ReadInt();
    //    float _health = _packet.ReadFloat();

    //    //GameManager.players[_id].SetHealth(_health);
    //}

    public static void PlayerRespawned(Packet _packet)
    {
        int _id = _packet.ReadInt();

       // GameManager.players[_id].Respawn();
    }

//    public static void CreateItemSpawner(Packet _packet)
//    {
//        int _spawnerId = _packet.ReadInt();
//        Vector3 _spawnerPosition = _packet.ReadVector3();
//        bool _hasItem = _packet.ReadBool();

//        GameManager.instance.CreateItemSpawner(_spawnerId, _spawnerPosition, _hasItem);
//    }

//    public static void ItemSpawned(Packet _packet)
//    {
//        int _spawnerId = _packet.ReadInt();

//        GameManager.itemSpawners[_spawnerId].ItemSpawned();
//    }

//    public static void ItemPickedUp(Packet _packet)
//    {
//        int _spawnerId = _packet.ReadInt();
//        int _byPlayer = _packet.ReadInt();

//       // GameManager.itemSpawners[_spawnerId].ItemPickedUp();
//       // GameManager.players[_byPlayer].itemCount++;
//    }
}
