using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Coin : MonoBehaviour
{
    public bool isSpawned  = false;
    public Text CoinText;
    public int Coin = 0;
    private GameObject CoinPrefab;
    public GameObject Coin_Deck;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(test());
        //CoinText.text = Coin.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        CoinText.text = Coin.ToString();
        if (isSpawned)
        {
            StartCoroutine (test());
            isSpawned = false;
        }

    }
    IEnumerator test()
    {
        //int Spawn_Time = Random.Range(2f,5f);

        // لیست مقادیر ممکن برای موقعیت x
        float[] possibleXPositions = { -7f, 2.5f, 14f };
            yield return new WaitForSeconds(Random.Range(2f, 5f));

            // انتخاب موقعیت x تصادفی
            float randomX = possibleXPositions[Random.Range(0, possibleXPositions.Length)];

            GameObject New_Coin = Instantiate(Coin_Deck, new Vector3(randomX, 2.491758f, -40), Quaternion.identity);
            New_Coin.name = Coin_Deck.name;
            //print("spawned");
            isSpawned = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Coin")
        {
            Coin++;
            CoinPrefab = GameObject.FindWithTag("Coin");
            Destroy(CoinPrefab);
            CoinText.text = Coin.ToString();
        }
    }
}
