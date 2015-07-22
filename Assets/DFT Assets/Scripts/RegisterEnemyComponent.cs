//----------------------------------------------------------------------
// @file RegisterEnemyComponent.cs
//
// This file contains the declaration of RegisterEnemyComponent class.
// This component registers and unregisters the gameObject as enemy in the EnemyManager
//
// @author Alberto Martinez Villaran <tukaram92@gmail.com>
// @date 22/07/2015
//----------------------------------------------------------------------



using UnityEngine;
using System.Collections;

public class RegisterEnemyComponent : MonoBehaviour
{

    #region Monobehavior calls

    private void OnEnable()
    {
        EnemyManager.Singleton.RegisterEnemy(gameObject);
    }

    private void OnDisable()
    {
        EnemyManager.Singleton.UnregisterEnemy(gameObject);
    }
       
    #endregion
}
