using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor;

namespace Assets.GameEngine.BattleSystem
{
    public class BattleManager : MonoBehaviour
    {
        public GameManager GameManager;
        private float _elapsedTimeFromStart;

        public static bool BattleBegin { get; private set; }
        public static bool BattleEnd { get; private set; }
        public static bool Defeated { get; private set; }
        //public List<UnitBase> Enemies;
        public List<UnitBase> HeroTeam;
        public GameObject PrefWolf;
        public GameObject PrefSkeletonArcher;

        public List<AIController.AIType> EnemyTeam;

        public static List<UnitBase> EnemyUnits;
        public static List<UnitBase> EnemyReserve;
        public static PlayerController Hero;

        private bool _isPause;
        private bool _isManuallyPause;

        public static UnitBase [,]EnemyGrid;
        public List<Transform> FrontPositions;
        public List<Transform> BackPositions;

        public Camera Camera;

        private void Start()
        {
            Camera = GetComponentInChildren<Camera>();
            Camera.enabled = false;
            BattleBegin = false;
        }

        public void InitiateBattle()
        {
            Defeated = false;

            EnemyGrid = new UnitBase[2, 3];

            EnemyUnits = new List<UnitBase>();
            EnemyReserve = new List<UnitBase>();

            Hero = (PlayerController)HeroTeam[0];


            foreach (var enemy in EnemyTeam)
            {
                UnitBase result = null;
                if (enemy == AIController.AIType.Wolf)
                    result = PlaceUnit(PrefWolf);
                else if (enemy == AIController.AIType.SkeletonArcher)
                    result = PlaceUnit(PrefSkeletonArcher);

                if (result != null)
                    EnemyUnits.Add(result);
            }

            foreach (var ally in HeroTeam)
            {
                if (ally == null)
                    continue;

                ally.TeamType = UnitBase.UnitType.Ally;
                EnemyUnits.Add(ally);
            }

            _isPause = false;
            _elapsedTimeFromStart = 0f;
            BattleBegin = true;

            BattleEnd = false;
        }

        private UnitBase PlaceUnit(GameObject unit)
        {
            var goEnemy = Instantiate(unit, Vector3.zero, Quaternion.identity) as GameObject;
            var enemy = goEnemy.GetComponent<UnitBase>();
            enemy.BaseStart();

            bool triedToPlaceFront = false;
            if (enemy.IsMelee)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (EnemyGrid[0, i] == null || EnemyGrid[0, i].IsDead)
                    {
                        triedToPlaceFront = true;
                        EnemyGrid[0, i] = enemy;
                        enemy.transform.position = FrontPositions[i].position;
                        enemy.GrPosition = UnitBase.GridPosition.Front;
                        enemy.Row = i;
                        break;
                    }
                }
                if (!triedToPlaceFront)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (EnemyGrid[1, i] == null || EnemyGrid[1, i].IsDead)
                        {
                            triedToPlaceFront = true;
                            EnemyGrid[1, i] = enemy;
                            enemy.transform.position = BackPositions[i].position;
                            enemy.GrPosition = UnitBase.GridPosition.Back;
                            enemy.Row = i;
                            break;
                        }
                    }
                }
            }

            bool triedToPlaceBack = false;
            if (!enemy.IsMelee)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (EnemyGrid[1, i] == null || EnemyGrid[1, i].IsDead)
                    {
                        triedToPlaceBack = true;
                        EnemyGrid[1, i] = enemy;
                        enemy.transform.position = BackPositions[i].position;
                        enemy.GrPosition = UnitBase.GridPosition.Back;
                        enemy.Row = i;
                        break;
                    }
                }
                if (!triedToPlaceBack)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (EnemyGrid[0, i] == null || EnemyGrid[0, i].IsDead)
                        {
                            triedToPlaceBack = true;
                            EnemyGrid[0, i] = enemy;
                            enemy.transform.position = FrontPositions[i].position;
                            enemy.GrPosition = UnitBase.GridPosition.Front;
                            enemy.Row = i;
                            break;
                        }
                    }
                }
            }

            if (!triedToPlaceFront && !triedToPlaceBack)
            {
                EnemyReserve.Add(enemy);
                enemy.transform.position = new Vector3(100f, 100f);
                return null;
            }
            return enemy;
        }

        // Update is called once per frame
        private void Update()
        {
            _elapsedTimeFromStart += Time.deltaTime;
            if (_elapsedTimeFromStart < 1.0f)
                return;

            if (!BattleBegin)
                return;

            if (Hero.IsDead)
            {
                Hero.Die();
                Defeated = true;
            }


            bool isOneEnemyAlive = false;
            foreach (var unit in EnemyUnits)
            {
                if (unit != null && !unit.IsDead && unit.TeamType == UnitBase.UnitType.Enemy)
                    isOneEnemyAlive = true;
                if (unit != null && unit.IsDead)
                    unit.Die();
            }
            BattleEnd = !isOneEnemyAlive;

            TimeManagement();
        }

        public void OnGUI()
        {
            if (!BattleBegin)
                return;

            var curSkillColor = GUI.color;
            if (_isPause)
            {
                GUI.color = Color.red;
                GUI.Label(new Rect(Screen.width / 2 - 50, 10, 100, 25), "Paused");
            }
            GUI.color = curSkillColor;

            //Battle is over
            if (Hero.IsDead || BattleEnd)
            {
                if (GUI.Button(new Rect(Screen.width/2 - 100, Screen.height - 120, 200, 50), "EndBattle"))
                    EndBattle();
            }
        }

        private void TimeManagement()
        {
            if (Hero.IsDead || BattleEnd)
            {
                Time.timeScale = 1f;
                _isPause = false;
                _isManuallyPause = false;
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!_isPause)
                {
                    Time.timeScale = 0f;
                    _isPause = true;
                    _isManuallyPause = true;
                }
                else
                {
                    Time.timeScale = 1f;
                    _isPause = false;
                    _isManuallyPause = false;
                }

            }

            if (!Hero.IsBusy && Hero.Queue.Count == 0)
            {
                Time.timeScale = 0;
                _isPause = true;
            }
            else if (!_isManuallyPause)
            {
                Time.timeScale = 1f;
                _isPause = false;
                _isManuallyPause = false;
            }
        }

        private void EndBattle()
        {
            BattleBegin = false;
            foreach (var enemy in EnemyUnits)
            {
                if (enemy.TeamType == UnitBase.UnitType.Enemy)
                    Destroy(enemy._gameObject);
            }
            GameManager.EndBattle();
        }


    }
}
