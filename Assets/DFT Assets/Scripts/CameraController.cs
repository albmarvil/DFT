///----------------------------------------------------------------------
/// @file CameraController.cs
///
/// This file contains the declaration of CameraController class.
///
/// This script contains the logic used to move the camera. It's based on logic orders that are received from InputManager.
/// Those orders are the following:
/// 
///        CAMERA_HORIZONTAL,
///        CAMERA_VERTICAL,
///        CAMERA_PITCH,
///        CAMERA_YAW,
///        CAMERA_ZOOM,
///        CAMERA_STRAFE,
///        CAMERA_FORWARD,
///        CAMERA_RUN,
/// 
/// 
/// 
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 22/07/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	#region Public params

    /// <summary>
    /// Camera movement speed
    /// </summary>
    public Vector3 m_Speed = Vector3.one;

    /// <summary>
    /// Multiplier of the base speed applied when running
    /// </summary>
    public float m_RunMultiplier = 2.0f;

    /// <summary>
    /// Zoom speed
    /// </summary>
    public float m_ZoomSpeed = 10.0f;


    /// <summary>
    /// Camera panning speed
    /// </summary>
    public Vector3 m_PanningSpeed = Vector3.one;

    /// <summary>
    /// Glag used to invert vertical panning
    /// </summary>
    public bool m_InvertVerticalPanning = true;


    /// <summary>
    /// Reference to the camera transform
    /// </summary>
    public Transform m_Transform = null;

    #endregion

    #region Private params

    /// <summary>
    /// Current movement direction.
    /// </summary>
    private Vector3 m_CurrentMovementDir = Vector3.zero;


    /// <summary>
    /// Current rotation. Stored in angles to rotate in each axis
    /// </summary>
    private Vector3 m_CurrentPan = Vector3.zero;

    /// <summary>
    /// Current rotation in X axis in degrees
    /// </summary>
    private float m_XRotation = 0.0f;


    /// <summary>
    /// Current rotation in Y axis in degrees
    /// </summary>
    private float m_YRotation = 0.0f;

    /// <summary>
    /// Current value of the run multiplier
    /// </summary>
    private float m_CurrentRunMultiplier = 1.0f;

    #endregion


    #region Private methods

    /// <summary>
    /// This methods receives the events of logic orders sended by InputManager
    /// </summary>
    /// <param name="order">Order received</param>
    /// <param name="ok">state of the order (Positive or Negative)</param>
    /// <param name="value">Extra value</param>
    private void onOrderReceived(InputManager.InputOrders order, bool ok, float value)
    {
        switch (order)
        {

            case InputManager.InputOrders.CAMERA_VERTICAL:
                m_CurrentMovementDir.z += value;
                break;

            case InputManager.InputOrders.CAMERA_HORIZONTAL:
                m_CurrentMovementDir.x += value;
                break;

            case InputManager.InputOrders.CAMERA_ZOOM:
                m_CurrentMovementDir.y += -value * 300.0f;
                break;

            case InputManager.InputOrders.CAMERA_PITCH:
                m_CurrentPan.x = m_InvertVerticalPanning ? value : -value;
                break;

            case InputManager.InputOrders.CAMERA_YAW:
                m_CurrentPan.y = value;
                break;

            case InputManager.InputOrders.CAMERA_STRAFE:
                m_CurrentMovementDir += m_Transform.right.normalized * value * 0.5f;
                break;

            case InputManager.InputOrders.CAMERA_FORWARD:
                m_CurrentMovementDir += m_Transform.forward.normalized * value * 0.5f;
                break;

            case InputManager.InputOrders.CAMERA_RUN:
                if (ok)
                {
                    m_CurrentRunMultiplier = m_RunMultiplier;
                }
                else
                {
                    m_CurrentRunMultiplier = 1.0f;
                }
                break;
        }
    }

    #endregion

    #region Monobehavior calls

    private void Start()
    {
        m_XRotation = m_Transform.rotation.eulerAngles.x;

        m_YRotation = m_Transform.rotation.eulerAngles.y;

        if(InputManager.Singleton != null)
            InputManager.Singleton.RegisterOrderEvent(onOrderReceived);
    }

    private void OnDestroy()
    {
        if(InputManager.Singleton != null)
            InputManager.Singleton.UnregisterOrderEvent(onOrderReceived);
    }

    /// <summary>
    /// Each frame we calculate the new camera position and rotation
    /// </summary>
    private void Update()
    {
        Vector3 speed = m_CurrentMovementDir;
        speed.x *= m_Speed.x;
        speed.y *= m_Speed.y;
        speed.z *= m_Speed.z;

        Vector3 newPosition = m_Transform.position + speed * m_CurrentRunMultiplier * Time.deltaTime;
        m_Transform.position = newPosition;

        //Update rotation
        m_XRotation = m_XRotation + m_CurrentPan.x * m_PanningSpeed.x * Time.deltaTime;
        m_YRotation = m_YRotation + m_CurrentPan.y * m_PanningSpeed.y * Time.deltaTime;

        m_Transform.rotation = Quaternion.Euler(m_XRotation, m_YRotation, 0.0f);

        m_CurrentPan = Vector3.zero;
        m_CurrentMovementDir = Vector3.zero;

    }


    //private void OnDrawGizmosSelected()
    //{
    //    Debug.DrawRay(m_Transform.position, m_Transform.forward * 100.0f, Color.yellow);
    //}

    #endregion

}
