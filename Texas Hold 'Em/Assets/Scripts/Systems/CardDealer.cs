using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDealer: MonoBehaviour
{
    public static CardDealer instance;
    const int CARDS_IN_HAND = 2;

    public GameObject DeckLocation;
    public List<Transform> CardLocations; 
    public List<GameObject> CardsInPlay;

    public Dictionary<PlayerManager, int> SeatDict;
    public int pid;
    GameObject temp;

    // Start is called before the first frame update
    //void Start()
    //{
    //    DeckLocation = GameObject.FindGameObjectWithTag("Deck");
    //}

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance.InitCardDealer();
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void InitCardDealer()
    {
        SeatDict = new Dictionary<PlayerManager, int>();
        pid = 0;
        temp = new GameObject();
        DeckLocation = GameObject.FindGameObjectWithTag("Deck");
        CardsInPlay = new List<GameObject>();
        

    }


    public int GeneratePlayerID()
    {
        return pid++;
    }


    /// <summary>
    /// Deals a card given value and suit
    /// </summary>
    /// <param name="value"></param>
    /// <param name="suit"></param>
    /// <returns></returns>
    public CardScriptableObject DealCard(int suit, int value, Transform location)
    {
        string s = SuitValueToStringConverter(suit, value);
        CardScriptableObject toDeal = CardManager.instance.cardGameObjects[s];
        GameObject cardPrefab = Instantiate(toDeal.prefab, location);

        CardsInPlay.Add(cardPrefab);

        return toDeal;
    }

    /// <summary>
    /// Deals a card given value and suit
    /// </summary>
    /// <param name="value"></param>
    /// <param name="suit"></param>
    /// <returns></returns>
    public CardScriptableObject DealCard(int suit, int value)
    {
        string s = SuitValueToStringConverter(suit, value);
        Debug.Log($"Suit: {suit} Value: {value} \n KEY: {s}");
        CardScriptableObject toDeal = CardManager.instance.cardGameObjects[s];
        GameObject cardPrefab = Instantiate(toDeal.prefab);

        //GameObject cardPrefab = Instantiate(toDeal.prefab, DeckLocation.transform);
        CardsInPlay.Add(cardPrefab);
        
        return toDeal;
    }

    public void AddCardToCommunity(int suit, int value)
    {
        if (CardManager.instance.communityCards.Count >= 5)
        {
            Debug.Log("Community Cards are full.");
            return;
        }
        string s = SuitValueToStringConverter(suit, value);

        //Debug.Log($"Community:  Suit: {suit} Value: {value} \n KEY: {s}");

        CardScriptableObject toDeal = CardManager.instance.cardGameObjects[s];
        //Adds card into the community cards list
        CardManager.instance.communityCards.Add(toDeal.cardInstance);

        Transform spot = CommunityCards.placeholders[CommunityCards.nextEmptySpot];
        GameObject cardPrefab = Instantiate(toDeal.prefab, spot);
        CardsInPlay.Add(cardPrefab);
        CommunityCards.nextEmptySpot++;
    }

    public string SuitValueToStringConverter(int suit, int value)
    {
        return suit.ToString() + value.ToString();
    }

    public string CardToStringConverter(Card c)
    {
        return c.suit.ToString() + c.rank.ToString();
    }


    //public Transform FindCardTransformAtSeat(Player p, int seat)
    //{
    
    //    CardPairTransform cpt = DeckLocation.transform.GetChild(seat).GetComponent<CardPairTransform>();
    //    if (cpt.seatOccupied == true && cpt.playerID != p.id)
    //    {
    //        return null;
    //    }
    //    return cpt.ReturnOpenCardTransform();

    
    //}

    //public void SpawnCardAtTable(Player p, CardScriptableObject c, int spot)
    //{
    //    Transform t = FindCardTransformAtSeat(p, spot);
    //    GameObject inPlayCard = Instantiate(c.prefab, t);
    //    CardsInPlay.Add(inPlayCard);

    //}

    //public void DealSpecificCard(Player p, CardScriptableObject card)
    //{
    //    //Find open spot at table and return another seat if it's occupied
    //    //SpawnCardAtTable(p, c, )
    //    //GameObject cardPrefab = Instantiate(card.prefab, p.hand.transform);
    //    //CardsInPlay.Add(cardPrefab);
    //    //p.hand.AddCard(card);
   
        
    //}

    public void DealSpecificCard(Hand p, CardScriptableObject card)
    {
        //Find open spot at table and return another seat if it's occupied
        //SpawnCardAtTable(p, c, )
        GameObject cardPrefab = Instantiate(card.prefab, p.transform);
        CardsInPlay.Add(cardPrefab);
        p.AddCard(card);


    }

    public Hand CreateNewHandObject()
    {
        Hand h = new Hand();

        h = Instantiate(temp, DeckLocation.transform).AddComponent<Hand>();
        h.InitStart();
        h.gameObject.name = "Player " + GeneratePlayerID();
        return h;
    }

    public Hand DealHand(int numCards)
    {
        Hand h = CreateNewHandObject();

   
        for (int i = 0; i < numCards; i++)
        {
            CardScriptableObject toDeal = CardManager.instance.deck.Dequeue();
            GameObject cardPrefab = Instantiate(toDeal.prefab, h.transform);
            CardsInPlay.Add(cardPrefab);
            h.cards.Add(toDeal); // adds in 7 cards to hand
        }
        
        h.AddHandtoPokerHandInfo();
           
        
        return h;

    }


    public void DeleteCardsInPlay()
    {
        foreach (GameObject o in CardsInPlay)
        {
            Destroy(o); // maybe change later if want to use object pool
        }
        CardsInPlay.Clear();
    }


    public void ReShuffleCards()
    {
        DeleteCardsInPlay();
        CardManager.instance.deck.Clear(); // clears current queue
        CardManager.instance.CreateDeck(); // creates new one from list of cardobjects
    }


    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
