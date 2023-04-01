using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestManager : MonoBehaviour
{
    private static PestManager instance;

    [SerializeField] private LayerMask pestsLayerMask;
    [SerializeField] private GameObject pestPrefab;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private float spawnCooldownSeconds = 30f;
    [SerializeField] private float spawnCheckInterval = 1f;
    [Range(1, 100)] [SerializeField] private int spawnChancePercent = 1;

    private bool canSpawn = true;


    public static PestManager Instance
    {
        get { return instance; }
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
        // Don't spawn if there's no fully grown field 
        // WARNING: This means that cooldown will still start if no pests spawns.
        // This encourages player to keep the fields harvested
        if (!WheatController.AllWheatControllers.Exists(w => w.IsGrown()))
        {
            return;
        }

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
