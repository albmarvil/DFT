///----------------------------------------------------------------------
/// @file MoneyManager.cs
///
/// This file contains the declaration of MoneyManager class.
///
/// This script manages the game money, used to build turrets.
/// Gained killing enemies
/// 
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 23/07/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections;

public class MoneyManager : MonoBehaviour {

	#region Singleton

    /// <summary>
    /// Singleton instance of the class
    /// </summary>
    private static MoneyManager m_Instance = null;

    /// <summary>
    /// Property to get the singleton instance of the class.
    /// </summary>
    public static MoneyManager Singleton { get { return m_Instance; } }

    // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
    static MoneyManager() { }

    /// <summary>
    /// This is like the Init but done by the MonoBehaviour
    /// </summary>
    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        else
        {
            Debug.LogError("Someone is trying to create various MoneyManager [" + name + "]");
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
    /// Starting quantity of money
    /// </summary>
    public float m_StartMoney = 100.0f;

    /// <summary>
    /// Reference to the MoneyHUDController
    /// </summary>
    public MoneyHUDController m_MoneyHUDController = null;

    /// <summary>
    /// Current quantity of money
    /// </summary>
    public float Money
    {
        get { return m_CurrentMoney; }
    }

    #endregion

    #region Private params

    /// <summary>
    /// Current quantity of money
    /// </summary>
    private float m_CurrentMoney = 100.0f;

    #endregion

    #region Public methods

    /// <summary>
    /// This methods try to spend the given quantity of money.
    /// If can be done, it will return true, false instead
    /// </summary>
    /// <param name="money">Quantity of money to spend</param>
    /// <returns>True if can be spent, false instead</returns>
    public bool SpendMoney(float money)
    {
        bool res = money <= m_CurrentMoney;

        if (res)
        {
            m_CurrentMoney -= money;
            m_MoneyHUDController.UpdateHUD(m_CurrentMoney.ToString());
        }

        return res;
    }

    /// <summary>
    /// Add the given quantity of money
    /// </summary>
    /// <param name="money">Quantity of money to add</param>
    public void AddMoney(float money)
    {
        m_CurrentMoney += money;
        m_MoneyHUDController.UpdateHUD(m_CurrentMoney.ToString());
    }

    #endregion

    //#region Private methods

    //#endregion

    #region Monobehavior calls

    private void OnEnable()
    {
        m_CurrentMoney = m_StartMoney;
        m_MoneyHUDController.UpdateHUD(m_CurrentMoney.ToString());
    }

    #endregion


}
