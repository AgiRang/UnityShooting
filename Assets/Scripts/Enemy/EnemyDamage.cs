using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    public Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);

    private GameObject hpBarPrefab;
    //private Canvas uiCanvas;
    private Image hpBarImage;

    private float hp = 100.0f;
    private float initHp = 100.0f;

    private Renderer[] renderers;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void OnEnable()
    {
        hp = initHp;
        if(hpBarImage != null)
            hpBarImage.fillAmount = 1.0f;

        StartCoroutine(CreateEffect());
    }

    private void Start()
    {
        //uiCanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();

        GameObject hpPanel = GameObject.Find("EnemyHpPanel");

        hpBarPrefab = Resources.Load<GameObject>("Prefabs/EnemyHpBar");

        GameObject hpBar = Instantiate<GameObject>(hpBarPrefab, hpPanel.transform);
        //GameObject hpBar = Instantiate<GameObject>(hpBarPrefab, uiCanvas.transform);
        hpBarImage = hpBar.GetComponentInChildren<Image>();
        hpBarImage.fillAmount = 1.0f;

        EnemyHpBar enemyHpBar = hpBar.GetComponent<EnemyHpBar>();
        enemyHpBar.target = transform;
        enemyHpBar.offset = hpBarOffset;        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Bullet"))
        {
            ContactPoint contact = collision.contacts[0];

            EffectManager.instance.Play("Damage",
                contact.point, Quaternion.identity);

            collision.gameObject.SetActive(false);

            //if (hp <= 0.0f)
            //return;

            //WeaponData weapon = DataManager.instance.GetWeaponData(1);
            //hp -= weapon.attack;
            hp -= GameManager.instance.gameData.damage;

            hpBarImage.fillAmount = hp / initHp;

            GetComponent<Animator>().SetTrigger("Damage");

            if(hp <= 0.0f)
            {
                GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
                GameManager.instance.IncKillCount();
                StartCoroutine(DissolveEffect());
            }
        }
    }

    private IEnumerator DissolveEffect()
    {
        float alpha = 0.0f;

        while(alpha < 1.0f)
        {
            alpha += Time.deltaTime * 0.2f;

            foreach(Renderer render in renderers)
            {
                render.material.SetFloat("AlphaValue", alpha);
            }            

            yield return null;
        }
    }

    private IEnumerator CreateEffect()
    {
        float alpha = 1.0f;

        while (alpha > 0.0f)
        {
            alpha -= Time.deltaTime * 0.5f;

            foreach (Renderer render in renderers)
            {
                render.material.SetFloat("AlphaValue", alpha);
            }

            yield return null;
        }
    }
}
