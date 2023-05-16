using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameManagement;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerWater : MonoBehaviour
    {
        [SerializeField] private int maxWaterAmount = 5;
        [SerializeField] private Slider indicator;
        [SerializeField] private float indicatorStayDuration = 2f;

        private PlayerInput input;

        private int waterAmount = 0;
        private Coroutine indicatorDisplayCoroutine;
        

        public int WaterAmount
        {
            get { return waterAmount; }
            set {
                waterAmount = Mathf.Min(value, maxWaterAmount);
                indicator.value = (float) waterAmount / maxWaterAmount;
                DisplayIndicator();
            }
        }

        public int MaxWaterAmount
        {
            get { return MaxWaterAmount; }
        }


        private void Start()
        {
            input = PlayerController.Instance.playerInput;
            GameManager.Instance.GetState<StateGameplay>().StateUpdate += OnStateUpdate;
        }


        public void OnStateUpdate()
        {
            if (input.interactionInput)
            {
                AttemptWater();
            }
        }


        private void AttemptWater()
        {
            if (WheatTarget.highlightedFields.Count > 0)
            {
                WheatGrowth target = WheatTarget.highlightedFields[0].WheatGrowth;
                target.Stage += 1;
                WaterAmount -= 1;
            }
        }


        private void DisplayIndicator()
        {
            if (indicatorDisplayCoroutine != null)
            {
                StopCoroutine(indicatorDisplayCoroutine);
            }
            indicatorDisplayCoroutine = StartCoroutine(IndicatorStay());
        }
        

        private IEnumerator IndicatorStay()
        {
            indicator.gameObject.SetActive(true);
            yield return new WaitForSeconds(indicatorStayDuration);
            indicator.gameObject.SetActive(false);
        }
    }
}