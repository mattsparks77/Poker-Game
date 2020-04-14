using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Seat : MonoBehaviour
{
    public int positionAtTable; // must be initialized in the client
    public bool isOccupied = false;
    public Button joinButton;

    public void Awake()
    {
        RegisterSeatToGameManager();
        joinButton = GetComponent<Button>();
    }

    public void RegisterSeatToGameManager()
    {
        GameState.seats[positionAtTable] = this;
    }

    public void DisplayJoinButton(bool isDisplaying)
    {
        joinButton.gameObject.SetActive(isDisplaying);
    }
}
