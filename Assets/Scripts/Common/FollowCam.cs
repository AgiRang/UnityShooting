using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    public float moveDamping = 15.0f;
    public float rotateDamping = 10.0f;
    public float distance = 5.0f;
    public float height = 4.0f;
    public float targetOffset = 2.0f;    

    private void Start()
    {
        //target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    /*
    private void LateUpdate()
    {
        Vector3 camPos = target.position
            - (target.forward * distance) + (target.up * height);

        transform.position = Vector3.Slerp(transform.position,
            camPos, Time.deltaTime * moveDamping);

        //y축회전
        //transform.rotation = Quaternion.Slerp(transform.rotation,
        //target.rotation, Time.deltaTime * rotateDamping);
        
        Quaternion destRot = target.rotation;                
        Vector3 targetPos = target.position + (target.up * targetOffset);
        Vector3 dir = (targetPos - transform.position).normalized;
        
        Vector3 rot = destRot.eulerAngles;
        float cosAngle = Vector3.Dot(dir, transform.forward);
        float angle = Mathf.Acos(cosAngle);
        rot.x = Mathf.Rad2Deg * angle;
        destRot.eulerAngles = rot;
        
        //destRot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(transform.rotation,
        destRot, Time.deltaTime * rotateDamping);
        
        //target바라보기
        transform.LookAt(target.position + (target.up * targetOffset));
    }*/

    private void Update()
    {        
        Vector3 dir = target.position - transform.position;

        RaycastHit hit;

        if(Physics.Raycast(transform.position, dir, out hit,
            dir.magnitude, LayerMask.GetMask("Wall")))
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            renderer.material.SetVector("Position", hit.point);
            renderer.material.SetFloat("DissolveAmount", 0.5f);

            hit.collider.GetComponent<RemoveBullet>().isDissolve = true;
        }
    }

    private void FixedUpdate()
    {
        Vector3 camPos = target.position
            - (target.forward * distance) + (target.up * height);

        transform.position = Vector3.Slerp(transform.position,
            camPos, Time.fixedDeltaTime * moveDamping);
        
        transform.LookAt(target.position + (target.up * targetOffset));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(target.position + (target.up * targetOffset), 0.1f);

        Gizmos.DrawLine(target.position + (target.up * targetOffset), transform.position);
    }
}
