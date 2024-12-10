using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_manager : MonoBehaviour
{
    public bool isMute = false;
    public AudioSource AudioSource; 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    
    public void Toggle_Music()
    {
        if (isMute == true)
        {
            AudioSource.volume = 1f;
            isMute = false;
        }
        else
        {
            AudioSource.volume = 0f;
            isMute = true;
        }
    }
}
