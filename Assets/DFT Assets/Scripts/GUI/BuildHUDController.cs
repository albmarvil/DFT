///----------------------------------------------------------------------
/// @file BuildHUDController.cs
///
/// This file contains the declaration of BuildHUDController class.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 22/07/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BuildHUDController : MonoBehaviour {

	
    #region Public params

    /// <summary>
    /// Reference to the object that does the highlight
    /// </summary>
    public GameObject m_Highlight = null;

    /// <summary>
    /// List of references to all the building buttons
    /// </summary>
    public List<GameObject> m_BuildingButtons = null;

    /// <summary>
    /// Reference to the build text with the remaining time
    /// </summary>
    public Text m_BuildTime = null;

    #endregion

    #region Private params

    #endregion

    #region Public methods

    /// <summary>
    /// Method used to communicate with the game logic and draw some feedback about the building chosen.
    /// It will call BuilderManager to change the logic and highlight the correct option
    /// </summary>
    /// <param name="type">Type of building chosen</param>
    public void SelectBuilding(BuildingType type)
    {
        Vector3 pos = m_BuildingButtons[(int)type].GetComponent<Transform>().position;

        m_Highlight.GetComponent<Transform>().position = pos;

        BuilderManager.Singleton.SelectBuilding(type);
    }



    public void UpdateHUD(string time)
    {
        m_BuildTime.text = time;
    }

    #endregion



}
