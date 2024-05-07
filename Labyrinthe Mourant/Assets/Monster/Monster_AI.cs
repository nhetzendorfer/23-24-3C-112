using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum E_Monster_Behavior { NONE = 0,SEARCH,CHASE }

public class Monster_AI : MonoBehaviour
{
    [Header("To Navigate")]
    public Transform player;
    public List<Transform> destinations;
    public NavMeshAgent monsterAi;
    [Header("Starte Delay")]
    public bool startDelay;
    public float starteDelayTime;
    [Header("Movement and such")]
    public float moveSpeed; // the enemy's move speed 
    public float sightDistance; // the distance at which the enemy starts chasing the player
    public float deathRange; // the distance at which the enemy kills the player
    public float chaseRange; // the distance at which the enemy kills the player
    public float rotationSpeed;
    public Transform mesh;
    [Header("Animation and sound")]
    public Animator animator;


    
    private E_Monster_Behavior behavior;
    void Start()
    {
        if (startDelay)
            Wait(starteDelayTime);
        behavior = E_Monster_Behavior.SEARCH;
        
    }

    private void Update()
    {
        //rotates the monster to first make it look in every direction
        //second to make it fit with the charakter
        mesh.position = transform.position;
        mesh.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
        //to see in wich direcion the player is
        Vector3 direction = (player.position - transform.position).normalized;
        //for raycast - the sight
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, sightDistance))
        {
            if (hit.collider.gameObject.tag == "player")
            {
                behavior = E_Monster_Behavior.CHASE;
            }
        }
        // calculate the distance between the enemy and the player
        float distance = Vector3.Distance(player.position, transform.position);
        if(behavior == E_Monster_Behavior.CHASE)
            ChasePlayer(distance); // method that holds the logic for enemy to chase player
        if (behavior == E_Monster_Behavior.SEARCH)
            SearchForPlayer();
        PlayerDeath(distance); // method that reloads the level when enemy catches player
    }

    private void PlayerDeath(float distance)
    {
        // if the distance is close enough to the player it reloads the scene
        if (distance < deathRange)
        {
            enabled = false;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    private void SearchForPlayer()
    {
        if (transform.position.x == monsterAi.destination.x && transform.position.z == monsterAi.destination.z)
        {
            float distance=1000000000f;
            Vector3 destinationCurrent=Vector3.zero;
            foreach (var destination in destinations)
            {
                float destinationDistance = Vector3.Distance(player.position, destination.position);
                if (distance > destinationDistance)
                {
                    distance = destinationDistance;
                    destinationCurrent = destination.position;
                }
            }
            //destinationCurrent.y = transform.position.y;
            monsterAi.destination = destinationCurrent;
        }
    }
    IEnumerator Wait(float time)
    {
        yield return new WaitForSecondsRealtime(time);
    }
}
