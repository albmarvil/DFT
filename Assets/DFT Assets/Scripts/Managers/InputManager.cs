//----------------------------------------------------------------------
// @file InputManager.cs
//
// This file contains the declaration of InputManager class.
//
// This manager will read all the input and give orders to the logic. One of the main tasks is to give orders to the MainCamera.
// Orders such "move up", "move left", "zoom in", "zoom out"
//
// @author Alberto Martinez Villaran <tukaram92@gmail.com>
// @date 21/07/2015
//----------------------------------------------------------------------




using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    /// <summary>
    /// Public enum to distinguish mouse buttons
    /// </summary>
    public enum MouseButton
    {
        LEFT = 0,
        RIGHT = 1,
        CENTER = 2
    }

    public enum InputOrders
    {
        ACCEPT,
        CAMERA_LEFT,
        CAMERA_RIGHT,
        CAMERA_UP,
        CAMERA_DOWN,
        CAMERA_LEFT_ROTATION,
        CAMERA_RIGHT_ROTATION,
        CAMERA_ZOOM_IN,
        CAMERA_ZOOM_OUT
    }

    /// <summary>
    /// Delegate used to throw input events. Mouse clicks
    /// </summary>
    /// <param name="button">Mouse button pressed</param>
    public delegate void onMousePressed(MouseButton button);


    /// <summary>
    /// Delegate used to throw input events. Logic orders
    /// </summary>
    /// <param name="order">Logic order given</param>
    /// <param name="ok">True to throw the order, false to cancel it</param>
    public delegate void onOrderReceived(InputOrders order, bool ok);

	#region Singleton

    /// <summary>
    /// Singleton instance of the class
    /// </summary>
    private static InputManager m_Instance = null;

    /// <summary>
    /// Property to get the singleton instance of the class.
    /// </summary>
    public static InputManager Singleton { get { return m_Instance; } }

    // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
    static InputManager() { }

    /// <summary>
    /// This is like the Init but done by the MonoBehaviour
    /// </summary>
    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        else
        {
            Debug.LogError("Someone is trying to create various InputManager [" + name + "]");
            this.enabled = false;
        }
    }
	
	/// <summary>
    /// This is like the Release but done by the MonoBehaviour
    /// </summary>
    private void OnDestroy()
    {
        if (m_Instance == this)
            m_Instance = null;
    }

    #endregion


    #region Private params

    /// <summary>
    /// Delegate register
    /// </summary>
    private onMousePressed m_MousePressed = null;

    /// <summary>
    /// Delegate register
    /// </summary>
    private onOrderReceived m_OrderReceived = null;

    #endregion

    #region Public methods

    /// <summary>
    /// Method used to register mouse pressed events
    /// </summary>
    public void RegisterMousePressedEvent(onMousePressed mousePressed)
    {
        m_MousePressed += mousePressed;
    }

    /// <summary>
    /// Method used to unregister mouse pressed events
    /// </summary>
    public void UnregisterMousePressedEvent(onMousePressed mousePressed)
    {
        m_MousePressed -= mousePressed;
    }


    /// <summary>
    /// Method used to register logic order events
    /// </summary>
    public void RegisterOrderEvent(onOrderReceived orderReceived)
    {
        m_OrderReceived += orderReceived;
    }

    /// <summary>
    /// Method used to unregister logic order events
    /// </summary>
    public void UnregisterOrderEvent(onOrderReceived orderReceived)
    {
        m_OrderReceived -= orderReceived;
    }

    #endregion

    #region Monobehavior calls

    /// <summary>
    /// Each frame the manager will chek the status of the mouse and throw different callbacks
    /// 
    /// Also will check the different keys and cases to throw logic orders
    /// </summary>
    private void Update()
    {

        if (Input.GetMouseButtonDown((int)MouseButton.LEFT))
        {
            if(m_MousePressed != null)
                m_MousePressed(MouseButton.LEFT);
        }

        if (Input.GetMouseButtonDown((int)MouseButton.RIGHT))
        {
            if (m_MousePressed != null)
                m_MousePressed(MouseButton.RIGHT);
        }

        if (Input.GetMouseButtonDown((int)MouseButton.CENTER))
        {
            if (m_MousePressed != null)
                m_MousePressed(MouseButton.CENTER);
        }


        if (Input.GetButtonDown("Submit"))
        {
            if (m_OrderReceived != null)
                m_OrderReceived(InputOrders.ACCEPT, true);
        }
        else if (Input.GetButtonUp("Submit"))
        {
            if (m_OrderReceived != null)
                m_OrderReceived(InputOrders.ACCEPT, false);
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_RIGHT, true);
                m_OrderReceived(InputOrders.CAMERA_LEFT, false);
            }
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_RIGHT, false);
                m_OrderReceived(InputOrders.CAMERA_LEFT, true);
            }
        }
        else
        {
            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_RIGHT, false);
                m_OrderReceived(InputOrders.CAMERA_LEFT, false);
            }
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_UP, true);
                m_OrderReceived(InputOrders.CAMERA_DOWN, false);
            }
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_UP, false);
                m_OrderReceived(InputOrders.CAMERA_DOWN, true);
            }
        }
        else
        {
            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_UP, false);
                m_OrderReceived(InputOrders.CAMERA_DOWN, false);
            }
        }


        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_ZOOM_IN, true);
                m_OrderReceived(InputOrders.CAMERA_ZOOM_OUT, false);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_ZOOM_IN, false);
                m_OrderReceived(InputOrders.CAMERA_ZOOM_OUT, true);
            }
        }
        else
        {
            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_ZOOM_IN, false);
                m_OrderReceived(InputOrders.CAMERA_ZOOM_OUT, false);
            }
        }

        
    }

    #endregion

}
