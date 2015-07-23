///----------------------------------------------------------------------
/// @file TimeToLive.cs
///
/// This file contains the declaration of TimeToLive class.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 23/07/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections;

public class TimeToLive : MonoBehaviour {

    #region Public params

    /// <summary>
    /// Time to live. Time that object will be active in scene
    /// </summary>
    public float m_SecondsToLive = 1.0f;

    #endregion

    #region Private params

    /// <summary>
    /// Time acumulator
    /// </summary>
    private float m_TimeAcum = 0.0f;

    #endregion

    #region MonoBehavior calls

    /// <summary>
    /// Update time acumulator, if grater than time to live, then destroy the gameObject
    /// </summary>
    private void Update()
    {
        m_TimeAcum += Time.deltaTime;

        if (m_TimeAcum >= m_SecondsToLive)
        {
            PoolManager.Singleton.destroyInstance(gameObject);
        }
    }

    /// <summary>
    /// Reset variables
    /// </summary>
    private void OnEnable()
    {
        m_TimeAcum = 0.0f;
    }

    #endregion
}
