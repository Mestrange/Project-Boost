﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : Asteroid
{
    [SerializeField] ParticleSystem deathParticles;
    
    void Start()
        {
        
        
    }

    // Update is called once per frame
    void Update()
    {


        Debug.Log(spawnedAsteroids1.Count);





    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Terrain" || other.gameObject.tag == "PlayerLaser" || other.gameObject.tag == "Player")
        {
            
           
            deathParticles.Play();
            Destroy(gameObject);

        }
        
    }

}
