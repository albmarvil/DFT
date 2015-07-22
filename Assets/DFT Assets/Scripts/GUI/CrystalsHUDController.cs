///----------------------------------------------------------------------
/// @file CrystalsHUDController.cs
///
/// This file contains the declaration of CrystalsHUDController class.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 22/07/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CrystalsHUDController : MonoBehaviour
{

    #region Public params

    public Text m_CurrentCrystals = null;

    public Text m_TotalCrystals = null;

    #endregion

    #region Public methods


    public void UpdateCrystalsHUD(int current, int total)
    {
        string sCurrent = current.ToString();

        string sTotal = total.ToString();

        m_CurrentCrystals.text = sCurrent;
        m_TotalCrystals.text = sTotal;
    }

    #endregion
}
