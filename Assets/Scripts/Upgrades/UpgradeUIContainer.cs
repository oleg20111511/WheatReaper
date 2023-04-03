using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUIContainer : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI upgradeName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI maxLevel;
    [SerializeField] private TextMeshProUGUI cost;

    private Upgrade upgrade;

    public Upgrade AssignedUpgrade
    {
        get { return upgrade; }
        set { 
            upgrade = value;
            upgradeName.text = upgrade.UpgradeName;
            description.text = upgrade.Description;
            cost.text = upgrade.Cost.ToString();
            if (upgrade.MaxLevel > 1)
            {
                level.text = (upgrade.CurrentLevel + 1).ToString();
                maxLevel.text = upgrade.MaxLevel.ToString();
            }
        }
    }

    public void ClickPurchase()
    {
        UpgradeManager.Instance.Purchase(this);
    }
}
