using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public static Shake instance;

    public bool shakeRotate = false;

    private Transform shakeCamera;

    private Vector3 originPos;
    private Quaternion originRot;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        shakeCamera = Camera.main.transform;
        originPos = shakeCamera.localPosition;
        originRot = shakeCamera.localRotation;
    }

    public IEnumerator ShakeCamera(float duration = 0.05f,
        float magnitudePos = 0.03f, float magnitudeRot = 0.1f)
    {
        float passTime = 0.0f;

        while(passTime < duration)
        {
            Vector3 shakePos = Random.insideUnitSphere;

            shakeCamera.localPosition = shakePos * magnitudePos;

            if(shakeRotate)
            {
                Vector3 shakeRot = new Vector3(0, 0,
                    Mathf.PerlinNoise(Time.deltaTime * magnitudeRot, 0.0f));

                shakeCamera.localRotation = Quaternion.Euler(shakeRot);                
            }

            passTime += Time.deltaTime;

            yield return null;
        }

        shakeCamera.localPosition = originPos;
        shakeCamera.localRotation = originRot;
    }
}
