///----------------------------------------------------------------------
/// @file TurretPerception.cs
///
/// This file contains the declaration of TurretPerception class.
/// 
/// This script controls turret's perception of enemies. The behavior it's very simple, a turret will take into account only the enemies
/// inside the "sight radius". Starting with the closer one, the turret will check if there aren't any obstacles. If everything it's correct the turret
/// will try to shoot once.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 23/07/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TurretPerception : MonoBehaviour {

	#region Public params

    /// <summary>
    /// Perception radius, where the turret can "see" the enemies
    /// </summary>
    public float m_PerceptionRadius = 10.0f;


    /// <summary>
    /// Reference to perception point
    /// </summary>
    public Transform m_PerceptionPoint = null;

    #endregion

    #region Private params

    /// <summary>
    /// Squared radius, used for distance comparations
    /// </summary>
    private float m_SqrPerceptionRadius;

    /// <summary>
    /// Refrence to the current target
    /// </summary>
    private GameObject m_CurrentTarget = null;

    /// <summary>
    /// Reference to the ShootComponent of the turret
    /// </summary>
    private ShootComponent m_ShootComponent = null;

    #endregion


    #region Monobehavior calls

    /// <summary>
    /// This method will evaluate which are the enemies inside the sight radius, and between them who will be shot
    /// </summary>
    private void FixedUpdate()
    {
        List<GameObject> enemies = EnemyManager.Singleton.Enemies;

        Dictionary<GameObject, float> perceivedEnemies = new Dictionary<GameObject, float>();
        for (int i = 0; i < enemies.Count; ++i)
        {
            float sqrDist = (enemies[i].GetComponent<Transform>().position - m_PerceptionPoint.position).sqrMagnitude;
            if (sqrDist <= m_SqrPerceptionRadius)
            {
                perceivedEnemies.Add(enemies[i], sqrDist);
            }
        }

        foreach (KeyValuePair<GameObject, float> enemy in perceivedEnemies.OrderBy(key => key.Value))
        {

            //direct sight validation
            Vector3 direction = ((enemy.Key.GetComponent<Transform>().position + new Vector3(0, 1.0f, 0)) - m_PerceptionPoint.position);

            Debug.DrawLine(m_PerceptionPoint.position, enemy.Key.GetComponent<Transform>().position + new Vector3(0, 1.0f, 0), Color.green);

            RaycastHit hitinfo;
            int mask = (1 << LayerMask.NameToLayer("Turret"));
            mask |= (1 << LayerMask.NameToLayer("Crystal"));
            mask |= (1 << LayerMask.NameToLayer("Ignore Raycast"));
            mask = ~mask;
            if (Physics.Raycast(m_PerceptionPoint.position, direction, out hitinfo, m_PerceptionRadius, mask))
            {
                GameObject other = hitinfo.collider.gameObject;
                if (other.tag == "Enemy")
                {
                    m_ShootComponent.Shoot(hitinfo.point, other);
                    m_CurrentTarget = other;
                    break;
                }
                else
                {
                    if (m_CurrentTarget != null)
                    {
                        //message to alert that there's no current target
                        gameObject.SendMessage("OnTargetLost", SendMessageOptions.DontRequireReceiver);
                    }
                    m_CurrentTarget = null;
                    continue;
                }
            }
            else
            {
                if (m_CurrentTarget != null)
                {
                    //message to alert that there's no current target
                    gameObject.SendMessage("OnTargetLost", SendMessageOptions.DontRequireReceiver);
                }
                m_CurrentTarget = null;
                continue;
            }
        }

        if ((m_CurrentTarget != null && !m_CurrentTarget.activeSelf) || perceivedEnemies.Count <= 0)
        {
            //message to alert that there's no current target
            gameObject.SendMessage("OnTargetLost", SendMessageOptions.DontRequireReceiver);
        }
    }

    /// <summary>
    /// Fields and references initialization
    /// </summary>
    private void OnEnable()
    {
        m_SqrPerceptionRadius = m_PerceptionRadius * m_PerceptionRadius;
        m_ShootComponent = gameObject.GetComponent<ShootComponent>();
        m_CurrentTarget = null;
    }

    /// <summary>
    /// Debug draw for the sight radius
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(m_PerceptionPoint.position, m_PerceptionRadius);
    }

    #endregion

}
