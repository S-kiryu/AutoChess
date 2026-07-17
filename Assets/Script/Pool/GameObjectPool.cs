using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
    private readonly GameObject prefab;
    private readonly Transform root;
    private readonly Queue<GameObject> pool = new Queue<GameObject>();

    public GameObjectPool(GameObject prefab, int initialCount, Transform root = null)
    {
        this.prefab = prefab;
        this.root = root;

        for (int i = 0; i < initialCount; i++)
        {
            GameObject obj = Create();
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject Get(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject obj = pool.Count > 0 ? pool.Dequeue() : Create();

        Transform objTransform = obj.transform;
        objTransform.SetParent(parent != null ? parent : root, false);
        objTransform.SetPositionAndRotation(position, rotation);

        obj.SetActive(true);
        return obj;
    }

    public void Release(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }

        obj.SetActive(false);
        obj.transform.SetParent(root, false);
        pool.Enqueue(obj);
    }

    private GameObject Create()
    {
        return Object.Instantiate(prefab, root);
    }
}