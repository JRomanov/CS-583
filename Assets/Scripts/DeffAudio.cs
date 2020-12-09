using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeffAudio : MonoBehaviour
{
    public AudioClip deffSound;
    AudioSource audioS;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(deffSound != null)
        {
            audioS.PlayOneShot(deffSound, 0.7f);
        }
    }
}
