///----------------------------------------------------------------------
/// @file CameraController.cs
///
/// This file contains the declaration of CameraController class.
///
/// This script contains the logic used to move the camera. It's based on logic orders that are received from InputManager.
/// Those orders are the following:
/// 
/// -CAMERA_UP
/// -CAMERA_DOWN
/// -CAMERA_LEFT
/// -CAMERA_RIGHT
/// -CAMERA_LEFT_ROTATION
/// -CAMERA_RIGHT_ROTATION
/// -CAMERA_ZOOM_IN
/// -CAMERA_ZOOM_OUT
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
    public float m_Speed = 1.0f;


    /// <summary>
    /// Reference to the camera transform
    /// </summary>
    public Transform m_Transform = null;

    #endregion

    #region Private params

    /// <summary>
    /// Current movement direction. It's values for each component would be -1, 0 or 1.
    /// Indicating only the direction on that axis
    /// </summary>
    private Vector3 m_CurrentMovementDir = Vector3.zero;

    #endregion

    #region Public methods

    #endregion

    #region Private methods

    /// <summary>
    /// This methods receives the events of logic orders sended by InputManager
    /// </summary>
    /// <param name="order">Order received</param>
    /// <param name="ok">state of the order (Positive or Negative)</param>
    private void onOrderReceived(InputManager.InputOrders order, bool ok)
    {
        if (order == InputManager.InputOrders.CAMERA_UP && ok)
        {
            m_CurrentMovementDir.z = 1.0f;
        }
        else if (order == InputManager.InputOrders.CAMERA_UP && !ok && m_CurrentMovementDir.z == 1.0f)
        {
            m_CurrentMovementDir.z = 0.0f;
        }

        if (order == InputManager.InputOrders.CAMERA_DOWN && ok)
        {
            m_CurrentMovementDir.z = -1.0f;
        }
        else if (order == InputManager.InputOrders.CAMERA_DOWN && !ok && m_CurrentMovementDir.z == -1.0f)
        {
            m_CurrentMovementDir.z = 0.0f;
        }


        if (order == InputManager.InputOrders.CAMERA_LEFT && ok)
        {
            m_CurrentMovementDir.x = -1.0f;
        }
        else if (order == InputManager.InputOrders.CAMERA_LEFT && !ok && m_CurrentMovementDir.x == -1.0f)
        {
            m_CurrentMovementDir.x = 0.0f;
        }


        if (order == InputManager.InputOrders.CAMERA_RIGHT && ok)
        {
            m_CurrentMovementDir.x = 1.0f;
        }
        else if (order == InputManager.InputOrders.CAMERA_RIGHT && !ok && m_CurrentMovementDir.x == 1.0f)
        {
            m_CurrentMovementDir.x = 0.0f;
        }


        if (order == InputManager.InputOrders.CAMERA_ZOOM_IN && ok)
        {
            m_CurrentMovementDir.y = -1.0f;
        }
        else if (order == InputManager.InputOrders.CAMERA_ZOOM_IN && !ok && m_CurrentMovementDir.y == -1.0f)
        {
            m_CurrentMovementDir.y = 0.0f;
        }


        if (order == InputManager.InputOrders.CAMERA_ZOOM_OUT && ok)
        {
            m_CurrentMovementDir.y = 1.0f;
        }
        else if (order == InputManager.InputOrders.CAMERA_ZOOM_OUT && !ok && m_CurrentMovementDir.y == 1.0f)
        {
            m_CurrentMovementDir.y = 0.0f;
        }

    }

    #endregion

    #region Monobehavior calls

    private void Start()
    {
        InputManager.Singleton.RegisterOrderEvent(onOrderReceived);
    }

    private void OnDisable()
    {
        InputManager.Singleton.UnregisterOrderEvent(onOrderReceived);
    }

    /// <summary>
    /// Each frame we calculate the new camera position
    /// </summary>
    private void Update()
    {
        Vector3 newPosition = m_Transform.position + m_CurrentMovementDir.normalized * m_Speed * Time.deltaTime;
        m_Transform.position = newPosition;
        //#region Horizontal movement
        //#endregion

        //#region Vertical movement
        //#endregion
    }

    #endregion

}
