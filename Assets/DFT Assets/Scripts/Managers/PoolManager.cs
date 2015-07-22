//----------------------------------------------------------------------
// @file PoolManager.cs
//
// This file contains the declaration of PoolManager class.
// Used as a pool of gameObjects.
//
// @author Alberto Martinez Villaran <tukaram92@gmail.com>
// @date 21/07/2015
//----------------------------------------------------------------------


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour {

	#region Singleton

	/// <summary>
	/// Singleton instance of the class
	/// </summary>
	private static PoolManager m_Instance = null;

	/// <summary>
	/// Property to get the singleton instance of the class.
	/// </summary>
	public static PoolManager Singleton { get { return m_Instance; } }

	// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
	static PoolManager() { }

	/// <summary>
	/// This is like the Init but done by the MonoBehaviour
	/// </summary>
	private void Awake()
	{
		if (m_Instance == null)
        {
            m_Instance = this;
            InitPool();
        }
		else
		{
			Debug.LogError("Someone is trying to create various ClassName [" + name + "]");
			this.enabled = false;
		}
	}

	#endregion

	#region Public params

    /// <summary>
    /// Parent Transform of the scene
    /// </summary>
    public Transform m_SceneTransform = null;

    /// <summary>
    /// Parent transform for the objects deactivated in the pool
    /// </summary>
    public Transform m_PoolTransform = null;

	/// <summary>
	/// list with the game object to instantiate and the number of instances for this object
	/// </summary>
	public List<PoolObjectCfg> m_Objects = new List<PoolObjectCfg>();

    /// <summary>
    /// pool with the game objects to instantiate
    /// </summary>
    public Dictionary<string, List<GameObject>> m_Pool = new Dictionary<string, List<GameObject>>();

	#endregion

	#region Private methods

    /// <summary>
    /// At the initialization, the pool of gameObjects will be filled
    /// </summary>
	private void InitPool()
	{
		//We fill the pool
		foreach (PoolObjectCfg obj in m_Objects)
		{
			GameObject go = obj.m_GameObject;
			PoolObject component = go.GetComponent<PoolObject>();
			if (component != null)
			{
				List<GameObject> listgo = new List<GameObject>();

				for (int i = 0; i < obj.m_NumInstances; ++i)
				{
					GameObject inst = (GameObject)GameObject.Instantiate(go);
					inst.SetActive(false);
                    inst.GetComponent<Transform>().parent = m_PoolTransform;
					listgo.Add(inst);
				}
				m_Pool.Add(component.PoolKey, listgo);
			}
		}
	}

	#endregion

	#region Exposed Methods

    /// <summary>
    /// This method will get an object from the pool. If there were any, it will be created instead
    /// </summary>
    /// <param name="go">Game Object to create</param>
    /// <param name="pos">position to create the GO</param>
    /// <param name="q">orientation of the GO</param>
    /// <returns>GameObject instantiated in the scene</returns>
	public GameObject getInstance(GameObject go, Vector3 pos, Quaternion q)
	{
		GameObject ret = null;
		PoolObject component = go.GetComponent<PoolObject>();

		if (m_Pool.ContainsKey(component.PoolKey))
		{
			int count = m_Pool[component.PoolKey].Count;
			if (count > 0)
			{
				ret = m_Pool[component.PoolKey][count - 1];
				m_Pool[component.PoolKey].Remove(ret);
                ret.transform.position = pos;
                ret.transform.rotation = q;
				ret.SetActive(true);

                ret.GetComponent<Transform>().parent = m_SceneTransform;
			}
			else
			{
                ret = (GameObject)GameObject.Instantiate(go, pos, q);
				ret.SetActive(true);

                ret.GetComponent<Transform>().parent = m_SceneTransform;
			}
		}
		else
		{
			Debug.LogWarning("GameObject not indexed in the PoolManager");
            ret = (GameObject)GameObject.Instantiate(go, pos, q);
            ret.SetActive(true);

            ret.GetComponent<Transform>().parent = m_SceneTransform;
		}
		return ret;
	}


    /// <summary>
    /// This method will get an object from the pool. If there were any, it will be created instead
    /// </summary>
    /// <param name="go">Game Object to create</param>
    /// <param name="pos">position to create the GO</param>
    /// <returns>GameObject instantiated in the scene</returns>
    public GameObject getInstance(GameObject go, Vector3 pos)
    {
        GameObject ret = null;
        PoolObject component = go.GetComponent<PoolObject>();

        if (m_Pool.ContainsKey(component.PoolKey))
        {
            int count = m_Pool[component.PoolKey].Count;
            if (count > 0)
            {
                ret = m_Pool[component.PoolKey][count - 1];
                m_Pool[component.PoolKey].Remove(ret);
                ret.transform.position = pos;
                ret.transform.rotation = go.GetComponent<Transform>().rotation;
                ret.SetActive(true);

                ret.GetComponent<Transform>().parent = m_SceneTransform;
            }
            else
            {
                ret = (GameObject)GameObject.Instantiate(go, pos, go.GetComponent<Transform>().rotation);
                ret.SetActive(true);

                ret.GetComponent<Transform>().parent = m_SceneTransform;
            }
        }
        else
        {
            Debug.LogWarning("GameObject not indexed in the PoolManager");
            ret = (GameObject)GameObject.Instantiate(go, pos, go.GetComponent<Transform>().rotation);
            ret.SetActive(true);

            ret.GetComponent<Transform>().parent = m_SceneTransform;
        }
        return ret;
    }

    /// <summary>
    /// This method "deletes" a gameObject from the scene. It will stored in the pool for a future use.
    /// </summary>
    /// <param name="go">GameObject to destroy from the scene</param>
	public void destroyInstance(GameObject go)
	{
		PoolObject component = go.GetComponent<PoolObject>();

        if (component != null)
        {
            go.SetActive(false);

            if (m_Pool.ContainsKey(component.PoolKey))
            {
                go.name = component.PoolKey;

                m_Pool[component.PoolKey].Add(go);

                go.GetComponent<Transform>().parent = m_PoolTransform;
            }
            else
            {
                GameObject.Destroy(go);
            }
        }
        else
        {
            GameObject.Destroy(go);
        }
		
	}

	#endregion
}
