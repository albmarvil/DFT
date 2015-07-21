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
