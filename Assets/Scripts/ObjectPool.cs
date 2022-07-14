using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int initailSize = 20;

    private Queue<GameObject> m_pool = new Queue<GameObject>();

    void Awake()
    {
        for (int cnt = 0; cnt < initailSize; cnt++)
        {
            GameObject go = Instantiate(prefab) as GameObject;
            m_pool.Enqueue(go); go.SetActive(false);
        }
    }

    public GameObject Get()
    {
        GameObject go = null;

        if (m_pool.Count > 0)
        {
            go = m_pool.Dequeue();
            go.SetActive(true);
        }
        else
        {
            go = Instantiate(prefab) as GameObject;
        }

        return go;
    }


    public void Return(GameObject recovery)
    {
        recovery.transform.position = Vector3.zero;
        recovery.transform.rotation = Quaternion.identity;
        m_pool.Enqueue(recovery);
        recovery.SetActive(false);
    }
}
