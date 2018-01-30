using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    public bool IsDynamic = true;
    public int StartSize = 100;
    public GameObject[] PrefabsToPool;
    public Dictionary<string, GameObject> ObjectsToPool;
    public Dictionary<string, List<GameObject>> ObjectPool;

    public void Start()
    {
        InitPoolDictionary();

        ConvertObjectArrayToDictionary();

        CreateInitialPool();
    }

    private void InitPoolDictionary()
    {
        ObjectPool = new Dictionary<string, List<GameObject>>();
        for (int i = 0; i < PrefabsToPool.Length; i++)
        {
            ObjectPool.Add(PrefabsToPool[i].name, new List<GameObject>());
        }
    }

    // Used to make thing easier, to get an object prefab just use the object name as the index
    // Faster than looping through the array of prefabs to get the required object
    private void ConvertObjectArrayToDictionary()
    {
        ObjectsToPool = new Dictionary<string, GameObject>();
        for (int i = 0; i < PrefabsToPool.Length; i++)
        {
            ObjectsToPool.Add(PrefabsToPool[i].name, PrefabsToPool[i]);
        }
    }

    // Creates an initial pool to use at game startup
    // The random creates an even amount of objects of each type to be used
    private void CreateInitialPool()
    {
        for (int i = 0; i < StartSize; i++)
        {
            string type = PrefabsToPool[UnityEngine.Random.Range(0, PrefabsToPool.Length)].name;
            GameObject obj = (GameObject)Instantiate(ObjectsToPool[type]);
            obj.SetActive(false);
            obj.transform.SetParent(this.transform);
            ObjectPool[type].Add(obj);
        }
    }

    public GameObject GetObjectOfType(string type)
    {
        if (ObjectPool.ContainsKey(type))
        {
            for (int i = 0; i < ObjectPool[type].Count; i++)
            {
                if (!ObjectPool[type][i].activeInHierarchy)
                {
                    return ObjectPool[type][i];
                }
            }
        }

        if (IsDynamic)
        {
            GameObject obj = (GameObject)Instantiate(ObjectsToPool[type]);
            obj.SetActive(false);
            ObjectPool[type].Add(obj);
            obj.transform.SetParent(this.transform);
            return obj;
        }

        return null;
    }

    public void DeactivateAllObjects()
    {
        foreach (List<GameObject> list in ObjectPool.Values)
        {
           for (int i = 0; i < list.Count; i++)
            {
                if (list[i].activeInHierarchy)
                {
                    list[i].SetActive(false);
                }
            }
        }
    }
}
