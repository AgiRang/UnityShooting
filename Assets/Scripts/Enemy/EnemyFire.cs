using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    [HideInInspector]
    public bool isFire = false;

    private Animator animator;

    private Transform playerTr;

    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");

    private float nextFire = 0.0f;
    private readonly float fireRate = 0.1f;
    private readonly float damping = 10.0f;

    private readonly float reloadTime = 2.0f;
    private readonly int maxBullet = 10;
    private int curBullet = 10;
    private bool isReload = false;

    private WaitForSeconds wsReload;

    private GameObject bulletPrefab;
    private Transform firePos;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");

        Transform[] transforms = GetComponentsInChildren<Transform>();

        for (int i = 0; i < transforms.Length; i++)
        {
            if (transforms[i].name == "FirePos")
                firePos = transforms[i];
        }
    }

    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;

        wsReload = new WaitForSeconds(reloadTime);
    }

    private void Fire()
    {
        animator.SetTrigger(hashFire);
        SoundManager.instance.Play(SoundKey.SHOT, transform);

        //GameObject bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        BulletManager.instance.Fire(firePos, "EnemyBullet");
        EffectManager.instance.Play("MuzzleFlash", firePos);

        isReload = (--curBullet % maxBullet == 0);

        if (isReload)
            StartCoroutine(Reloading());
    }

    private IEnumerator Reloading()
    {
        animator.SetTrigger(hashReload);
        SoundManager.instance.Play(SoundKey.RELOAD, transform);

        yield return wsReload;

        curBullet = maxBullet;
        isReload = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isReload && isFire)
        {
            if(Time.time >= nextFire)
            {
                Fire();
                nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f);
            }

            Quaternion rot = Quaternion.LookRotation(playerTr.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * damping);
        }
    }
}
