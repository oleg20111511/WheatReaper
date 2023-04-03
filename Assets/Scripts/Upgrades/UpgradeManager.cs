using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    private static UpgradeManager instance;

    [SerializeField] private GameObject upgradeUIContainerPrefab;
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private Transform upgradeMenuContent;
    [SerializeField] private TextMeshProUGUI balanceText;

    public List<Upgrade> availableUpgrades {get; private set;} = new List<Upgrade>();
    public List<Upgrade> upgradeUnlockQueue {get; private set;} = new List<Upgrade>();

    
    public static UpgradeManager Instance
    {
        get { return instance; }
    }

    
    public bool IsMenuOpen
    {
        get { return upgradeMenu.activeSelf; }
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
            if (upgrade.IsAvailable)
            {
                upgradeUnlockQueue.Add(upgrade);
            }
        }
        InstantiateUpgradesFromQueue();
    }


    public void OpenMenu()
    {
        PlayerController.Instance.Freeze();
        UpdateBalanceDisplay();
        InstantiateUpgradesFromQueue();
        upgradeMenu.SetActive(true);
    }


    public void CloseMenu()
    {
        PlayerController.Instance.Unfreeze();
        upgradeMenu.SetActive(false);
    }


    public void UpdateBalanceDisplay()
    {
        balanceText.text = Balance.ToString();
    }


    public void Purchase(UpgradeUIContainer upgradeUI)
    {
        Upgrade upgrade = upgradeUI.AssignedUpgrade;
        if (!availableUpgrades.Contains(upgrade))
        {
            return;
        }
        if (upgrade.Cost > Balance)
        {
            return;
        }

        Balance -= upgrade.Cost;
        upgrade.Activate();        

        if (upgrade.unlocks.Count > 0)
        {
            upgradeUnlockQueue.AddRange(upgrade.unlocks);
            upgrade.unlocks.Clear();
        }
        
        // Remove UI representation, but only remove from available upgrades list if max level reached
        if (upgrade.CurrentLevel >= upgrade.MaxLevel)
        {
            availableUpgrades.Remove(upgrade);
        }
        GameObject.Destroy(upgradeUI.gameObject);
    }


    private void InstantiateUpgradesFromQueue()
    {
        foreach (Upgrade upgrade in upgradeUnlockQueue)
        {
            availableUpgrades.Add(upgrade);
            UpgradeUIContainer upgradeUI = GameObject.Instantiate(upgradeUIContainerPrefab, upgradeMenuContent).GetComponent<UpgradeUIContainer>();
            upgradeUI.AssignedUpgrade = upgrade;
        }
        upgradeUnlockQueue.Clear();
    }
}
