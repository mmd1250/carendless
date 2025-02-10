using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puase_Menu : MonoBehaviour
{
    public GameObject Menu;
    public GameObject MainCanvas;
    private bool MenuCanvasSetactive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Change_SetActive();
    }


    public void Change_SetActive()
    {
            if (MenuCanvasSetactive == false)
            {
                MenuCanvasSetactive = true;
                Time.timeScale = 0;
                Menu.SetActive(true);
            }
            else if (MenuCanvasSetactive == true)
            {
                MenuCanvasSetactive= false;
                Time.timeScale  = 1;
                Menu.SetActive(false);
            }

    }
    public void ResumeGame()
    {
        MenuCanvasSetactive = false;
        Menu.SetActive(false);
        Time.timeScale = 1;
    }
    public void ExitGame()
    {
        // خروج از بازی
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
