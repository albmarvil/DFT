///----------------------------------------------------------------------
/// @file BulletController.cs
///
/// This file contains the declaration of BulletController class.
///
/// This script controls a gameObject, moving it in a given direction.
/// 
/// To start the movement it's necessary to set up the component, with the desired movement direction and movement speed.
/// Damage can also be configured.
/// 
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 23/07/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	#region Public params

    /// <summary>
    /// Reference to the GameObject transform
    /// </summary>
    public Transform m_Transform = null;

    #endregion

    #region Private params

    /// <summary>
    /// Current Movement Speed
    /// </summary>
    private Vector3 m_Speed = Vector3.zero;

    /// <summary>
    /// Current damage
    /// </summary>
    private float m_Damage = 0.0f;

    #endregion

    #region Public methods

    /// <summary>
    /// Method used to set up all the bullet variables
    /// </summary>
    /// <param name="speed">Max movement speed</param>
    /// <param name="direction">Movement direction(Not normalized)</param>
    /// <param name="damage">Bullet damage (OPTIONAL). 0.0f by default</param>
    public void setBulletConfiguration(float speed, Vector3 direction, float damage = 0.0f)
    {
        direction.Normalize();
        m_Speed = speed * direction;

        m_Damage = damage;
    }

    #endregion


    #region Monobehavior calls

    /// <summary>
    /// Update of the bullet position
    /// </summary>
    private void Update()
    {
        Vector3 newPos = m_Transform.position + m_Speed * Time.deltaTime;

        m_Transform.position = newPos;
    }

    /// <summary>
    /// Trigger function when the bullet collides with somenthing. If the collider is an enemy, the bullet inflicts damage.
    /// In Any case, the bullet will be destroyed after colliding
    /// </summary>
    /// <param name="other">Other collider</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.SendMessage("Damage", m_Damage, SendMessageOptions.DontRequireReceiver);
        }

        PoolManager.Singleton.destroyInstance(gameObject);
    }

    /// <summary>
    /// Reset old values
    /// </summary>
    public void OnEnable()
    {
        m_Speed = Vector3.zero;
        m_Damage = 0.0f;
    }

    #endregion

}
