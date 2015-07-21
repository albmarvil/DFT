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
        get { return !(m_TileType == TileType.OBSTACLE || m_available); }
    }


    /// <summary>
    /// Property used to know if a tile is available for construction.
    /// </summary>
    public bool Available
    {
        get { return m_TileType != TileType.OBSTACLE && m_available; }
        set { m_available = value; }
    }

    /// <summary>
    /// Property to access to the navigation point position
    /// </summary>
    public Vector3 NavigationPosition
    {
        get { return m_NavPoint.position; }
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
    /// Identification number
    /// </summary>
    private int m_ID;

    /// <summary>
    /// Flag that indicates if a tile is available for construction
    /// </summary>
    private bool m_available = true;

    /////////////////Attributes for pathfinding//////////////////////

    /// <summary>
    /// Flag that indicates that this tile is an origin node in the graph (used for pathfinding)
    /// </summary>
    private bool m_origin = false;

    /// <summary>
    /// Flag that indicates that this tile is an destination node in the graph (used for pathfinding)s
    /// </summary>
    private bool m_destination = false;

    #endregion

    #region Public methods

    /// <summary>
    /// Config of the tile usign it's position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void setTileConfig(int x, int y)
    {
        m_name = x + "_" + y + "_TILE";
        m_ID = (int)m_TileType + x / y;
    }

    /// <summary>
    /// Method used to get the tile weight for pathfinding.
    /// </summary>
    /// <returns>Node weight for this tile in the navigation graph</returns>
    public int getWeight()
    {
        return (int)m_TileType;
    }

    #endregion

    #region Private methods

    #endregion

    #region Monobehavior calls

    #endregion

}
