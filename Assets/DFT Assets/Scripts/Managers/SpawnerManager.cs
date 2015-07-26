///----------------------------------------------------------------------
/// @file SpawnerManager.cs
///
/// This file contains the declaration of SpawnerManager class.
///
/// This manager has a register of all enemy spawners in the scene.
/// Also has the logic of how the enemy spawn it's done.
/// 
/// A brief explanation of how are the spawn rules. The manager will spawn enemies in waves. Between waves
/// will be a short period of time. Each wave has the following params:
/// 
/// -Number of enemies
/// -Time between each enemy spawned
/// 
/// Those params will bre increased in each wave
/// 
/// 
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date SomeMonth on SomeYear
///----------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerManager : MonoBehaviour {

    #region Singleton

    /// <summary>
    /// Singleton instance of the class
    /// </summary>
    private static SpawnerManager m_Instance = null;

    /// <summary>
    /// Property to get the singleton instance of the class.
    /// </summary>
    public static SpawnerManager Singleton { get { return m_Instance; } }

    // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
    static SpawnerManager() { }

    /// <summary>
    /// This is like the Init but done by the MonoBehaviour
    /// </summary>
    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        else
        {
            Debug.LogError("Someone is trying to create various SpawnerManager [" + name + "]");
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
    /// Helper class that holds the state info of a wave
    /// </summary>
    [System.Serializable]
    public class WaveCfg
    {
        public WaveCfg(int numEnemies, float spawnTime)
        {
            m_NumEnemies = numEnemies;
            m_RemainingEnemies = numEnemies;
            m_SpawnTime = spawnTime;
            m_TimeAcum = float.MaxValue;
            m_NumEnemiesKilled = 0;
        }

        /// <summary>
        /// Total number of enemies to spawn
        /// </summary>
        public int m_NumEnemies;

        /// <summary>
        /// Remaining enemies to spawn
        /// </summary>
        [System.NonSerialized]
        public int m_RemainingEnemies;

        /// <summary>
        /// Time between enemy spawn
        /// </summary>
        public float m_SpawnTime;

        /// <summary>
        /// Time acum to spawn enemies
        /// </summary>
        [System.NonSerialized]
        public float m_TimeAcum;

        /// <summary>
        /// Number of enemies killed in this wave
        /// </summary>
        [System.NonSerialized]
        public int m_NumEnemiesKilled;
    }

    /// <summary>
    /// List of the different prefabs to spawn radomly all the enemies
    /// </summary>
    public List<GameObject> m_EnemiesPrefabs = new List<GameObject>();

    /// <summary>
    /// Time in seconds between two different waves
    /// </summary>
    public float m_TimeBetweenWaves = 30.0f;

    /// <summary>
    /// Initial wave config
    /// </summary>
    public WaveCfg m_InitWaveCfg = null;

    /// <summary>
    /// % of contribuition to the values of a wave
    /// </summary>
    public float m_WaveContribuition = 1.0f;

    /// <summary>
    /// Reference to the Wave HUD controller
    /// </summary>
    public WavesHUDController m_WaveHUDController = null;

    /// <summary>
    /// Reference to the Enemies HUD Controller
    /// </summary>
    public EnemiesHUDController m_EnemiesHUDController = null;

    /// <summary>
    /// List of the active spawners in the scene
    /// </summary>
    public List<GameObject> Spawners
    {
        get { return m_spawners; }
    }

    #endregion

    #region Private params

    /// <summary>
    /// Flag that says if we are actually spawning enemies of a wave
    /// </summary>
    private bool m_inWave = false;

    /// <summary>
    /// Current configuration of the current wave
    /// </summary>
    private WaveCfg m_CurrentWave = null;

    /// <summary>
    /// Current number f waves completed + 1
    /// </summary>
    private int m_WaveCount = 0;

    /// <summary>
    /// Time acum to count time between waves
    /// </summary>
    private float m_TimeBetweenWavesAcum = 0.0f;

    /// <summary>
    /// List of the active spawners in the scene
    /// </summary>
    private List<GameObject> m_spawners = new List<GameObject>();

    #endregion

    #region Public methods

    /// <summary>
    /// Method used to increase the current number of enemies killed
    /// </summary>
    public void EnemyKilled()
    {
        if (m_CurrentWave != null)
        {
            m_CurrentWave.m_NumEnemiesKilled++;
            m_EnemiesHUDController.UpdateHUD((m_CurrentWave.m_NumEnemies - m_CurrentWave.m_NumEnemiesKilled).ToString(), m_CurrentWave.m_NumEnemies.ToString());
        } 
    }

    /// <summary>
    /// Registers a spawner in the manager
    /// </summary>
    /// <param name="spawner">Spawner to register</param>
    public void RegisterSpawner(GameObject spawner)
    {
        if (!m_spawners.Contains(spawner))
        {
            m_spawners.Add(spawner);
        }
    }

    /// <summary>
    /// Unregisters a spawner in the manager
    /// </summary>
    /// <param name="spawner">Spawner to unregister</param>
    public void UnregisterSpawner(GameObject spawner)
    {
        m_spawners.Remove(spawner);
    }

    #endregion

    #region Private methods

    /// <summary>
    /// This method spawn a random enemy in a random spawn point
    /// </summary>
    private void SpawnEnemy()
    {
        GameObject enemy = m_EnemiesPrefabs[Random.Range(0, m_EnemiesPrefabs.Count)];

        GameObject spawner = m_spawners[Random.Range(0, m_spawners.Count)];
        Vector3 position = spawner.GetComponent<Transform>().position;

        PoolManager.Singleton.getInstance(enemy, position, Quaternion.identity);

        --m_CurrentWave.m_RemainingEnemies;

        //Debug.Log("Enem: " + m_CurrentWave.m_RemainingEnemies + " / " + m_CurrentWave.m_NumEnemies);

        //Debug.Log("Time: " + m_CurrentWave.m_SpawnTime + " / " + m_InitWaveCfg.m_SpawnTime);
    }

    /// <summary>
    /// This method starts a new wave, updating all the values to increase difficulty through waves
    /// </summary>
    private void StartWave()
    {
        m_TimeBetweenWavesAcum = 0.0f;
        BuilderManager.Singleton.SetBuildingTurn(false);

        ++m_WaveCount;

        m_WaveHUDController.UpdateHUD(m_WaveCount);

        int newNumEnemies = (int)(m_CurrentWave.m_NumEnemies + m_WaveCount * m_WaveContribuition);

        float newSpawnTime = Mathf.Max(0.8f ,m_CurrentWave.m_SpawnTime - m_WaveCount * m_WaveContribuition * 0.08f);

        m_EnemiesHUDController.UpdateHUD(newNumEnemies.ToString(), newNumEnemies.ToString());

        WaveCfg newWave = new WaveCfg(newNumEnemies, newSpawnTime);

        m_CurrentWave = newWave;

        m_inWave = true;
    }

    /// <summary>
    /// This methods receives the events of logic orders sended by InputManager
    /// </summary>
    /// <param name="order">Order received</param>
    /// <param name="ok">state of the order (Positive or Negative)</param>
    /// <param name="value">Extra value</param>
    private void onOrderReceived(InputManager.InputOrders order, bool ok, float value)
    {
        if (order == InputManager.InputOrders.ACCEPT && ok)
        {
            if (!m_inWave)
            {
                StartWave();
            }
        }
    }

    #endregion

    #region Monobehavior calls

    private void Update()
    {

        if (m_inWave && m_CurrentWave != null)
        {
            m_CurrentWave.m_TimeAcum += Time.deltaTime;

            if (m_CurrentWave.m_TimeAcum >= m_CurrentWave.m_SpawnTime && m_CurrentWave.m_RemainingEnemies > 0)
            {
                SpawnEnemy();
                m_CurrentWave.m_TimeAcum = 0.0f;
            }

            //if we have spawned all the required enemies and there aren't any alive, we have finished the current wave
            if (m_CurrentWave.m_RemainingEnemies <= 0 && m_CurrentWave.m_NumEnemies == m_CurrentWave.m_NumEnemiesKilled)
            {
                m_inWave = false;
                BuilderManager.Singleton.SetBuildingTurn(!m_inWave);
            }
        }
        else
        {
            m_EnemiesHUDController.UpdateHUD("-", "-");

            m_TimeBetweenWavesAcum += Time.deltaTime;

            BuilderManager.Singleton.UpdateHUD(m_TimeBetweenWaves - m_TimeBetweenWavesAcum);

            if (m_TimeBetweenWavesAcum >= m_TimeBetweenWaves)
            {
                StartWave();
            }
        }
        
    }

    /// <summary>
    /// Initialization of the first wave
    /// </summary>
    private void Start()
    {
        m_InitWaveCfg.m_NumEnemiesKilled = m_InitWaveCfg.m_NumEnemies;
        m_CurrentWave = m_InitWaveCfg;
        m_inWave = true;

        InputManager.Singleton.RegisterOrderEvent(onOrderReceived);

    }

    /// <summary>
    /// Clear the spawners register
    /// </summary>
    private void OnDisable()
    {
        m_spawners.Clear();

        if(InputManager.Singleton != null)
            InputManager.Singleton.UnregisterOrderEvent(onOrderReceived);

    }

    #endregion

}
