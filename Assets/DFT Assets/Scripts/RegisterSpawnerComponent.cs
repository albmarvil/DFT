//----------------------------------------------------------------------
// @file RegisterSpawnerComponent.cs
//
// This file contains the declaration of RegisterSpawnerComponent class.
// This component registers and unregisters the gameObject as spawner in the SpawnerManager
//
// @author Alberto Martinez Villaran <tukaram92@gmail.com>
// @date 22/07/2015
//----------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class RegisterSpawnerComponent : MonoBehaviour
{

    #region Monobehavior calls

    private void OnEnable()
    {
        if(SpawnerManager.Singleton != null)
            SpawnerManager.Singleton.RegisterSpawner(gameObject);
    }

    private void OnDisable()
    {
        if (SpawnerManager.Singleton != null)
            SpawnerManager.Singleton.UnregisterSpawner(gameObject);
    }
       
    #endregion
}

