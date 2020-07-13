using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
    public List<Transform> wayPoints;

    private int nextIndex;

    private NavMeshAgent agent;

    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;
    private float damping = 1.0f;

    private bool _patrolling;

    public bool patrolling
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrolSpeed;
                damping = 1.0f;
                MoveWayPoint();
            }
        }
    }

    private Vector3 _traceTarget;

    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            damping = 7.0f;

            TraceTarget(_traceTarget);
        }
    }

    public float speed
    {
        get { return agent.velocity.magnitude; }
    }


    private void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale)
            return;

        agent.destination = pos;
        agent.isStopped = false;
    }


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.updateRotation = false;
    }

    private void OnEnable()
    {
        if(wayPoints.Count > 0)
            MoveWayPoint();
    }

    void Start()
    {
        GameObject group = GameObject.Find("WayPointGroup");
        group.GetComponentsInChildren<Transform>(wayPoints);
        wayPoints.RemoveAt(0);        
    }

    // Update is called once per frame
    void Update()
    {
        if(!agent.isStopped && agent.desiredVelocity != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * damping);
        }

        if (agent.velocity.sqrMagnitude >= 0.1f && agent.remainingDistance <= 0.5f)
        {
            nextIndex = Random.Range(0, wayPoints.Count);
            MoveWayPoint();
        }

    }

    private void MoveWayPoint()
    {
        if (agent.isPathStale)
            return;

        agent.destination = wayPoints[nextIndex].position;
        agent.isStopped = false;
    }

    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        patrolling = false;
    }
}
