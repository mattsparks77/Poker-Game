using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPosition : MonoBehaviour
{
    int amountToBet { get; set; }
    int raise { get; set; }
    bool fold { get; set; }
    bool check { get; set; }
    bool call { get; set; }
    int currentBet { get; set; }
}
