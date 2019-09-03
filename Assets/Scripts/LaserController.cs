using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (gameObject.tag == "EnemyLaser" && (other.gameObject.tag == "Terrain" ||  other.gameObject.tag == "Player"))
        {
            Invoke("DestroyLaser",0.2f);

        }

    }

    private void DestroyLaser()
    {
        Destroy(gameObject);
    }
}
