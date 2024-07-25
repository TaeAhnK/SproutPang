using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    public GameObject prefab;

    public Queue<GameObject> pool = new Queue<GameObject>();

    public ObjectPool(GameObject prefab, int count)
    {
        this.prefab = prefab;

        for (int i = 0; i < count; i++)
        {
            pool.Enqueue(CreateNewObject());
        }
    }

    private GameObject CreateNewObject()
    {
        if (prefab == null)
        {
            // Error
            return null;
        }

        var obj = Object.Instantiate(prefab);
        obj.SetActive(false);

        return obj;
    }

    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            var obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            var obj = CreateNewObject();
            obj.SetActive(true);
            return obj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

}
