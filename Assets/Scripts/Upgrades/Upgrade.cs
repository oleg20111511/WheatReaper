using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    [SerializeField] protected string upgradeName;
    [SerializeField] protected string description;
    [SerializeField] protected int cost;
    [SerializeField] protected bool isAvailable = false;
    [SerializeField] protected int maxLevel = 1;
    [SerializeField] protected Sprite icon;
    public List<Upgrade> unlocks;

    protected int level = 0;


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
        get { return cost; }
    }

    public bool IsAvailable
    {
        get { return isAvailable; }
    }

    public int MaxLevel
    {
        get { return maxLevel; }
    }

    public int CurrentLevel
    {
        get { return level; }
    }


    public abstract void Activate();
}
