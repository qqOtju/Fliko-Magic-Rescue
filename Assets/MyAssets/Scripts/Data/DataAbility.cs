using System;
using MyAssets.Scripts.Data.SO;
using UnityEngine;

namespace MyAssets.Scripts.Data
{
    public class DataAbility
    {
        private const string CountKey = "AbilityCount";
        private bool _activated = false;
        private int _count;

        public AbilitySO Ability { get; }

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                SetCount(_count);
            }
        }

        public bool Activated
        {
            get => _activated;
            set
            {
                _activated = value;
                if(_activated)
                    OnAbilityUse?.Invoke(Ability.ID);
                else
                    OnAbilityEnd?.Invoke(Ability.ID);
            }
        }

        public event Action<int> OnAbilityUse;
        public event Action<int> OnAbilityEnd; 

        public DataAbility(AbilitySO ability)
        {
            Ability = ability;            
            _count = GetCount();
            Activated = false;
        }
        
        private int GetCount() =>
            PlayerPrefs.GetInt($"{CountKey}{Ability.Name}");
        
        private void SetCount(int count) =>
            PlayerPrefs.SetInt($"{CountKey}{Ability.Name}", count);
    }
}