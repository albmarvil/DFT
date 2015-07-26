//----------------------------------------------------------------------
// @file Tile.cs
//
// This file contains the declaration of Tile class.
//
// @author Alberto Martinez Villaran <tukaram92@gmail.com>
// @date 21/07/2015
//----------------------------------------------------------------------


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Tile : MonoBehaviour {

	#region Public params

    /// <summary>
    /// Type of tile used. This will define the node weight in the navigation graph and other properties of the tile
    /// </summary>
    public TileType m_TileType = TileType.DEFAULT;

    /// <summary>
    /// Navigation point of the tile. Used for the movement of the enemies
    /// </summary>
    public Transform m_NavPoint = null;

    /// <summary>
    /// Property used to know if a tile is navigable by enemies.
    /// A tile won't be navigable if it is of the type NONE(means none navigation) or it has a construction
    /// </summary>
    public bool Navigable
    {
        get { return m_navigable; }
        set { m_navigable = value; }
    }


    /// <summary>
    /// Property used to know if a tile is available for construction.
    /// </summary>
    public bool Available
    {
        get { return m_available; }
        set { m_available = value; }
    }

    /// <summary>
    /// Property to access to the navigation point position
    /// </summary>
    public Vector3 NavigationPosition
    {
        get { return m_NavPoint.position; }
    }

    /// <summary>
    /// Position on the squared grid
    /// </summary>
    public TilePosition NavigationTilePosition
    {
        get { return m_TilePosition; }
    }

    /// <summary>
    /// Navigation weight for this tile
    /// </summary>
    public float m_NavigationWeight = 1.0f;

    public float NavigationWeight
    {
        get { return m_NavigationWeight; }
    }

    public bool isOrigin
    {
        get { return m_origin && !m_destination; }
        set { m_origin = value; }
    }

    public bool isDestination
    {
        get { return m_destination && !m_origin; }
        set { m_destination = value; }
    }

    #endregion

    #region Private params

    /// <summary>
    /// Identification name of the Tile
    /// </summary>
    private string m_name;

    /// <summary>
    /// Flag that indicates if a tile is available for construction
    /// </summary>
    private bool m_available = true;

    /// <summary>
    /// Flag that indicates if a tile is navigable by agents
    /// </summary>
    [SerializeField]
    private bool m_navigable = true;

    /////////////////Attributes for pathfinding//////////////////////

    /// <summary>
    /// Flag that indicates that this tile is an origin node in the graph (used for pathfinding)
    /// </summary>
    private bool m_origin = false;

    /// <summary>
    /// Flag that indicates that this tile is an destination node in the graph (used for pathfinding)s
    /// </summary>
    private bool m_destination = false;

    /// <summary>
    /// Position of the tile in the squared grid
    /// </summary>
    private TilePosition m_TilePosition = new TilePosition();

    #endregion

    #region Public methods

    /// <summary>
    /// Config of the tile usign it's position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void setTileConfig(int row, int column)
    {
        m_name = row + "_" + column + "_TILE_"+m_TileType.ToString();
        gameObject.name = m_name;

        m_TilePosition.Column = column;
        m_TilePosition.Row = row;
    }

    /// <summary>
    /// Method used to get the tile weight for pathfinding.
    /// </summary>
    /// <returns>Node weight for this tile in the navigation graph</returns>
    

    /// <summary>
    /// Method used for pathfinding. It will return all the children of this Tile.
    /// Also will filter the tiles, giving only those where navigation is allowed
    /// </summary>
    /// <param name="useDiagonals">True to use diagonal tiles as valid children</param>
    /// <returns>Valid children of this tile</returns>
    public List<Tile> getChildren(bool useDiagonals)
    {
        List<Tile> children = new List<Tile>();

        Tile child = null;


        int row = NavigationTilePosition.Row + 1;
        int column = NavigationTilePosition.Column;
        if (0 <= row && row < GameManager.Singleton.MapHeight)
        {
            child = MapManager.Singleton.Matrix[row][column];
            children.Add(child);
        }

        row = NavigationTilePosition.Row - 1;
        column = NavigationTilePosition.Column;
        if (0 <= row && row < GameManager.Singleton.MapHeight)
        {
            child = MapManager.Singleton.Matrix[row][column];
            children.Add(child);
        }

        row = NavigationTilePosition.Row;
        column = NavigationTilePosition.Column + 1;
        if (0 <= column && column < GameManager.Singleton.MapWidth)
        {
            child = MapManager.Singleton.Matrix[row][column];
            children.Add(child);
        }

        row = NavigationTilePosition.Row;
        column = NavigationTilePosition.Column - 1;
        if (0 <= column && column < GameManager.Singleton.MapWidth)
        {
            child = MapManager.Singleton.Matrix[row][column];
            children.Add(child);
        }

        if (useDiagonals)
        {
            row = NavigationTilePosition.Row + 1;
            column = NavigationTilePosition.Column - 1;
            if (0 <= row && row < GameManager.Singleton.MapHeight && 0 <= column && column < GameManager.Singleton.MapWidth)
            {
                child = MapManager.Singleton.Matrix[row][column];
                children.Add(child);
            }

            row = NavigationTilePosition.Row + 1;
            column = NavigationTilePosition.Column + 1;
            if (0 <= row && row < GameManager.Singleton.MapHeight && 0 <= column && column < GameManager.Singleton.MapWidth)
            {
                child = MapManager.Singleton.Matrix[row][column];
                children.Add(child);
            }


            row = NavigationTilePosition.Row - 1;
            column = NavigationTilePosition.Column - 1;
            if (0 <= row && row < GameManager.Singleton.MapHeight && 0 <= column && column < GameManager.Singleton.MapWidth)
            {
                child = MapManager.Singleton.Matrix[row][column];
                children.Add(child);
            }


            row = NavigationTilePosition.Row - 1;
            column = NavigationTilePosition.Column + 1;
            if (0 <= row && row < GameManager.Singleton.MapHeight && 0 <= column && column < GameManager.Singleton.MapWidth)
            {
                child = MapManager.Singleton.Matrix[row][column];
                children.Add(child);
            }
        }

        return children;
    }

    #endregion

    #region Private methods

    #endregion

    #region Monobehavior calls

    #endregion

}
