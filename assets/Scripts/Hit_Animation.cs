using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_Animation : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;
    public GameObject FadeObject;
    // Start is called before the first frame update
    void Start()
    {
        //FadeObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Obstacle"))
        {
            FadeObject.SetActive(true);
            audioSource.Play();
            //animator.enabled |= true;
            animator.SetTrigger("Change");
        }
    }
}
