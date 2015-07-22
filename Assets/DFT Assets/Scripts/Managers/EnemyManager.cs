//----------------------------------------------------------------------
// @file EnemyManager.cs
//
// This file contains the declaration of EnemyManager class.
//
// All enemies will be registered in this manager
// So we can access them easily
//
// @author Alberto Martinez Villaran <tukaram92@gmail.com>
// @date 22/07/2015
//----------------------------------------------------------------------



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	#region Singleton

    /// <summary>
    /// Singleton instance of the class
    /// </summary>
    private static EnemyManager m_Instance = null;

    /// <summary>
    /// Property to get the singleton instance of the class.
    /// </summary>
    public static EnemyManager Singleton { get { return m_Instance; } }

    // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
    static EnemyManager() { }

    /// <summary>
    /// This is like the Init but done by the MonoBehaviour
    /// </summary>
    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        else
        {
            Debug.LogError("Someone is trying to create various EnemyManager [" + name + "]");
            this.enabled = false;
        }
    }
	
	/// <summary>
    /// This is like the Release but done by the MonoBehaviour
    /// </summary>
    private void OnDestroy()
    {
        if (m_Instance == this)
            m_Instance = null;
    }

    #endregion

    #region Public params

    /// <summary>
    /// Property to access to the enemy list
    /// </summary>
    public List<GameObject> Enemies
    {
        get { return m_Enemies; }
    }

    #endregion

    #region Private params

    /// <summary>
    /// List of all active enemies in scene
    /// </summary>
    private List<GameObject> m_Enemies = new List<GameObject>();

    #endregion

    #region Public methods

    /// <summary>
    /// This method registers an enemy in the manager, as an active enemy in the scene
    /// </summary>
    /// <param name="enemy">Enemy to register</param>
    public void RegisterEnemy(GameObject enemy)
    {
        if (!m_Enemies.Contains(enemy))
        {
            m_Enemies.Add(enemy);
        }
    }

    /// <summary>
    /// This method unregisters an enemy from the manager
    /// </summary>
    /// <param name="enemy">Enemy to unregister</param>
    public void UnregisterEnemy(GameObject enemy)
    {
        m_Enemies.Remove(enemy);
    }

    #endregion

    #region Private methods

    #endregion

    #region Monobehavior calls

    private void OnDisable()
    {
        m_Enemies.Clear();
    }

    #endregion

}
