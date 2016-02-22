using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

namespace Assets.GameEngine.BattleSystem
{
    public class AIController : UnitBase
    {
        [Serializable]
        public enum AIType
        {
            Wolf,
            Bear,
            SkeletonArcher
        }

        public AIType AiType;
        private bool _hasActiveSkill;
        private bool _isWaiting;
        private int _numberOfUse;
        private AbilityBase _currentAbility;
        private UnitBase _target;

        private Color _baseColor;
        // Use this for initialization
        public override void BaseStart()
        {
            base.BaseStart();
            _hasActiveSkill = false;
            _isWaiting = false;
            _baseColor = _spriteRend.color;
            _target = BattleManager.Hero;
            WaitForAWhile(1.0f + Random.value);
        }

        // Update is called once per frame
        void Update()
        {
            if (!BattleManager.BattleBegin || IsDead)
                return;
            if (_isWaiting)
                _anim.SetInteger("ActionType", 0);
            if (IsBusy || _isWaiting)
                return;


            if (_hasActiveSkill)
            {
                ContinueSkill();
                return;
            }

            if (AiType == AIType.Wolf)
                WolfBehavior();
            else if (AiType == AIType.SkeletonArcher)
                SkeletonBehaviour();

        }

        private void WolfBehavior()
        {
            if ((GrPosition == GridPosition.Back || GrPosition == GridPosition.Reserve) && BattleManager.EnemyGrid[0, Row] == null)
            {
                StepForward();
            }

            float random = Random.value * 100.0f;
            if (random > 80f)
            {
                TripleAttack();
            }
            else
            {
                SimpleAttack();
            }
        }

        private void SkeletonBehaviour()
        {
            SimpleAttack();
        }

        private void StepForward()
        {
            //Debug.Log("StepForward");
            WaitForAWhile(4.0f);
            Invoke("StepForwardFinalize", 4.0f);
        }

        private void StepForwardFinalize()
        {
            BattleManager.EnemyGrid[0, Row] = this;
            BattleManager.EnemyGrid[1, Row] = null;
            transform.position += Vector3.right * 4;
            GrPosition = GridPosition.Front;
        }

        private void ContinueSkill()
        {
            if (_numberOfUse <= 0)
            {
                _hasActiveSkill = false;
                _spriteRend.color = _baseColor;
                WaitForAWhile(Random.value + 1.5f);
            }
            else
            {
                _numberOfUse--;
                _currentAbility.Effect(_target);
            }
        }

        private void TripleAttack()
        {
            //Debug.Log("tripleAttack");
            _hasActiveSkill = true;
            _numberOfUse = 3;
            _baseColor = _spriteRend.color;
            _spriteRend.color = Color.red;
            foreach (var ability in LAbilities)
            {
                if (ability._abilityInd == AbilityBase.AbilityInd.Attack)
                    _currentAbility = ability;
            }
        }

        private void SimpleAttack()
        {
            //Debug.Log("SimpleAttack");
            _hasActiveSkill = true;
            _numberOfUse = 0;
            foreach (var ability in LAbilities)
            {
                if (ability._abilityInd == AbilityBase.AbilityInd.Attack)
                    _currentAbility = ability;
            }
            _currentAbility.Effect(_target);
        }

        private void WaitForAWhile(float time)
        {
            //Debug.Log("Waiting");
            _isWaiting = true;
            Invoke("DisableWaiting", time);
        }

        private void DisableWaiting()
        {
            _isWaiting = false;
        }
    }
}
