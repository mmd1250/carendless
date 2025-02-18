using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player_Move : MonoBehaviour
{
    //public GameObject Coin_Prefab;

    public ParticleSystem Right_Smoke_Particle;
    public ParticleSystem Left_Smoke_Particle;

    public GameObject Player;
    public bool CanIncreaseSpeed = false;
    public bool is_moving = true;
    public float Current_speed = 10f;
    public float Min_Speed = 30f;
    public float Max_speed = 20f; // حداکثر سرعت بازیکن
    public float Speed_increment = 2f; // مقدار افزایش سرعت
    private float Speed_timer = 0f; // تایمر برای افزایش سرعت
    public float Time_to_increase = 1.5f; // مدت زمان برای افزایش سرعت
    public int Player_Score = 0;
    public float Score_timer = 0f;
    public float Score_increment_interval = 0.01f; // فاصله زمانی بین هر افزایش امتیاز
    private int Score_to_add = 0; // مقدار امتیاز باقی‌مانده برای اضافه شدن
    public Text Score_Text;
    public GameObject FirstGround;
    public GameObject GameLoseCanvas;
    public GameObject Puase_Btn;
    //public GameObject Fuel_Can;
    public float Player_Health = 10;
    private Vector3 PlayerHitMovement = new Vector3(0, 0, 25);
    public float Hit_BackTime = 2;
    private bool isHit = false;
    private float hitSlowdownTimer = 0f; // تایمر برای بازگشت سرعت
    private bool isMovingBack = false;
    private bool isRecoveringSpeed = false; // پرچم برای بازگشت سرعت پس از برخورد


    public float Toward_maxSpeed = 12f; // حداکثر سرعت حرکت چپ و راست
    public float acceleration = 6f; // شتاب حرکت هنگام لمس صفحه
    public float deceleration = 15f; // کاهش سرعت هنگام برداشتن لمس
    private float Toward_currentSpeed = 0f; // سرعت فعلی حرکت

    public float minX = -3.92f; // کمترین مقدار مجاز موقعیت افقی
    public float maxX = 5.6f; // بیشترین مقدار مجاز موقعیت افقی


    private PushOut PushOutScript;
    // Start is called before the first frame update
    void Start()
    {
        PushOutScript = GetComponent<PushOut>();
        Time.timeScale = 1;
        Current_speed = Min_Speed;
        Speed_increment = 5;
    }

    // Update is called once per frame
    void Update()
    {

        //Touch touch = Input.GetTouch(0);


        is_moving = true;
        if (is_moving)
        {
            if (PushOutScript.CanControl())
            {
                Player_Movement();
                IncreaseSpeedOverTime();
            }

            //IncrementPlayerScore();
        }
        //Player_Movement();

    }
    
    private IEnumerator MovePlayerBack()
    {
        is_moving = true;
        isMovingBack = true;
        float elapsedTime = 0f;
        Vector3 initialPosition = Player.transform.position; // موقعیت اولیه پلیر
        Vector3 targetPosition = initialPosition - PlayerHitMovement; // موقعیت نهایی پلیر

        while (elapsedTime < Hit_BackTime)
        {
            Vector3 backwardMovement = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / Hit_BackTime);

            // اضافه کردن حرکت چپ و راست بازیکن
            Vector3 sideMovement = new Vector3(Player.transform.position.x, 0, 0);

            Player.transform.position = new Vector3(
                sideMovement.x,
                backwardMovement.y,
                backwardMovement.z
            );

            Vector3 forwardMovement = new Vector3(0, 0, Current_speed * Time.deltaTime);
            GameObject[] Ground = GameObject.FindGameObjectsWithTag("Ground");
            foreach (GameObject Single_Ground in Ground)
            {
                Single_Ground.transform.position -= forwardMovement *1f;
                //print("mmli");
            }
            GameObject[] ObstaclePrefab = GameObject.FindGameObjectsWithTag("Obstacle");
            foreach (GameObject Obstacle in ObstaclePrefab)
            {
                Obstacle.transform.position -= forwardMovement * Random.Range(0.1f, 0.2f); // حرکت تدریجی بر اساس زمان
            }
            GameObject[] Coin_Prefab = GameObject.FindGameObjectsWithTag("Coin_Prefab_4");
            foreach (GameObject coin in Coin_Prefab)
            {
                coin.transform.position -= forwardMovement ; // حرکت تدریجی بر اساس زمان
            }
            GameObject[] Fuel_Prefab = GameObject.FindGameObjectsWithTag("fuel");
            foreach (GameObject Fuel_Can in Fuel_Prefab)
            {
                Fuel_Can.transform.position -= forwardMovement ; // حرکت تدریجی بر اساس زمان
            }
            GameObject[] Star_Prefab = GameObject.FindGameObjectsWithTag("Star");
            foreach (GameObject Star in Star_Prefab)
            {
                Star.transform.position -= forwardMovement; // حرکت تدریجی بر اساس زمان
            }
            elapsedTime += Time.deltaTime; // افزایش زمان طی شده
            yield return null; // صبر برای فریم بعدی
        }

        // اطمینان از رسیدن دقیق به موقعیت نهایی
        //Player.transform.position = targetPosition;


        isMovingBack = false;
    }
    private IEnumerator Particle_Collision()
    { 
        Right_Smoke_Particle.Stop();
        Left_Smoke_Particle.Stop();

        yield return new WaitForSeconds(1.5f);

        Right_Smoke_Particle.Play();
        Left_Smoke_Particle.Play();

    }

    private void OnCollisionEnter(Collision collision)
    {
        // شروع حرکت تدریجی به عقب

        if (collision.transform.CompareTag("Obstacle"))
        {
            StartCoroutine(Particle_Collision());
            StartCoroutine(MovePlayerBack());
            isHit = true;
            print("Obstacle Hited");
            Current_speed = Min_Speed;
            Speed_increment = 20f;
            Time_to_increase = 2f;


            Player_Health -= 2;
            if (Player_Health <= 0)
            {
                GameLoseCanvas.SetActive(true);
                Puase_Btn.SetActive(false);
                is_moving = false;
                Time.timeScale = 0;
                print("you died");
            }

        }
    }
    public void Player_Movement()
    {
        float targetSpeed = 0f; // سرعت موردنظر برای رسیدن
        Vector3 movement = Vector3.zero;
        // بررسی لمس صفحه برای حرکت به چپ یا راست
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // دریافت اولین لمس
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                // لمس روی دکمه یا UI است؛ این لمس را نادیده بگیر
                is_moving = true;
            }

            else
            {
                // مقدار حرکت بر اساس لمس صفحه
                float moveSpeed = 12f * Time.deltaTime;
                if (touch.position.x > Screen.width / 2 )
            {
                    // لمس سمت راست: حرکت به سمت راست
                    //movement.x += 12f * Time.deltaTime; // مقدار سرعت افقی به راست
                    //Toward_currentSpeed = Mathf.MoveTowards(Toward_currentSpeed, Toward_maxSpeed, acceleration * Time.deltaTime);
                    //Player.transform.Translate(Vector3.right *Toward_currentSpeed* Time.deltaTime);
                    targetSpeed = Toward_maxSpeed;
            }
            else if (touch.position.x < Screen.width / 2 )
            {
                    // لمس سمت چپ: حرکت به سمت چپ
                    //movement.x -= 12f * Time.deltaTime; // مقدار سرعت افقی به چپ
                    //Toward_currentSpeed = Mathf.MoveTowards(Toward_currentSpeed, -Toward_maxSpeed, acceleration * Time.deltaTime);

                    //Player.transform.Translate(Vector3.left * Mathf.Abs(Toward_currentSpeed) * Time.deltaTime);
                    targetSpeed = -Toward_maxSpeed;
            }  
            }
            
        }
        // افزایش یا کاهش تدریجی سرعت برای حرکت نرم
        Toward_currentSpeed = Mathf.Lerp(Toward_currentSpeed, targetSpeed, (targetSpeed == 0 ? deceleration : acceleration) * Time.deltaTime);

        // اعمال حرکت
        transform.Translate(Vector3.right * Toward_currentSpeed * Time.deltaTime);

        // جلوگیری از خروج از محدوده
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y, transform.position.z);
        Vector3 forwardMovement = new Vector3(0, 0, Current_speed * Time.deltaTime);
        GameObject[] Ground = GameObject.FindGameObjectsWithTag("Ground");
        foreach (GameObject Single_Ground in Ground)
        {
            Single_Ground.transform.position -= forwardMovement;
        }
        if (FirstGround != null)
        {
            FirstGround.transform.position -= forwardMovement / 2;
        }

        Player.transform.position += new Vector3(movement.x,0,0);

        GameObject[] ObstaclePrefab = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject Obstacle in ObstaclePrefab)
        {
            Obstacle.transform.position -= forwardMovement * Random.Range(0.4f, 0.8f); // حرکت تدریجی بر اساس زمان
        }

        GameObject[] Coin_Prefab = GameObject.FindGameObjectsWithTag("Coin_Prefab_4");
        foreach (GameObject coin in Coin_Prefab)
        {
            coin.transform.position -= forwardMovement; // حرکت تدریجی بر اساس زمان
        }
        GameObject[] Fuel_Prefab = GameObject.FindGameObjectsWithTag("fuel");
        foreach (GameObject Fuel_Can in Fuel_Prefab)
        {
            Fuel_Can.transform.position -= forwardMovement; // حرکت تدریجی بر اساس زمان
        }
        GameObject[] Star_Prefab = GameObject.FindGameObjectsWithTag("Star");
        foreach (GameObject Star in Star_Prefab)
        {
            Star.transform.position -= forwardMovement; // حرکت تدریجی بر اساس زمان
        }




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
        if (isHit)
        {
            // کاهش تدریجی سرعت به حداقل
            hitSlowdownTimer += Time.deltaTime;
            //is_moving = true;
            Current_speed = Mathf.Lerp(Current_speed, Min_Speed, hitSlowdownTimer / Time_to_increase);

            // اگر سرعت به حداقل رسید، بازگشت به حالت عادی
            if (hitSlowdownTimer >= Time_to_increase)
            {
                Current_speed = Min_Speed;
                isHit = false; // بازگشت به حالت عادی
                isRecoveringSpeed = true; // شروع بازگشت سرعت
                hitSlowdownTimer = 0f;
            }
        }
        else if (isRecoveringSpeed )
        {
            // بازگشت تدریجی سرعت به حالت عادی
            hitSlowdownTimer += Time.deltaTime;
            Current_speed = Mathf.Lerp(Min_Speed, Max_speed, hitSlowdownTimer / Time_to_increase);

            // اگر سرعت به حداکثر رسید، پایان بازگشت سرعت
            if (hitSlowdownTimer >= Time_to_increase)
            {
                Current_speed = Max_speed;
                isRecoveringSpeed = false; // پایان حالت بازگشت سرعت
                hitSlowdownTimer = 0f; // ریست تایمر
            }
        }
        else
        {
            // افزایش سرعت به حالت عادی

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
    }
}