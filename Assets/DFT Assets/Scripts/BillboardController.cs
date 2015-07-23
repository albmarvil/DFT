///----------------------------------------------------------------------
/// @file BillboardController.cs
///
/// This file contains the declaration of BillboardController class.
///
/// This Monobehavior script is used to make a gameObject always to face the camera, as a billboard
/// 
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 23/07/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections;


public class BillboardController : MonoBehaviour {

    #region Private Params
    
    /// <summary>
    /// Reference to the GameObject transform
    /// </summary>
    private Transform m_Transform = null;

    #endregion

    #region MonoBehavior calls

    /// <summary>
    /// Initialization of references
    /// </summary>
	private void OnEnable () {
        
        m_Transform = gameObject.GetComponent<Transform>();
        m_Transform.up = Vector3.up;
	}
	
	/// <summary>
	/// To face the camera, the gameObject forward direction should be camera forward
	/// </summary>
	private void Update () {
        Vector3 dir = GameManager.Singleton.MainCamera.GetComponent<Transform>().forward;
        m_Transform.forward = dir;
    }

    #endregion
}
