using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPairTransform : MonoBehaviour
{
    public Transform Card1;
    public Transform Card2;
    public bool seatOccupied = false;
    public bool Card1Occupied = false;
    public bool Card2Occupied = false;
    public int playerID;

    public Transform ReturnOpenCardTransform()
    {
        if (Card1Occupied)
        {
            Card2Occupied = true;
            return Card2;
        }
        else if (Card2Occupied)
        {
            return null;
        }
        Card1Occupied = true;
        return Card1;
        
    }

}
