using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    Rigidbody rigidBody;
    AudioSource audioSource;
   
    enum State
    {
        Alive, Dying, Transcending
    }
    State state;
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
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
    }

    void FixedUpdate()
    {
        
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                {
                    
                    break;
                }
            case "Finish":
                {
                    state = State.Transcending;
                    Invoke("LoadNextScene", 1f);
                    break;
                }
            default:
                print("Dead");
                state = State.Dying;
                rigidBody.Sleep();
                Invoke("LoadPreviousScene", 2f);
                
                break;
        }
    }

    private void LoadPreviousScene()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNewScene()
    {
        SceneManager.LoadScene(1);
    }
    
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
