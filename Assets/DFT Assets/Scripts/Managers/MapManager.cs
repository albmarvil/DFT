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
    public GameObject m_TileDefaultPrefab = null;

    /// <summary>
    /// Special tile prefab
    /// It's an obstacle. Enemies can't navigate through them
    /// </summary>
    public GameObject m_TileObstaclePrefab = null;

    /// <summary>
    /// Special tile prefab
    /// Enemies will slowdown when navigatin through them
    /// </summary>
    public GameObject m_TileSlow1Prefab = null;

    /// <summary>
    /// Enemy Spawner prefab
    /// </summary>
    public GameObject m_EnemySpawnerPrefab = null;

    /// <summary>
    /// Crystal prefab
    /// </summary>
    public GameObject m_CrystalPrefab = null;

    /// <summary>
    /// Property to access the matrix that represents the navigation grid
    /// </summary>
    public Tile[][] Matrix
    {
        get { return m_Matrix; }
    }

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

    /// <summary>
    /// Returns a Tile giving a world position. Null if the position is wrong
    /// </summary>
    /// <param name="position">World position</param>
    /// <returns>Tile in that wolrd position</returns>
    public Tile getTileByWorldPosition(Vector3 position)
    {
        int row = (int)(position.z / GameManager.Singleton.TileSize);
        int column = (int)(position.x / GameManager.Singleton.TileSize);

        if (row < GameManager.Singleton.MapHeight && column < GameManager.Singleton.MapWidth)
            return m_Matrix[row][column];
        else
            return null;
    }

    #endregion

    #region Private methods

    /// <summary>
    /// This method creates and places all the tiles in a single map.
    /// 
    /// The creation of the map will be done in three steps:
    ///  -Creation of special tiles (TileType different from default)
    ///  -Creation of default tiles
    ///  -Creation of Crystals and Enemy Spawners
    ///  
    /// As special rule, for each crystal and spawner the player will have extra money
    ///  
    /// </summary>
    /// <param name="width">Map width in tile number</param>
    /// <param name="height">Map height in tile number</param>
    /// <param name="tileSize">Tile size given(it will define how big or small is the map in general terms)</param>
    /// <param name="numSpecialTiles">Number of special tiles in the map. This number will never be more than 50% of the total number of tiles</param>
    private void createTileMap(int width, int height, float tileSize, int numSpecialTiles)
    {

        #region Matrix initialization

        int specialTiles = Mathf.Min((int)(width * height * 0.5f), numSpecialTiles); 

        m_Matrix = new Tile[height][];

        for (int i = 0; i < height; ++i)
        {
            m_Matrix[i] = new Tile[width];
        }

        #endregion

        #region Creation of Special Tiles

        ///Special tiles will be created within [2, height - 3] interval.
        ///The line height = 0 will be were crystals are spawned
        ///The line height = heigth-1 will be were enemy spawners are spawned
        for (int i = 0; i < specialTiles; ++i)
        {
            
            int column = Random.Range(0, width);

            int row = Random.Range(2, height - 2);

            while (m_Matrix[row][column] != null)
            {
                column = Random.Range(0, width);

                row = Random.Range(2, height - 2);
            }

            TileType type = TileType.DEFAULT;

            while (type == TileType.DEFAULT)
            {
                type = (TileType)Random.Range((int)TileType.OBSTACLE, (int)TileType.END);
            }

            createTile(row, column, type, tileSize);

        }

        #endregion

        #region Creation of Default Tiles
        for (int i = 0; i < height; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                if (m_Matrix[i][j] == null)
                {
                    createTile(i, j, TileType.DEFAULT, tileSize);
                }
            }
        }
        #endregion

        #region Creation of Spawners

        // The number of spawners will be choosen randomly whithin this interval [1, width * 0.5]
        int numSpawners = Random.Range(1, (int)(width * 0.5f));

        //add extra money for each spawner
        MoneyManager.Singleton.AddMoney(numSpawners * GameManager.Singleton.SpawnerExtraMoney);

        for (int i = 0; i <= numSpawners - 1; ++i)
        {
            int row = height - 1;
            int column = -1;

            while (column == -1)
            {
                column = Random.Range(0, width);
                Tile tile = m_Matrix[row][column];

                if (!tile.isOrigin)
                {
                    tile.isOrigin = true;
                    tile.Available = false;
                    GameObject spawner = PoolManager.Singleton.getInstance(m_EnemySpawnerPrefab, tile.NavigationPosition, Quaternion.identity);
                    spawner.GetComponent<Transform>().localScale *= tileSize;
                }
                else
                {
                    column = -1;
                }
            }
        }

        #endregion

        #region Creation of Crystals

        // The number of crystals will be choosen randomly whithin this interval [1, width * 0.5]
        int numCrystals = Random.Range(1, (int)(width * 0.5f));

        //add extra money for each crystal
        MoneyManager.Singleton.AddMoney(numCrystals * GameManager.Singleton.CrystalExtraMoney);

        CrystalManager.Singleton.setTotalCrystals(numCrystals);

        for (int i = 0; i <= numCrystals - 1; ++i)
        {
            int row = 0;
            int column = -1;

            while (column == -1)
            {
                column = Random.Range(0, width);
                Tile tile = m_Matrix[row][column];

                if (!tile.isDestination)
                {
                    tile.isDestination = true;
                    tile.Navigable = false;
                    tile.Available = false;
                    GameObject spawner = PoolManager.Singleton.getInstance(m_CrystalPrefab, tile.NavigationPosition, Quaternion.identity);
                    spawner.GetComponent<Transform>().localScale *= tileSize;
                }
                else
                {
                    column = -1;
                }
            }
        }

        #endregion

    }

    /// <summary>
    /// This method creates a single tile, in the correct world position 
    /// </summary>
    /// <param name="row">Matrix row. Used to calculate Z position in world space</param>
    /// <param name="column">Matrix column. Used to calculate X position in world space</param>
    /// <param name="type">Tile type used to create the new tile</param>
    /// <param name="tileSize">Tile size, used to obtain the correct world position</param>
    private void createTile(int row, int column, TileType type, float tileSize)
    {
        GameObject prefab = null;
        switch (type)
        {
            case TileType.OBSTACLE:
                prefab = m_TileObstaclePrefab;
                break;
            case TileType.SLOW1:
                prefab = m_TileSlow1Prefab;
                break;
            case TileType.DEFAULT:
                prefab = m_TileDefaultPrefab;
                break;
            default:
                prefab = m_TileDefaultPrefab;
                break;
        }

        Vector3 pos = new Vector3(column * tileSize, 0, row * tileSize);

        GameObject goTile = PoolManager.Singleton.getInstance(prefab, pos, Quaternion.identity);

        Transform transform = goTile.GetComponent<Transform>();

        Vector3 scale = transform.localScale;
        scale.x = tileSize;
        scale.y *= tileSize; 
        scale.z = tileSize;

        transform.localScale = scale;

        transform.parent = m_MapGameObject.GetComponent<Transform>();

        Tile tileComponent = goTile.GetComponent<Tile>();

        //if the tile is in [2, height-3] interval, we will ban construction
        if (row < 2 || GameManager.Singleton.MapHeight - 3 < row)
        {
            tileComponent.Available = false;
        }

        tileComponent.setTileConfig(row, column);

        m_Matrix[row][column] = tileComponent;
    }

    #endregion

    #region Monobehavior calls

    private void Start()
    {
        m_MapGameObject = GameObject.FindGameObjectWithTag("Map");

        createTileMap(GameManager.Singleton.MapWidth, GameManager.Singleton.MapHeight, GameManager.Singleton.TileSize, GameManager.Singleton.TotalSpecialTiles);
    }

    #endregion

}
