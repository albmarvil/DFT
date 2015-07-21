//----------------------------------------------------------------------
//
// @file PoolObjectCfg.cs
//
// This file contains the declaration of PoolObjectCfg class.
//
// @author Alberto Martinez Villaran  <tukaram92@gmail.com>
// @date 21/07/2015
//----------------------------------------------------------------------


using UnityEngine;
using System.Collections;


/// <summary>
/// Contains all the configuration of an Pool Object
/// </summary>
[System.Serializable]
public class PoolObjectCfg {

    #region Public params

    /// <summary>
    /// GameObject to instantiate in the Pool Manager
    /// </summary>
    public GameObject m_GameObject;


    /// <summary>
    /// Number of instaces to instatiate in the PoolManager
    /// </summary>
    public int m_NumInstances = 10;

    #endregion
}
