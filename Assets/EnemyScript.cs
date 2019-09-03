using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    public GameObject newEnemyLaser;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] float Health;
    AudioSource audioSource;

    [SerializeField] ParticleSystem LeftEngineParticles;
    [SerializeField] ParticleSystem RightEngineParticles;
    [SerializeField] ParticleSystem deathParticles;


    [SerializeField] Vector3 movementVector;
    [SerializeField] float period;
    [Range(0, 1)] [SerializeField] float movementFactor;
    Vector3 startingPos;
    private List<GameObject> spawnedEnemyLasersList = new List<GameObject>();
    Rigidbody rigidBodyLaser;
    Rigidbody rigidBodyEnemy;

    enum State
    {
        Alive, Duying, Transcending
    }
    State state;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("GenerateEnemy", 1f);
        Invoke("GetStartPosition", 1f);
       
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.fixedTime > 4f)
        {
            StartMoving();
        }

        if ((Time.fixedTime % 1 == 0 && state == State.Alive) && (Time.fixedTime > 4f))
        {
            StartShooting();
        }

        if (spawnedEnemyLasersList.Count > 1)
        {
            Destroy(spawnedEnemyLasersList[0].gameObject);
            spawnedEnemyLasersList.RemoveAt(0);
            Debug.Log("Destroyed");
            Debug.Log(spawnedEnemyLasersList.Count);
        }


    }
    private void GenerateEnemy()
    {
        
        state = State.Alive;
        
        

        rigidBodyEnemy = GetComponent<Rigidbody>();
        rigidBodyEnemy.useGravity = true;
        //transform.position = new Vector3(-64.39f, 38.97f, -0.299947f);
    }

    private void GetStartPosition()
    {
        rigidBodyEnemy.useGravity = false;
        rigidBodyEnemy.constraints = RigidbodyConstraints.FreezePositionY;
        var restartPosition = transform.position;
        startingPos = new Vector3(-62.2f, 42.5f, -0.299947f);
    }


    private void StartMoving()
    {
       
        if (period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);
        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }


    private void StartShooting()
    {
            GameObject EnemyLaserClone = Instantiate(newEnemyLaser);
            EnemyLaserClone.transform.position = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z); ;
            rigidBodyLaser = EnemyLaserClone.AddComponent<Rigidbody>();
            rigidBodyLaser.useGravity = false;
            rigidBodyLaser.AddRelativeForce(Vector3.forward * 1500);
            spawnedEnemyLasersList.Add(EnemyLaserClone);
        

    }


    private void DestroyLaser(GameObject laser)
    {
        Destroy(laser);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "PlayerLaser")
        {
            state = State.Duying;
            Object rocketBody = GameObject.Find("EnemyAppearance");
            Destroy(rocketBody);
            deathParticles.Play();
            audioSource.PlayOneShot(deathSound);
            DestroyEnemy();


            //Destroy(gameObject);

        }

    }

    private void DestroyEnemy()
    {
        Object Enemy = GameObject.Find("EnemyAppearance");
        Destroy(Enemy);
    }

}
