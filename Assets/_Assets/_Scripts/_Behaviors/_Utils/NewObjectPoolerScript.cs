using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewObjectPoolerScript : MonoBehaviour 
{
    public GameObject pooledObject;
    public int pooledAmout = 20;
    public bool willGrow = true;

    List<GameObject> pooledObjects;

    // Use this for initialization
    void Awake () {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmout; i++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
			obj.name = pooledObject.name + "_" + i.ToString();
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
	}

    public GameObject GetPooledObject()
    {
        if (pooledObjects == null)
            return null;

        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (willGrow)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            pooledObjects.Add(obj);
            return obj;
        }

        return null;
    }
}
