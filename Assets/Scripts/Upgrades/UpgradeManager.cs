using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameManagement;
using Player;

namespace Upgrades
{
    public class UpgradeManager : MonoBehaviour
    {
        private static UpgradeManager instance;

        [SerializeField] private GameObject upgradeUIContainerPrefab;
        [SerializeField] private GameObject upgradeMenu;
        [SerializeField] private Transform upgradeMenuContent;
        [SerializeField] private TextMeshProUGUI balanceText;
        [SerializeField] private List<Upgrade> availableUpgrades = new List<Upgrade>();
        private List<Upgrade> allUpgrades = new List<Upgrade>();


        public static UpgradeManager Instance
        {
            get { return instance; }
        }


        public bool IsMenuOpen
        {
            get { return upgradeMenu.activeSelf; }
        }


        public int AvailableUpgrades
        {
            get { return availableUpgrades.Count; }
        }


        // Reference to player's balance
        private int Balance
        {
            get { return PlayerController.Instance.Balance; }
            set
            {
                PlayerController.Instance.Balance = value;
                UpdateBalanceDisplay();
            }
        }


        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;

            Upgrade[] upgrades = gameObject.GetComponents<Upgrade>();
            foreach (Upgrade upgrade in upgrades)
            {
                allUpgrades.Add(upgrade);
            }
            LoadNewUpgrades();
        }


        private void Update()
        {
            if (Debug.isDebugBuild && Input.GetKeyDown(KeyCode.T))
            {
                Balance += 30;
                OpenMenu();
            }
        }


        public void OpenMenu()
        {
            GameManager.Instance.ChangeState<StateUpgradesMenu>();
            UpdateBalanceDisplay();
            LoadNewUpgrades();
            upgradeMenu.SetActive(true);
        }


        public void CloseMenu()
        {
            GameManager.Instance.ChangeState<StateGameplay>();
            upgradeMenu.SetActive(false);
        }


        public void Purchase(UpgradeUIContainer upgradeUI)
        {
            // 1) Check that upgrade can be purchased
            Upgrade upgrade = upgradeUI.AssignedUpgrade;
            if (!availableUpgrades.Contains(upgrade))
            {
                return;
            }
            if (upgrade.Cost > Balance)
            {
                return;
            }

            // 2) Apply upgrade's effect
            Balance -= upgrade.Cost;
            upgrade.Activate();

            // 3) Remove this upgrade from menu
            availableUpgrades.Remove(upgrade);
            GameObject.Destroy(upgradeUI.gameObject);
        }


        private void UpdateBalanceDisplay()
        {
            balanceText.text = Balance.ToString();
        }


        private void LoadNewUpgrades()
        {
            foreach (Upgrade upgrade in allUpgrades)
            {
                if (upgrade.IsAvailable && !availableUpgrades.Contains(upgrade))
                {
                    availableUpgrades.Add(upgrade);
                    UpgradeUIContainer upgradeUI = GameObject.Instantiate(upgradeUIContainerPrefab, upgradeMenuContent).GetComponent<UpgradeUIContainer>();
                    upgradeUI.AssignedUpgrade = upgrade;
                }
            }
        }
    }
}
