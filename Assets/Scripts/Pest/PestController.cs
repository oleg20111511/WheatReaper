using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PestController : MonoBehaviour
{
    public static List<PestController> AllPestControllers {get; private set;} = new List<PestController>();

    [SerializeField] private int movementSpeed = 10;
    [SerializeField] private float cropEatingDuration = 4f;

    private Rigidbody2D rb2d;

    private Transform movementTarget;
    private WheatController target;
    private PestState currentState;
    private Coroutine eatingTask;



    private void Awake()
    {
        if (!AllPestControllers.Contains(this))
        {
            AllPestControllers.Add(this);
        }

        rb2d = GetComponent<Rigidbody2D>();
    }


    private void Start()
    {
        ChangeTarget();
    }


    private void FixedUpdate()
    {
        if (currentState == PestState.RunningToField)
        {
            float distance = (movementTarget.position - transform.position).magnitude;
            if (distance < 0.1f)
            {
                rb2d.velocity = Vector2.zero;
                movementTarget = null;
                eatingTask = StartCoroutine(EatCrop());
            }
        }
    }


    private void ChangeTarget()
    {
        target = FindRandomTarget();
        if (target)
        {
            currentState = PestState.RunningToField;
            RunTowards(target.transform);
        }
        else
        {
            currentState = PestState.RunningAway;
            Transform extractionPoint = Utils.FindClosestTransform(PestManager.Instance.SpawnPoints, transform);
            RunTowards(extractionPoint);
        }
        
    }


    private WheatController FindRandomTarget()
    {
        List<WheatController> grownFields = WheatController.AllWheatControllers.FindAll(w => w.IsGrown());
        if (grownFields.Count > 0)
        {
            target = grownFields[Random.Range(0, grownFields.Count)];
        }
        else
        {
            target = null;
        }

        return target;
    }


    private void RunTowards(Transform newTarget)
    {
        movementTarget = newTarget;
        Vector2 direction = (newTarget.position - transform.position).normalized;
        rb2d.velocity = direction * movementSpeed;
    }




    private IEnumerator EatCrop()
    {
        // Check if target is still grown
        if (!target.IsGrown())
        {
            ChangeTarget();
            yield break;
        }

        currentState = PestState.EatingCrop;
        yield return new WaitForSeconds(cropEatingDuration);
        eatingTask = null;
        target.Infest();
        ChangeTarget();
    }


    public void GetHit()
    {
        if (eatingTask != null)
        {
            StopCoroutine(eatingTask);
        }
        
        Die();
    }


    private void Die()
    {
        AllPestControllers.Remove(this);
        GameObject.Destroy(this);
    }
}


public enum PestState {
    RunningToField,
    EatingCrop,
    RunningAway
}
