using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>  
{
    //List of things that you want to be pooled
    private GameObject objectPoolContainer;
    private Dictionary<Utils.Collectable, List<GameObject>> pooledObjects = new Dictionary<Utils.Collectable, List<GameObject>>();

    [Header("UI Pool")]
    [SerializeField]
    private Transform canvas;
    [SerializeField]
    private ObjectToPool[] objectsToPool;
    [Serializable]
    public struct ObjectToPool
    {
        public Utils.Collectable collectable;
        public GameObject prefab;
        [Min(0)]
        public int amount;
    }

    protected override void Awake()
    {
        base.Awake();
        objectPoolContainer = new GameObject("ObjectPool");
        objectPoolContainer.transform.parent = canvas;

        foreach (ObjectToPool objectToPool in objectsToPool)
        {
            pooledObjects.Add(objectToPool.collectable, new List<GameObject>());

            if (objectToPool.prefab != null)
            {
                for (int i = 0; i < objectToPool.amount; i++)
                {
                    GameObject tmp = Instantiate(objectToPool.prefab, objectPoolContainer.transform);
                    tmp.SetActive(false);

                    pooledObjects[objectToPool.collectable].Add(tmp);
                }
            }    
        }
    }

    public bool TryGetPooledObject(Utils.Collectable collectable, out GameObject prefab)
    {
        prefab = null;
        if (!pooledObjects.ContainsKey(collectable))
            return false;

        foreach (GameObject obj in pooledObjects[collectable])
        {
            if (!obj.activeInHierarchy)
            {
                prefab = obj;
                return true;
            }
        }

        //Add more objects to the pool if there arent enough
        if(GetAvailablePrefab(collectable, out GameObject original))
        {
            GameObject tmp = Instantiate(original, objectPoolContainer.transform);
            tmp.SetActive(false);
            pooledObjects[collectable].Add(tmp);
            prefab = tmp;
            return true;
        }

        return false;
    }

    private bool GetAvailablePrefab(Utils.Collectable collectable, out GameObject prefab)
    {
        foreach (ObjectToPool objectToPool in objectsToPool)
        {
            if (objectToPool.collectable == collectable)
            {
                prefab = objectToPool.prefab;
                return true;
            }
        }

        prefab = null;
        return false;
    }

}
