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
    public Transform leftDoor;
    public Transform rightDoor;
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
            transform.position += new Vector3(0, 100, 0);
            open = true;
            HasKey = false;
        }
        if (open)
        {

        }
    }
}
