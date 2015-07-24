///----------------------------------------------------------------------
/// @file BulletShootComponent.cs
///
/// This file contains the declaration of BulletShootComponent class.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 23/07/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections;

public class BulletShootComponent : ShootComponent {

    #region Public params

    /// <summary>
    /// Bullet to shoot
    /// </summary>
    public GameObject m_Bullet = null;

    /// <summary>
    /// Bullet speed. To be configured in BulletController
    /// </summary>
    public float m_BulletSpeed = 1.0f;

    /// <summary>
    /// True -> GameObject doesn't have to wait an initial m_ShootDelay time to shoot for first time
    /// False-> ... have to wait ...
    /// </summary>
    public bool m_FreshStart = true;

    #endregion

    #region Public methods

    /// <summary>
    /// This method will instantiate a gameObject an set-up all it's properties
    /// </summary>
    /// <param name="target">Position to shoot</param>
    /// <param name="GOTarget">GameObject reference to shoot</param>
    public override void Shoot(Vector3 target, GameObject GOTarget)
    {
        if (CanShoot)
        {
            Vector3 origin = m_ShootPoint.position;

            Vector3 direction = target - origin;

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
