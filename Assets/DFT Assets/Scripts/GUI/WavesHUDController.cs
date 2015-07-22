///----------------------------------------------------------------------
/// @file WavesHUDController.cs
///
/// This file contains the declaration of WavesHUDController class.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 22/07/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WavesHUDController : MonoBehaviour {

    #region Public params

    /// <summary>
    /// Reference to the HUD text
    /// </summary>
    public Text m_WaveText = null;

    #endregion


    #region Public methods

    public void UpdateHUD(int numWaves)
    {
        m_WaveText.text = numWaves.ToString();
    }

    #endregion
}
