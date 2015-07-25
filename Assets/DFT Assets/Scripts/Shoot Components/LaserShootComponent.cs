///----------------------------------------------------------------------
/// @file LaserShootComponent.cs
///
/// This file contains the declaration of LaserShootComponent class.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 24/07/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserShootComponent : ShootComponent
{

    #region Public params

    /// <summary>
    /// Reference to the laser renderer
    /// </summary>
    public LineRenderer m_Laser = null;

    /// <summary>
    /// Reference to the end particle
    /// </summary>
    public GameObject m_ParticleEnd = null;

    /// <summary>
    /// reference to the end particle transform
    /// </summary>
    public Transform m_ParticleEndTransform = null;

    #endregion

    #region Private params

    /// <summary>
    /// Current target reference
    /// </summary>
    private GameObject m_CurrentTarget = null;

    /// <summary>
    /// Current target transform reference
    /// </summary>
    private Transform m_CurrentTargetTransform = null;

    /// <summary>
    /// Aprox. value of the offset given by the perception and the real position of the target
    /// </summary>
    private float m_AproxVerticalOffset = 0.0f;

    #endregion

    #region Public methods

    /// <summary>
    /// This method will draw the laser line with the laser renderer
    /// </summary>
    /// <param name="target">Position to Shoot</param>
    /// <param name="GOTarget">(OPTIONAL) GameObject reference to shoot</param>
    public override void Shoot(Vector3 target, GameObject GOTarget)
    {
        if (CanShoot)
        {
            m_CurrentTarget = GOTarget;
            
            m_CurrentTargetTransform = m_CurrentTarget.GetComponent<Transform>();
            m_AproxVerticalOffset = target.y - m_CurrentTargetTransform.position.y;


            UpdateLaserPosition();

            m_CurrentTarget.SendMessage("Damage", m_Damage, SendMessageOptions.DontRequireReceiver);

            m_TimeAcum = 0.0f;

        }
    }

    /// <summary>
    /// Process the message OnTargetLost, to update the laser state
    /// </summary>
    public void OnTargetLost()
    {
        m_CurrentTarget = null;
        m_Laser.gameObject.SetActive(false);
        m_ParticleEnd.SetActive(false);
    }

    #endregion


    #region Private methods
    /// <summary>
    /// Mehtod used to update origin and destination of the laser renderer
    /// </summary>
    private void UpdateLaserPosition()
    {
        m_Laser.gameObject.SetActive(true);
        m_ParticleEnd.SetActive(true);
        m_Laser.SetPosition(0, m_ShootPoint.position);
        m_Laser.SetPosition(1, m_CurrentTargetTransform.position + new Vector3(0, m_AproxVerticalOffset, 0));
        m_ParticleEndTransform.position = m_CurrentTargetTransform.position + new Vector3(0, m_AproxVerticalOffset, 0);

    }

    #endregion


    #region MonoBehavior calls

    /// <summary>
    /// While there is a curren enemy, the laser should draw correctly and update it's position
    /// </summary>
    protected override void Update()
    {
        base.Update();

        if (m_CurrentTarget != null && m_CurrentTarget.activeSelf)
        {
            UpdateLaserPosition();
        }
        else
        {
            m_CurrentTarget = null;
            m_Laser.gameObject.SetActive(false);
            m_ParticleEnd.SetActive(false);
        }
    }


    /// <summary>
    /// Set-Up of fresh start
    /// </summary>
    private void OnEnable()
    {
        m_CurrentTarget = null;
        m_CurrentTargetTransform = null;
        m_TimeAcum = 0.0f;
        m_AproxVerticalOffset = 0.0f;
    }

    #endregion
}
