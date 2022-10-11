using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform playerObj;
    public Transform Orientation;
    public float Rotation;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Dir = player.position - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Orientation.forward = Dir.normalized;

        float horizontal = Input.GetAxis("Horizontal");
        float Vertical =  Input.GetAxis("Vertical");

        Vector3 Inputdir = Orientation.forward * Vertical + Orientation.right * horizontal;

        if(Inputdir != Vector3.zero)
        {
            Debug.Log("dsf");
            playerObj.forward = Vector3.Slerp(playerObj.forward, Inputdir.normalized,Time.deltaTime *Rotation);
        }
    }
}
