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

    /// <summary>
    /// Delegate used to throw input events. Mouse clicks
    /// </summary>
    /// <param name="button"></param>
    public delegate void onMousePressed(MouseButton button);

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

    private onMousePressed m_MousePressed = null;

    #endregion

    #region Public methods

    /// <summary>
    /// Method used to register events
    /// </summary>
    public void RegisterMousePressedEvent(onMousePressed mousePressed)
    {
        m_MousePressed += mousePressed;
    }

    /// <summary>
    /// Method used to unregister events
    /// </summary>
    public void UnregisterMousePressedEvent(onMousePressed mousePressed)
    {
        m_MousePressed -= mousePressed;
    }

    #endregion

    #region Monobehavior calls

    /// <summary>
    /// Each frame the manager will chek the status of the mouse and thro different callbacks
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
    }

    #endregion

}
