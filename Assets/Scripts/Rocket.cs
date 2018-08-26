using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip newStartSound;

    
    Rigidbody rigidBody;
    AudioSource audioSource;
   
    enum State
    {
        Alive, Duying, Transcending
    }
    State state;

    
    // Use this for initialization
    void Start () {
        ;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
        
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
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
            case "StartSpot":
                {
            //        StopAnySound();
            //        PlayNewStartSound();
            //        Invoke("StopAnySound", 2f);
            //        //e = State.Transcending;
                    break;
                }
            case "Friendly":
                {
                    
                    break;
                }
            case "Terrain":
                {
                    StartDeathSequence();

                    break;
                }
            case "Finish":
                {
                    StartSuccessSequence();

                    break;
                }
            default:
                StartDeathSequence();
                break;
        }
    }

     void StopAnySound()
    {
        audioSource.Stop();
    }

    private void StartSuccessSequence()
    {
        audioSource.Stop();
        PlayNewStartSound();
        state = State.Transcending;

        Invoke("LoadNewScene", 2f);
    }

    private void StartDeathSequence()
    {
        audioSource.Stop();
        PlayDeathSound();
        state = State.Duying;

        Invoke("LoadPreviousScene", 2f);
    }

    void SceneLoaded()
    {
        //audioSource.Stop();
        audioSource.PlayOneShot(newStartSound);
    }

    void PlayNewStartSound()
    {
        audioSource.PlayOneShot(newStartSound);
    }

    void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathSound);
    }

    private void LoadPreviousScene()
    {
        SceneManager.LoadSceneAsync(0);
        Scene currentScene = SceneManager.GetActiveScene();
        //if (currentScene.isLoaded)
        //{
        //    PlayNewStartSound();
        //}
        
    }

    private void LoadNewScene()
    {
        SceneManager.LoadScene(1);
        //SceneLoaded();
    }
    
    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        //else if ((Input.GetKey(KeyCode.Space) == false) & (audioSource.isPlaying))
        //{
        //    StopAnySound();
        //}
        else audioSource.Stop();
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    private void RespondToRotateInput()
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
