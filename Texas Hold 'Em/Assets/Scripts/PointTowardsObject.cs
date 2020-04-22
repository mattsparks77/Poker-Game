using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTowardsObject : MonoBehaviour
{

    public Transform target;

    public float damping = 5.0f;

    void Start()
    {
        //pointAt = GameManager.players[Client.instance.myId].gameObject;
        //target = pointAt.transform;
        //target = pointAt.transform;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            //transform.LookAt(target);
            var lookPos = target.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }
    }
}
