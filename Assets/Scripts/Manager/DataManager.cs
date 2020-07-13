using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using DataInfo;

public struct WeaponData
{
    public string name;
    public int attack;
    public int price;
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private string dataPath;

    private Dictionary<int, WeaponData> weaponData = new Dictionary<int, WeaponData>();
    private void Awake()
    {
        instance = this;

        LoadWeaponData();

        dataPath = Application.dataPath + "/Resources/TextData/gameData.dat";        
    }

    private void LoadWeaponData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TextData/WeaponTable");

        string temp = textAsset.text.Replace("\r\n", "\n");

        string[] row = temp.Split('\n');

        for(int i = 1; i < row.Length; i++)
        {
            string[] data = row[i].Split(',');
            int key = int.Parse(data[0]);

            WeaponData weapon;
            weapon.name = data[1];
            weapon.attack = int.Parse(data[2]);
            weapon.price = int.Parse(data[3]);

            weaponData.Add(key, weapon);
        }
    }

    public WeaponData GetWeaponData(int key)
    {
        return weaponData[key];
    }

    public void Save(GameData gameData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(dataPath);

        bf.Serialize(file, gameData);

        file.Close();
    }

    public GameData Load()
    {
        if(File.Exists(dataPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();

            return data;
        }else
        {
            GameData data = new GameData();
            return data;
        }
    }
}
