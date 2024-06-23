using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//an example of an enemy script that can see and hear, and values can also be individually modified here or in the sense inspector
public class EnemyBehavior : MonoBehaviour
{
    enum States
    {
        StartPatrol,
        Patrol,
        Chase,
        Attack,
        Search
    }

    [SerializeField] States currentState;
    NavMeshAgent agent;
    Sight sight;
    Hearing hearing;
    Transform playerTrans;

    [SerializeField] Transform[] waypoints;
    [SerializeField] float patrolWait;
    [SerializeField] float patrolSpeed;
    [SerializeField] float patrolAcceleration;
    [SerializeField] float alertSpeed;
    [SerializeField] float alertAcceleration;
    [SerializeField] float attackAlertSpeed;
    [SerializeField] float attackRange;

    bool reachedDestination = false;
    int currentWaypointIndex = 0;

    BTNode BTRoot;
    Vector3 lastPos;
    bool isAlert;

    //BTSelector root = new(new BTNode[] {new BTCondition(()=>)})
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        sight = GetComponent<Sight>();
        hearing = GetComponent<Hearing>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        InitializeBT();
    }

    private void FixedUpdate()
    {
        BTRoot.Run();
    }

    //construct the behaviour tree of this enemy type, different enemy types can have a different tree
    //a visual representation of the tree is attached to the delivery
    //if the player is seen, chase and try to attack
    //if the player goes out of sight, investigate last seen position
    //if the player is heard, investigate the location of the sound
    //otherwise or if the investigation doesnt find the player, patrol.
    void InitializeBT()
    {
        BTRoot =
            new BTSelector(new BTNode[]{
                new BTSequence(new BTNode[]{
                    new BTCondition(()=>sight.IsTargetInVision),
                    new BTAction(()=>lastPos = sight.lastSeen),
                    new BTAction(()=>isAlert = true),
                    new BTSelector(new BTNode[]
                    {
                        new BTSequence(new BTNode[]
                        {
                            new BTCondition(()=>sight.targetDistance < attackRange),
                            new BTAction(Attack)
                        }),
                        new BTAction(Chase)
                    })
                }),
                new BTSequence(new BTNode[]
                {
                    new BTSelector(new BTNode[]
                    {
                        new BTSequence(new BTNode[]
                        {
                            new BTCondition(()=>hearing.isAlerted),
                            new BTAction(()=>lastPos = hearing.GetLastHeard()),
                            new BTAction(()=>isAlert = true),
                            
                        }),
                        new BTCondition(()=>isAlert),
                    }),
                    new BTAction(Search)
                }),
                new BTAction(Patrol)
            });
    }

    #region Patrol
    //patrol between predefined positions, wait a set amount of time when reaching position.
    //remaining distance updates a frame late, so it must be skipped when changing destination.
    void Patrol()
    {
        if (currentState != States.Patrol)  //code to run for only first frame of this state
        {
            currentState = States.Patrol;
            Debug.Log("Patrol");
            agent.speed = patrolSpeed;
            agent.acceleration = patrolAcceleration;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            agent.stoppingDistance = 0;
            reachedDestination = false;
        }
        else if (!reachedDestination && agent.remainingDistance < 0.1)  //reached destination condition, only run once when reaching
        {
            Debug.Log("Arrived at wp");
            reachedDestination = true;
            Invoke(nameof(SetNextWaypoint), patrolWait);
        }
    }

    //cycle to the next waypoint
    void SetNextWaypoint()
    {
        if (currentState != States.Patrol) return;  //if state was changed, ignore this method

        Debug.Log("Next wp");
        currentWaypointIndex++;
        if (currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = 0;
        }
        currentState = States.StartPatrol;    //force the Patrol method to skip the remaining distance check for a frame.
    }
    #endregion

    void Chase()
    {
        if (currentState != States.Chase)   //code to run for only first frame of this state
        {
            currentState = States.Chase;
            Debug.Log("Chase");
            agent.speed = alertSpeed;
            agent.acceleration = alertAcceleration;
            agent.stoppingDistance = attackRange;
        }
        agent.SetDestination(playerTrans.position); //follow the player
    }

    void Attack()
    {
        if (currentState != States.Attack)  //code to run for only first frame of this state
        {
            currentState = States.Attack;
            Debug.Log("Attack");
            agent.speed = attackAlertSpeed;
        }
        //attack logic would be here
        agent.SetDestination(playerTrans.position); //follow the player
    }

    void Search()
    {
        if (currentState != States.Search)  //code to run for only first frame of this state
        {
            currentState = States.Search;
            Debug.Log("Search");
            agent.SetDestination(lastPos);
            agent.speed = alertSpeed;
            agent.acceleration = alertAcceleration;
            agent.stoppingDistance = 0;
            reachedDestination = false;
        }
        else if (!reachedDestination && agent.remainingDistance < 0.1)  //reached destination condition, only run once
        {
            reachedDestination = true;
            StartCoroutine(nameof(LookAround));
        }
    }

    IEnumerator LookAround()
    {
        for (int i = 0; i < 4; i++)
        {
            transform.Rotate(0, 90, 0);
            yield return new WaitForSeconds(1);
            if (currentState != States.Search) break;
        }
        isAlert = false;
    }
}
