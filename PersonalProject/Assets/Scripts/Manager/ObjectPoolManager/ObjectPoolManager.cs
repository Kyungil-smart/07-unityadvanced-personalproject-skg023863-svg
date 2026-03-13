using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    private Dictionary<GameObject, Queue<GameObject>> _pools = new Dictionary<GameObject, Queue<GameObject>>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        // _pools에 prefeb으로 등록된 키가 없으면 Queue 새로 만듬 
        if (!_pools.ContainsKey(prefab))
        {
            _pools.Add(prefab, new Queue<GameObject>());
        }

        GameObject obj;
     

        if (_pools[prefab].Count > 0)
        {
            obj = _pools[prefab].Dequeue();
        }
        else
        {
            // ObjectPoolManager 자식으로 Instantiate
            obj = Instantiate(prefab, transform);
            obj.name = prefab.name;
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        if (obj.TryGetComponent<IPoolable>(out IPoolable poolable))
        {
            poolable.OnSpawn();
        }

        return obj;
    }

    public void Release(GameObject prefab, GameObject obj)
    {
        if (!_pools.ContainsKey(prefab))
        {
            _pools.Add(prefab, new Queue<GameObject>());
        }

        if (obj.TryGetComponent<IPoolable>(out IPoolable poolable))
        {
            poolable.OnDespawn();
        }
        
        obj.SetActive(false);
        
        _pools[prefab].Enqueue(obj);
    }
}
