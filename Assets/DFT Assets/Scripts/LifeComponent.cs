///----------------------------------------------------------------------
/// @file LifeComponent.cs
///
/// This file contains the declaration of LifeComponent class.
/// 
/// Every GameObject that can be damage, must have this component. This component controls life variables
/// also the dead conditions. When the GameObject dies, a message "onDie" will be sent.
/// 
/// This component is for general purpose, if a specification of this component were needed, this class should be inherited by another one
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 22/07/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections;

public class LifeComponent : MonoBehaviour {

	#region Public params
    
    /// <summary>
    /// Max life of the GameObject
    /// </summary>
    public float m_MaxLife = 100.0f;


    /// <summary>
    /// Initial life of the GameObject
    /// </summary>
    public float m_InitialLife = 100.0f;


    /// <summary>
    /// Life regeneration over time. Life quantity per second
    /// </summary>
    public float m_LifeRegeneration = 1.0f;

    /// <summary>
    /// Current GameObject life
    /// </summary>
    public float Life
    {
        get { return m_Life; }
    }

    /// <summary>
    /// Max life of the gameObject
    /// </summary>
    public float MaxLife
    {
        get { return m_MaxLife; }
    }

    #endregion

    #region Private params

    /// <summary>
    /// Current life of the gameobjet
    /// </summary>
    private float m_Life;

    #endregion

    #region Public methods

    /// <summary>
    /// Method used to damage the gameObject a certain quantity of life.
    /// If life reaches 0 (or below) the GameObject will die.
    /// </summary>
    /// <param name="damage">Life to decrease</param>
    public virtual void Damage(float damage)
    {
        m_Life -= damage;

        if (m_Life <= 0.0f)
        {
            Die();
        }
    }

    /// <summary>
    /// Method used to kill inmediatly the GameObject
    /// It will send "onDie" message
    /// </summary>
    public virtual void Die()
    {
        gameObject.SendMessage("onDie", SendMessageOptions.DontRequireReceiver);

        PoolManager.Singleton.destroyInstance(gameObject);
    }

    #endregion

    #region Private methods

    #endregion

    #region Monobehavior calls

    /// <summary>
    /// Initialization of current life to intial life values
    /// </summary>
    private void OnEnable()
    {
        m_Life = m_InitialLife;
    }


    /// <summary>
    /// Calculation of the life regeneration
    /// </summary>
    private void Update()
    {
        float regenaratedLife = m_Life + m_LifeRegeneration * Time.deltaTime;
        m_Life = Mathf.Min(m_MaxLife, regenaratedLife);

        //Regeneration could be negative, so we can die D_:
        if (m_Life <= 0.0f)
        {
            Die();
        }
    }

    #endregion

}
