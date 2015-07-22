///----------------------------------------------------------------------
/// @file EnemySimpleBehavior.cs
///
/// This file contains the declaration of EnemySimpleBehavior class.
///
/// This script contains a simple (VERY SIMPLE) behavior for an enemy. May it be considered AI? May it. :)
/// The behavior is the following:
/// 
/// -At the start (OnEnable) the enemy will calculate the route choosing a random crystal and start moving
/// -If the crystal is destroyed, will choose another one and keep moving
/// 
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 22/07/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySimpleBehavior : MonoBehaviour
{

    #region Public params

    public NavigationAgent m_NavAgent = null;

    #endregion

    #region Private params

    private GameObject m_CurrentTarget = null;

    private Tile m_CurrentTargetTile = null;

    #endregion


    #region Public methods

    /// <summary>
    /// Choose a new target between all the crystals active in scene
    /// </summary>
    public void ChooseNewTarget()
    {
        if (CrystalManager.Singleton != null && CrystalManager.Singleton.Crystals.Count > 0)
        {
            m_CurrentTarget = CrystalManager.Singleton.Crystals[Random.Range(0, CrystalManager.Singleton.Crystals.Count)];

            m_CurrentTargetTile = MapManager.Singleton.getTileByWorldPosition(m_CurrentTarget.GetComponent<Transform>().position);
        }
        else
        {
            m_CurrentTarget = null;
            m_CurrentTargetTile = null;
            m_NavAgent.stopRoute();
        }
    }

    /// <summary>
    /// Calculates the route from the actual position
    /// </summary>
    public void CalculateRouteToTarget()
    {
        if (m_CurrentTargetTile != null && m_CurrentTarget != null)
        {
            List<Tile> path = NavigationPathfinder.Singleton.calculateOptimalPath(MapManager.Singleton.getTileByWorldPosition(gameObject.GetComponent<Transform>().position), m_CurrentTargetTile);

            if (path != null)
            {
                m_NavAgent.setPathToFollow(path);
            }
        }
    }

    #endregion


    #region Monobehavior calls

    private void OnEnable()
    {
        m_CurrentTarget = null;
        m_CurrentTargetTile = null;
        ChooseNewTarget();

        CalculateRouteToTarget();
    }

    private void OnDisable()
    {
        m_NavAgent.stopRoute();
    }

    private void FixedUpdate()
    {
        //if the target is not active in the scene
        if (!m_CurrentTarget.activeSelf)
        {
            ChooseNewTarget();
            CalculateRouteToTarget();
        }
    }

    #endregion
}
