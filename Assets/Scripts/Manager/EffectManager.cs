using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    static private EffectManager _instance;
    static public EffectManager instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("EffectManager");
                _instance = obj.AddComponent<EffectManager>();
            }

            return _instance;
        }
    }

    private Dictionary<string, List<GameObject>> totalEffect =
        new Dictionary<string, List<GameObject>>();

    public void CreateEffect()
    {
        AddEffect(30, "BigExplosion");
        AddEffect(30, "SmallExplosion");
        AddEffect(20, "MuzzleFlash");
        AddEffect(30, "Damage");
    }

    private void AddEffect(int poolCount, string name)
    {
        GameObject prefab =
            Resources.Load<GameObject>("Prefabs/Particles/" + name);

        List<GameObject> effects = new List<GameObject>();
        for(int i = 0; i < poolCount; i++)
        {
            GameObject effect = Instantiate(prefab, transform);
            effect.AddComponent<Particle>();
            effect.SetActive(false);
            effect.name = name + i;

            effects.Add(effect);
        }

        totalEffect.Add(name, effects);
    }

    public void Play(string key, Transform pos)
    {
        //foreach (var effects in totalEffect)
        /*
        foreach(KeyValuePair<string, List<GameObject>> effects in totalEffect)        
        {
            if(effects.Key == key)
            {
                foreach(GameObject effect in effects.Value)
                {
                    if(!effect.activeSelf)
                    {
                        effect.SetActive(true);
                        effect.transform.position = pos.position;
                        effect.transform.rotation = pos.rotation;
                        effect.transform.localScale = pos.localScale;

                        effect.GetComponent<ParticleSystem>().Play();
                        return;
                    }
                }
            }
        }*/

        if (!totalEffect.ContainsKey(key))
            return;

        List<GameObject> effects = totalEffect[key];

        foreach (GameObject effect in effects)
        {
            if (!effect.activeSelf)
            {
                effect.SetActive(true);
                effect.transform.SetParent(pos);
                effect.transform.localPosition = Vector3.zero;
                effect.transform.localRotation = Quaternion.identity;                

                effect.GetComponent<ParticleSystem>().Play();
                return;
            }
        }
    }

    public void Play(string key, Vector3 pos, Quaternion rot)
    {       
        if (!totalEffect.ContainsKey(key))
            return;

        List<GameObject> effects = totalEffect[key];

        foreach (GameObject effect in effects)
        {
            if (!effect.activeSelf)
            {
                effect.SetActive(true);
                effect.transform.position = pos;
                effect.transform.rotation = rot;                

                effect.GetComponent<ParticleSystem>().Play();
                return;
            }
        }
    }
}
