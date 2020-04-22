using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public GameObject foldButton;
    public GameObject raiseButton;
    public GameObject checkCallButton;
    public GameObject raiseAmountTextInput;
    public GameObject allInButton;
    public Button ConfirmButton;

    private GameObject[] playerUI;

    public PlayerManager playerManager;

    public bool isTurn;


    public void Initialize(PlayerManager _pm)
    {
        playerUI = new GameObject[5] { foldButton, raiseButton, checkCallButton, raiseAmountTextInput, allInButton };
        playerManager = _pm;
    }
    
    public void ToggleUI()
    {
        if (isTurn)
        {
            foreach (GameObject g in playerUI)
            {
                g.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject g in playerUI)
            {
                g.gameObject.SetActive(false);
            }
        }
    }

  

}
