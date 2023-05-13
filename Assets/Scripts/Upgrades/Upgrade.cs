using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Upgrades
{
    public abstract class Upgrade : MonoBehaviour
    {
        [SerializeField] protected string upgradeName;
        [SerializeField] protected string description;
        [SerializeField] protected int[] costByLevel;
        [SerializeField] protected bool isAvailable = false;
        [SerializeField] protected Sprite icon;
        [Serializable]
        public struct UnlockableUpgrade
        {
            public int level;
            public Upgrade upgrade;
        }
        [SerializeField] protected List<UnlockableUpgrade> unlocks;

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

        public Sprite Icon
        {
            get { return icon; }
        }


        public abstract void Activate();


        private void Awake()
        {

        }


        protected void OnActivate()
        {
            level++;
            if (level == MaxLevel)
            {
                IsAvailable = false;
            }

            foreach (UnlockableUpgrade unlockableUpgrade in unlocks)
            {
                if (unlockableUpgrade.level == CurrentLevel)
                {
                    unlockableUpgrade.upgrade.IsAvailable = true;
                }
            }
        }
    }
}
