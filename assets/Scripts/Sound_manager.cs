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
        isMute = PlayerPrefs.GetInt("isMute", 0) == 1;
        ApplySoundSettings();
    }

    // Update is called once per frame
    void Update()
    {

    }


    
    public void Toggle_Music()
    {
        isMute = !isMute; // ????? ????? ???/??? ???
        ApplySoundSettings();

        // ????? ????? ?? PlayerPrefs
        PlayerPrefs.SetInt("isMute", isMute ? 1 : 0);
        PlayerPrefs.Save();
    }
    void ApplySoundSettings()
    {
        AudioSource.volume = isMute ? 0f : 1f;
    }
}
