using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkPlayerUI : MonoBehaviour
{
    public GameObject networkPlayerUI;
    public GameObject chipsUI;
    public TMPro.TextMeshProUGUI usernameText;
    public TMPro.TextMeshProUGUI chipTotalText;
    public TMPro.TextMeshProUGUI betText;
    public PlayerManager player;


    public void Initialize(PlayerManager p)
    {
        player = p;
        usernameText.text = p.username;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        chipTotalText.text = $"${player.chipTotal}";
    }
}
