using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColor : MonoBehaviour
{
    public ParticleSystem particleSystem; // ????? ???????
    [Range(0, 1)] public float transparency = 1.0f; // ????? ??????
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ????? ?????? ???????
        var main = particleSystem.main;
        Color color = main.startColor.color;
        color.a = transparency; // ????? ?????? ?? ?????????
        main.startColor = color;    
    }
}
