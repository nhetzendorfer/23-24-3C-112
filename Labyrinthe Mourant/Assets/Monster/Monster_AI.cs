using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster_AI : MonoBehaviour
{
    public Transform Player;
    public NavMeshAgent MonsterAi;
    public bool startDelay;
    public float starteDelayTime;

    // Start is called before the first frame update
    void Start()
    {
        if (startDelay)
            Wait(starteDelayTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSecondsRealtime(time);
    }
}
