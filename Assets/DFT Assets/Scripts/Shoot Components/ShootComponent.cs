///----------------------------------------------------------------------
/// @file BulletShootComponent.cs
///
/// This file contains the declaration of BulletShootComponent class.
/// 
/// Generic shoot component.
/// If a specification of this component is needed, it can be extended by other MonoBehavior scripts
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 23/07/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections;

public abstract class ShootComponent : MonoBehaviour
{
    #region Public params

    /// <summary>
    /// Reference to the shoot point
    /// </summary>
    public Transform m_ShootPoint = null;

    /// <summary>
    /// Time between shots
    /// </summary>
    public float m_ShootDelay = 1.0f;

    /// <summary>
    /// Shoot damage. To be configured in BulletController
    /// </summary>
    public float m_Damage = 10.0f;

    public virtual bool CanShoot
    {
        get { return m_TimeAcum >= m_ShootDelay; }
    }

    #endregion


    #region Private params

    /// <summary>
    /// Time acum to count time between shots
    /// </summary>
    protected float m_TimeAcum = 0.0f;

    #endregion

    #region Public methods

    /// <summary>
    /// Generic method used to shoot. A specification is needed
    /// </summary>
    /// <param name="target">Position to shoot</param>
    /// <param name="GOTarget">GameObject reference to shoot</param>
    public abstract void Shoot(Vector3 target, GameObject GOTarget);

    #endregion


    #region MonoBehavior calls

    /// <summary>
    /// Update acumulated time
    /// </summary>
    protected virtual void Update()
    {
        m_TimeAcum += Time.deltaTime;
    }

    #endregion
}
