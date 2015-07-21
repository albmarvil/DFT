//----------------------------------------------------------------------
// @file MapManager.cs
//
// This file contains the declaration of MapManager class.
// This class is the one that creates all the elements of a map given a initial size
//
// @author Alberto Martinez Villaran <tukaram92@gmail.com>
// @date 21/07/2015
//----------------------------------------------------------------------


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour {

    #region Singleton

    /// <summary>
    /// Singleton instance of the class
    /// </summary>
    private static MapManager m_Instance = null;

    /// <summary>
    /// Property to get the singleton instance of the class.
    /// </summary>
    public static MapManager Singleton { get { return m_Instance; } }

    // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
    static MapManager() { }

    /// <summary>
    /// This is like the Init but done by the MonoBehaviour
    /// </summary>
    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        else
        {
            Debug.LogError("Someone is trying to create various MapManager [" + name + "]");
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
    /// Default tile prefab
    /// </summary>
    public GameObject m_TilePrefab = null;

    #endregion

    #region Private params

    /// <summary>
    /// Map tile matrix, used for pathfinding
    /// </summary>
    private Tile[][] m_Matrix;

    /// <summary>
    /// Reference to the Parent gameObject for the map Elements
    /// </summary>
    private GameObject m_MapGameObject = null;

    #endregion

    #region Public methods

    #endregion

    #region Private methods

    /// <summary>
    /// This method creates and places all the tiles in a single map
    /// </summary>
    /// <param name="width">Map width in tile number</param>
    /// <param name="height">Map height in tile number</param>
    /// <param name="tileSize">Tile size given(it will define how big or small is the map in general terms)</param>
    private void createTileMap(int width, int height, float tileSize)
    {

        m_Matrix = new Tile[height][];

        for (int i = 0; i < height; ++i)
        {
            m_Matrix[i] = new Tile[width];
            for (int j = 0; j < width; ++j)
            {
                Vector3 pos = new Vector3(j * tileSize, 0, i * tileSize);

                GameObject goTile = PoolManager.Singleton.getInstance(m_TilePrefab, pos, Quaternion.identity);

                Transform transform = goTile.GetComponent<Transform>();

                Vector3 scale = transform.localScale;
                scale.x = tileSize;
                scale.z = tileSize;

                transform.localScale = scale;

                transform.parent = m_MapGameObject.GetComponent<Transform>();

                m_Matrix[i][j] = goTile.GetComponent<Tile>();

            }
        }

    }

    #endregion

    #region Monobehavior calls

    private void Start()
    {
        m_MapGameObject = GameObject.FindGameObjectWithTag("Map");

        createTileMap(GameManager.Singleton.MapWidth, GameManager.Singleton.MapHeight, GameManager.Singleton.TileSize);
    }

    #endregion

}
