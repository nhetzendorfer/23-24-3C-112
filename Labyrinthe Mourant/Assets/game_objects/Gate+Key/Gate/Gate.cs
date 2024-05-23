using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Gate : MonoBehaviour
{
    [Header("open")]
    public bool HasKey;
    bool triggered;
    [Header("Animation")]
    // -90 rotation
    public Transform leftDoor;
    // + 90 rotation
    public Transform rightDoor;
    private float rotation=90;
    public float openSpeed;
    private bool open;

    public void OnTriggerEnter(Collider other)
    {
        triggered = true;
    }

    public void OnTriggerExit(Collider other)
    {
        triggered = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && HasKey && triggered)
        {
            open = true;
            HasKey = false;
        }
        if (open&&rotation>0)
        {
            leftDoor.Rotate(new Vector3(0,-openSpeed * Time.deltaTime,0));
            rightDoor.Rotate(new Vector3(0, openSpeed * Time.deltaTime, 0));
            rotation -= openSpeed * Time.deltaTime;
        }
    }
}
