///----------------------------------------------------------------------
/// @file EnemyKilledComponent.cs
///
/// This file contains the declaration of EnemyKilledComponent class.
/// 
/// This simple script updates in SpawnerManager the quantity of enemies killed in the current wave, when the gameObject receives "onDie" message
/// 
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 23/07/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections;

public class EnemyKilledComponent : MonoBehaviour
{

    #region Public methods

    /// <summary>
    /// Increases the number of enemies killed in the current wave
    /// </summary>
    public void onDie()
    {
        SpawnerManager.Singleton.EnemyKilled();
    }

    #endregion
}
