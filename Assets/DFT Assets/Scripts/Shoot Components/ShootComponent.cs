///----------------------------------------------------------------------
/// @file ShootComponent.cs
///
/// This file contains the declaration of ShootComponent class.
/// 
/// Generic shoot component, it will shoot a bullet/projectile every X seconds
/// If a specification of this component is needed, it can be extended by other MonoBehavior scripts
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 23/07/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections;

public class ShootComponent : MonoBehaviour {

    #region Public params

    /// <summary>
    /// Reference to the shoot point
    /// </summary>
    public Transform m_ShootPoint = null;

    /// <summary>
    /// Bullet to shoot
    /// </summary>
    public GameObject m_Bullet = null;

    /// <summary>
    /// Bullet speed. To be configured in BulletController
    /// </summary>
    public float m_BulletSpeed = 1.0f;

    /// <summary>
    /// Time between shots
    /// </summary>
    public float m_ShootDelay = 1.0f;

    /// <summary>
    /// Shoot damage. To be configured in BulletController
    /// </summary>
    public float m_Damage = 10.0f;

    /// <summary>
    /// True -> GameObject doesn't have to wait an initial m_ShootDelay time to shoot for first time
    /// False-> ... have to wait ...
    /// </summary>
    public bool m_FreshStart = true;

    public virtual bool CanShoot
    {
        get { return m_TimeAcum >= m_ShootDelay; }
    }

    #endregion

    #region Private params

    /// <summary>
    /// Time acum to count time between shots
    /// </summary>
    private float m_TimeAcum = 0.0f;

    #endregion

    #region Public methods

    /// <summary>
    /// Virtual method used to shoot a simple bullet. It will instatiate the gameObject and setup the it's configuration.
    /// </summary>
    /// <param name="target">Target to Shoot</param>
    /// <param name="verticalOffset">Vertical offset relative to target position (OPTIONAL)</param>
    public virtual void Shoot (GameObject target, float verticalOffset = 0.0f)
    {
        if (CanShoot)
        {
            Vector3 origin = m_ShootPoint.position;
            Vector3 destination = target.GetComponent<Transform>().position;
            destination.y += verticalOffset;

            Vector3 direction = destination - origin;

            GameObject bullet = PoolManager.Singleton.getInstance(m_Bullet, origin);

            BulletController controller = bullet.GetComponent<BulletController>();

            if (controller != null)
            {
                controller.setBulletConfiguration(m_BulletSpeed, direction, m_Damage);
            }

            m_TimeAcum = 0.0f;
        }
    }

    #endregion


    #region MonoBehavior calls

    /// <summary>
    /// Update acumulated time
    /// </summary>
    private void Update()
    {
        m_TimeAcum += Time.deltaTime;
    }
    
    /// <summary>
    /// Set-Up of fresh start
    /// </summary>
    private void OnEnable()
    {
        if (m_FreshStart)
        {
            m_TimeAcum = m_ShootDelay;
        }
    }

    #endregion
}
