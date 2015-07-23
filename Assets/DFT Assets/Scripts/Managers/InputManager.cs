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
    /// <param name="value">Extra value</param>
    public delegate void onOrderReceived(InputOrders order, bool ok, float value);

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


    #region Public params

    /// <summary>
    /// Horizontal threshold to for camera movement with mouse input.
    /// Coordinates in screen relative position
    /// 
    /// x -> min horizontal threshold
    /// y -> max horizontal threshold
    /// </summary>
    public Vector2 m_HorizontalThreshold = Vector2.zero;


    /// <summary>
    /// Vertical threshold to for camera movement with mouse input.
    /// Coordinates in screen relative position
    /// 
    /// x -> min Vertical threshold
    /// y -> max Vertical threshold
    /// </summary>
    public Vector2 m_VerticalThreshold = Vector2.zero;

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

        Vector3 mousePos = Input.mousePosition;

        ///make position relative to screen dimensions
        mousePos.x /= Screen.width;
        mousePos.y /= Screen.height;


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
                m_OrderReceived(InputOrders.ACCEPT, true, 1.0f);
        }
        else if (Input.GetButtonUp("Submit"))
        {
            if (m_OrderReceived != null)
                m_OrderReceived(InputOrders.ACCEPT, false, 0.0f);
        }


        ///Horizontal Camera movement
        if (Input.GetAxis("Horizontal") > 0.0f)
        {
            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_RIGHT, true, Input.GetAxis("Horizontal"));
                m_OrderReceived(InputOrders.CAMERA_LEFT, false, Input.GetAxis("Horizontal"));
            }
        }
        else if (Input.GetAxis("Horizontal") < 0.0f)
        {
            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_RIGHT, false, Input.GetAxis("Horizontal"));
                m_OrderReceived(InputOrders.CAMERA_LEFT, true, Input.GetAxis("Horizontal"));
            }
        }
        else if (0.0f <= mousePos.x && mousePos.x <= m_HorizontalThreshold.x)
        {
            float value = (1.0f - (mousePos.x / m_HorizontalThreshold.x));

            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_LEFT, true, -value);
                m_OrderReceived(InputOrders.CAMERA_RIGHT, false, 0.0f);
            }

        }
        else if (m_HorizontalThreshold.y <= mousePos.x && mousePos.x <= 1.0f)
        {
            float value = (mousePos.x - m_HorizontalThreshold.y) / (1.0f - m_HorizontalThreshold.y);

            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_LEFT, false, 0.0f);
                m_OrderReceived(InputOrders.CAMERA_RIGHT, true, value);
            }
        }
        else
        {
            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_RIGHT, false, 0.0f);
                m_OrderReceived(InputOrders.CAMERA_LEFT, false, 0.0f);
            }
        }



        //Vertical Camera movement
        if (Input.GetAxis("Vertical") > 0.0f)
        {
            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_UP, true, Input.GetAxis("Vertical"));
                m_OrderReceived(InputOrders.CAMERA_DOWN, false, Input.GetAxis("Vertical"));
            }
        }
        else if (Input.GetAxis("Vertical") < 0.0f)
        {
            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_UP, false, Input.GetAxis("Vertical"));
                m_OrderReceived(InputOrders.CAMERA_DOWN, true, Input.GetAxis("Vertical"));
            }
        }
        else if (0.0f <= mousePos.y && mousePos.y <= m_VerticalThreshold.x)
        {
            float value = (1.0f - (mousePos.y / m_VerticalThreshold.x));

            //Debug.Log("CAMERA_LEFT: " + value);

            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_DOWN, true, -value);
                m_OrderReceived(InputOrders.CAMERA_UP, false, 0.0f);
            }

        }
        else if (m_VerticalThreshold.y <= mousePos.y && mousePos.y <= 1.0f)
        {
            float value = (mousePos.y - m_VerticalThreshold.y) / (1.0f - m_VerticalThreshold.y);

            //Debug.Log("CAMERA_RIGHT: " + value);

            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_DOWN, false, 0.0f);
                m_OrderReceived(InputOrders.CAMERA_UP, true, value);
            }
        }
        else
        {
            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_UP, false, 0.0f);
                m_OrderReceived(InputOrders.CAMERA_DOWN, false, 0.0f);
            }
        }


        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {
            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_ZOOM_IN, true, Input.GetAxis("Mouse ScrollWheel"));
                m_OrderReceived(InputOrders.CAMERA_ZOOM_OUT, false, Input.GetAxis("Mouse ScrollWheel"));
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_ZOOM_IN, false, Input.GetAxis("Mouse ScrollWheel"));
                m_OrderReceived(InputOrders.CAMERA_ZOOM_OUT, true, Input.GetAxis("Mouse ScrollWheel"));
            }
        }
        else
        {
            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_ZOOM_IN, false, 0.0f);
                m_OrderReceived(InputOrders.CAMERA_ZOOM_OUT, false, 0.0f);
            }
        }



        #region Camera movement with mouse

        //Vector3 mousePos = Input.mousePosition;

        ////Debug.Log("Absolute: " + mousePos);

        /////make position relative to screen dimensions
        //mousePos.x /= Screen.width;
        //mousePos.y /= Screen.height;

        ////Debug.Log("Relative: " + mousePos + "Screen: " + Screen.width + ", " + Screen.height);

        /////Now considering the screen threshold for mouse movement we will send the logic orders

        /////Horizontal movement
        //if (0.0f <= mousePos.x && mousePos.x <= m_HorizontalThreshold.x)
        //{
        //    float value =  (1.0f - (mousePos.x / m_HorizontalThreshold.x));

        //    //Debug.Log("CAMERA_LEFT: " + value);

        //    if (m_OrderReceived != null)
        //    {
        //        m_OrderReceived(InputOrders.CAMERA_LEFT, true, -value);
        //        m_OrderReceived(InputOrders.CAMERA_RIGHT, false, 0.0f);
        //    }

        //}
        //else if (m_HorizontalThreshold.y <= mousePos.x && mousePos.x <= 1.0f)
        //{
        //    float value = (mousePos.x - m_HorizontalThreshold.y) / (1.0f - m_HorizontalThreshold.y);

        //    //Debug.Log("CAMERA_RIGHT: " + value);

        //    if (m_OrderReceived != null)
        //    {
        //        m_OrderReceived(InputOrders.CAMERA_LEFT, false, 0.0f);
        //        m_OrderReceived(InputOrders.CAMERA_RIGHT, true, value);
        //    }
        //}
        //else
        //{

        //    //Debug.Log("CAMERA_STOP");
        //    if (m_OrderReceived != null)
        //    {
        //        m_OrderReceived(InputOrders.CAMERA_LEFT, false, 0.0f);
        //        m_OrderReceived(InputOrders.CAMERA_RIGHT, false, 0.0f);
        //    }
        //}


        /////Vertical movement
        //if (0.0f <= mousePos.y && mousePos.y <= m_VerticalThreshold.x)
        //{
        //    float value = (1.0f - (mousePos.y / m_VerticalThreshold.x));

        //    //Debug.Log("CAMERA_LEFT: " + value);

        //    if (m_OrderReceived != null)
        //    {
        //        m_OrderReceived(InputOrders.CAMERA_DOWN, true, -value);
        //        m_OrderReceived(InputOrders.CAMERA_UP, false, 0.0f);
        //    }

        //}
        //else if (m_VerticalThreshold.y <= mousePos.y && mousePos.y <= 1.0f)
        //{
        //    float value = (mousePos.y - m_VerticalThreshold.y) / (1.0f - m_VerticalThreshold.y);

        //    //Debug.Log("CAMERA_RIGHT: " + value);

        //    if (m_OrderReceived != null)
        //    {
        //        m_OrderReceived(InputOrders.CAMERA_DOWN, false, 0.0f);
        //        m_OrderReceived(InputOrders.CAMERA_UP, true, value);
        //    }
        //}
        //else
        //{

        //    //Debug.Log("CAMERA_STOP");
        //    if (m_OrderReceived != null)
        //    {
        //        m_OrderReceived(InputOrders.CAMERA_DOWN, false, 0.0f);
        //        m_OrderReceived(InputOrders.CAMERA_UP, false, 0.0f);
        //    }
        //}

        #endregion

    }

    #endregion

}
