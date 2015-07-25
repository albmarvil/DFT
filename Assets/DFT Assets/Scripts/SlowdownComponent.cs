///----------------------------------------------------------------------
/// @file SlowdownComponent.cs
///
/// This file contains the declaration of SlowdownComponent class.
///
/// This component is incharge of the "Slowdown state" of a gameObject with a NavigationAgent component. The gameObject can only be slowed down once
/// 
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 24/07/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavigationAgent))]
public class SlowdownComponent : MonoBehaviour {

    #region Public params

    /// <summary>
    /// Reference to a gameObject used to give visual feedback
    /// </summary>
    public GameObject m_Particles = null;

    #endregion

    #region Private params

    /// <summary>
    /// Reference to the navigation agent
    /// </summary>
    private NavigationAgent m_NavAgent = null;

    /// <summary>
    /// Original speed value
    /// </summary>
    private float m_OriginalSpeed = 0.0f;

    /// <summary>
    /// Flag that says if an object is slowed or not
    /// </summary>
    private bool m_Slowed = false;

    #endregion

    #region Public methods

    /// <summary>
    /// This method will slowdown the gameObject or set it to it's original speed
    /// </summary>
    /// <param name="slow">True to slowdown, false to set it's original speed again</param>
    public void Slowdown(bool slow, float multiplier)
    {

        if(!slow)
        {
            m_Slowed = slow;

            m_NavAgent.Speed = m_OriginalSpeed;

            m_Particles.SetActive(false);
        }
        else if (!m_Slowed)
        {
            m_Slowed = slow;

            m_OriginalSpeed = m_NavAgent.Speed;

            m_NavAgent.Speed = m_OriginalSpeed * multiplier;

            m_Particles.SetActive(true);
        }
    }

    #endregion

    #region Monobehavior calls

    private void OnEnable()
    {
        m_NavAgent = gameObject.GetComponent<NavigationAgent>();
        m_OriginalSpeed = m_NavAgent.Speed;
        m_Slowed = false;
        Slowdown(m_Slowed, 1.0f);
    }

    private void OnDisable()
    {
        m_Slowed = false;
        Slowdown(m_Slowed, 1.0f);
    }

    #endregion


}
