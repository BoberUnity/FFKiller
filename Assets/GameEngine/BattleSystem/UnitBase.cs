using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngineInternal;

namespace Assets.GameEngine.BattleSystem
{
    public class UnitBase : MonoBehaviour
    {
        public enum UnitType
        {
            Ally,
            Enemy
        }

        // To determine where the unit positions on the battle field grid.
        [Serializable]
        public enum GridPosition
        {
            Front,
            Back,
            Reserve
        }

        [HideInInspector]
        public GridPosition GrPosition;
        [HideInInspector]
        public int Row;

        public GameObject _gameObject;
        public Transform _transform;

        public int Level { get; private set; }

        public int _strength;
        public int Strength { get; set; }

        public int _agility;
        public int Agility { get; set; }
        public int Vitality { get; set; }
        public int Intelligence { get; private set; }


        public string Name;
        public Collider2D Collider;
        public UnitType TeamType;
        public bool IsMelee;

        public bool IsDead
        {
            get { return Health <= 0; }
        }

        public bool IsActive
        {
            get { return _gameObject != null && _gameObject.activeSelf; }
        }

        private int MaxHealth;
        public int Health { get; private set; }
        public int Stamina { get; private set; }
        public int Mana { get; private set; }

        //���������� ������ � ���������
        public int DefencePercent { get; private set; }

        //���������� ������ � ��������, ����������� ����� ������� ����� DefencePercent
        public int DefenceUnits { get; private set; }

        //�������������� ������� ������, ������������ � DefenceUnits
        public int DefenceUnitsAdditional { get; set; }


        public List<AbilityBase.AbilityInd> LAbilityInds;
        [HideInInspector]
        public List<AbilityBase> LAbilities;

        protected int _comboSeq = 0;

        [HideInInspector]
        public AbilityBase.AbilityInd CurAbilityInd;
        [HideInInspector]
        public AbilityBase.AbilityInd PrevAbilityInd;
        public bool IsBusy { get; private set; }

        private UnitBase _activeTarget;
        private AbilityBase _activeAbility;

        protected Animator _anim;
        protected SpriteRenderer _spriteRend;
        protected Image _healthBar;

        // Use this for initialization
        public virtual void BaseStart()
        {
            _anim = GetComponent<Animator>();
            _spriteRend = GetComponent<SpriteRenderer>();
            _healthBar = GetComponentInChildren<Canvas>().GetComponentInChildren<Image>();


            MaxHealth = Health = 100;
            Level = 3;
            Strength = _strength;
            Agility = _agility;
            Intelligence = 10;

            LAbilities = new List<AbilityBase>();
            foreach (var abilityInd in LAbilityInds)
            {
                if (abilityInd == AbilityBase.AbilityInd.Attack)
                    LAbilities.Add(new Attack(this, _comboSeq));
                else if (abilityInd == AbilityBase.AbilityInd.Defence)
                    LAbilities.Add(new Defence(this));
                else if (abilityInd == AbilityBase.AbilityInd.Heal)
                    LAbilities.Add(new Heal(this));
                else if (abilityInd == AbilityBase.AbilityInd.Wait)
                    LAbilities.Add(new Wait(this));
            }
            CurAbilityInd = AbilityBase.AbilityInd.None;

            _gameObject = gameObject;
            _transform = transform;

            IsBusy = false;
        }

        public void StartAbilityTimer(AbilityBase activeAbility, UnitBase activeTarget, float time)
        {
            IsBusy = true;
            _activeAbility = activeAbility;
            _activeTarget = activeTarget;

            PrevAbilityInd = CurAbilityInd;
            CurAbilityInd = activeAbility._abilityInd;

            _anim.SetInteger("ActionType", activeAbility.AnimNumber);
            Invoke("EndAbilityTimer", time);
        }

        public void EndAbilityTimer()
        {
            IsBusy = false;
            _activeAbility.DealEffect(_activeTarget);
        }

        public void UpdateUI()
        {
            if (_healthBar.type == Image.Type.Filled)
                _healthBar.fillAmount = ((float)Health) / MaxHealth;
        }

        public void ReseveDamage(int damage)
        {
            if (damage < 0)
                Debug.Log(Name + ". Added Damage is smaller then 0");
            Health -= damage;
            UpdateUI();
        }

        public void AddHP(int hp)
        {
            if (hp < 0)
                Debug.Log(Name + ". Added HP is smaller then 0");
            Health += hp;
            if (Health > MaxHealth)
                Health = MaxHealth;
            UpdateUI();
        }

        public void Die()
        {
            Health = -1;

            _anim.SetInteger("ActionType", 3);
            //_gameObject.SetActive(false);
        }

        public void BaseUpdate()
        {
            foreach (var ability in LAbilities)
            {
                ability.AbilityElapsedTime += Time.deltaTime;
                if (ability.IsProlonged && ability.IsActive && ability.AbilityElapsedTime > ability.AbilityDuration)
                    ability.DisableEffect();
            }
        }
    }
}
