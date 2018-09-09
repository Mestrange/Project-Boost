using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject player;
    private Vector3 offsetY;
    private Vector3 offsetX;
    private Vector3 offset;
    private Vector3 currentPosition;
    
    // Use this for initialization
    void Start ()
    {
        //transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        offset = transform.position - player.transform.position;
        
    }

    // Update is called once per frame
    void Update () {
        if (player == null)
        {
            transform.position = currentPosition;
            return;
        }
        transform.position = player.transform.position + offset;
        currentPosition = transform.position;
        
         
        /*  transform.up = transform.up + offsetY;
          transform.right = transform.right + offsetX;*/
        // CameraMovement();
    }

    private void CameraMovement()
    {
       
        
    }

}
