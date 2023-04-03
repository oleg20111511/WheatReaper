using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    [SerializeField] protected string upgradeName;
    [SerializeField] protected string description;
    [SerializeField] protected int[] costByLevel;
    [SerializeField] protected bool isAvailable = false;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected List<Upgrade> unlocks;

    protected int level = 0;
    
    protected bool wasActivatedBefore = false;


    public string UpgradeName
    {
        get { return upgradeName; }
    }

    public string Description
    {
        get { return description; }
    }

    public int Cost
    {
        get { return costByLevel[CurrentLevel]; }
    }

    public bool IsAvailable
    {
        get { return isAvailable; }
        set { isAvailable = value; }
    }

    public int MaxLevel
    {
        get { return costByLevel.Length; }
    }

    public int CurrentLevel
    {
        get { return level; }
    }


    public abstract void Activate();


    protected void OnActivate()
    {
        level++;
        if (level == MaxLevel)
        {
            IsAvailable = false;
        }
        if (!wasActivatedBefore)
        {
            wasActivatedBefore = true;
            foreach (Upgrade upgrade in unlocks)
            {
                upgrade.IsAvailable = true;
            }
        }
    }
}
