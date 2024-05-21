using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Gate gate;
    bool triggered;

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
        if (Input.GetKeyDown(KeyCode.E) && triggered)
        {
            gate.HasKey = true;
            Destroy(gameObject);
        }
    }
}
