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

    #region Singleton

    /// <summary>
    /// Singleton instance of the class
    /// </summary>
    private static BuilderManager m_Instance = null;

    /// <summary>
    /// Property to get the singleton instance of the class.
    /// </summary>
    public static BuilderManager Singleton { get { return m_Instance; } }

    // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
    static BuilderManager() { }

    /// <summary>
    /// This is like the Init but done by the MonoBehaviour
    /// </summary>
    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        else
        {
            Debug.LogError("Someone is trying to create various BuilderManager [" + name + "]");
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
    /// Buildings availables to build
    /// </summary>
    public List<BuildingCfg> m_Buildings = new List<BuildingCfg>();

    /// <summary>
    /// Reference to the Build HUD Controller
    /// </summary>
    public BuildHUDController m_BuildHUD = null;

    /// <summary>
    /// Reference to the MoneyHUDController
    /// </summary>
    public MoneyHUDController m_MoneyHUDController = null;

    #endregion

    #region Private params

    /// <summary>
    /// Index to the current building
    /// </summary>
    private int m_CurrentBuildingIndex = 0;

    /// <summary>
    /// Actual tile hovered by the mouse.
    /// </summary>
    private Tile m_CurrentTile = null;

    /// <summary>
    /// GameObject used as a ghost for building feedback
    /// </summary>
    private GameObject m_CurrentBuildingGhost = null;


    /// <summary>
    /// Flag that indicates if we are in building turn
    /// </summary>
    private bool m_CanBuild = false;

    #endregion

    #region Public methods

    /// <summary>
    /// Method that configures if it is or not the building turn
    /// </summary>
    /// <param name="canBuild">flag to set up</param>
    public void SetBuildingTurn(bool canBuild)
    {
        m_CanBuild = canBuild;
        m_BuildHUD.gameObject.SetActive(canBuild);

        if (!canBuild && m_CurrentBuildingGhost != null)
        {
            PoolManager.Singleton.destroyInstance(m_CurrentBuildingGhost);
            m_CurrentBuildingGhost = null;
            m_MoneyHUDController.EnoughMoney(true);
        }
        else
        {
            m_CurrentBuildingIndex = 0;
            m_BuildHUD.SelectBuilding((BuildingType)m_CurrentBuildingIndex);
        }
       
    }

    public void UpdateHUD(float time)
    {
        m_BuildHUD.UpdateHUD(((int)time).ToString() + " s");
    }

    /// <summary>
    /// Method that configures the actual building
    /// </summary>
    /// <param name="type">Build type (used as index)</param>
    public void SelectBuilding(BuildingType type)
    {
        m_CurrentBuildingIndex = (int)type;
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
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (index != -1 && tile != null && tile.Available)
            {
                //we can't block the routes throug the map
                tile.Navigable = false; //has to do this to test if there is a valid path

                Tile spawners = MapManager.Singleton.getTileByWorldPosition(SpawnerManager.Singleton.Spawners[0].GetComponent<Transform>().position);
                Tile crystals = MapManager.Singleton.getTileByWorldPosition(CrystalManager.Singleton.Crystals[0].GetComponent<Transform>().position);

                List<Tile> path = NavigationPathfinder.Singleton.calculatePath(spawners, crystals);

                if (path != null)
                {
                    if (MoneyManager.Singleton.SpendMoney(m_Buildings[index].m_Cost))
                    {
                        PoolManager.Singleton.getInstance(m_Buildings[index].m_Building, tile.NavigationPosition, Quaternion.identity);

                        //Tell the enemies that they must recalculate their routes
                        foreach (GameObject enemy in EnemyManager.Singleton.Enemies)
                        {
                            enemy.SendMessage("CalculateRouteToTarget", SendMessageOptions.DontRequireReceiver);
                        }

                        tile.Available = false;
                        tile.Navigable = false;
                    }
                    else
                    {
                        Debug.LogWarning("Not enough money");
                        tile.Navigable = true;
                        tile.Available = false;
                    }

                }
                else
                {
                    Debug.LogWarning("Cannot block map routes with buildings");
                    tile.Navigable = true;
                    tile.Available = false;
                }

                drawGhost(m_Buildings[index].m_Building, tile, m_Buildings[index].m_Cost);
            }
        }   
    }

    /// <summary>
    /// This method draws a building ghost in the given tile
    /// </summary>
    /// <param name="building">Ghost to draw</param>
    /// <param name="tile">Tile where to draw</param>
    /// <param name="cost">Cost of the building</param>
    private void drawGhost(GameObject building, Tile tile, float cost)
    {
        //if there were any ghost in a previous tile, first destroy it
        if (m_CurrentBuildingGhost != null)
        {
            PoolManager.Singleton.destroyInstance(m_CurrentBuildingGhost);
            m_CurrentBuildingGhost = null;
        }

        bool enoughMoney = MoneyManager.Singleton.Money >= cost;
        m_MoneyHUDController.EnoughMoney(enoughMoney);

        if (tile.Available && enoughMoney)
        {
            m_CurrentBuildingGhost = PoolManager.Singleton.getInstance(building.GetComponent<BuildGhostsRegister>().m_GreenGhostPrefab, tile.NavigationPosition, Quaternion.identity);
        }
        else
        {
            m_CurrentBuildingGhost = PoolManager.Singleton.getInstance(building.GetComponent<BuildGhostsRegister>().m_RedGhostPrefab, tile.NavigationPosition, Quaternion.identity);
        }

    }


    /// <summary>
    /// Callback used when mouse is pressed
    /// </summary>
    /// <param name="button">Button pressed</param>
    private void onMousePressed(InputManager.MouseButton button)
    {

        if (InputManager.MouseButton.LEFT == button && m_CanBuild)
            createBuilding(m_CurrentBuildingIndex, m_CurrentTile);

    }

    #endregion

    #region Monobehavior calls

    /// <summary>
    /// In this update the manager will check which tile is the current and if it must draw the building ghost of the actual building
    /// </summary>
    private void FixedUpdate()
    {
        if (m_CanBuild)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            int mask = (1 << LayerMask.NameToLayer("Tiles"));
            mask |= (1 << LayerMask.NameToLayer("HUD"));

            RaycastHit hitInfo;

            //Debug.DrawRay(ray.origin, ray.direction, Color.green);
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                if (Physics.Raycast(ray, out hitInfo, 1000.0f, mask))
                {
                    //Debug.Log(hitInfo.collider.gameObject.tag);
                    GameObject col = hitInfo.collider.gameObject;
                    if (col.tag == "Tiles" && m_CurrentTile != col.GetComponent<Tile>())
                    {
                        m_CurrentTile = col.GetComponent<Tile>();

                        if (m_CurrentBuildingIndex != -1)
                            drawGhost(m_Buildings[m_CurrentBuildingIndex].m_Building, m_CurrentTile, m_Buildings[m_CurrentBuildingIndex].m_Cost);
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
