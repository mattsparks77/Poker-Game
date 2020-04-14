using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldGameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public List<Player> players;
    public Seat[] seats;
    public int availableID;
    public int availableSeat;
    public Dictionary<Player, int> SeatDict;
    public Hashtable playerSeatLookup;
    public int maxNumSeats;
    // Start is called before the first frame update
    void Start()
    {
        availableID = 0;
        availableSeat = 0;
        maxNumSeats = 8;
        seats = new Seat[maxNumSeats];
        players = new List<Player>();
        playerSeatLookup = new Hashtable();
        SeatDict = new Dictionary<Player, int>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //public Player CreatePlayer() // probably move to game manager
    //{
    //    //Player p;

    //    //p = Instantiate(playerPrefab).AddComponent<Player>();
    //    //p.hand = p.gameObject.AddComponent<Hand>();

    //    //p.hand.InitStart(p);
        
    //    //int id = GeneratePlayerID();
    //    //p.gameObject.name = "Player " + id;
    //    //p.ID = id;

    //    //return p;
    //}

    //public void RegisterPlayer(Player p)
    //{
    //    p.ID = GeneratePlayerID();
    //}


    //public void AutoAssignToSeat(Player p)
    //{
    //    int i = GenerateNextSeat();
    //    SeatDict[p] = i;
    //    seats[i].RegisterPlayer(p);
    //    playerSeatLookup.Add(p, i);
    //}


    //public int GeneratePlayerID()
    //{
    //    return availableID++;
    //}


    //public int GenerateNextSeat()
    //{
    //    if (availableSeat > maxNumSeats)
    //    {

    //        Debug.Log("Max seats reached..");
    //        return -1;
    //    }
    //    return availableSeat++;
    //}
}
