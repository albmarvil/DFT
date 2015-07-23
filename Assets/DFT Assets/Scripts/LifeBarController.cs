///----------------------------------------------------------------------
/// @file LifeBarController.cs
///
/// This file contains the declaration of LifeBarController class.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 23/07/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections;

public class LifeBarController : MonoBehaviour {

    #region Public Params

    /// <summary>
    /// Reference to the life component
    /// </summary>
    public LifeComponent m_LifeComponent = null;

    /// <summary>
    /// Transform of the current life bar
    /// </summary>
    public Transform m_CurrentLifeBarTransform = null;

    #endregion


    #region Private Params

    /// <summary>
    /// Actual scale of the lifebar
    /// </summary>
    private float m_Scale = 1.0f;

    /// <summary>
    /// Actual localPos of the lifebar
    /// </summary>
    private Vector3 m_LocalPos = Vector3.zero;

    #endregion


    #region Monobehavior Calls

    /// <summary>
    /// Initialization of scale and position variables
    /// </summary>
    private void OnEnable()
    {
        m_Scale = 1.0f;
        Vector3 scale;
        scale.x = m_Scale;
        scale.y = m_Scale;
        scale.z = m_Scale;
        m_LocalPos.x = 0.0f;
        m_LocalPos.y = 0.0f;
        m_LocalPos.z = -0.01f;

        m_CurrentLifeBarTransform.localScale = scale;
        m_CurrentLifeBarTransform.localPosition = m_LocalPos;
    }

    /// <summary>
    /// Update of the lifebar dimensions and position, depending of the actual life
    /// </summary>
    private void Update()
    {
        float diff = m_LifeComponent.Life / m_LifeComponent.MaxLife;

        Vector3 scale = m_CurrentLifeBarTransform.localScale;

        scale.x = diff;

        float scaleDiff = m_Scale - scale.x;

        Vector3 localPos = m_CurrentLifeBarTransform.localPosition;

        localPos.x -= scaleDiff * 0.5f;

        m_Scale = scale.x;

        m_CurrentLifeBarTransform.localPosition = localPos;

        m_CurrentLifeBarTransform.localScale = scale;
    }
    #endregion

	
}
