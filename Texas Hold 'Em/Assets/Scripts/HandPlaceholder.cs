using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPlaceholder : MonoBehaviour
{

    public Transform[] placeholders = new Transform[2];
    public int nextEmptySpot = 0;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        placeholders[0] = this.transform.GetChild(0);
        placeholders[1] = this.transform.GetChild(1);
    }

    public void Reset()
    {
        nextEmptySpot = 0;
    }


}
