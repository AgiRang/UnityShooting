using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float damage = 20.0f;
    public float speed = 1000.0f;

    private Rigidbody rigidbody;
    private TrailRenderer trailRenderer;

    private void Awake() 
    {
        rigidbody = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void OnEnable() 
    {
        trailRenderer.Clear();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.AddForce(transform.forward * speed);
    }
}
