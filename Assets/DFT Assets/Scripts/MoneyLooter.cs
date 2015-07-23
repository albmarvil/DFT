///----------------------------------------------------------------------
/// @file MoneyLooter.cs
///
/// This file contains the declaration of MoneyLooter class.
///
/// This script will drop a given quantity of money when the gameObject gets KILLED
/// 
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 23/07/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections;

public class MoneyLooter : MonoBehaviour
{

    #region Public params

    /// <summary>
    /// Quantity of money to drop
    /// </summary>
    public float m_MoneyLoot = 5.0f;

    #endregion

    #region Public methods

    /// <summary>
    /// Add the money quantity to the MoneyManager
    /// </summary>
    public void onDie()
    {
        MoneyManager.Singleton.AddMoney(m_MoneyLoot);
    }

    #endregion
}
