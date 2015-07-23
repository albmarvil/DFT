//----------------------------------------------------------------------
// @file RegisterCrystalComponent.cs
//
// This file contains the declaration of RegisterCrystalComponent class.
// This component registers and unregisters the gameObject as Crystal in the CrystalManager
//
// @author Alberto Martinez Villaran <tukaram92@gmail.com>
// @date 22/07/2015
//----------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class RegisterCrystalComponent : MonoBehaviour
{

    #region Monobehavior calls

    private void OnEnable()
    {
        if(CrystalManager.Singleton != null)
            CrystalManager.Singleton.RegisterCrystal(gameObject);
    }

    private void OnDisable()
    {
        if (CrystalManager.Singleton != null)
            CrystalManager.Singleton.UnregisterCrystal(gameObject);
    }
       
    #endregion
}


