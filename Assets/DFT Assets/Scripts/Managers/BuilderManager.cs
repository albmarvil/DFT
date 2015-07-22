//----------------------------------------------------------------------
// @file BuilderManager.cs
//
// This file contains the declaration of BuilderManager class.
//
// @author Alberto Martinez Villaran <tukaram92@gmail.com>
// @date 21/07/2015
//----------------------------------------------------------------------


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuilderManager : MonoBehaviour {

    #region Public params

    /// <summary>
    /// Buildings availables to build
    /// </summary>
    public List<GameObject> m_Buildings = new List<GameObject>();

    #endregion

    #region Private params

    /// <summary>
    /// Index to the current building
    /// </summary>
    private int m_CurrentBuildingIndex = -1;

    /// <summary>
    /// Actual tile hovered by the mouse.
    /// </summary>
    private Tile m_CurrentTile = null;

    /// <summary>
    /// GameObject used as a ghost for building feedback
    /// </summary>
    private GameObject m_CurrentBuildingGhost = null;


    /// <summary>
    /// Flag that shows if we can build in the current tile (Pathfinding conditions included)
    /// </summary>
    //private bool m_CanBuild = false;

    #endregion

    #region Public methods

    public void selectBlueCannon()
    {
        m_CurrentBuildingIndex = 0;
    }

    public void selectRedCannon()
    {
        m_CurrentBuildingIndex = 1;
    }

    /// <summary>
    /// Callback used when mouse is pressed
    /// </summary>
    /// <param name="button">Button pressed</param>
    public void onMousePressed(InputManager.MouseButton button){

        if(InputManager.MouseButton.LEFT == button)
            createBuilding(m_CurrentBuildingIndex, m_CurrentTile);

        if (InputManager.MouseButton.RIGHT == button)
        {
            Tile destination = MapManager.Singleton.getTileByWorldPosition(new Vector3(0,0,0));

            NavigationPathfinder.Singleton.calculateOptimalPath(m_CurrentTile, destination);
        }

        if (InputManager.MouseButton.CENTER == button)
        {
            Tile destination = MapManager.Singleton.getTileByWorldPosition(new Vector3(0, 0, 0));

            NavigationPathfinder.Singleton.calculatePath(m_CurrentTile, destination);
        }

    }

    #endregion

    #region Private methods

    /// <summary>
    /// This method will create a building in the current tile if it is possible
    /// There is one important conditions when building something.
    ///     -A valid route from route to crystals should exist
    /// </summary>
    /// <param name="index">Index that references the building</param>
    /// <param name="tile">Tile to build</param>
    private void createBuilding(int index, Tile tile)
    {
        //TODO
        //comprobar el coste de dinero

        if (index != -1 && tile != null && tile.Available)
        {
            //we can't block the routes throug the map
            tile.Navigable = false; //has to do this to test if there is a valid path

            //TODO
            //recoger un spawner y un crystal validos
            Tile spawners = MapManager.Singleton.getTileByWorldPosition(new Vector3(0,0,(GameManager.Singleton.MapHeight - 1) * GameManager.Singleton.TileSize));
            Tile crystals = MapManager.Singleton.getTileByWorldPosition(new Vector3(0,0,0));

            List<Tile> path = NavigationPathfinder.Singleton.calculatePath(spawners, crystals);

            if (path != null)
            {
                PoolManager.Singleton.getInstance(m_Buildings[index], tile.NavigationPosition, Quaternion.identity);
                tile.Available = false;
                tile.Navigable = false;
                
            }
            else
            {
                Debug.LogWarning("Cannot block map routes with buildings");
                tile.Navigable = true;
                tile.Available = false;
            }

            drawGhost(m_Buildings[index], tile);
        }
            
    }

    /// <summary>
    /// This method draws a building ghost in the given tile
    /// </summary>
    /// <param name="building">Ghost to draw</param>
    /// <param name="tile">Tile where to draw</param>
    private void drawGhost(GameObject building, Tile tile)
    {
        //if there were any ghost in a previous tile, first destroy it
        if (m_CurrentBuildingGhost != null)
        {
            PoolManager.Singleton.destroyInstance(m_CurrentBuildingGhost);
            m_CurrentBuildingGhost = null;
        }

        if (tile.Available)
        {
            m_CurrentBuildingGhost = PoolManager.Singleton.getInstance(building.GetComponent<BuildGhostsRegister>().m_GreenGhostPrefab, tile.NavigationPosition, Quaternion.identity);
        }
        else
        {
            m_CurrentBuildingGhost = PoolManager.Singleton.getInstance(building.GetComponent<BuildGhostsRegister>().m_RedGhostPrefab, tile.NavigationPosition, Quaternion.identity);
        }

    }

    #endregion

    #region Monobehavior calls

    /// <summary>
    /// In this update the manager will check which tile is the current and if it must draw the building ghost of the actual building
    /// </summary>
    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int mask = (1<<LayerMask.NameToLayer("Tiles"));

        RaycastHit hitInfo;

        //Debug.DrawRay(ray.origin, ray.direction, Color.green);

        if (Physics.Raycast(ray, out hitInfo, 1000.0f, mask))
        {
            //Debug.Log(hitInfo.collider.gameObject.tag);
            GameObject col = hitInfo.collider.gameObject;
            if (col.tag == "Tiles" && m_CurrentTile != col.GetComponent<Tile>())
            {
                m_CurrentTile = col.GetComponent<Tile>();

                if (m_CurrentBuildingIndex != -1)
                    drawGhost(m_Buildings[m_CurrentBuildingIndex], m_CurrentTile);
            }
        }
        else
        {
            //Debug.Log("NONE");
            m_CurrentTile = null;
            if (m_CurrentBuildingGhost != null)
            {
                PoolManager.Singleton.destroyInstance(m_CurrentBuildingGhost);
                m_CurrentBuildingGhost = null;
            }
        }
    }

    /// <summary>
    /// At the begining we will register the proper event callback
    /// </summary>
    private void Start()
    {
        InputManager.Singleton.RegisterMousePressedEvent(onMousePressed);
    }

    /// <summary>
    /// At the end we will unregister the event callback
    /// </summary>
    private void OnDestory()
    {
        InputManager.Singleton.UnregisterMousePressedEvent(onMousePressed);
    }

    #endregion

}
