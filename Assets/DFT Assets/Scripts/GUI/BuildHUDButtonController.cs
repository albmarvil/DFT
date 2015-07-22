///----------------------------------------------------------------------
/// @file BuildHUDButtonController.cs
///
/// This file contains the declaration of BuildHUDButtonController class.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 22/07/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections;

public class BuildHUDButtonController : MonoBehaviour
{

    #region Public params

    /// <summary>
    /// Index that makes reference to which building has to build
    /// </summary>
    public BuildingType m_BuildingType = BuildingType.NONE;

    /// <summary>
    /// Reference to the BuildHUDController
    /// </summary>
    public BuildHUDController m_HUDController = null;

    #endregion

    #region Public methods

    public void OnButtonPressed()
    {
        m_HUDController.SelectBuilding(m_BuildingType);
    }

    #endregion
}
