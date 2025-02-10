using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_Spawn : MonoBehaviour
{
    public GameObject Player;
    public GameObject GroundPrefab; // پریفب زمین
    public float GroundLength = 25f; // طول زمین (z) - این مقدار را با طول واقعی پریفب هماهنگ کنید
    public Transform LastGround; // آخرین Ground موجود
    public Transform InitialGround;
    public float SpawnDelay = 3f; // زمان تأخیر در تولید
    public float Ground_Destroy_Time = 12f;
    public float DestroyDistance = 150f; // فاصله از بازیکن برای نابودی
    public float SpawnTriggerDistance = 100f;//فاصله ای که رسیدیم زمین تولید بشه

    public bool isInitialized = false;

    private List<GameObject> activeGrounds = new List<GameObject>(); // لیست زمین‌های فعال
    // Start is called before the first frame update
    void Start()
    {

        // افزودن زمین اولیه به لیست
        if (InitialGround != null)
        {
            activeGrounds.Add(InitialGround.gameObject);
            //InitialGround.position += new Vector3(0, 0, +24.6f);
            LastGround = InitialGround;
            isInitialized = true;
        }
        else
        {
            Debug.LogError("InitialGround is not assigned! Please assign it in the Inspector.");
        }

        // مقداردهی اولیه LastGround به InitialGround

        //if (LastGround != null)
        //{
        //activeGrounds.Add(LastGround.gameObject); // افزودن اولین زمین به لیست
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitialized) return;
        // چک کردن اگر بازیکن به انتهای نزدیک شده باشد، زمین جدید ایجاد شود
        if (Player.transform.position.z + SpawnTriggerDistance >= LastGround.position.z + GroundLength / 2)
        {
            SpawnGround();
        }
        CheckAndDestroyGrounds();
    }
    void SpawnGround()
    {

            // محاسبه موقعیت جدید برای Ground بعدی
            Vector3 newPosition = new Vector3(
                LastGround.position.x, // x ثابت
                LastGround.position.y, // y ثابت
                LastGround.position.z + GroundLength * 2 // z جدید (جلوی زمین قبلی)
            );
            // ذخیره مرجع زمین قبلی برای نابودی
            // ایجاد زمین جدید
            GameObject newGround = Instantiate(GroundPrefab, newPosition,Quaternion.identity );
            activeGrounds.Add(newGround);
            // به‌روزرسانی آخرین Ground
            LastGround = newGround.transform;
        
    }
    void CheckAndDestroyGrounds()
    {
        for (int i = activeGrounds.Count - 1; i >= 0; i--) // از آخر به اول بررسی می‌کنیم
        {
            GameObject ground = activeGrounds[i];

            // اگر زمین از فاصله مشخص از بازیکن عبور کرده باشد
            if (ground.transform.position.z + GroundLength / 2 < Player.transform.position.z - DestroyDistance)
            {
                Destroy(ground); // نابودی زمین
                activeGrounds.RemoveAt(i); // حذف از لیست
            }
        }
    }
}
