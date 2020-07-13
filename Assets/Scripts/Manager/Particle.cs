using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private float playTime = 0.0f;
    
    private void Awake() {
        particleSystem = GetComponent<ParticleSystem>();

    }
    // Start is called before the first frame update
    private void OnEnable() {
        playTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        playTime += Time.deltaTime;

        if(playTime >= particleSystem.main.duration)
        {
            //Destroy(gameObject);      
            transform.SetParent(EffectManager.instance.transform);
            gameObject.SetActive(false);
        }
    }
}
