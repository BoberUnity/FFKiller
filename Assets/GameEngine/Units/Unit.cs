using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using Tiled2Unity;
using Assets.GameEngine.Map;
using UnityEngine.UI;

namespace Assets.GameEngine.Units
{
    public class Unit : MonoBehaviour
    {

        protected enum Action
        {
            MoveLeft,
            MoveRight,
            MoveUp,
            MoveBottom,
            Stop,
            Speak
        }

        protected enum Direction
        {
            Top = 0,
            Right = 1,
            Bottom = 2,
            Left = 3,
            Error = 4
        }

        public Transform _transform { get; private set; }
        public GameObject _gameObject { get; private set; }

        //Ссылка на SpriteRenderer.
        private SpriteRenderer _spriteRenderer;

        //Смещение персонажа относительно центра текущей клетки, чтобы его ноги были ровно в центре клетки.
        private Vector2 _offset;
        // Координаты персонажа на текущей карте. Соответствуют индексам двумерного массива клеток текущей карты (MapGrid)
        private int _x, _y;
        // Направление движения\стояния персонажа.
        private Direction _dir;
        // Текущее действие задаваемое персонажу классом наследником. При возможности выполняется или не выполняется.
        protected Action _action;
        //Флаг движения персонажа.
        public bool _isMov { get; private set; }
        //Произошло ли новое полноценное наступание на новую клетку.
        private bool _isNewCell;

        private Vector2 _prevPos;
        private Vector2 _nextPos;
        private float _movTime;
        private float _lastUpdateTime;
        // Amount of remained time, from previous step to centre of the direction cell
        private float _leftTime;
        public float MovingTime = 0.5f;

        private Animator _anim;
        private Canvas _textCanvas;
        private Text _text;

        //Текущая карта, на которой находится персонаж.
        protected TiledMap CurrentMap;
        //Текущая клетка текущей карты
        public Cell Cell;
        //Предыдующая клетка, на которой был персонаж. Нужна для того, чтобы стирать информацию о текущем персонаже спустя некоторе время, после изменения положения.
        public Cell PrevCell;

        public void BaseStart()
        {
            _transform = transform;
            _gameObject = gameObject;
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            CurrentMap = GameManager.CurrentMap;
            _offset = new Vector2(0f, 0.3f);
            _x = (int)(transform.position.x - CurrentMap.LeftBotMapPoint.x);
            _y = (int)(transform.position.y - CurrentMap.LeftBotMapPoint.y);
            Cell = CurrentMap.MapGrid[_x, _y];
            _dir = Direction.Bottom;
            _isMov = false;
            _anim = GetComponent<Animator>();
            transform.position = new Vector2(_x, _y) + CurrentMap.StartPoint + _offset;

            var caption = transform.FindChild("Caption");
            if (caption != null)
            {
                _textCanvas = caption.GetComponent<Canvas>();
                _text = _textCanvas.GetComponentInChildren<Text>();
                _textCanvas.enabled = false;
            }

            _isNewCell = false;
        }

