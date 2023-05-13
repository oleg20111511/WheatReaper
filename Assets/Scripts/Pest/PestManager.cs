using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;

namespace Pests
{
    public class PestManager : MonoBehaviour
    {
        private static PestManager instance;

        [SerializeField] private int reward = 3;
        [SerializeField] private LayerMask pestsLayerMask;
        [SerializeField] private GameObject pestPrefab;
        [SerializeField] private List<Transform> spawnPoints;
        [SerializeField] private float spawnCooldownSeconds = 30f;
        [SerializeField] private float spawnCheckInterval = 1f;
        [SerializeField] private bool canSpawn = false;
        [SerializeField] private int packSize = 3;
        [SerializeField] private int packSizeRandomizationOffset = 2;
        [Range(1, 100)] [SerializeField] private int spawnChancePercent = 1;


        public static PestManager Instance
        {
            get { return instance; }
        }


        public int KillReward
        {
            get { return reward; }
            set { reward = value; }
        }


        public List<Transform> SpawnPoints
        {
            get { return spawnPoints; }
        }


        public LayerMask PestsLayerMask
        {
            get { return pestsLayerMask; }
        }


        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }


        private void Start()
        {
            StartCoroutine(SpawnCheck());
            GameManager.Instance.GameStateChanged += OnGameStateChanged;
        }


        private void OnGameStateChanged(GameStateBase newState)
        {
            if (newState.GetType() == typeof(StateGameplay))
            {
                canSpawn = true;
            }
            else
            {
                canSpawn = false;
            }
        }


        private IEnumerator SpawnCheck()
        {
            while (true)
            {
                if (canSpawn && Random.Range(1, 101) <= spawnChancePercent)
                {
                    SpawnPest();
                    StartCoroutine(StartCooldown());
                }
                yield return new WaitForSeconds(spawnCheckInterval);
            }
        }


        private void SpawnPest()
        {
            int amount = Random.Range(packSize - packSizeRandomizationOffset, packSize + packSizeRandomizationOffset + 1);
            for (int i = 0; i < amount; i++)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                GameObject.Instantiate(pestPrefab, spawnPoint.position, spawnPoint.rotation);
            }
        }


        private IEnumerator StartCooldown()
        {
            canSpawn = false;
            yield return new WaitForSeconds(spawnCooldownSeconds);
            canSpawn = true;
        }
    }
}