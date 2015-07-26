///----------------------------------------------------------------------
/// @file CrystalTrigger.cs
///
/// This file contains the declaration of CrystalTrigger class.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 22/07/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections;

public class CrystalTrigger : MonoBehaviour
{

    #region Monobehavior calls

    /// <summary>
    /// When Colliding with an enemy, the crystal will be destroyed
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            PoolManager.Singleton.destroyInstance(gameObject);
            CrystalManager.Singleton.CrystalDestroyed();
        }
            
    }

    #endregion
}
