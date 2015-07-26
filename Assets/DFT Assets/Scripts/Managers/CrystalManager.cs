///----------------------------------------------------------------------
/// @file CrystalManager.cs
///
/// This file contains the declaration of CrystalManager class.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 22/07/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrystalManager : MonoBehaviour {

	#region Singleton

    /// <summary>
    /// Singleton instance of the class
    /// </summary>
    private static CrystalManager m_Instance = null;

    /// <summary>
    /// Property to get the singleton instance of the class.
    /// </summary>
    public static CrystalManager Singleton { get { return m_Instance; } }

    // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
    static CrystalManager() { }

    /// <summary>
    /// This is like the Init but done by the MonoBehaviour
    /// </summary>
    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        else
        {
            Debug.LogError("Someone is trying to create various CrystalManager [" + name + "]");
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
    /// List of all the active crystals in the scene
    /// </summary>
    public List<GameObject> Crystals
    {
        get { return m_Crystals; }
    }


    /// <summary>
    /// Reference of the crystal hud to update their values
    /// </summary>
    public CrystalsHUDController m_CrystalsHUD = null;

    #endregion

    #region Private params

    /// <summary>
    /// List of all the active crystals in the scene
    /// </summary>
    private List<GameObject> m_Crystals = new List<GameObject>();

    /// <summary>
    /// Total number of crystals generated at the start
    /// </summary>
    private int m_total = 0;

    #endregion

    #region Public methods

    /// <summary>
    /// Total number of crystals in the current game
    /// </summary>
    /// <param name="total">total number of crystals</param>
    public void setTotalCrystals(int total)
    {
        m_total = total;
    }

    /// <summary>
    /// Registers a crystal in the manager
    /// </summary>
    /// <param name="crystal">Crystal to register</param>
    public void RegisterCrystal(GameObject crystal)
    {
        if (!m_Crystals.Contains(crystal))
        {
            m_Crystals.Add(crystal);

            m_CrystalsHUD.UpdateCrystalsHUD(m_Crystals.Count, m_total);
        }
    }

    /// <summary>
    /// Unregisters a crystal in the manager
    /// </summary>
    /// <param name="crystal">Crystal to uynregister</param>
    public void UnregisterCrystal(GameObject crystal)
    {
        m_Crystals.Remove(crystal);

        m_CrystalsHUD.UpdateCrystalsHUD(m_Crystals.Count, m_total);
    }

    /// <summary>
    /// Method used when a crystal is destroyed
    /// </summary>
    public void CrystalDestroyed()
    {
        --m_total;

        if (m_total <= 0)
        {
            GameManager.Singleton.EndGame();
        }
    }

    #endregion

}
