///----------------------------------------------------------------------
/// @file OnDieExplosion.cs
///
/// This file contains the declaration of OnDieExplosion class.
/// 
/// This script will spawn an explosion effect when the gameObject it's killed. Using "onDie" message
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 26/07/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections;

public class OnDieExplosion : MonoBehaviour {

    #region Public params

    /// <summary>
    /// Explosion prefab to be instantiated
    /// </summary>
    public GameObject m_ExplosionPrefab = null; 

    #endregion


    #region Public methods

    /// <summary>
    /// This method will be called by a message when the gameObject dies. In that moment we instantiate the explosion object in the scene
    /// </summary>
    public void onDie()
    {
        PoolManager.Singleton.getInstance(m_ExplosionPrefab, gameObject.GetComponent<Transform>().position, Quaternion.identity);
    }

    #endregion
}
