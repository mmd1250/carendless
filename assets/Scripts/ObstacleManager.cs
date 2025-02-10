using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Timeline.TimelinePlaybackControls;

public class ObstacleManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject ObstaclePrefab;
    public GameObject[] ObstaclePrefabs;
    public bool isSpawned = false;
    public float ObstacleLength = 5;
    public float DestroyDistance = 10f;
    public bool CanSpawn = false;
    public bool canmovagain = false;
    public bool mmd = false;
    public bool CanSpawnAgain = true;

    private List<GameObject> activeObstacles = new List<GameObject>(); // لیست موانع  فعال
    private HashSet<GameObject> reachedObstacles = new HashSet<GameObject>(); // لیست موانعی که به مقصد رسیدند
    private Dictionary<GameObject, float> targetDestinations = new Dictionary<GameObject, float>();
    private List<GameObject> inactiveObstacles = new List<GameObject>(); // لیست موانع غیرفعال

    // Start is called before the first frame update
    void Start()
    {
        CanSpawn = true;
            StartCoroutine(MoveObstaclesPeriodically());
  
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (CanSpawn)
            {
                CanSpawn = false;
                StartCoroutine(ObstacleSpawn());
            }
        }
                if (isSpawned && CanSpawn)
        {
            isSpawned = false;
        }
        //MoveObstacles();
        CheckAndDestroyObstacles();
        //StartCoroutine(NextMoves());

    }
    IEnumerator NextMoves()
    {
        yield return new WaitForSeconds(3f);
        mmd = false;
        StartCoroutine(MoveObstaclesPeriodically());
    }
    IEnumerator MoveObstaclesPeriodically()
    {
        while (true) // حلقه بی‌نهایت
        {
            float posibbleWAitTimes = Random.Range(2,3.5f);
            yield return new WaitForSeconds(posibbleWAitTimes);
            float elapsedTime = 0;
            while (elapsedTime < 6f)
            {
                MoveObstacles(); // حرکت موانع
                yield return new WaitForSeconds(0.01f); // صبر برای 5 ثانیه
                elapsedTime += 0.01f;
            }

            // صبر برای چند ثانیه قبل از اجرای دوباره حرکت
            //float delayAfterMovement = Random.Range(1, 1.5f); // مقدار تأخیر بین 2 تا 5 ثانیه
            //yield return new WaitForSeconds(delayAfterMovement);
            canmovagain = true;
        }
    }
    void MoveObstacles()
    {

        foreach (GameObject Obstacle in activeObstacles)
        {
            // اگر این مانع قبلاً به مقصد رسیده باشد، حرکت نکند
            if (reachedObstacles.Contains(Obstacle))
            {
                continue;
            }

            // موقعیت فعلی x مانع
            float currentX = Obstacle.transform.position.x;

            if (!targetDestinations.ContainsKey(Obstacle))
            {
                // تعیین موقعیت هدف بر اساس موقعیت فعلی
                float targetX = currentX;
                float threshold = 0.1f;  // آستانه برای بررسی نزدیکی به مقصد
                if (Mathf.Abs(currentX + 4.5f) <= threshold) // اگر ماشین به چپ نزدیک است
                {
                    //چپ -4
                    targetX = 1; // حرکت به وسط

                }
                else if (Mathf.Abs(currentX - 1) < 0.1f) // اگر ماشین وسط است
                {
                    // انتخاب تصادفی بین چپ یا راست
                    //وسط 1
                    targetX = Random.Range(0, 2) == 0 ? -4 : 6;
                }
                else if ((currentX - 5.9f) <= threshold) // اگر ماشین به راست نزدیک است
                {
                    //راست 6
                    targetX = 1; // حرکت به وسط
                }
                else
                {
                    continue; // اگر موقعیت نامعتبر بود، هیچ کاری انجام نشود
                }
                targetDestinations[Obstacle] = targetX;
            }
            // حرکت تدریجی به سمت مقصد
            float destinationX = targetDestinations[Obstacle];
            // حرکت تدریجی به سمت موقعیت هدف
            Vector3 targetPosition = new Vector3(destinationX, Obstacle.transform.position.y, Obstacle.transform.position.z);
                //Obstacle.transform.position = Vector3.Lerp(Obstacle.transform.position, targetPosition, 0.1f);
                if (Vector3.Distance(Obstacle.transform.position, targetPosition) < 0.1f)
                {
                    Obstacle.transform.position = targetPosition; // رسیدن به موقعیت نهایی
                    reachedObstacles.Add(Obstacle); // اضافه کردن مانع به لیست موانع به مقصد رسیده
                }
                else
                {
                    // محاسبه بردار جهت
                    Vector3 direction = (targetPosition - Obstacle.transform.position).normalized;
                    // حرکت به سمت مقصد با سرعت ثابت
                    float speed = 10f; // تنظیم سرعت
                    float distanceToMove = Mathf.Min(speed * Time.deltaTime, Vector3.Distance(Obstacle.transform.position, targetPosition));
                    Obstacle.transform.Translate(direction * distanceToMove, Space.World);
                }
                
            
        }
    }
    void CheckAndDestroyObstacles()
    {
        for (int i = activeObstacles.Count - 1; i >= 0; i--) // از آخر به اول بررسی می‌کنیم
        {
            GameObject Obstacle = activeObstacles[i];

            // اگر ماشین از فاصله مشخص از بازیکن عبور کرده باشد
            if (Obstacle.transform.position.z  + ObstacleLength / 2< Player.transform.position.z - DestroyDistance)
            {
                Destroy(Obstacle); // نابودی ماشین
                activeObstacles.RemoveAt(i); // حذف از لیست
            }
        }
    }

    IEnumerator ObstacleSpawn()
    {
        // لیست مقادیر ممکن برای موقعیت x
        float[] possibleXPositions = { -4, 1f, 6f };

        if (activeObstacles.Count <= 5)
        {


                int randomObstaclePrefab = Random.Range(0, ObstaclePrefabs.Length);
                yield return new WaitForSeconds(Random.Range(1f, 1.5f));
                float randomX = Mathf.Round(possibleXPositions[Random.Range(0, possibleXPositions.Length)] * 100 / 100);
                GameObject New_Obstacle = Instantiate(ObstaclePrefabs[randomObstaclePrefab], new Vector3(randomX, 3.26f, -35f), Quaternion.identity);
                inactiveObstacles.Add(New_Obstacle);//اضافه به لیست غیر قعال
                New_Obstacle.transform.position = new Vector3(Mathf.Round(randomX * 100) / 100, 3.26f, -35f);
                //New_Obstacle.transform.position = new Vector3(randomX, 3.26f, -40);
                New_Obstacle.name = ObstaclePrefab.name;
                //activeObstacles.Add(New_Obstacle);
                isSpawned = true;
                CanSpawn = true;
                float ObstacleMoveHorizontalWait = Random.Range(1.5f, 2.5f);
                // تأخیر قبل از فعال شدن مانع
                yield return new WaitForSeconds(ObstacleMoveHorizontalWait);  // مقدار زمان تأخیر
                inactiveObstacles.Remove(New_Obstacle);
                activeObstacles.Add(New_Obstacle); // اضافه کردن مانع به لیست فعال
            
        }
        else if (activeObstacles.Count >=5)
        {
            yield return new WaitForSeconds(Random.Range(5f, 6.5f));
            float randomX = Mathf.Round(possibleXPositions[Random.Range(0, possibleXPositions.Length)] * 100 / 100);
            GameObject New_Obstacle = Instantiate(ObstaclePrefab, new Vector3(randomX, 3.26f, -40), Quaternion.identity);
            inactiveObstacles.Add(New_Obstacle);//اضافه به لیست غیر قعال
            New_Obstacle.transform.position = new Vector3(Mathf.Round(randomX * 100) / 100, 3.26f, -40);
            //New_Obstacle.transform.position = new Vector3(randomX, 3.26f, -40);
            New_Obstacle.name = ObstaclePrefab.name;
            //activeObstacles.Add(New_Obstacle);
            isSpawned = true;
            CanSpawn = true;
            float ObstacleMoveHorizontalWait = Random.Range(1.5f, 2.5f);
            // تأخیر قبل از فعال شدن مانع
            yield return new WaitForSeconds(ObstacleMoveHorizontalWait);  // مقدار زمان تأخیر
            inactiveObstacles.Remove(New_Obstacle);
            activeObstacles.Add(New_Obstacle); // اضافه کردن مانع به لیست فعال
        }
    }
}
