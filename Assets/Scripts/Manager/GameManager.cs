using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataInfo;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;    

    public bool isGameOver = false;    
    public int enemyActiveCount = 0;    

    public CanvasGroup inventoryCG;

    public int killCount = 0;
    public Text killCountText;

    private Transform[] points;

    private float createTime = 2.0f;
    private int maxEnemy = 10;    

    private List<GameObject> enemies = new List<GameObject>();

    private bool isPaused;

    //public GameData gameData;

    public delegate void ItemChangeDelegate();
    public static event ItemChangeDelegate OnItemChange;

    private GameObject slotList;
    public GameObject[] itemObjects;

    public GameDataObject gameData;
    private void Awake()
    {
        instance = this;

        BulletManager.instance.CreateBullet();
        EffectManager.instance.CreateEffect();        
    }

    private void Start()
    {
        slotList = inventoryCG.transform.Find("SlotList").gameObject;

        LoadGameData();

        SoundManager.instance.PlayBgm(SoundKey.PLAY);

        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();

        CreateEnemy();
        StartCoroutine(RespawnEnemy());

        OnInventoryOpen(false);        
    }

    private void CreateEnemy()
    {
        GameObject enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");

        for(int i = 0; i < maxEnemy; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform);
            enemy.SetActive(false);
            enemies.Add(enemy);
        }
    }

    private IEnumerator RespawnEnemy()
    {
        while(!isGameOver)
        {
            if(enemyActiveCount < maxEnemy)
            {
                yield return new WaitForSeconds(createTime);

                int index = Random.Range(1, points.Length);

                foreach(GameObject enemy in enemies)
                {
                    if(!enemy.activeSelf)
                    {
                        enemy.SetActive(true);
                        enemy.transform.position = points[index].position;
                        enemy.transform.rotation = points[index].rotation;
                        break;
                    }
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    public void OnPauseClick()
    {
        isPaused = !isPaused;

        Time.timeScale = (isPaused) ? 0.0f : 1.0f;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
            script.enabled = !isPaused;

        //CanvasGroup canvasGroup = GameObject.Find("WeaponPanel").GetComponent<CanvasGroup>();
        //canvasGroup.blocksRaycasts = !isPaused;
        Button button = GameObject.Find("WeaponPanel").GetComponent<Button>();
        button.interactable = !isPaused;
    }

    public void OnInventoryOpen(bool isOpened)
    {
        inventoryCG.alpha = (isOpened) ? 1.0f : 0.0f;
        inventoryCG.interactable = isOpened;
        inventoryCG.blocksRaycasts = isOpened;
    }

    private void LoadGameData()
    {
        /*
        //killCount = PlayerPrefs.GetInt("KILL_COUNT", 0);
        gameData = DataManager.instance.Load();
        
        killCountText.text = "KILL " + gameData.killCount;
        */

        if(gameData.equipItem.Count > 0)
        {
            InventorySetup();
        }

        killCountText.text = "KILL " + gameData.killCount;
    }
    private void InventorySetup()
    {
        Transform[] slots = slotList.GetComponentsInChildren<Transform>();

        for(int i = 0; i < gameData.equipItem.Count; i++)
        {
            for(int j = 1; j < slots.Length; j++)
            {
                if (slots[j].childCount > 0)
                    continue;

                int itemIndex = (int)gameData.equipItem[i].itemType;

                itemObjects[itemIndex].transform.SetParent(slots[j]);
                itemObjects[itemIndex].GetComponent<ItemInfo>().itemInfo = gameData.equipItem[i];
                break;
            }
        }
    }

    private void SaveGameData()
    {
        //DataManager.instance.Save(gameData);
        //UnityEditor.EditorUtility.SetDirty(gameData);
    }

    public void IncKillCount()
    {
        //killCount++;
        //killCountText.text = "KILL " + killCount;
        //PlayerPrefs.SetInt("KILL_COUNT", killCount);        
        gameData.killCount++;
        killCountText.text = "KILL " + gameData.killCount;
    }    

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    public void AddItem(Item item)
    {
        if (gameData.equipItem.Contains(item))
            return;

        gameData.equipItem.Add(item);

        switch (item.itemType)
        {
            case Item.ItemType.HP:
                gameData.hp += item.value;
                break;
            case Item.ItemType.SPEED:
                gameData.speed += gameData.speed * item.value;
                break;            
            case Item.ItemType.DAMAGE:
                gameData.damage += gameData.damage * item.value;
                break;
            default:
                break;
        }

        OnItemChange();
    }

    public void RemoveItem(Item item)
    {
        gameData.equipItem.Remove(item);

        switch (item.itemType)
        {
            case Item.ItemType.HP:
                gameData.hp -= item.value;
                break;
            case Item.ItemType.SPEED:
                gameData.speed = gameData.speed / (1.0f + item.value);
                break;            
            case Item.ItemType.DAMAGE:
                gameData.damage = gameData.damage / (1.0f + item.value);
                break;
            default:
                break;
        }

        OnItemChange();
    }
}
