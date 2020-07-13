using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FireCtrl : MonoBehaviour
{
    private GameObject bullet;
    private Transform firePos;    
    private GameObject bulletObj;
    // Start is called before the first frame update    

    public Image magazineImg;
    public Text magazineText;

    public int maxBullet = 10;
    public int remainingBullet = 10;

    public float reloadTime = 2.0f;

    public float fireRate = 0.1f;

    private bool isReloading = false;

    public Sprite[] weaponIcons;
    public Image weaponImage;

    private int curWeapon = 0;

    private int rayLayer;

    private bool isFire = false;
    private float nextFire;


    private void Awake() 
    {
        //bullet = (GameObject)Resources.Load("Prefabs/Bullet");
        //bullet = Resources.Load("Prefabs/Bullet") as GameObject;
        bullet = Resources.Load<GameObject>("Prefabs/Bullet");
        //muzzleFlash = GameObject.Find("MuzzleFlash01").GetComponent<ParticleSystem>();        
        
        Transform[] transforms = GetComponentsInChildren<Transform>();

        for(int i = 0 ; i < transforms.Length ; i++)
        {
            if(transforms[i].name == "FirePos")
                firePos = transforms[i];
        }
        /*
                foreach(Transform child in transforms)
                {
                    if(child.name == "FirePos")
                        firePos = child;
                }
                */

        rayLayer = LayerMask.GetMask("Enemy", "Obstacle");
    }
    void Start()
    {
        //firePos = GameObject.Find("FirePos").transform;
        //shake = Camera.main.transform.parent.GetComponent<Shake>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 20.0f, Color.green);

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        RaycastHit hit;

        if(Physics.Raycast(firePos.position, firePos.forward,
            out hit, 20.0f, rayLayer))
        {
            isFire = hit.collider.CompareTag("Enemy");
        }else
        {
            isFire = false;
        }

        //if(!isFire && Input.GetMouseButtonDown(0))        
        if (!isReloading && isFire)
        {
            if (Time.time > nextFire)
            {
                remainingBullet--;
                Fire();

                if (remainingBullet == 0)
                {
                    StartCoroutine(Reloading());
                }

                nextFire = Time.time + fireRate;
            }
        }

        /*
        if(isFire)
        {
            time += Time.deltaTime;

            if(time > 0.5f)
            {
                MeshRenderer mesh = bulletObj.GetComponentInChildren<MeshRenderer>();
                mesh.enabled = false;
                isFire = false;
            }
        }*/
    }

    private IEnumerator Reloading()
    {
        isReloading = true;
        SoundManager.instance.Play(SoundKey.RELOAD);

        yield return new WaitForSeconds(reloadTime);

        isReloading = false;
        magazineImg.fillAmount = 1.0f;
        remainingBullet = maxBullet;

        UpdateBulletText();
    }

    private void UpdateBulletText()
    {
        magazineText.text = string.Format("<color=#ff0000>{0}</color>/{1}", remainingBullet,
            maxBullet);
    }

    private void Fire()
    {
        //StartCoroutine(shake.ShakeCamera());
        StartCoroutine(Shake.instance.ShakeCamera());

        //bulletObj = Instantiate(bullet, firePos.position, firePos.rotation);
        BulletManager.instance.Fire(firePos);
        EffectManager.instance.Play("MuzzleFlash", firePos);
        //time = 0.0f;
        //isFire = true;

        SoundManager.instance.Play(SoundKey.SHOT, firePos);

        magazineImg.fillAmount = (float)remainingBullet / (float)maxBullet;
        UpdateBulletText();
    }

    public void OnChangeWeapon()
    {
        curWeapon = ++curWeapon % weaponIcons.Length;
        weaponImage.sprite = weaponIcons[curWeapon];
    }
}
