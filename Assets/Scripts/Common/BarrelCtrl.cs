using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    //public GameObject expEffect;
    public Mesh[] meshes;
    public Texture[] textures;

    private int hitCount = 0;
    private float expRadius = 10.0f;
    private Rigidbody rigidbody;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private void Awake() 
    {
        rigidbody = GetComponent<Rigidbody>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();        
    }

    private void Start() 
    {
        //meshRenderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
        meshRenderer.material.SetTexture("_BaseMap", textures[Random.Range(0, textures.Length)]);
    }

    private void ExpBarrel()
    {
        StartCoroutine(Shake.instance.ShakeCamera(1.0f, 0.5f, 0.2f));

        //Instantiate(expEffect, transform.position, Quaternion.identity);
        EffectManager.instance.Play("BigExplosion", transform);
        SoundManager.instance.Play(SoundKey.EXP, transform.position);

        rigidbody.mass = 1.0f;
        rigidbody.AddForce(Vector3.up * 500.0f);

        int index = Random.Range(0, meshes.Length);
        meshFilter.sharedMesh = meshes[index];

        IndirectDamage(transform.position);
    }

    private void IndirectDamage(Vector3 pos)
    {
        //int mask = LayerMask.GetMask("Barrel");
        int mask = LayerMask.NameToLayer("Barrel");        

        Collider[] colls = Physics.OverlapSphere(pos, expRadius, 1 << mask);

        foreach(Collider coll in colls)
        {
            Rigidbody rb = coll.GetComponent<Rigidbody>();
            rb.mass = 1.0f;
            rb.AddExplosionForce(500.0f, pos, expRadius, 500.0f);
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.collider.tag == "Bullet")
        {
            //hitCount++;
            if(++hitCount == 3)
            {
                ExpBarrel();
            }
        }
    }
}
