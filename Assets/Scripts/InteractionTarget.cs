using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Interaction
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class InteractionTarget : MonoBehaviour
    {
        public delegate bool InteractionCheck();
        public delegate void InteractionHandler();
        public event InteractionHandler Interacted;

        public static InteractionTarget currentTarget { get; private set; }

        public InteractionCheck interactionCheck;

        [SerializeField] private GameObject keyPrompt;

        private GameObject player;


        private void Start()
        {
            player = PlayerController.Instance.gameObject;
        }


        public bool IsInteractionEnabled()
        {
            if (interactionCheck != null)
            {
                return interactionCheck();
            }
            else
            {
                return true;
            }
        }


        public void Approach()
        {
            if (IsInteractionEnabled())
            {
                keyPrompt.SetActive(true);
            }
        }


        public void Leave()
        {
            keyPrompt.SetActive(false);
        }


        public void Interact()
        {
            Interacted?.Invoke();
        }


        private void OnTriggerEnter2D(Collider2D col)
        {
            // If player approaches set this as interaction target at notify previous interaction target
            if (col.gameObject == player)
            {
                if (currentTarget && currentTarget != this)
                {
                    currentTarget.Leave();
                }
                Approach();
                currentTarget = this;
            }
        }


        private void OnTriggerExit2D(Collider2D col)
        {
            if (currentTarget == this)
            {
                currentTarget.Leave();
                currentTarget = null;
            }
        }
    }
}
