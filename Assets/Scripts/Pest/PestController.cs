using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;

namespace Pests
{
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
        public static string ANIMATION_RUN = "RatRun";
        public static string ANIMATION_EAT = "RatEat";

        [SerializeField] private int movementSpeed = 10;
        [SerializeField] private float cropEatingDuration = 4f;
        [SerializeField] private int damageLimit = 3;
        [SerializeField] private Animator animator;

        private Rigidbody2D rb2d;

        private Transform movementTarget;
        private GameObject target;
        private PestState currentState;
        private Coroutine eatingTask;
        private int objectsDamaged = 0;
        private string currentAnimation;


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
            ChangeTarget(true);
            GameManager.Instance.GetState<StateGameplay>().StateFixedUpdate += OnStateFixedUpdate;
        }


        private void OnDestroy()
        {
            AllPestControllers.Remove(this);
            GameManager.Instance.GetState<StateGameplay>().StateFixedUpdate -= OnStateFixedUpdate;
        }


        public void GetHit()
        {
            if (eatingTask != null)
            {
                StopCoroutine(eatingTask);
            }
            
            Die();
        }


        private void OnStateFixedUpdate()
        {
            if (currentState == PestState.RunningToField && Utils.IsClose(movementTarget, transform))
            {
                rb2d.velocity = Vector2.zero;
                movementTarget = null;
                eatingTask = StartCoroutine(DoDamage());
            }
            else if (currentState == PestState.RunningAway && Utils.IsClose(movementTarget, transform))
            {
                GameObject.Destroy(gameObject);
            }
        }


        private void PlayAnimation(string animationName)
        {
            currentAnimation = animationName;
            animator.Play(animationName);
        }


        private void MoveTo(Transform newTarget)
        {
            PlayAnimation(ANIMATION_RUN);
            movementTarget = newTarget;
            Vector2 direction = (newTarget.position - transform.position).normalized;
            rb2d.velocity = direction * movementSpeed;
            
            if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }


        private void ChangeTarget(bool random)
        {
            FindTarget(random);
            if (target != null)
            {
                currentState = PestState.RunningToField;
                MoveTo(target.transform);
            }
            else
            {
                RunAway();
            }
        }


        private void FindTarget(bool random)
        {
            List<WheatController> grownFields = WheatController.AllWheatControllers.FindAll(w => w.CanBeDamagedByPest);
            if (grownFields.Count > 0)
            {
                if (random)
                {
                    target = grownFields[Random.Range(0, grownFields.Count)].gameObject;
                }
                else
                {
                    target = Utils.FindClosestObject<WheatController>(grownFields, transform, w => w.transform).gameObject;
                }
            }
            else if (CartController.Instance.CanBeDamagedByPest)
            {
                target = CartController.Instance.gameObject;
            }
            else
            {
                target = null;
            }
        }


        private IEnumerator DoDamage()
        {
            IPestVulnerability targetVulnerability = target.GetComponent<IPestVulnerability>();
            // Check if target is still vulnerable
            if (!targetVulnerability.CanBeDamagedByPest)
            {
                ChangeTarget(false);
                yield break;
            }

            currentState = PestState.EatingCrop;
            PlayAnimation(ANIMATION_EAT);
            yield return new WaitForSeconds(cropEatingDuration);
            // Check if target is still vulnerable
            if (!targetVulnerability.CanBeDamagedByPest)
            {
                ChangeTarget(false);
                yield break;
            }
            eatingTask = null;
            targetVulnerability.DamageByPest();
            objectsDamaged++;

            if (objectsDamaged < damageLimit)
            {
                ChangeTarget(false);
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


        private void Die()
        {
            GameObject.Destroy(gameObject);
        }
    }
}
