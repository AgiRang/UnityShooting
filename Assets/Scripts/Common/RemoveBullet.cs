using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    //private GameObject smallExp;
    public bool isDissolve = false;

    private Renderer renderer;
    private void Awake() {
        //smallExp = Resources.Load<GameObject>("Prefabs/SmallExplosion");
        renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if(isDissolve)
        {
            renderer.material.SetFloat("DissolveAmount", 0.5f);
        }else
        {
            renderer.material.SetFloat("DissolveAmount", 0.0f);
        }
    }

    private void LateUpdate()
    {
        isDissolve = false;
    }

    private void OnCollisionEnter(Collision other) {
        if(other.collider.CompareTag("Bullet"))
        {
            ContactPoint contact = other.contacts[0];

            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

            //GameObject exp = Instantiate(smallExp, contact.point, rot);            
            EffectManager.instance.Play("SmallExplosion", contact.point, rot);
            SoundManager.instance.Play(SoundKey.REMOVE, contact.point);

            //ParticleSystem particle = exp.GetComponent<ParticleSystem>();
            //Destroy(exp, particle.main.duration);
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }

    private void OnCollisionStay(Collision other) {        

    }

    private void OnCollisionExit(Collision other) {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("EnemyBullet"))
        {   
            EffectManager.instance.Play("SmallExplosion",
                other.transform.position, other.transform.rotation);
            SoundManager.instance.Play(SoundKey.REMOVE, other.transform.position);
            other.gameObject.SetActive(false);
        }
    }    
}
