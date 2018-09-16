using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Rocket : MonoBehaviour {
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip newStartSound;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem newStartParticles;

    [SerializeField] Text countText;
    public float moveSpeed = 8f;
    public Joystick joystick;
    [SerializeField] Button button1;
    //Canvas canvas;
    int score = 0;
    GameObject bonus;
    Rigidbody rigidBody;
    AudioSource audioSource;
    bool isRotating = false;

    bool CollisionAreEnabled = true;
   
    enum State
    {
        Alive, Duying, Transcending
    }
    State state;

    
    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        bonus = GetComponent<GameObject>();
        //canvas = GetComponent<Canvas>();
    }
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
            if (Debug.isDebugBuild)
            {
                RespondToCheats();
            }
        }
    }

    private void RespondToCheats()
    {
        if (Input.GetKeyDown(KeyCode.L)) LoadNewScene();
        else if (Input.GetKeyDown(KeyCode.C) & (CollisionAreEnabled == true)) CollisionAreEnabled = false;
        else if (Input.GetKeyDown(KeyCode.C) & (CollisionAreEnabled == false)) CollisionAreEnabled = true;
    }

    void FixedUpdate()
    {
        
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bonus"))
        {
            
            Destroy(other.gameObject);
            //if (other.gameObject == null)    ToDo больше счета чем нужно
            //{
            //    score++;
            //}
            score++;
            countText.enabled = true;
            countText.text = "Счет: " + score;
            Invoke("SetCountText", 2);
        }
    }

    void SetCountText()
    {
        
        countText.enabled = false;
    }

    //Обработка столкновений
    void OnCollisionEnter(Collision collision)
    {

        if (state != State.Alive || !CollisionAreEnabled)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Bonus":
                {

                    //collision.gameObject.SetActive(false);


                    break;
                }
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
                    ReloadSequence();
                    break;
                }
            case "Finish":
                {

                    //if (SceneManager.GetActiveScene().buildIndex == 1)
                    //{
                    //    GameObject canvas = GameObject.Find("Canvas");
                    //    canvas.SetActive(true);
                    //    LoadPreviousScene();
                    //}

                    StartSuccessSequence();


                    break;
                }
            default:
                ReloadSequence();
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
        mainEngineParticles.Stop();
        PlayNewStartSound();
        rigidBody.freezeRotation = true;
        newStartParticles.Play();
        state = State.Transcending;
        countText.enabled = true;
        countText.text = "Уровень пройден!";
        Invoke("LoadNewScene", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        audioSource.Stop();
        PlayDeathSound();
        deathParticles.Play();
        UnityEngine.Object rocketBody = GameObject.Find("RocketAppearance");
        UnityEngine.Object rocket = GameObject.Find("Rocket");
        GameObject.Destroy(rocketBody);
        rigidBody.Sleep();

        
        state = State.Duying;

        Invoke("LoadPreviousScene", levelLoadDelay);
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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalNumberOfScenes = SceneManager.sceneCountInBuildSettings - 1;
        if (currentSceneIndex == totalNumberOfScenes)
        {
            SceneManager.LoadScene(0);
            return;
        }
        SceneManager.LoadScene(currentSceneIndex + 1);
        //SceneManager.LoadScene(1);
        //SceneLoaded();
    }

    private void ReloadSequence()
    {
        audioSource.Stop();
        PlayDeathSound();
        deathParticles.Play();
        UnityEngine.Object rocketBody = GameObject.Find("RocketAppearance");
        UnityEngine.Object rocket = GameObject.Find("Rocket");
        MeshRenderer.Destroy(rocketBody);
        //GameObject.Destroy(rocketBody);
        rigidBody.Sleep();


        state = State.Duying;

        Invoke("ReloadScene", levelLoadDelay);

        //SceneLoaded();
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    

    private void RespondToThrustInput()
    {
        if ((Input.touchCount == 1) && (isRotating == true))
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
            return;
        }


        if (Input.GetKey(KeyCode.Space) || (Input.touchCount > 0))
        {
            ApplyThrust();
        }
        //else if ((Input.GetKey(KeyCode.Space) == false) & (audioSource.isPlaying))
        //{
        //    StopAnySound();
        //}
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        

        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
    {
        
        Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.forward * joystick.Vertical);
        Debug.Log(moveVector);

        Vector3 joystickDirection = new Vector3(0,0, joystick.Direction.x);

        rigidBody.angularVelocity = Vector3.zero;
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (moveVector.z > 0)
        {
            isRotating = true;
            transform.rotation = Quaternion.Euler(0, 0, -joystick.Direction.x * rcsThrust);
            
        }
        if (moveVector.z < 0)
        {

            isRotating = true;
            transform.rotation = Quaternion.Euler(0, 0, 180 + joystick.Direction.x * rcsThrust);
        }


            ///Разкоментировать
            //        if (moveVector.x > 0.05)
            //{
            //    isRotating = true;
            //    transform.Rotate(-Vector3.forward * rotationThisFrame * 0.5f);
            //}
            //else if (moveVector.x < -0.05)
            //{
            //    isRotating = true;
            //    transform.Rotate(Vector3.forward * rotationThisFrame * 0.5f);
            //}
            //else
            //{
            //    isRotating = false;
            //}
            /////



            if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotationThisFrame);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        
    }
}
