///----------------------------------------------------------------------
/// @file BuildingCfg.cs
///
/// This file contains the declaration of BuildingCfg class.
///
/// Configuration of a building: GameObject and cost
/// 
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 22/07/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections;

[System.Serializable]
public class BuildingCfg
{

    #region Public params

    /// <summary>
    /// Reference to the GameObjec of the building
    /// </summary>
    public GameObject m_Building = null;

    /// <summary>
    /// necessary money to build
    /// </summary>
    public float m_Cost = 0.0f;

    #endregion

}
