using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private ObjectPoolManager()
    {

    }
    //单例外部调用
    public static ObjectPoolManager instance;
    //储存池配套的预制体
    public static Dictionary<string, GameObject> objectDic = new Dictionary<string, GameObject>();
    //储存不同的池
    public static Dictionary<string, Queue<GameObject>> poolDic = new Dictionary<string, Queue<GameObject>>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreateNewPool(string PoolName, GameObject Prefab, int PrefabNum = 3)
    {
        //如果池已存在，结束函以防字典重复添加
        if (poolDic.ContainsKey(PoolName))
            return;
        Queue<GameObject> pool;
        pool = new Queue<GameObject>();
        poolDic.Add(PoolName, pool);
        objectDic.Add(PoolName, Prefab);
        if (pool.Count == 0 && PrefabNum != 0)
        {
            AddObjectToPool(PoolName, PrefabNum, Prefab);
        }



    }

    public Queue<GameObject> GetPool(string PoolName)
    {
        Queue<GameObject> pool = null;
        if (poolDic.TryGetValue(PoolName, out pool))
        {
            return pool;
        }
        else
        {
            Debug.Log("未找到对象池");
            return null;
        }

    }

    public void AddObjectToPool(string PoolName, int num = 1, GameObject Prefab = null)
    {
        Queue<GameObject> pool = null;
        pool = GetPool(PoolName);
        if (pool != null)
        {
            GameObject gameObject;
            if (Prefab != null)
            {
                for (int i = 0; i < num; i++)
                {
                    gameObject = Instantiate(Prefab, this.transform);
                    gameObject.SetActive(false);
                    pool.Enqueue(gameObject);
                }
            }
            else if (Prefab == null)
            {
                GameObject prefab;
                if (objectDic.TryGetValue(PoolName, out prefab))
                {
                    for (int i = 0; i < num; i++)
                    {
                        gameObject = Instantiate(prefab, this.transform);
                        gameObject.SetActive(false);
                        pool.Enqueue(gameObject);
                    }

                }
                else
                {
                    Debug.Log("预制体池和传入预制体均为空添加对象失败");
                }
            }

        }
        else
        {
            Debug.Log("对象池中不存在这个池，添加对象失败");
        }
    }

    public void RemovePool(string PoolName)
    {
        Queue<GameObject> pool = null;
        if (poolDic.TryGetValue(PoolName, out pool))
        {
            poolDic.Remove(PoolName);
            pool.Clear();
            pool = null;
        }
        else
        {
            Debug.Log("未找到" + PoolName + "无法移除池");
        }
    }

    public GameObject GetObject(string PoolName)
    {
        Queue<GameObject> pool = null;
        GameObject obj = null;
        pool = GetPool(PoolName);
        if (pool != null)
        {
            if (pool.Count > 0)
                obj = pool.Dequeue();
            else if (pool.Count == 0)
            {
                AddObjectToPool(PoolName);
                if (pool.Count != 0)
                    obj = pool.Dequeue();
            }
            Debug.Log("对象池：" + PoolName + "的数量为：" + pool.Count);
        }
        return obj;
    }

    public void ReleaseObject(string PoolName, GameObject Object, GameObject Prefab = null)
    {
        Queue<GameObject> pool = null;
        pool = GetPool(PoolName);
        if (pool == null && Object != null)
        {
            pool = new Queue<GameObject>();
            poolDic.Add(PoolName, pool);
            pool = GetPool(PoolName);
        }
        if (GetPool(PoolName) == null)
        {

            Debug.Log("释放对象失败，该对象为有对象池：" + PoolName);
            return;
        }
        Object.transform.parent = gameObject.transform;
        Object.SetActive(false);
        pool.Enqueue(Object);



    }
}
