//----------------------------------------------------------------------
// @file PoolObject.cs
//
// This file contains the declaration of PoolObject class.
// This file contains the key used to store the objects in the pool
//
// @author Alberto Martinez Villaran <tukaram92@gmail.com>
// @date 21/07/2015
//----------------------------------------------------------------------


using UnityEngine;
using System.Collections;

public class PoolObject : MonoBehaviour {

    #region Public params

    public string m_PoolKey = "";

    public string PoolKey
    {
        get { return m_PoolKey; }
    }

    #endregion
}
