///----------------------------------------------------------------------
/// @file MoneyHUDController.cs
///
/// This file contains the declaration of MoneyHUDController class.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 23/07/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoneyHUDController : MonoBehaviour
{

    #region Public params

    /// <summary>
    /// Reference to the UI element
    /// </summary>
    public Text m_CurrentMoney = null;

    #endregion

    #region Public methods

    /// <summary>
    /// Updates the money quantity show in the GUI
    /// </summary>
    /// <param name="money">money quantity</param>
    public void UpdateHUD(string money)
    {
        m_CurrentMoney.text = money;
    }

    /// <summary>
    /// Updates the color of the indicator if there isn't enough money the indicator will turn to red
    /// </summary>
    /// <param name="enough"></param>
    public void EnoughMoney(bool enough)
    {
        if (enough)
        {
            m_CurrentMoney.color = Color.white;
        }
        else
        {
            m_CurrentMoney.color = Color.red;
        }
    }

    #endregion
}
