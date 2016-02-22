using System;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

namespace Assets.GameEngine.BattleSystem
{
    public abstract class AbilityBase
    {
        [Serializable]
        public enum AbilityInd
        {
            Attack,
            Heal,
            Defence,
            Wait,
            None
        }

        public enum TargetType
        {
            Ally,
            Enemy,
            Self
        }


        public Texture2D Icon;

        public int StaminaCost;
        public int ManaCost;
        protected float _abilityCastTime;
        public float AbilityDuration { get; protected set; }
        public float AbilityElapsedTime;
        // Владелец способности
        protected UnitBase _owner;
        // Animation number in controller
        public int AnimNumber { get; protected set; }

        public bool IsProlonged { get; protected set; }
        public bool IsActive { get; protected set; }
        public TargetType Type { get; protected set; }



        public AbilityInd _abilityInd { get; protected set; }

        public abstract bool IsTargetCorrect(UnitBase target);
        public abstract void Effect(UnitBase target);
        public abstract void DealEffect(UnitBase target);
        public abstract void DisableEffect();

        protected void BasisEffect(UnitBase target)
        {
            _owner.StartAbilityTimer(this, target, _abilityCastTime);
            _owner.CurAbilityInd = _abilityInd;

            //DealEffect(target);
        }

    }


    /// <summary>
    /// Attack Ability
    /// </summary>
    public class Attack : AbilityBase
    {
        private int _comboSeq;
        private int _seqNum;

        public Attack(UnitBase owner, int comboSeq = 0)
        {
            _owner = owner;
            StaminaCost = 1;
            ManaCost = 0;
            IsProlonged = false;
            Type = TargetType.Enemy;
            _abilityInd = AbilityInd.Attack;
            _abilityCastTime = 1.0f;

            _seqNum = 0;
            _comboSeq = comboSeq;
            AnimNumber = 1;
        }

        public override bool IsTargetCorrect(UnitBase target)
        {
            if (_owner.TeamType == target.TeamType)
                return false;
            return true;
        }

        public override void Effect(UnitBase target)
        {
            if (_owner.TeamType == target.TeamType || target.IsDead)
                return;
            BasisEffect(target);

        }
        public override void DealEffect(UnitBase target)
        {
            if (_owner.PrevAbilityInd == _abilityInd)
                _seqNum++;
            if (_seqNum >= _comboSeq)
                _seqNum = 0;

            float combo = 1 + _seqNum * 0.25f;
            float damage = (_owner.Strength + (_owner.Strength / 10) * (_owner.Strength / 10)) * combo;
            target.ReseveDamage((int)damage);

            Debug.Log(_owner.Name + " to " + target.Name + ", deal " + damage + " damage");
        }

        public override void DisableEffect() {}
    }

    /// <summary>
    /// Defence Ability
    /// </summary>
    public class Defence : AbilityBase
    {
        //Added defence bonus after start an effect. Need to get defence back right the same points.
        private int _addedDefBonus;

        public Defence(UnitBase owner)
        {
            _owner = owner;
            StaminaCost = 3;
            ManaCost = 0;
            IsProlonged = true;
            IsActive = false;
            Type = TargetType.Self;
            _abilityInd = AbilityInd.Defence;
            _abilityCastTime = 1f;
            AbilityDuration = 5f;

            AnimNumber = 4;
        }

        public override bool IsTargetCorrect(UnitBase target)
        {
            return target == _owner;
        }

        //Effect target with current ability, if skill is self applied then target could be refered to this
        public override void Effect(UnitBase target)
        {
            BasisEffect(target);

        }

        public override void DealEffect(UnitBase target)
        {
            //if (!IsActive)
            //{
            //    AbilityElapsedTime = 0f;
            //    IsActive = true;
            //    int defAmount = 5;
            //    _addedDefBonus += defAmount;
            //    _owner.DefenceUnitsAdditional += defAmount;
            //    Debug.Log("To " + _owner.Name + ", was added " + defAmount + " defence units");
            //}
            //else
            //{
            //    IsActive = false;
            //    _owner.DefenceUnitsAdditional -= _addedDefBonus;
            //    Debug.Log("From " + _owner.Name + ", was took " + _addedDefBonus + " defence units");
            //    _addedDefBonus = 0;
            //}
            AbilityElapsedTime = 0f;
            IsActive = true;
            int defAmount = 5;
            _addedDefBonus += defAmount;
            _owner.DefenceUnitsAdditional += defAmount;
            Debug.Log("To " + _owner.Name + ", was added " + defAmount + " defence units");

        }

        public override void DisableEffect()
        {
            IsActive = false;
            _owner.DefenceUnitsAdditional -= _addedDefBonus;
            _addedDefBonus = 0;
        }
    }


    /// <summary>
    /// Heal Ability
    /// </summary>
    public class Heal : AbilityBase
    {
        public Heal(UnitBase owner)
        {
            _owner = owner;
            StaminaCost = 0;
            ManaCost = 3;
            IsProlonged = false;
            Type = TargetType.Ally;
            _abilityInd = AbilityInd.Heal;
            _abilityCastTime = 1.0f;

            AnimNumber = 4;
        }

        public override bool IsTargetCorrect(UnitBase target)
        {
            if (_owner.TeamType != target.TeamType)
                return false;
            return true;
        }

        // I may create abilities, that take mana permanent, not once. And if mana ends, i will off an ability.
        public override void Effect(UnitBase target)
        {
            if (_owner.TeamType != target.TeamType)
                return;
            BasisEffect(target);

        }

        public override void DealEffect(UnitBase target)
        {
            int hp = (_owner.Intelligence + _owner.Level) * 3;
            target.AddHP(hp);

            Debug.Log(_owner.Name + " to " + target.Name + ", heal with " + hp);
        }

        public override void DisableEffect() { }
    }


    /// <summary>
    /// Defence Ability
    /// </summary>
    public class Wait : AbilityBase
    {
        //Added defence bonus after start an effect. Need to get defence back right the same points.
        private int _addedDefBonus;

        public Wait(UnitBase owner)
        {
            _owner = owner;
            StaminaCost = 0;
            ManaCost = 0;
            IsProlonged = false;
            Type = TargetType.Self;
            _abilityInd = AbilityInd.Wait;
            _abilityCastTime = 1f;

            AnimNumber = 0;
        }

        public override bool IsTargetCorrect(UnitBase target)
        {
            return target == _owner;
        }

        //Effect target with current ability, if skill is self applied then target could be refered to this
        public override void Effect(UnitBase target)
        {
            BasisEffect(target);

        }

        public override void DealEffect(UnitBase target)
        {
            Debug.Log(_owner.Name + "was waiting for " + _abilityCastTime + " seconds!");
        }

        public override void DisableEffect() { }
    }


}
