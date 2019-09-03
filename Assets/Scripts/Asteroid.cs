using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public GameObject asteroid;
    int NumberOfAsteroids = 0;
    public List<GameObject> spawnedAsteroids1 = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(spawnedAsteroids1.Count);
        createAsteroid();
    }

    // Update is called once per frame
    void Update()
    {

        if (spawnedAsteroids1.Count > 4)
        {
            Destroy(spawnedAsteroids1[0].gameObject);
            spawnedAsteroids1.RemoveAt(0);
            Debug.Log("Destroyed");
            Debug.Log(spawnedAsteroids1.Count);
        }


        if (Time.fixedTime % 3 == 0)
        {
            createAsteroid();
        }
    }

    public void createAsteroid()
    {
        //  GameObject Asteroid = Instantiate(asteroid);
        GameObject TestClone1 = Instantiate(asteroid);
        spawnedAsteroids1.Add(TestClone1);
        asteroid.transform.position = new Vector3(Random.Range(-72f, 0f), 57.1f, 0);
        //deathParticles.Play();
        Debug.Log(spawnedAsteroids1.Count);

    }




}
