//----------------------------------------------------------------------
// @file RegisterMainCamera.cs
//
// This file contains the declaration of RegisterMainCamera class.
//
// @author Alberto Martinez Villaran <tukaram92@gmail.com>
// @date 21/07/2015
//----------------------------------------------------------------------



using UnityEngine;
using System.Collections;

public class RegisterMainCamera : MonoBehaviour {

    #region Monobehavior calls

    /// <summary>
    /// We will register the camera at initialization as the main camera in GameManager
    /// </summary>
    private void Start()
    {
        GameManager.Singleton.registerMainCamera(gameObject);
    }

    #endregion

}
