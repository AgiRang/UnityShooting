using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float viewRange = 15.0f;

    [Range(0, 360)]
    public float viewAngle = 120.0f;

    private Transform playerTr;
    private int layerMask;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("Player", "Obstacle");
    }

    private void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public Vector3 CirclePoint(float angle)
    {
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(
            angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public bool IsTracePlayer()
    {
        bool isTrace = false;

        Collider[] colls = Physics.OverlapSphere(transform.position, 
            viewRange, LayerMask.GetMask("Player"));

        if(colls.Length > 0)
        {
            Vector3 dir = (playerTr.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, dir) < viewAngle * 0.5f)
            {
                isTrace = true;
            }
        }

        return isTrace;
    }

    public bool IsViewPlayer()
    {
        bool isView = false;
        RaycastHit hit;

        Vector3 dir = (playerTr.position - transform.position).normalized;

        if(Physics.Raycast(transform.position, dir, out hit, 
            viewRange, layerMask))
        {
            isView = hit.collider.CompareTag("Player");
        }

        return isView;
    }
}
