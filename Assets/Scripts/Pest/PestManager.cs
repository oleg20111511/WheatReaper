using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestManager : MonoBehaviour
{
    private static PestManager instance;
    private static int reward = 5;

    [SerializeField] private LayerMask pestsLayerMask;
    [SerializeField] private GameObject pestPrefab;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private float spawnCooldownSeconds = 30f;
    [SerializeField] private float spawnCheckInterval = 1f;
    [SerializeField] private bool canSpawn = false;
    [Range(1, 100)] [SerializeField] private int spawnChancePercent = 1;


    public static PestManager Instance
    {
        get { return instance; }
    }


    public static int KillReward
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
    }


    public void Enable()
    {
        canSpawn = true;
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
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject.Instantiate(pestPrefab, spawnPoint.position, spawnPoint.rotation);
    }


    private IEnumerator StartCooldown()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnCooldownSeconds);
        canSpawn = true;
    }
}
