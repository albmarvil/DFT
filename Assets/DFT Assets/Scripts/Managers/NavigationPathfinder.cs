//----------------------------------------------------------------------
// @file NavigationPathfinder.cs
//
// This file contains the declaration of NavigationPathfinder class.
// NavigationPathfinder is a navigation manager. Pathfindig is done using Bredth-First Search on the graph formed by the Squared Grid contained in MapManager
// 
// The pathfinding algorithm supports node weights. A node is represented by Tile class. There are different tile types (obstacles, slower tiles...)
// Each one will have a different weight, being 1 the weight to the defaul tile.
//
// @author Alberto Martinez Villaran <tukaram92@gmail.com>
// @date 22/07/2015
//----------------------------------------------------------------------



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavigationPathfinder : MonoBehaviour {

    #region Singleton

    /// <summary>
    /// Singleton instance of the class
    /// </summary>
    private static NavigationPathfinder m_Instance = null;

    /// <summary>
    /// Property to get the singleton instance of the class.
    /// </summary>
    public static NavigationPathfinder Singleton { get { return m_Instance; } }

    // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
    static NavigationPathfinder() { }

    /// <summary>
    /// This is like the Init but done by the MonoBehaviour
    /// </summary>
    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        else
        {
            Debug.LogError("Someone is trying to create various NavigationPathfinder [" + name + "]");
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
    /// This flag allows to navigate using diagonals of the square grid
    /// </summary>
    public bool m_DiagonalNavigation = false;

    #endregion

    #region Private params

    /// <summary>
    /// Helper class that contains the info required for an BFS problem
    /// </summary>
    private class BFSState
    {
        public Queue<Tile> m_OpenedNodes = new Queue<Tile>();

        public float m_bound = float.MaxValue;

        public Dictionary<Tile, KeyValuePair<Tile, float>> m_VisitedNodes = new Dictionary<Tile, KeyValuePair<Tile, float>>();

        public Tile CurrentNode
        {
            get { return m_OpenedNodes.Peek(); }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// This method calculates the optimal path between two tiles using BFS.
    /// This version of the BFS algorithm includes concepts of branch and bound search, optimizing the optimal search avoiding branches
    /// with a higher cost than the best found.
    /// </summary>
    /// <param name="from">Origin tile for the path finding</param>
    /// <param name="to">Destination tile for the path finding</param>
    /// <returns>List of tiles (path) ordered from Destination to Origin</returns>
    public List<Tile> calculateOptimalPath(Tile from, Tile to)
    {
        BFSState currentState = new BFSState();

        currentState.m_VisitedNodes.Add(from, new KeyValuePair<Tile, float>(null, 0.0f));

        currentState.m_OpenedNodes.Enqueue(from);

        while (currentState.m_OpenedNodes.Count > 0)
        {
            List<Tile> children = currentState.CurrentNode.getChildren(m_DiagonalNavigation);

            //filter the children to obtain the Valid ones
            List<Tile> validChildren = new List<Tile>();
            foreach (Tile child in children)
            {
                if (child.Navigable || child == to) //destinations (node with crystals) aren't navigables
                {
                    validChildren.Add(child);
                }
            }

            foreach (Tile child in validChildren)
            {
                
                float totalPathWeight = child.NavigationWeight + currentState.m_VisitedNodes[currentState.CurrentNode].Value;

                if (totalPathWeight < currentState.m_bound)
                {
                    if (currentState.m_VisitedNodes.ContainsKey(child))
                    {
                        //if our current path is better than the existing one, we update values
                        if (currentState.m_VisitedNodes[child].Value > totalPathWeight)
                        {
                            currentState.m_VisitedNodes[child] = new KeyValuePair<Tile, float>(currentState.CurrentNode, totalPathWeight);
                            currentState.m_OpenedNodes.Enqueue(child);
                        }
                    }
                    else
                    {
                        currentState.m_VisitedNodes.Add(child, new KeyValuePair<Tile, float>(currentState.CurrentNode, totalPathWeight));
                        currentState.m_OpenedNodes.Enqueue(child);
                    }

                    //if the child is the destination, update the bound
                    if (child == to)
                    {
                        currentState.m_bound = totalPathWeight;
                    }
                }
            }

            currentState.m_OpenedNodes.Dequeue();
        }

        //now we have to rebuild the path
        //if we have reached the destination, there is a valid path
        if (currentState.m_VisitedNodes.ContainsKey(to))
        {
            List<Tile> path = new List<Tile>();
            path.Add(to);

            Tile auxNode = currentState.m_VisitedNodes[to].Key;

            while (auxNode != null)
            {
                path.Add(auxNode);
                auxNode = currentState.m_VisitedNodes[auxNode].Key;
            }

            path.Add(from);

            return path;
        }
        else
        {
            return null;
        }
    }



    /// <summary>
    /// Simpler version of the last algorithm. It also uses BFS but it will stop with the first paht found (optimal or not) and doesn't tkae into account node weights
    /// </summary>
    /// <param name="from">Origin tile for the path finding</param>
    /// <param name="to">Destination tile for the path finding</param>
    /// <returns>List of tiles (path) ordered from Destination to Origin</returns>
    public List<Tile> calculatePath(Tile from, Tile to)
    {
        BFSState currentState = new BFSState();

        currentState.m_VisitedNodes.Add(from, new KeyValuePair<Tile, float>(null, 0.0f));

        currentState.m_OpenedNodes.Enqueue(from);

        bool loop = true;

        while (currentState.m_OpenedNodes.Count > 0 && loop)
        {
            List<Tile> children = currentState.CurrentNode.getChildren(m_DiagonalNavigation);

            //filter the children to obtain the Valid ones
            List<Tile> validChildren = new List<Tile>();
            foreach (Tile child in children)
            {
                if (child.Navigable || child == to) //destinations (node with crystals) aren't navigables
                {
                    validChildren.Add(child);
                }
            }

            foreach (Tile child in validChildren)
            {
                float totalPathWeight = 1 + currentState.m_VisitedNodes[currentState.CurrentNode].Value;

                if (currentState.m_VisitedNodes.ContainsKey(child))
                {
                    //if our current path is better than the existing one, we update values
                    if (currentState.m_VisitedNodes[child].Value > totalPathWeight)
                    {
                        currentState.m_VisitedNodes[child] = new KeyValuePair<Tile, float>(currentState.CurrentNode, totalPathWeight);
                        currentState.m_OpenedNodes.Enqueue(child);
                    }
                }
                else
                {
                    currentState.m_VisitedNodes.Add(child, new KeyValuePair<Tile, float>(currentState.CurrentNode, totalPathWeight));
                    currentState.m_OpenedNodes.Enqueue(child);
                }

                //if the child is the destination, update the bound
                if (child == to)
                {
                    loop = false;
                    break;
                }
            }

            currentState.m_OpenedNodes.Dequeue();
        }

        //now we have to rebuild the path
        //if we have reached the destination, there is a valid path
        if (currentState.m_VisitedNodes.ContainsKey(to))
        {
            List<Tile> path = new List<Tile>();
            path.Add(to);

            Tile auxNode = currentState.m_VisitedNodes[to].Key;

            while (auxNode != null)
            {
                path.Add(auxNode);
                auxNode = currentState.m_VisitedNodes[auxNode].Key;
            }

            path.Add(from);

            return path;
        }
        else
        {
            return null;
        }
    }


    #endregion

}
