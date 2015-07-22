//----------------------------------------------------------------------
// @file EnemyNavigation.cs
//
// This file contains the declaration of EnemyNavigation class.
//
// This script moves the enemy from navPoint to navPoint on the square grid.
//
// @author Alberto Martinez Villaran <tukaram92@gmail.com>
// @date 21/07/2015
//----------------------------------------------------------------------



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyNavigation : MonoBehaviour {

	#region Public params

    public float m_Speed = 1.0f;

    public float m_StoppingDistance = 1.0f;

    public Vector3 CurrentDestination
    {
        get { return m_CurrentRoute.Peek().NavigationPosition; }
    }

    #endregion

    #region Private params

    private Stack<Tile> m_CurrentRoute = new Stack<Tile>();

    private Vector3 m_CurrentSpeed = Vector3.zero;

    private float m_SqrStoppingDistance;

    private Vector3 m_MovingDirection = Vector3.zero;

    private bool m_canNavigate = true;

    private Transform m_Transform;

    #endregion

    #region Public methods

    //lista ordenada naturalmente de Origen --> destino, la recorremos al reves
    public void setPathToFollow(List<Tile> path)
    {
        m_CurrentRoute.Clear();

        for (int i = 0; i < path.Count; --i)
        {
            m_CurrentRoute.Push(path[i]);
        }
        ///puede que haya que hacer un primer pop para quitar el origen

        m_CurrentRoute.Pop();

        resumeRoute();
    }

    public void pauseRoute()
    {
        m_CurrentSpeed = Vector3.zero;

        m_canNavigate = false;
    }

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

        }
        
    }


    public float getRemainingDistance(Vector3 navPoint)
    {
        return Mathf.Sqrt(getSqrRemainingDistance(navPoint));
    }

    public float getRemainingDistance(Vector3 from, Vector3 to)
    {
        return Mathf.Sqrt(getSqrRemainingDistance(from, to));
    }

    public float getSqrRemainingDistance(Vector3 navPoint)
    {
        return getSqrRemainingDistance(m_Transform.position, navPoint);
    }

    public float getSqrRemainingDistance(Vector3 from, Vector3 to)
    {
        return (to - from).sqrMagnitude;
    }

    #endregion

    #region Private methods

    #endregion

    #region Monobehavior calls

    private void OnEnable()
    {
        m_CurrentRoute.Clear();

        m_SqrStoppingDistance = m_StoppingDistance * m_StoppingDistance;
    }

    private void Update()
    {
        if (m_canNavigate && m_CurrentRoute.Count > 0)
        {
            //stop?
            if (getSqrRemainingDistance(m_CurrentRoute.Peek().NavigationPosition) <= m_SqrStoppingDistance)
            {
                //we have reached a new navPoint
                m_CurrentRoute.Pop();

                resumeRoute();
            }

            //update position


            Vector3 newPos = m_Transform.position + m_CurrentSpeed * Time.deltaTime;

            m_Transform.position = newPos;

        }
        else
        {
            m_CurrentSpeed = Vector3.zero;
            m_canNavigate = false;
            m_MovingDirection = Vector3.zero;
        }
    }

    #endregion

}
