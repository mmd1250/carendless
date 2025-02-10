using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawn : MonoBehaviour
{
    public GameObject Player;
    public float StarLength = 8f;
    public GameObject Star_Prefab;
    public bool Can_Spawn = false;
    public float DestroyDistance;
    private List<GameObject> activeStars = new List<GameObject>(); // لیست زمین‌های فعال

    // Start is called before the first frame update
    void Start()
    {
        Can_Spawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Can_Spawn) 
        {
            Can_Spawn = false;
            StartCoroutine(Star_Spawn());
        }
        CheckAndDestroyStars();
    }
    IEnumerator Star_Spawn()
    {
        yield return new WaitForSeconds(3);

        float[] xPositions = { -3.6f, 1.7f, 7.4f };

        // ?????? ?????? ??? ?? ??????
        float randomX = xPositions[UnityEngine.Random.Range(0, xPositions.Length)];

        GameObject NewStar = Instantiate(Star_Prefab, new Vector3(randomX, 3.7f, -45f), Quaternion.identity);
        activeStars.Add(NewStar);
        Can_Spawn = true;

    }
    void CheckAndDestroyStars()
    {
        for (int i = activeStars.Count - 1; i >= 0; i--) // از آخر به اول بررسی می‌کنیم
        {
            GameObject Star = activeStars[i];

            // بررسی اگر Fuel از قبل حذف شده باشد
            if (Star == null)
            {
                activeStars.RemoveAt(i); // حذف از لیست
                continue; // ادامه حلقه برای جلوگیری از خطا
            }
            // اگر زمین از فاصله مشخص از بازیکن عبور کرده باشد
            if (Star.transform.position.z + StarLength / 2 < Player.transform.position.z - DestroyDistance)
            {
                Destroy(Star); // نابودی زمین
                activeStars.RemoveAt(i); // حذف از لیست

            }
        }
    }
}
