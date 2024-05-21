using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum E_Monster_Behavior { NONE = 0, SEARCH, CHASE }
public class Monster_ai : MonoBehaviour
{
    [Header("To Navigate")]
    public Transform player;
    public List<Transform> destinations;
    public NavMeshAgent monsterAi;
    [Header("Starte Delay")]
    public bool startDelay;
    public float starteDelayTime;
    [Header("Movement and such")]
    public float sightDistance; // the distance at which the enemy starts chasing the player
    public float deathRange; // the distance at which the enemy kills the player
    public float chaseRange; // the distance at which the enemy kills the player
    public float rotationSpeed;
    //public Transform mesh;
    //[Header("Animation and sound")]
    //public Animation animaton;

    private E_Monster_Behavior behavior;
    // Start is called before the first frame update
    void Start()
    {
        lastPlayerPostion = player.position;
        behavior = E_Monster_Behavior.SEARCH;
        monsterAi.destination = FindDestination();
    }

    // Update is called once per frame
    void Update()
    {
        //may be replaced with animator
        /*
        mesh.position = transform.position;
        mesh.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
        */
        //mesh.position = transform.position;
        //transform.position = new Vector3(0,transform.position.y,0);
        //animaton.Play();
        //to see in wich direcion the player is
        Vector3 direction = (player.position - transform.position).normalized;
        //for raycast - the sight
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, sightDistance))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                behavior = E_Monster_Behavior.CHASE;
            }
        }
        // calculate the distance between the enemy and the player
        float distance = Vector3.Distance(player.position, transform.position);
        //does something for each behavior
        if(behavior == E_Monster_Behavior.CHASE)
            ChasePlayer(distance); // method that holds the logic for enemy to chase player
        if (behavior == E_Monster_Behavior.SEARCH)
            SearchForPlayer();
        PlayerDeath(distance); // method that jumpscares the player when enemy catches player
    }

    private void PlayerDeath(float distance)
    {
        if (distance < deathRange)
        {
            enabled = false;
        }
    }

    private void ChasePlayer(float distance)
    {
        if(distance < chaseRange)
        {
            monsterAi.destination = player.position;
        }
        else
        {
            behavior = E_Monster_Behavior.SEARCH;
        }
    }
    private Vector3 lastPlayerPostion;
        
    private void SearchForPlayer()
    {
        if (transform.position.x == monsterAi.destination.x && transform.position.z == monsterAi.destination.z&& player.position!= lastPlayerPostion)
        {
            lastPlayerPostion = player.position;
            monsterAi.destination = FindDestination();
        }
    }

    private Vector3 FindDestination()
    {
        float distance = 1000000000f;
        Vector3 destinationPostion = Vector3.zero;
        foreach (var destination in destinations)
        {
            float destinationDistance = Vector3.Distance(player.position, destination.position);
            if (distance > destinationDistance)
            {
                distance = destinationDistance;
                destinationPostion = destination.position;
            }
        }
        return destinationPostion;
    }
}
