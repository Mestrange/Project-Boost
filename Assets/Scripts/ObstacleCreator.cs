using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCreator : MonoBehaviour
{
    [SerializeField] ParticleSystem deathParticles;

    public GameObject asteroidd;
    int NumberOfAsteroids = 0;
    public List<GameObject> spawnedAsteroids1 = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(spawnedAsteroids1.Count);
        createAsteroid();
    }

    public void createAsteroid()
    {
        //  GameObject Asteroid = Instantiate(asteroidd);
        GameObject TestClone1 = Instantiate(asteroidd);
        //  spawnedAsteroids1.Add(newAsteroid);
        asteroidd.transform.position = new Vector3(Random.Range(-72f, 0f), 57.1f, 0);
        deathParticles.Play();
        Debug.Log(spawnedAsteroids1.Count);

    }

    // Update is called once per frame
    void Update()
    {
        //if (spawnedAsteroids1.Count >= 2)
        //{
        //    Destroy(spawnedAsteroids1[0].gameObject);
        //    spawnedAsteroids1.RemoveAt(0);
        //    Debug.Log("Destroyed");
        //}



        if (Time.fixedTime % 4 == 0)
        {
            createAsteroid();
        }
    }
}
