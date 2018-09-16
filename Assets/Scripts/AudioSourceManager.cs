using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour {

    // Use this for initialization
    public static AudioSourceManager instance = null;
    // Use this for initialization
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            Debug.Log("One Music Destroyed");
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        //if(GlobalOptions.isSound()){
    
        //}
    }

    void Update()
    {
       // if (Rocket.isGameQuit == true) gameObject.SetActive(false); //Предполагалось, что это выключит фоновую музыку в случае победы
    }

}
