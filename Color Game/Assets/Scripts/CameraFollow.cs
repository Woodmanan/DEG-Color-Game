using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    //Player is moved with physics, so we need to use the physics update to move the camera - avoids jitters
    private void FixedUpdate()
    {
        Vector3 newPos = Vector3.Lerp(transform.position, player.transform.position, .025f);
        newPos.z = transform.position.z;
        transform.position = newPos;
    }
}
