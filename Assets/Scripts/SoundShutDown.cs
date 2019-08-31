using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundShutDown : MonoBehaviour
{
    
        public void turnSoundOff()
        {
            AudioListener.pause = true;
        }
        

    
    
}
