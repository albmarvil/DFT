//----------------------------------------------------------------------
// @file NavigationAgent.cs
//
// This file contains the declaration of NavigationAgent class.
//
// This script moves a gameObject from navPoint to navPoint on the square grid.
//
// @author Alberto Martinez Villaran <tukaram92@gmail.com>
// @date 21/07/2015
//----------------------------------------------------------------------



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavigationAgent : MonoBehaviour {

	#region Public params

    /// <summary>
    /// Max agent speed
    /// </summary>
    public float m_Speed = 1.0f;

    /// <summary>
    /// Distance used as stopping criteria
    /// </summary>
    public float m_StoppingDistance = 1.0f;

    /// <summary>
    /// Property to access to the current navPoint where the agent is moving
    /// </summary>
    public Vector3 CurrentDestination
    {
        get { return m_CurrentRoute.Peek().NavigationPosition; }
    }

    /// <summary>
    /// Max agent speed
    /// </summary>
    public float Speed
    {
        get { return m_Speed; }
        set 
        { 
            m_Speed = value;
            if (m_CurrentRoute.Count > 0)
            {
                m_CurrentSpeed = m_CurrentSpeed.normalized * m_Speed;
            }
        }
    }

    #endregion

    #region Private params

    /// <summary>
    /// Current route, where the peek is the next navPoint in the route
    /// </summary>
    private Stack<Tile> m_CurrentRoute = new Stack<Tile>();

    /// <summary>
    /// Current speed of the agent
    /// </summary>
    private Vector3 m_CurrentSpeed = Vector3.zero;

    /// <summary>
    /// Squared value of the stopping distance
    /// </summary>
    private float m_SqrStoppingDistance;

    /// <summary>
    /// Direction where the agent is moving. It's a normalized vector
    /// </summary>
    private Vector3 m_MovingDirection = Vector3.zero;

    /// <summary>
    /// Flag that indicates if the agent can move or not
    /// </summary>
    private bool m_canNavigate = true;

    /// <summary>
    /// Reference to the agent transform
    /// </summary>
    private Transform m_Transform = null;

    /// <summary>
    /// Last remaining distance, used to know if we are going towards the next navPoint. If not, route will be corrected
    /// </summary>
    private float m_LastsqrRemainingDistance = float.MaxValue;

    #endregion

    #region Public methods

    /// <summary>
    /// This is the method used to set a new path to follow.
    /// The path will be a Tile list to follow, ordered from Destination to Origin.
    /// </summary>
    /// <param name="path">List of tiles to follow</param>
    public void setPathToFollow(List<Tile> path)
    {
        m_CurrentRoute.Clear();

        for (int i = 0; i < path.Count; ++i)
        {
            m_CurrentRoute.Push(path[i]);
        }

        m_CurrentRoute.Pop();

        resumeRoute();
    }

    /// <summary>
    /// Public method used to stop and erase the current route
    /// </summary>
    public void stopRoute()
    {
        pauseRoute();

        m_CurrentRoute.Clear();
    }

    /// <summary>
    /// Public method used to pause the movement of the agent
    /// </summary>
    public void pauseRoute()
    {
        m_CurrentSpeed = Vector3.zero;
        m_MovingDirection = Vector3.zero;

        m_canNavigate = false;
    }

    /// <summary>
    /// Public method used to start the movemen of the agent
    /// </summary>
    public void resumeRoute()
    {
        if(m_CurrentRoute.Count > 0)
        {
            m_canNavigate = true;

            m_MovingDirection = CurrentDestination - m_Transform.position;
            m_MovingDirection.y = 0;

            m_MovingDirection.Normalize();


            m_CurrentSpeed.x = m_Speed * m_MovingDirection.x;
            m_CurrentSpeed.y = m_Speed * m_MovingDirection.y;
            m_CurrentSpeed.z = m_Speed * m_MovingDirection.z;

            //update orientation
            m_Transform.forward = m_MovingDirection;
        }
        
    }

    /// <summary>
    /// This method returns the remaining distance of the agent to the navPoint given.
    /// For distance comparations should use getSqrReminingDistance instead.
    /// </summary>
    /// <param name="navPoint">Navpoint to calculate the distance</param>
    /// <returns>Remaining distance between AGENT and navPoint</returns>
    public float getRemainingDistance(Vector3 navPoint)
    {
        return Mathf.Sqrt(getSqrRemainingDistance(navPoint));
    }

    /// <summary>
    /// This method returns the remaining distance between two different points
    /// </summary>
    public float getRemainingDistance(Vector3 from, Vector3 to)
    {
        return Mathf.Sqrt(getSqrRemainingDistance(from, to));
    }

    /// <summary>
    /// This method returns the remaining Squared distance of the agent to the navPoint given
    /// </summary>
    /// <param name="navPoint">Navpoint to calculate the distance</param>
    /// <returns>Remaining squared distance between AGENT and navPoint</returns>
    public float getSqrRemainingDistance(Vector3 navPoint)
    {
        return getSqrRemainingDistance(m_Transform.position, navPoint);
    }

    /// <summary>
    /// This method returns the remaining squared distance between two different points
    /// </summary>
    public float getSqrRemainingDistance(Vector3 from, Vector3 to)
    {
        return (to - from).sqrMagnitude;
    }

    #endregion

    #region Monobehavior calls

    /// <summary>
    /// Each time that the agent is activated we clear the route
    /// </summary>
    private void OnEnable()
    {
        m_CurrentRoute.Clear();

        m_SqrStoppingDistance = m_StoppingDistance * m_StoppingDistance;

        m_Transform = gameObject.GetComponent<Transform>();

        m_LastsqrRemainingDistance = float.MaxValue;
    }


    /// <summary>
    /// In each update we calculate the new position and if the route has finished
    /// </summary>
    private void Update()
    {
        if (m_canNavigate && m_CurrentRoute.Count > 0)
        {
            float remainingDist = getSqrRemainingDistance(m_CurrentRoute.Peek().NavigationPosition);
            //stop?
            if (remainingDist <= m_SqrStoppingDistance)
            {
                //we have reached a new navPoint
                m_CurrentRoute.Pop();

                resumeRoute();
            }

            ///Are we going forward the route?
            if (m_LastsqrRemainingDistance <= remainingDist)
            {
                resumeRoute();
            }

            m_LastsqrRemainingDistance = remainingDist;

            //update position
            Vector3 newPos = m_Transform.position + m_CurrentSpeed * Time.deltaTime;
            newPos.y = CurrentDestination.y;

            m_Transform.position = newPos;

        }
        else
        {
            m_CurrentSpeed = Vector3.zero;
            m_canNavigate = false;
            m_MovingDirection = Vector3.zero;
        }
    }


    private void OnDrawGizmos()
    {
        Tile[] route = m_CurrentRoute.ToArray();
        for(int i = 0; i < route.Length-1; ++i)
        {
            Tile t = route[i];
            Tile t1 = route[i+1];
            Debug.DrawLine(t.NavigationPosition, t1.NavigationPosition);
        }


        //Vector3 forward = m_Transform.forward.normalized;

        //Debug.DrawLine(m_Transform.position, forward + m_Transform.position, Color.green);
    }

    #endregion

}