        // Update is called once per frame
        protected void BaseUpdate()
        {
            _spriteRenderer.sortingOrder = CurrentMap.NumTilesHigh - Cell.Y;

            _leftTime = 0;
            if (CurrentMap.MapID != GameManager.CurrentMap.MapID)
                return;

            if (_isMov)
            {
                _movTime += Time.deltaTime;
                //_lastUpdateTime += Time.smoothDeltaTime;
                if (_movTime > MovingTime)
                {
                    _isMov = false;
                    _leftTime = _movTime - MovingTime;
                    transform.position = _nextPos;
                    _isNewCell = true;
                }
                else
                {
                    //_lastUpdateTime = 0f;
                    transform.position = Vector2.Lerp(_prevPos, _nextPos, _movTime / MovingTime);
                }

            }

            if (_isNewCell)
            {
                _isNewCell = false;
                foreach (var warp in CurrentMap.Warps)
                {
                    if (warp.WarpStartPoint.Cell == Cell)
                    {
                        Cell.Unit = null;
                        CurrentMap.ChangeGlobalMap(warp);
                        ChangeMap(warp.WarpDestMap, warp.WaprDestPoint.Cell);
                        return;
                    }
                }
            }


            if (_action == Action.MoveLeft && !_isMov)
            {
                if (_x - 1 > -1 && _x - 1 < CurrentMap.NumTilesWide && CurrentMap.MapGrid[_x - 1, _y].IsWalkable() && !CurrentMap.MapGrid[_x - 1, _y].HasUnit())
                {
                    PrevCell = Cell;
                    _x--;
                    _prevPos = transform.position;
                    _nextPos = new Vector2(_x, _y) + CurrentMap.StartPoint + _offset;
                    _movTime = _leftTime;
                    _lastUpdateTime = 0;
                    _dir = Direction.Left;
                    _isMov = true;

                    //Нужно проверить доступность клетки на следующей карте, если мы на варп поинте, и сделать на следующей карте алгоритм волновой, для определяния
                    // необходимого количества сободных клеток, и размещения туда героя и остальных сопартийцев. Т.к. пока в GameManager не установлена новая карта,
                    // ни один юнит на новой не двинется, то и можно не бояться за алгоримт и параллельное движение остальных героев. Хотя у нас ведь однопоточное приложение.
                    Cell = CurrentMap.MapGrid[_x, _y];
                    Cell.Unit = this;
                    Invoke("DeleteUnitOnPrevCell", MovingTime / 2);
                }
            }

            if (_action == Action.MoveRight && !_isMov)
            {
                if (_x + 1 > -1 && _x + 1 < CurrentMap.NumTilesWide && CurrentMap.MapGrid[_x + 1, _y].IsWalkable() && !CurrentMap.MapGrid[_x + 1, _y].HasUnit())
                {
                    PrevCell = Cell;
                    _x++;
                    _prevPos = transform.position;
                    _nextPos = new Vector2(_x, _y) + CurrentMap.StartPoint + _offset;
                    _movTime = _leftTime;
                    _lastUpdateTime = 0;
                    _dir = Direction.Right;
                    _isMov = true;
                    Cell = CurrentMap.MapGrid[_x, _y];
                    Cell.Unit = this;
                    Invoke("DeleteUnitOnPrevCell", MovingTime / 2);
                }
            }

            if (_action == Action.MoveUp && !_isMov)
            {
                if (_y + 1 > -1 && _y + 1 < CurrentMap.NumTilesHigh && CurrentMap.MapGrid[_x, _y + 1].IsWalkable() && !CurrentMap.MapGrid[_x, _y + 1].HasUnit())
                {
                    PrevCell = Cell;
                    _y++;
                    _prevPos = transform.position;
                    _nextPos = new Vector2(_x, _y) + CurrentMap.StartPoint + _offset;
                    _movTime = _leftTime;
                    _lastUpdateTime = 0;
                    _dir = Direction.Top;
                    _isMov = true;
                    Cell = CurrentMap.MapGrid[_x, _y];
                    Cell.Unit = this;
                    Invoke("DeleteUnitOnPrevCell", MovingTime / 2);
                }
            }

            if (_action == Action.MoveBottom && !_isMov)
            {
                if (_y - 1 > -1 && _y - 1 < CurrentMap.NumTilesHigh && CurrentMap.MapGrid[_x, _y - 1].IsWalkable() && !CurrentMap.MapGrid[_x, _y - 1].HasUnit())
                {
                    PrevCell = Cell;
                    _y--;
                    _prevPos = transform.position;
                    _nextPos = new Vector2(_x, _y) + CurrentMap.StartPoint + _offset;
                    _movTime = _leftTime;
                    _lastUpdateTime = 0;
                    _dir = Direction.Bottom;
                    _isMov = true;
                    Cell = CurrentMap.MapGrid[_x, _y];
                    Cell.Unit = this;
                    Invoke("DeleteUnitOnPrevCell", MovingTime / 2);
                }
            }

      // _anim.SetBool("IsMoving", _isMov);
      //_anim.SetInteger("Direction", (int)_dir);       
    }


        public void ChangeMap(TiledMap newMap, Cell cell)
        {
            var freeCells = newMap.FreeCells(cell, 1);
            if (freeCells.Count > 0)
            {
                CurrentMap = newMap;
                Cell = cell;
                _isMov = false;
                _x = Cell.X;
                _y = Cell.Y;
                transform.position = new Vector2(_x, _y) + CurrentMap.StartPoint + _offset;
            }

        }

        public void AssignMap(TiledMap newMap, int x, int y)
        {
            CurrentMap = newMap;
            _x = x;
            _y = y;
            transform.position = CurrentMap.StartPoint + _offset + new Vector2(_x, _y);
        }

        protected void DeclareMessage(string message, float time)
        {
            _text.text = message;
            _textCanvas.enabled = true;
            Invoke("DisableDeclaring", time);
        }

        private void DisableDeclaring()
        {
            _textCanvas.enabled = false;
        }

        private void DeleteUnitOnPrevCell()
        {
            PrevCell.Unit = null;
        }
    }

}
