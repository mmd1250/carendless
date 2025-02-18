using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Coin : MonoBehaviour
{
    public GameObject Player;
    public AudioSource AudioSource;
    public bool isSpawned  = false;
    public Text CoinText;
    public int Coin = 0;
    private GameObject CoinPrefab;
    public GameObject Coin_Deck;
    public GameObject[] Coin_Decks; // آرایه‌ای از آبجکت‌هایی که می‌خواهید Instantiate کنید
    public float DestroyDistance = 10f;
    public float Coin_Deck_Length = 15f;
    public bool CanSpawn = false;


    private List<GameObject> activeCoins = new List<GameObject>(); // لیست موانع  فعال

    // Start is called before the first frame update
    void Start()
    {
            CanSpawn = true;
            //StartCoroutine(Coin_Deck_Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        CoinText.text = Coin.ToString();
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (CanSpawn)
                {
                    CanSpawn = false;
                    StartCoroutine(Coin_Deck_Spawn());
                }
            }
         if (isSpawned && CanSpawn)
            {
                isSpawned = false;
            }
        
        CheckAndDestroyCoins();

    }
    void CheckAndDestroyCoins()
    {
        for (int i = activeCoins.Count - 1; i >= 0; i--) // از آخر به اول بررسی می‌کنیم
        {
            GameObject Coin = activeCoins[i];

            // اگر ماشین از فاصله مشخص از بازیکن عبور کرده باشد
            if (Coin.transform.position.z + Coin_Deck_Length / 2 < Player.transform.position.z - DestroyDistance)
            {
                Destroy(Coin); // نابودی ماشین
                activeCoins.RemoveAt(i); // حذف از لیست
            }
        }
    }
    IEnumerator Coin_Deck_Spawn()
    {
        //int Spawn_Time = Random.Range(2f,5f);

        // لیست مقادیر ممکن برای موقعیت x
        float[] possibleXPositions = { -1.76f, 3.86f, 9.50f };
        // انتخاب یک ایندکس تصادفی از آرایه
        int randomCoinPrefab = Random.Range(0, Coin_Decks.Length);
        yield return new WaitForSeconds(Random.Range(2f, 3.5f));

            // انتخاب موقعیت x تصادفی
            float randomX = possibleXPositions[Random.Range(0, possibleXPositions.Length)];

            GameObject New_Coin = Instantiate(Coin_Decks[randomCoinPrefab], new Vector3(randomX, 4f, -45f), Quaternion.identity);
            New_Coin.name = Coin_Deck.name;
            activeCoins.Add(New_Coin);
            isSpawned = true;
            CanSpawn = true;
            
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Coin"))
        {
            AudioSource.Play();
            Coin++;
            //CoinPrefab = GameObject.FindWithTag("Coin");
            //CoinPrefab = collision.transform.name
            Destroy(collision.gameObject);
            CoinText.text = Coin.ToString();
        }
    }
}
