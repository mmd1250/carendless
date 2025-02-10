using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel_Manager : MonoBehaviour
{
    public GameObject Player;
    public float FuelLength = 5;
    public GameObject Fuel_Prefab;
    public bool Can_Spawn = false;
    public float DestroyDistance = 10f;
    private List<GameObject> activeFuels = new List<GameObject>(); // لیست زمین‌های فعال
    // Start is called before the first frame update
    void Start()
    {
        Can_Spawn = true;
        //StartCoroutine(Fuel_Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        if (Can_Spawn)
        {

            Can_Spawn = false;
            StartCoroutine(Fuel_Spawn());
        }
        CheckAndDestroyFuels();
        // 5.30  0.90  -3.75
    }

    IEnumerator Fuel_Spawn()
    {
        yield return new WaitForSeconds(3);

        float[] xPositions = { -3.75f, 0.90f, 5.30f };

        // ?????? ?????? ??? ?? ??????
        float randomX = xPositions[UnityEngine.Random.Range(0, xPositions.Length)];

        GameObject Newfuel = Instantiate(Fuel_Prefab, new Vector3(randomX, 3.7f, -40f), Quaternion.identity);
        activeFuels.Add(Newfuel);
        Can_Spawn = true;
    }
    void CheckAndDestroyFuels()
    {
        for (int i = activeFuels.Count - 1; i >= 0; i--) // از آخر به اول بررسی می‌کنیم
        {
            GameObject Fuel = activeFuels[i];

            // بررسی اگر Fuel از قبل حذف شده باشد
            if (Fuel == null)
            {
                activeFuels.RemoveAt(i); // حذف از لیست
                continue; // ادامه حلقه برای جلوگیری از خطا
            }
            // اگر زمین از فاصله مشخص از بازیکن عبور کرده باشد
            if (Fuel.transform.position.z + FuelLength / 2 < Player.transform.position.z - DestroyDistance)
            {
                    Destroy(Fuel); // نابودی زمین
                    activeFuels.RemoveAt(i); // حذف از لیست

            }
        }
    }
}