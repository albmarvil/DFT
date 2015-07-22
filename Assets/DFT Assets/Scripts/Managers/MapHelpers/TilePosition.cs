//----------------------------------------------------------------------
// @file TilePosition.cs
//
// This file contains the declaration of TilePosition class.
//
// @author Alberto Martinez Villaran <tukaram92@gmail.com>
// @date 22/07/2015
//----------------------------------------------------------------------



using UnityEngine;
using System.Collections;

public class TilePosition
{

    #region Public params

    public int Row
    {
        get { return m_row; }
        set { m_row = value; }
    }

    public int Column
    {
        get { return m_column; }
        set { m_column = value; }
    }

    #endregion

    #region Private params

    private int m_row = -1;

    private int m_column = -1;

    #endregion

}
