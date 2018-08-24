using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    // Use this for initialization
    void Start () {
        // rigidBody = GetComponent<Rigidbody>();
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        
    }
	
	// Update is called once per frame
	void Update () {
        Thrust();
        Rotate();
        
    }

    void FixedUpdate()
    {
        
        
    }

   /* void OnCollisionEnter(Collision collision)
    {
        print("Collided");
    }*/

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //rigidBody.AddRelativeForce(Vector2.up);
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying) audioSource.Play();
        }
        else audioSource.Stop();
    }

     private void Rotate()
    {
        rigidBody.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime;


        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotationThisFrame);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;
    }
}
