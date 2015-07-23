///----------------------------------------------------------------------
/// @file EnemiesHUDController.cs
///
/// This file contains the declaration of EnemiesHUDController class.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 23/07/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemiesHUDController : MonoBehaviour {

    #region Public params

    /// <summary>
    /// Reference to the HUD indicator of total wave enemies
    /// </summary>
    public Text m_TotalEnemiesText = null;

    /// <summary>
    /// Reference to the HUD indicator of current number of enemies to kill
    /// </summary>
    public Text m_CurrentEnemiesText = null;

    #endregion

    #region Public methods

    /// <summary>
    /// Method used to update the number of enemies
    /// </summary>
    /// <param name="currentEnemies">Remaining number of enemies of the current wave</param>
    /// <param name="totalEnemies">Total number of enemies in the current wave</param>
    public void UpdateHUD(string currentEnemies, string totalEnemies)
    {
        m_TotalEnemiesText.text = totalEnemies;
        m_CurrentEnemiesText.text = currentEnemies;
    }

    #endregion
}
