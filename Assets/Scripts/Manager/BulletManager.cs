using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    static private BulletManager _instance;
    static public BulletManager instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject obj = new GameObject("BulletManager");
                _instance = obj.AddComponent<BulletManager>();
            }

            return _instance;
        }
    }

    public int poolCount = 50;

    private GameObject bulletPrefab;
    private List<GameObject> bullets = new List<GameObject>();

    private void Awake()
    {
        //_instance = this;
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
    }

    public void CreateBullet()
    {
        for (int i = 0; i < poolCount; i++)
        {
            //GameObject bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
            //bullet.transform.SetParent(transform);
            GameObject bullet = Instantiate(bulletPrefab, transform);
            bullet.SetActive(false);
            bullets.Add(bullet);
        }
    }
  
    public void Fire(Transform firePos, string tag = "Bullet")
    { 
        foreach(GameObject bullet in bullets)
        {
            if (!bullet.activeSelf)
            {                
                bullet.transform.tag = tag;
                bullet.transform.position = firePos.position;
                bullet.transform.rotation = firePos.rotation;
                bullet.SetActive(true);

                if(tag == "Bullet")
                {
                    bullet.GetComponent<Collider>().isTrigger = false;
                }else
                {
                    bullet.GetComponent<Collider>().isTrigger = true;
                }
                break;
            }
        }     
    }
}
