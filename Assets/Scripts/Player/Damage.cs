using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    public SkinnedMeshRenderer mesh;
    private Material material;

    private float initHp = 100.0f;
    public float curHp;

    public Image bloodScreen;
    public Image hpBar;

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    private float bloodScreenSpeed = 10.0f;

    private readonly Color initColor = new Color(0, 1, 0, 1);
    private Color curColor;

    private void Awake()
    {
        material = mesh.material;
    }

    void Start()
    {
        GameManager.OnItemChange += UpdateSetup;

        initHp = GameManager.instance.gameData.hp;
        curHp = initHp;

        bloodScreen.color = Color.clear;
        hpBar.color = initColor;
        curColor = initColor;
    }

    private void UpdateSetup()
    {
        curHp += GameManager.instance.gameData.hp - initHp;
        initHp = GameManager.instance.gameData.hp;        

        DisplayHpBar();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("EnemyBullet"))
        {
            collision.collider.gameObject.SetActive(false);

            curHp -= 5.0f;
            print("HP : " + curHp);

            if(curHp <= 0.0f)
            {
                print("Die");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            other.gameObject.SetActive(false);

            curHp -= 5.0f;
            print("HP : " + curHp);

            StartCoroutine(ShowBloodScreen());

            DisplayHpBar();

            if (curHp <= 0.0f)
            {
                PlayerDie();
            }
        }
    }

    private void PlayerDie()
    {
        /*
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in enemies)
        {
            enemy.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }*/
        OnPlayerDie();
    }

    private void DisplayHpBar()
    {
        if ((curHp / initHp) > 0.5f)
        {
            curColor.r = (1 - (curHp / initHp)) * 2.0f;
        }
        else
            curColor.g = (curHp / initHp) * 2.0f;

        hpBar.color = curColor;
        hpBar.fillAmount = (curHp / initHp);
    }

    private IEnumerator ShowBloodScreen()
    {
        bloodScreen.color = new Color(1, 0, 0, 0);

        bool isIncrease = true;

        material.SetFloat("Time", 0);

        do
        {
            Color color = bloodScreen.color;
            if (isIncrease)
                color.a += Time.deltaTime * bloodScreenSpeed;                            
            else
                color.a -= Time.deltaTime * bloodScreenSpeed;

            material.SetFloat("Time", color.a);

            if (isIncrease && color.a >= 1.0f)
                isIncrease = false;

            bloodScreen.color = color;

            yield return null;
        } while (bloodScreen.color.a > 0.0f);

        material.SetFloat("Time", 0);
    }
}
