using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private static UpgradeManager instance;

    [SerializeField] private GameObject upgradeUIContainerPrefab;
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private Transform upgradeMenuContent;

    private List<Upgrade> availableUpgrades = new List<Upgrade>();


    public UpgradeManager Instance
    {
        get { return instance; }
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenMenu();
        }
    }


    public void AddAvailableUpgrade(Upgrade upgrade)
    {
        // availableUpgrades.Add(upgrade);
        // UpgradeUIContainer upgradeUI = GameObject.Instantiate(upgradeUIContainerPrefab, upgradeMenuContent).GetComponent<UpgradeUIContainer>();
        // upgradeUI.upgradeName.text = upgrade.UpgradeName;
        for (int i = 0; i < 30; i++)
        {
            availableUpgrades.Add(upgrade);
            UpgradeUIContainer upgradeUI = GameObject.Instantiate(upgradeUIContainerPrefab, upgradeMenuContent).GetComponent<UpgradeUIContainer>();
            upgradeUI.upgradeName.text = upgrade.UpgradeName;
        }
        
    }


    public void OpenMenu()
    {
        upgradeMenu.SetActive(true);
    }


    public void CloseMenu()
    {
        upgradeMenu.SetActive(false);
    }
}
