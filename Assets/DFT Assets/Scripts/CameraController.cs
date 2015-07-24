///----------------------------------------------------------------------
/// @file CameraController.cs
///
/// This file contains the declaration of CameraController class.
///
/// This script contains the logic used to move the camera. It's based on logic orders that are received from InputManager.
/// Those orders are the following:
/// 
        /*CAMERA_HORIZONTAL,
        CAMERA_VERTICAL,
        CAMERA_PITCH,
        CAMERA_YAW,
        CAMERA_VERTICAL_ZOOM,
        CAMERA_STRAFE,
        CAMERA_FORWARD,
        CAMERA_RUN,
        CAMERA_HORIZONTAL_ROTATION,
        CAMERA_VERTICAL_ROTATION,
        CAMERA_ZOOM,*/
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
    public float m_RunMultiplier = 4.0f;

    /// <summary>
    /// Zoom speed
    /// </summary>
    public float m_ZoomSpeed = 15.0f;


    /// <summary>
    /// Camera rotation speed
    /// </summary>
    public Vector3 m_RotationSpeed = Vector3.one;

    /// <summary>
    /// Glag used to invert vertical rotation
    /// </summary>
    public bool m_InvertVerticalRotation = false;

    /// <summary>
    /// Reference to the camera transform
    /// </summary>
    public Transform m_CameraTransform = null;

    /// <summary>
    /// Reference to the focal point transform
    /// </summary>
    public Transform m_FocalPointTransform = null;

    /// <summary>
    /// Reference to the focal point meshRenderer. Used for visual feedback
    /// </summary>
    public MeshRenderer m_FocalPointRenderer = null;

    /// <summary>
    /// Focus target distance
    /// </summary>
    public float m_FocalPointDistance = 10.0f;

    /// <summary>
    /// Flag to always draw the focal point
    /// </summary>
    public bool m_AlwaysDrawFocalPoint = false;

    #endregion

    #region Private params

    /// <summary>
    /// Current movement direction.
    /// </summary>
    private Vector3 m_CurrentMovementDir = Vector3.zero;


    /// <summary>
    /// Current rotation. Stored in angles to rotate in each axis
    /// </summary>
    private Vector3 m_CurrentRotation = Vector3.zero;

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


    /// <summary>
    /// Flag used to lock the focal point
    /// </summary>
    private bool m_LockFocalPoint = false;

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
                Vector3 verticalDir = m_CameraTransform.forward * value;
                verticalDir.y = 0.0f;

                m_CurrentMovementDir += verticalDir;
                break;

            case InputManager.InputOrders.CAMERA_HORIZONTAL:
                Vector3 horizontalDir = m_CameraTransform.right * value;
                horizontalDir.y = 0.0f;
                m_CurrentMovementDir += horizontalDir;
                break;

            case InputManager.InputOrders.CAMERA_ZOOM:
                float movement = value * m_ZoomSpeed;
                m_FocalPointDistance = Mathf.Max(m_FocalPointDistance - movement, 1.0f);
                if(m_FocalPointDistance > 1.0f)
                    m_CameraTransform.position = m_CameraTransform.position + movement * m_CameraTransform.forward;
                break;

            ///Another kind of zoom that only moves in Y axis up or down
            case InputManager.InputOrders.CAMERA_VERTICAL_ZOOM:
                m_CurrentMovementDir.y += -value * m_ZoomSpeed;
                break;

            case InputManager.InputOrders.CAMERA_PITCH:
                m_CurrentRotation.x = m_InvertVerticalRotation ? value : -value;
                break;

            case InputManager.InputOrders.CAMERA_YAW:
                m_CurrentRotation.y = value;
                break;

            case InputManager.InputOrders.CAMERA_STRAFE:
                m_CurrentMovementDir += m_CameraTransform.right* value;
                break;

            case InputManager.InputOrders.CAMERA_FORWARD:
                m_CurrentMovementDir += m_CameraTransform.forward * value;
                break;

            case InputManager.InputOrders.CAMERA_RUN:
                m_CurrentRunMultiplier = ok ? m_RunMultiplier : 1.0f;
                break;

            case InputManager.InputOrders.CAMERA_HORIZONTAL_ROTATION:
                m_LockFocalPoint = true;
                m_CurrentRotation.y = -value;
                break;

            case InputManager.InputOrders.CAMERA_VERTICAL_ROTATION:
                m_LockFocalPoint = true;
                m_CurrentRotation.x = -value;
                break;

            case InputManager.InputOrders.DRAW_FOCAL_POINT:
                m_AlwaysDrawFocalPoint = ok;
                break;
        }
    }

    /// <summary>
    /// This methods draws or undraws the focal point feedback
    /// </summary>
    /// <param name="draw">True to draw</param>
    private void drawFocalPoint(bool draw)
    {
        m_FocalPointRenderer.enabled = draw || m_AlwaysDrawFocalPoint;
    }

    #endregion

    #region Monobehavior calls

    private void Start()
    {
        m_XRotation = m_CameraTransform.rotation.eulerAngles.x;

        m_YRotation = m_CameraTransform.rotation.eulerAngles.y;

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
        
        if (!m_LockFocalPoint)
        {

            Vector3 speed = m_CurrentMovementDir;
            speed.x *= m_Speed.x;
            speed.y *= m_Speed.y;
            speed.z *= m_Speed.z;

            Vector3 newPosition = m_CameraTransform.position + speed * m_CurrentRunMultiplier * Time.deltaTime;
            m_CameraTransform.position = newPosition;

            //Update rotation
            m_XRotation = m_XRotation + m_CurrentRotation.x * m_RotationSpeed.x * Time.deltaTime;
            m_YRotation = m_YRotation + m_CurrentRotation.y * m_RotationSpeed.y * Time.deltaTime;

            m_CameraTransform.rotation = Quaternion.Euler(m_XRotation, m_YRotation, 0.0f);

            ///Target Transform update
            m_FocalPointTransform.position = (m_CameraTransform.forward * m_FocalPointDistance) + m_CameraTransform.position;
            m_FocalPointTransform.rotation = Quaternion.Euler(m_XRotation, m_YRotation, 0);
            m_FocalPointTransform.forward = -m_FocalPointTransform.forward;

            drawFocalPoint(false);
            
        }
        else
        {
            //Update rotation
            m_XRotation = m_XRotation + m_CurrentRotation.x * m_RotationSpeed.x * Time.deltaTime;
            m_YRotation = m_YRotation + m_CurrentRotation.y * m_RotationSpeed.y * Time.deltaTime;

            m_FocalPointTransform.rotation = Quaternion.Euler(m_XRotation, m_YRotation, 0);
            m_FocalPointTransform.forward = -m_FocalPointTransform.forward;


            m_CameraTransform.position = (m_FocalPointTransform.forward * m_FocalPointDistance) + m_FocalPointTransform.position;
            m_CameraTransform.LookAt(m_FocalPointTransform.position);

            drawFocalPoint(true);
        }

        m_LockFocalPoint = false;
        m_CurrentRotation = Vector3.zero;
        m_CurrentMovementDir = Vector3.zero;

    }

    #endregion

}
