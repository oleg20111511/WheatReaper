using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PestState {
    RunningToField,
    EatingCrop,
    RunningAway
}


[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PestController : MonoBehaviour
{
    public static List<PestController> AllPestControllers {get; private set;} = new List<PestController>();

    [SerializeField] private int movementSpeed = 10;
    [SerializeField] private float cropEatingDuration = 4f;
    [SerializeField] private int eatingLimit = 3;

    private Rigidbody2D rb2d;

    private Transform movementTarget;
    private WheatController target;
    private PestState currentState;
    private Coroutine eatingTask;
    private int fieldsEaten = 0;


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
        if (currentState == PestState.RunningToField && Utils.IsClose(movementTarget, transform))
        {
            rb2d.velocity = Vector2.zero;
            movementTarget = null;
            eatingTask = StartCoroutine(EatCrop());
        }
        else if (currentState == PestState.RunningAway && Utils.IsClose(movementTarget, transform))
        {
            GameObject.Destroy(gameObject);
        }
    }


    private void OnDestroy()
    {
        AllPestControllers.Remove(this);
    }


    public void GetHit()
    {
        if (eatingTask != null)
        {
            StopCoroutine(eatingTask);
        }
        
        Die();
    }


    private void ChangeTarget()
    {
        target = FindRandomTarget();
        if (target)
        {
            currentState = PestState.RunningToField;
            MoveTo(target.transform);
        }
        else
        {
            RunAway();
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


    private WheatController FindClosestTarget()
    {
        List<WheatController> grownFields = WheatController.AllWheatControllers.FindAll(w => w.IsGrown());
        if (grownFields.Count > 0)
        {
            target = Utils.FindClosestObject<WheatController>(grownFields, transform, w => w.transform);
        }
        else
        {
            target = null;
        }

        return target;
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
        fieldsEaten++;

        if (fieldsEaten < eatingLimit)
        {
            ChangeTarget();
        }
        else
        {
            RunAway();
        }
    }


    private void RunAway()
    {
        currentState = PestState.RunningAway;
        Transform extractionPoint = Utils.FindClosestTransform(PestManager.Instance.SpawnPoints, transform);
        MoveTo(extractionPoint);
    }


    private void MoveTo(Transform newTarget)
    {
        movementTarget = newTarget;
        Vector2 direction = (newTarget.position - transform.position).normalized;
        rb2d.velocity = direction * movementSpeed;
    }


    private void Die()
    {
        GameObject.Destroy(gameObject);
    }
}
