using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public Player_Move player_Move; [SerializeField]
    public Slider Fuel_Slider;
    public MonoBehaviour Player_Movement;
    public float DecreaseRate = 5f;
    public float MinValue;
    public float Fuel_Decrease_Time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        // پیدا کردن اسکریپت Player_Move روی همان GameObject
        player_Move = GetComponent<Player_Move>();

        // اگر اسکریپت روی GameObject دیگری است:
        // playerMove = GameObject.Find("Player").GetComponent<Player_Move>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player_Move != null && player_Move.is_moving)
        {
            if (Fuel_Slider.value > MinValue)
            {
                Fuel_Slider.value -= DecreaseRate * Time.deltaTime;
            }
            
        }
    }
}
