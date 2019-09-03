using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RocketAsteroid : MonoBehaviour {
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;
    public GameObject newPlayerLaser;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip newStartSound;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip bonucCollectedSound;
    [SerializeField] GameObject Enemy;
    [SerializeField] float Health;

    [SerializeField] ParticleSystem LeftEngineParticles;
    [SerializeField] ParticleSystem RightEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem newStartParticles;

    [SerializeField] Text countText;
    [SerializeField] Text healthText;
    public float moveSpeed = 8f;
    public Joystick joystick;
    [SerializeField] Button button1;
    //Canvas canvas;
    public static bool isGameQuit = false; //Обозначает, что игра должна завершиться
    int score = 0;
    GameObject bonus;
    Rigidbody rigidBody;
    Rigidbody rigidBodyLaser;
    AudioSource audioSource;
    bool isRotating = false;

    bool CollisionAreEnabled = true;
   
    enum State
    {
        Alive, Duying, Transcending
    }
    State state;

    private List<GameObject> spawnedLaserBeams = new List<GameObject>();


    // Use this for initialization
    void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        bonus = GetComponent<GameObject>();
        //canvas = GetComponent<Canvas>();
        healthText.text = Health + "hp";
        Instantiate(Enemy);
        

    }

    private void StartShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject TestClone1 = Instantiate(newPlayerLaser);
            TestClone1.transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z); ;
            rigidBodyLaser = TestClone1.AddComponent<Rigidbody>();
            rigidBodyLaser.useGravity = false;
            rigidBodyLaser.AddRelativeForce(-Vector3.forward * 1500);
            spawnedLaserBeams.Add(TestClone1);
        }
        
    }


    private void DestroyLaser(GameObject laser)
    {
        Destroy(laser);
    }

    // Update is called once per frame
    void Update () {

        if (Health <= 0f && state == State.Alive)
        {
            healthText.text = "Вы умерли";
            healthText.transform.position = new Vector3(Screen.width / 2, Screen.height / 1.5f, 0);
            ReloadSequence();
            
        }


        if (state == State.Alive)
        {
            healthText.text = Health + "hp";
            //RespondToThrustInput();
            //RespondToRotateInput();
            if (Debug.isDebugBuild)
            {
                RespondToCheats();
            }
            ApplyThrust();
            StartShooting();
        }


        if (spawnedLaserBeams.Count > 0)
        {
            int k = 0;
            foreach (GameObject i in spawnedLaserBeams)
            {
                k++;
                if (i.transform.position.y > 80)
                {
                    Destroy(i);
                    spawnedLaserBeams.RemoveAt(k - 1);
                    k--;
                }
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
        if (other.gameObject.CompareTag("Terrain"))
        {
            Health = 0f; 
        }
        else if (other.gameObject.CompareTag("EnemyLaser"))
        {
            Health = Health - 40f;
        }
        else if (other.gameObject.CompareTag("Asteroid1"))
        {
            Health = Health - 10f;
        }
    }

   

    

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || !CollisionAreEnabled)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            
            
            case "Terrain":
                {
                    Health = 0f;
                    break;
                }
            case "Asteroid1":
                {
                    Health = Health - 10f;
                    break;
                }
            
            case "Finish":
                {
                   // StartSuccessSequence();
                    break;
                }
            default:
                //вернутьReloadSequence();
                Health = Health - 10f;
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
        LeftEngineParticles.Stop();
        RightEngineParticles.Stop();
        rigidBody.freezeRotation = true;
        newStartParticles.Play();
        state = State.Transcending;
        if (SceneManager.GetActiveScene().buildIndex == (SceneManager.sceneCountInBuildSettings - 1))
        {
            isGameQuit = true;
            PlayWinSound();

            Invoke("GameQuit",5);
            Invoke("LoadNewScene", 7);
        }
        else
        {
            Debug.Log(SceneManager.GetActiveScene().buildIndex);
            PlayNewStartSound();
            countText.enabled = true;
            countText.text = "Уровень пройден!";
            Invoke("LoadNewScene", levelLoadDelay);

        }
       
    }

    private void GameQuit()
    {
        Application.Quit();
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
        audioSource.PlayOneShot(newStartSound);
    }

    void PlayWinSound()
    {
        audioSource.PlayOneShot(winSound);
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
        rigidBody.Sleep();
        


        state = State.Duying;
        
        Invoke("ReloadScene", levelLoadDelay);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        healthText.text = Health + "hp";
        
    }


    private void ApplyThrust()
    {

        if (Input.GetKey(KeyCode.A))
        {

            rigidBody.AddRelativeForce(Vector3.forward * mainThrust);

        }
        else if (Input.GetKey(KeyCode.D))
        {

            rigidBody.AddRelativeForce(-Vector3.forward * mainThrust);
        }
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        LeftEngineParticles.Play();
        RightEngineParticles.Play();
    }

}
