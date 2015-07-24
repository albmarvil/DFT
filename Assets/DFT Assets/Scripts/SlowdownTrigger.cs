///----------------------------------------------------------------------
/// @file SlowdownTrigger.cs
///
/// This file contains the declaration of SlowdownTrigger class.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 24/07/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections;

public class SlowdownTrigger : MonoBehaviour {

    /// <summary>
    /// Speed multiplier applied when the gameObject is in slow state.
    /// This number should be in [0, 1] interval
    /// 0.6 means that the speed will be 0.6 times than the original one
    /// </summary>
    public float m_SlowdownMultiplier = 0.5f;

    #region MonoBehavior calls

    /// <summary>
    /// Checks if the slowdown multiplier is correct (max value 1.0f)
    /// </summary>
    private void OnEnable()
    {
        m_SlowdownMultiplier = Mathf.Min(1.0f, m_SlowdownMultiplier);
    }

    /// <summary>
    /// If a gameObject enters in the trigger it will be slowed down
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        SlowdownComponent component = other.gameObject.GetComponent<SlowdownComponent>();
            if(component != null)
                component.Slowdown(true, m_SlowdownMultiplier);
    }


    /// <summary>
    /// If a gameObject exits the trigger it's speed will be reset
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        SlowdownComponent component = other.gameObject.GetComponent<SlowdownComponent>();
        if (component != null)
            component.Slowdown(false, 1.0f);
    }

    /// <summary>
    /// If a gameObject enters in the trigger it will be slowed down
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        SlowdownComponent component = other.gameObject.GetComponent<SlowdownComponent>();
        if (component != null)
            component.Slowdown(true, m_SlowdownMultiplier);
    }

    #endregion

}
