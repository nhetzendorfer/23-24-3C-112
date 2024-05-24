using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    [Header("AudioSource")]
    public AudioSource FlashlightAudio;
    public Light FlashLight;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FlashLight.enabled = !FlashLight.enabled;
            FlashlightAudio.Play();
        }
    }
}
