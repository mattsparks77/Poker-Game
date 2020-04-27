using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Toggle2DMode();
        }
    }

    private void FixedUpdate()
    {
        SendInputToServer();

    }

    public void Toggle2DMode()
    {
        //Cursor.visible = !Cursor.visible;
        UIManager.instance.Toggle2DMode();
    }

    /// <summary>Sends player input to the server.</summary>
    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.Space)
        };

        ClientSend.PlayerMovement(_inputs);
    }


}
