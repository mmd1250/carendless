using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;

public class Player_Move : MonoBehaviour
{   
    public GameObject Obstacle;
    //public GameObject Coin_Prefab;
    public GameObject Ground;
    public GameObject Player;
    public bool is_moving = false;
    public float Current_speed = 10f;
    public float Max_speed = 20f; // حداکثر سرعت بازیکن
    public float Speed_increment = 2f; // مقدار افزایش سرعت
    private float Speed_timer = 0f; // تایمر برای افزایش سرعت
    public float Time_to_increase = 1.5f; // مدت زمان برای افزایش سرعت
    public int Player_Score = 0;
    public float Score_timer = 0f;
    public float Score_increment_interval = 0.01f; // فاصله زمانی بین هر افزایش امتیاز
    private int Score_to_add = 0; // مقدار امتیاز باقی‌مانده برای اضافه شدن
    public Text Score_Text;
    // Start is called before the first frame update
    void Start()
    {        

    }

    // Update is called once per frame
    void Update()
    {
        //MoveAllCoins();
        //Score_Text.text = Player_Score.ToString();

            //Touch touch = Input.GetTouch(0);

            if (Input.touchCount > 0&& Input.GetTouch(0).phase == TouchPhase.Began && !is_moving)
            {
                is_moving = true;

            }
            if (is_moving)
            {
                Player_Movement();
                IncreaseSpeedOverTime();
                //IncrementPlayerScore();
            }
            
        }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Obstacle")
        {
            print("Obstacle Hited");
            is_moving = false;
        }
    }
    public void Player_Movement()
    {
        Vector3 movement = new Vector3(0, 0, Current_speed * Time.deltaTime);
        // بررسی لمس صفحه برای حرکت به چپ یا راست
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // دریافت اولین لمس

            if (touch.position.x > Screen.width / 2)
            {
                // لمس سمت راست: حرکت به سمت راست
                movement.x += 5f * Time.deltaTime; // مقدار سرعت افقی به راست
            }
            else if (touch.position.x < Screen.width / 2)
            {
                // لمس سمت چپ: حرکت به سمت چپ
                movement.x -= 5f * Time.deltaTime; // مقدار سرعت افقی به چپ
            }
        }
        Ground.transform.position -= movement;
        Obstacle.transform.position -= movement;


        GameObject[] Coin_Prefab =  GameObject.FindGameObjectsWithTag("Coin_Prefab_4");
        foreach (GameObject coin in Coin_Prefab)
        {
            coin.transform.position -= movement; // حرکت تدریجی بر اساس زمان
        }
        // if (GameObject.FindWithTag("Coin_Prefab_4") != null)
        //{
        //  Coin_Prefab.transform.position -= movement * Time.deltaTime;
        //}




    }
    void MoveAllCoins()
    {
        Vector3 movement = new Vector3(0, 0, Current_speed * Time.deltaTime);
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin_Prefab_4");

        foreach (GameObject coin in coins)
        {
            coin.transform.position -= movement; // حرکت تدریجی بر اساس زمان
        }
    }
    private void IncreaseSpeedOverTime()
    {
        // به‌روزرسانی تایمر
        Speed_timer += Time.deltaTime;

        // هر Time_to_increase ثانیه سرعت را افزایش بده
        if (Speed_timer >= Time_to_increase)
        {
            Speed_timer = 0f; // ریست کردن تایمر
            if (Current_speed < Max_speed)
            {
                Current_speed += Speed_increment; // افزایش سرعت
                Current_speed = Mathf.Min(Current_speed, Max_speed); // اطمینان از نرسیدن به بیش از Max_speed
            }
        }
    }
   // private void IncrementPlayerScore()
   // {
        // به‌روزرسانی تایمر
       // Score_timer += Time.deltaTime;
     //   if (Score_timer >= 1f)
     //   {
       //     Score_timer = 0f; // ریست تایمر
       //     Score_to_add += 100; // مقدار امتیاز برای اضافه شدن
       // }
        // دونه‌دونه امتیاز اضافه کن
      //  if (Score_to_add > 0)
       // {
       //     Score_to_add -= 1; // کاهش مقدار باقی‌مانده
       //     Player_Score += 1; // افزایش امتیاز
        //    print($"Player Score: {Player_Score}"); // نمایش امتیاز (اختیاری)
       // }
    //}
    private void LateUpdate()
    {
        
    }
}
