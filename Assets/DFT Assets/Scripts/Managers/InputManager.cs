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
        CAMERA_HORIZONTAL,
        CAMERA_VERTICAL,
        CAMERA_PITCH,
        CAMERA_YAW,
        CAMERA_VERTICAL_ZOOM,
        CAMERA_STRAFE,
        CAMERA_FORWARD,
        CAMERA_RUN,
        CAMERA_HORIZONTAL_ROTATION,
        CAMERA_VERTICAL_ROTATION,
        CAMERA_ZOOM,
        DRAW_FOCAL_POINT,
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

        #region Mouse buttons detection
        if (Input.GetMouseButtonDown((int)MouseButton.LEFT))
        {
            if (m_MousePressed != null)
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
        #endregion


        #region Keyboard detection
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


        if (Input.GetButton("Run"))
        {
            if (m_OrderReceived != null)
                m_OrderReceived(InputOrders.CAMERA_RUN, true, 1.0f);
        }
        else
        {
            if (m_OrderReceived != null)
                m_OrderReceived(InputOrders.CAMERA_RUN, false, 0.0f);
        }


        if (Input.GetButton("FocalPoint"))
        {
            if (m_OrderReceived != null)
                m_OrderReceived(InputOrders.DRAW_FOCAL_POINT, true, 1.0f);
        }
        else
        {
            if (m_OrderReceived != null)
                m_OrderReceived(InputOrders.DRAW_FOCAL_POINT, false, 0.0f);
        }
        #endregion


        #region Logic orders detection --> Camera movement orders

        Vector3 mousePos = Input.mousePosition;

        ///make position relative to screen dimensions
        mousePos.x /= Screen.width;
        mousePos.y /= Screen.height;

        #region Camera focal rotation (Max priority)
        //Detection of the camera rotation, keeping the focal point
        if (Input.GetMouseButton((int)MouseButton.CENTER))
        {

            float xAxis = Input.GetAxis("Mouse X");
            float yAxis = Input.GetAxis("Mouse Y");

            if (m_OrderReceived != null)
            {
                m_OrderReceived(InputOrders.CAMERA_HORIZONTAL_ROTATION, true, xAxis);
                m_OrderReceived(InputOrders.CAMERA_VERTICAL_ROTATION, true, yAxis);
            }
        }
        #endregion
        else
        {
            #region Camera local rotation and free movement mode
            //while pressing right mouse button, have to send camera panning orders
            if (Input.GetMouseButton((int)MouseButton.RIGHT))
            {
                #region Camera pitch/yaw

                float xAxis = Input.GetAxis("Mouse X");
                float yAxis = Input.GetAxis("Mouse Y");

                if (m_OrderReceived != null)
                {
                    m_OrderReceived(InputOrders.CAMERA_YAW, true, xAxis);
                    m_OrderReceived(InputOrders.CAMERA_PITCH, true, yAxis);
                }

                #endregion

                #region Camera Strafe left/right
                ///camera strafeleft/right
                if (Input.GetAxis("Horizontal") != 0.0f)
                {
                    if (m_OrderReceived != null)
                    {
                        m_OrderReceived(InputOrders.CAMERA_STRAFE, true, Input.GetAxis("Horizontal"));
                    }
                }
                else
                {
                    if (m_OrderReceived != null)
                    {
                        m_OrderReceived(InputOrders.CAMERA_STRAFE, false, 0.0f);
                    }
                }
                #endregion

                #region Camera forward/backwards
                ///move forward or backwards
                if (Input.GetAxis("Vertical") != 0.0f)
                {
                    if (m_OrderReceived != null)
                    {
                        m_OrderReceived(InputOrders.CAMERA_FORWARD, true, Input.GetAxis("Vertical"));
                    }
                }
                else
                {
                    if (m_OrderReceived != null)
                    {
                        m_OrderReceived(InputOrders.CAMERA_FORWARD, false, 0.0f);
                    }
                }
                #endregion

                #region Camera zoom (Y axis)
                if (Input.GetAxis("Mouse ScrollWheel") != 0.0f)
                {
                    if (m_OrderReceived != null)
                    {
                        m_OrderReceived(InputOrders.CAMERA_VERTICAL_ZOOM, true, Input.GetAxis("Mouse ScrollWheel"));
                    }
                }
                else
                {
                    if (m_OrderReceived != null)
                    {
                        m_OrderReceived(InputOrders.CAMERA_VERTICAL_ZOOM, false, 0.0f);
                    }
                }
                #endregion

            }
            #endregion
            #region Camera normal movements
            else
            {
                #region Camera horizontal movement (X axis)
                ///Horizontal Camera movement
                if (Input.GetAxis("Horizontal") != 0.0f)
                {
                    if (m_OrderReceived != null)
                    {
                        m_OrderReceived(InputOrders.CAMERA_HORIZONTAL, true, Input.GetAxis("Horizontal"));
                    }
                }
                else if (0.0f <= mousePos.x && mousePos.x <= m_HorizontalThreshold.x)
                {
                    float value = (1.0f - (mousePos.x / m_HorizontalThreshold.x));

                    if (m_OrderReceived != null)
                    {
                        m_OrderReceived(InputOrders.CAMERA_HORIZONTAL, true, -value);
                    }

                }
                else if (m_HorizontalThreshold.y <= mousePos.x && mousePos.x <= 1.0f)
                {
                    float value = (mousePos.x - m_HorizontalThreshold.y) / (1.0f - m_HorizontalThreshold.y);

                    if (m_OrderReceived != null)
                    {
                        m_OrderReceived(InputOrders.CAMERA_HORIZONTAL, true, value);
                    }
                }
                else
                {
                    if (m_OrderReceived != null)
                    {
                        m_OrderReceived(InputOrders.CAMERA_HORIZONTAL, false, 0.0f);
                    }
                }
                #endregion

                #region Camera Vertical movements(Z Axis)
                //Vertical Camera movement
                if (Input.GetAxis("Vertical") != 0.0f)
                {
                    if (m_OrderReceived != null)
                    {
                        m_OrderReceived(InputOrders.CAMERA_VERTICAL, true, Input.GetAxis("Vertical"));
                    }
                }
                else if (0.0f <= mousePos.y && mousePos.y <= m_VerticalThreshold.x)
                {
                    float value = (1.0f - (mousePos.y / m_VerticalThreshold.x));

                    if (m_OrderReceived != null)
                    {
                        m_OrderReceived(InputOrders.CAMERA_VERTICAL, true, -value);
                    }

                }
                else if (m_VerticalThreshold.y <= mousePos.y && mousePos.y <= 1.0f)
                {
                    float value = (mousePos.y - m_VerticalThreshold.y) / (1.0f - m_VerticalThreshold.y);

                    if (m_OrderReceived != null)
                    {
                        m_OrderReceived(InputOrders.CAMERA_VERTICAL, true, value);
                    }
                }
                else
                {
                    if (m_OrderReceived != null)
                    {
                        m_OrderReceived(InputOrders.CAMERA_VERTICAL, false, 0.0f);
                    }
                }
                #endregion

                #region Camera zoom
                if (Input.GetAxis("Mouse ScrollWheel") != 0.0f)
                {
                    if (m_OrderReceived != null)
                    {
                        m_OrderReceived(InputOrders.CAMERA_ZOOM, true, Input.GetAxis("Mouse ScrollWheel"));
                    }
                }
                else
                {
                    if (m_OrderReceived != null)
                    {
                        m_OrderReceived(InputOrders.CAMERA_ZOOM, false, 0.0f);
                    }
                }
                #endregion

            }
            #endregion
        }
        #endregion
    }

    #endregion

}
