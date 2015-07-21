//----------------------------------------------------------------------
// @file GameManager.cs
//
// This file contains the declaration of GameManager class.
// This Singleton will be in charge of the execution of the different game states, also
// holding general info about the game.
//
// @author Alberto Martinez Villaran <tukaram92@gmail.com>
// @date 21/07/2015
//----------------------------------------------------------------------


using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	#region Singleton

    /// <summary>
    /// Singleton instance of the class
    /// </summary>
    private static GameManager m_Instance = null;

    /// <summary>
    /// Property to get the singleton instance of the class.
    /// </summary>
    public static GameManager Singleton { get { return m_Instance; } }

    // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
    static GameManager() { }

    /// <summary>
    /// This is like the Init but done by the MonoBehaviour
    /// </summary>
    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        else
        {
            Debug.LogError("Someone is trying to create various GameManager [" + name + "]");
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
    /// Public property to access to the mapWidth
    /// </summary>
    public int MapWidth
    {
        get { return m_MapWidth; }
    }

    /// <summary>
    /// Public property to access to the mapHeight
    /// </summary>
    public int MapHeight
    {
        get { return m_MapHeight; }
    }

    /// <summary>
    /// Public property to access to the Tile size
    /// </summary>
    public float TileSize
    {
        get { return m_TileSize; }
    }

    /// <summary>
    /// Public property to access to the number of SpecialTiles
    /// </summary>
    public int TotalSpecialTiles
    {
        get { return m_TotalSpecialTiles; }
    }


    /// <summary>
    /// Map width in number of tiles
    /// </summary>
    public int m_MapWidth = 20;

    /// <summary>
    /// Map height in number of tiles
    /// </summary>
    public int m_MapHeight = 20;

    /// <summary>
    /// Tile Size used (height and width)
    /// </summary>
    public float m_TileSize = 1.0f;

    /// <summary>
    /// Total number of special tiles (NonWalkable, small rocks, huge rocks ...etc)
    /// </summary>
    public int m_TotalSpecialTiles = 100;

    /// <summary>
    /// Property used to access to the MainCamera GameObject
    /// </summary>
    public GameObject MainCamera
    {
        get { return m_MainCamera; }
    }

    #endregion

    #region Private params

    /// <summary>
    /// Reference of the Global camera
    /// </summary>
    private GameObject m_MainCamera = null;

    #endregion

    #region Public methods

    /// <summary>
    /// This method is used at the begining of each game, so we can have general access for the main camera
    /// </summary>
    /// <param name="mainCamera">GameObject of the MainCamera</param>
    public void registerMainCamera(GameObject mainCamera)
    {
        m_MainCamera = mainCamera;
    }

    /// <summary>
    /// This function creates and instantiates everything needed for a new game
    /// </summary>
    public void InitGame()
    {
        ///Cargo aditivamente el nivel de juego
        //GameObject scene = GameObject.FindGameObjectWithTag("Scene");
        //GameObject.Destroy(scene);

        //Application.LoadLevelAdditive("Game");
    }

    #endregion


    #region Monobehavior calls

    private void Start()
    {
        InitGame();
    }

    #endregion


}
