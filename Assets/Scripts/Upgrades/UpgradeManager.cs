using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private static UpgradeManager instance;

    [SerializeField] private GameObject upgradeUIContainerPrefab;
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private Transform upgradeMenuContent;

    public List<Upgrade> availableUpgrades {get; private set;} = new List<Upgrade>();


    public static UpgradeManager Instance
    {
        get { return instance; }
    }

    
    public bool IsMenuOpen
    {
        get { return upgradeMenu.activeSelf; }
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
                AddAvailableUpgrade(upgrade);
            }
        }
    }


    private void Update()
    {
        if (PlayerController.Instance.playerInput.exitInput && IsMenuOpen)
        {
            CloseMenu();
        }
    }


    public void AddAvailableUpgrade(Upgrade upgrade)
    {
        availableUpgrades.Add(upgrade);
        UpgradeUIContainer upgradeUI = GameObject.Instantiate(upgradeUIContainerPrefab, upgradeMenuContent).GetComponent<UpgradeUIContainer>();
        upgradeUI.upgradeName.text = upgrade.UpgradeName;
        
    }


    public void OpenMenu()
    {
        PlayerController.Instance.Freeze();
        upgradeMenu.SetActive(true);
    }


    public void CloseMenu()
    {
        PlayerController.Instance.Unfreeze();
        upgradeMenu.SetActive(false);
    }
}
