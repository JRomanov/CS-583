using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float xOffset, yOffset, zOffset;
    public GameObject Player;

    public void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player1");
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Player.transform.position + new Vector3(xOffset, yOffset, zOffset);
        transform.LookAt(Player.transform.position);
    }
}
