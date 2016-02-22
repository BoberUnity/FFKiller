using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace Assets.GameEngine.BattleSystem
{
    public class PlayerController : UnitBase
    {
        public class QueueElement
        {
            public AbilityBase Ability;
            public UnitBase Target;

            public QueueElement(AbilityBase ability, UnitBase target)
            {
                Ability = ability;
                Target = target;
            }
        }

        public List<QueueElement> Queue { get; private set; }
        public QueueElement curQueueElement;

        public UnitBase PickedUnit;
        private string PickedUnitName;

        public Texture2D attack;
        public Texture2D defence;
        public Texture2D heal;
        public Texture2D blank;
        public Texture2D wait;

        public AbilityBase CurrentAbility {get; private set;}

        // Use this for initialization
        private void Awake()
        {
            _comboSeq = 3;
            BaseStart();

            foreach (var ability in LAbilities)
            {
                if (ability._abilityInd == AbilityBase.AbilityInd.Attack)
                    ability.Icon = attack;
                else if (ability._abilityInd == AbilityBase.AbilityInd.Defence)
                    ability.Icon = defence;
                else if (ability._abilityInd == AbilityBase.AbilityInd.Heal)
                    ability.Icon = heal;
                else if (ability._abilityInd == AbilityBase.AbilityInd.Wait)
                    ability.Icon = wait;
            }

            if (LAbilities.Count > 0)
                CurrentAbility = LAbilities[0];

            Queue = new List<QueueElement>();

        }

        // Update is called once per frame
        private void Update()
        {
            if (!BattleManager.BattleBegin || IsDead)
                return;
            BaseUpdate();
            HotKeyMenu();

            bool isUnitPicked = false;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PickedUnit = null;
            foreach (var unit in BattleManager.EnemyUnits)
            {
                if (unit != null && unit.IsActive && unit.Collider.OverlapPoint(mousePosition))
                {
                    PickedUnitName = unit.Name;
                    PickedUnit = unit;
                    isUnitPicked = true;
                    break;
                }
            }

            if (Input.GetMouseButtonDown(0) && Queue.Count < 5 && isUnitPicked && CurrentAbility != null && PickedUnit != null && CurrentAbility.IsTargetCorrect(PickedUnit))
            {
                QueueElement el = new QueueElement(CurrentAbility, PickedUnit);
                Queue.Add(el);
            }

            if (Input.GetMouseButtonDown(1) && Queue.Count > 0)
            {
                Queue.RemoveAt(Queue.Count - 1);
            }

            if (Queue.Count > 0 && !IsBusy)
            {
                QueueElement el = Queue[0];
                el.Ability.Effect(el.Target);
                Queue.RemoveAt(0);
            }

            if (Input.GetMouseButton(0) && !IsBusy && Queue.Count == 0 && isUnitPicked && CurrentAbility != null && PickedUnit != null)
            {
                CurrentAbility.Effect(PickedUnit);
            }
            if (!IsBusy)
                _anim.SetInteger("ActionType", 0);
        }

        private void OnGUI()
        {
            if (!BattleManager.BattleBegin)
                return;

            GUI.Label(new Rect(5, 5, 100, 20), PickedUnitName);

            int offset = 0;
            foreach (var ability in LAbilities)
            {
                var oldColor = GUI.color;
                if (ability.IsProlonged && ability.IsActive)
                    GUI.color = Color.yellow;
                if (GUI.Button(new Rect(10 + offset, Screen.height - 60, 50, 50), ability.Icon))
                {
                    if (ability.Type == AbilityBase.TargetType.Self)
                    {
                        QueueElement el = new QueueElement(ability, this);
                        Queue.Add(el);
                    }
                    else
                    {
                        CurrentAbility = ability;
                    }
                }

                GUI.color = oldColor;
                offset += 60;
            }

            var curSkillColor = GUI.color;
            if (IsBusy)
                GUI.color = Color.red;

            offset = Screen.width/2 - 25;
            if (CurrentAbility != null)
                GUI.Box(new Rect(offset, Screen.height - 60, 50, 50), CurrentAbility.Icon);
            else
                GUI.Box(new Rect(offset, Screen.height - 60, 50, 50), blank);

            GUI.color = curSkillColor;


            offset += 60;
            for (int i = 0; i < 5; i++)
            {
                if (i < Queue.Count)
                    GUI.Box(new Rect(10 + offset, Screen.height - 60, 50, 50), Queue[i].Ability.Icon);
                else
                    GUI.Box(new Rect(10 + offset, Screen.height - 60, 50, 50), "");
                offset += 60;
            }
        }

        public void HotKeyMenu()
        {
            int keyCode = -1;
            if (Input.GetKeyDown(KeyCode.Alpha1))
                keyCode = 1;
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                keyCode = 2;
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                keyCode = 3;
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                keyCode = 4;
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                keyCode = 5;
            else if (Input.GetKeyDown(KeyCode.Alpha6))
                keyCode = 6;
            else if (Input.GetKeyDown(KeyCode.Alpha7))
                keyCode = 7;
            else if (Input.GetKeyDown(KeyCode.Alpha8))
                keyCode = 8;
            else if (Input.GetKeyDown(KeyCode.Alpha9))
                keyCode = 9;

            keyCode--;
            if (IsDead)
                return;
            if (keyCode >= 0 && keyCode < LAbilities.Count)
            {
                var ability = LAbilities[keyCode];
                if (ability.Type == AbilityBase.TargetType.Self)
                {
                    QueueElement el = new QueueElement(ability, this);
                    Queue.Add(el);
                }
                else
                {
                    CurrentAbility = ability;
                }
            }

        }
    }
}
