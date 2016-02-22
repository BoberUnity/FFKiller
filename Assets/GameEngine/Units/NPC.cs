using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.GameEngine.Cutscenes;
using Assets.GameEngine.Map;

namespace Assets.GameEngine.Units
{
    public class NPC : Unit
    {
        private enum AIStatus
        {
            FindNextPath,
            Moving,
            Speaking,
            Waiting,
            DownTime
        }

        //Текущий путь персонажа, полученный через PathFind
        private List<Cell> _curPath;
        //Индекс клетки в _curPath, к которой персонаж двигается в текущий момент.
        private int _curPathInd;

        //Тип действия, совершаемый NPC в данный момент. Во всяком случае он пытается его выполнить)) 
        private AIStatus _status;

        //Время ожидания NPC
        private float WaitTime = 2.0f;
        //Прошедшее время, с момента начала ожидания, на момент старта ожидания должно быть меньше WaitTime
        private float _elapsedTime;

        // Список совершаемых NPC действий
        public Sequence Sequence;
        // Индекс, номер, совершаемого действия в текущем списке.
        private int _seqInd;
        // Статус текущего действия.
        private bool _isCurActDone;

        // Точка назначения, к которой персонаж вигается, получается из PathFind'a
        private Cell _destPoint;

        //public List<Sequence> lSequence;

        // Use this for initialization
        public void Start()
        {
            BaseStart();

            _status = AIStatus.DownTime;
            _action = Action.Stop;
            _elapsedTime = WaitTime;

            _seqInd = -1;
            _isCurActDone = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (_seqInd == Sequence.lActivities.Count - 1 && Sequence.IsLooped && _isCurActDone)
            {
                _seqInd = -1;
                _isCurActDone = true;
            }

            if (_isCurActDone && _seqInd < Sequence.lActivities.Count - 1 && _status == AIStatus.DownTime)
            {
                _isCurActDone = false;
                _seqInd++;
                if (Sequence.lActivities[_seqInd].actionType == Sequence.Activity.ActionType.Move)
                {
                    _status = AIStatus.FindNextPath;
                    _destPoint = Sequence.lActivities[_seqInd].Destination.Cell;
                }
                else if (Sequence.lActivities[_seqInd].actionType == Sequence.Activity.ActionType.Speak)
                {
                    float time = Sequence.lActivities[_seqInd].Time;
                    WaitForSome(time);
                    DeclareMessage(Sequence.lActivities[_seqInd].Message, time);
                }
                else if (Sequence.lActivities[_seqInd].actionType == Sequence.Activity.ActionType.Wait)
                {
                    float time = Sequence.lActivities[_seqInd].Time;
                    WaitForSome(time);
                }
            }


            //_action = Action.Stop;
            if (_status == AIStatus.Waiting)
            {
                _elapsedTime += Time.deltaTime;
                BaseUpdate();
                if (_elapsedTime < WaitTime || _isMov)
                    return;
                _isCurActDone = true;
                _status = AIStatus.DownTime;

            }

            if (_status == AIStatus.FindNextPath)
            {
                _curPath = CurrentMap.PathFind(this, _destPoint);
                //_pathInd = (_pathInd + 1) % _path.Count;
                _curPathInd = 0;

                _status = AIStatus.Moving;
            }

            if (_status == AIStatus.Moving)
            {
                MoveNPC();
            }

            BaseUpdate();
            
        }

        private void WaitForSome(float time)
        {
            _status = AIStatus.Waiting;
            _action = Action.Stop;
            WaitTime = time;
            _elapsedTime = 0;
        }

        private void MoveNPC()
        {
            if (_curPathInd >= _curPath.Count || _curPath == null || _curPath.Count == 0)
            {
                WaitForSome(0f);
                return;
            }
            if (Cell == _curPath[_curPathInd])
            {
                _curPathInd++;
                if (_curPathInd >= _curPath.Count)
                    return;
            }

            Unit TestCellUnit = _curPath[_curPathInd].Unit;
            if (TestCellUnit != null && TestCellUnit != this)
            {
                _status = AIStatus.FindNextPath;
            }

            var dir = DefineDirection(Cell, _curPath[_curPathInd]);
            if (dir == Direction.Top)
                _action = Action.MoveUp;
            else if (dir == Direction.Right)
                _action = Action.MoveRight;
            else if (dir == Direction.Bottom)
                _action = Action.MoveBottom;
            else if (dir == Direction.Left)
                _action = Action.MoveLeft;
        }

        private Direction DefineDirection(Cell old, Cell next)
        {
            Direction dir;
            if (old.X - next.X == 1)
                dir = Direction.Left;
            else if (old.X - next.X == -1)
                dir = Direction.Right;
            else if (old.Y - next.Y == 1)
                dir = Direction.Bottom;
            else if (old.Y - next.Y == -1)
                dir = Direction.Top;
            else
                dir = Direction.Error;
            return dir;
        }
    }
}
