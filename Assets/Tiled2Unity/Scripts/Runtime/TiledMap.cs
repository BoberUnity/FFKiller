using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.GameEngine.BattleSystem;
using Assets.GameEngine.Map;
using Assets.GameEngine.Units;
using UnityEngine;

namespace Tiled2Unity
{
    public class TiledMap : MonoBehaviour
    {
        public int NumTilesWide = 0;
        public int NumTilesHigh = 0;
        public int TileWidth = 0;
        public int TileHeight = 0;
        public float ExportScale = 1.0f;
        public PolygonCollider2D Colliders;

        //Нужно, чтобы определить центр клетки (0,0), и оттакливаясь от этого центра построить массив всего поля.
        public Vector2 HalfCell;
        [HideInInspector]
        public Vector2 StartPoint;
        // Note: Because maps can be isometric and staggered we simply can't multply tile width (or height) by number of tiles wide (or high) to get width (or height)
        // We rely on the exporter to calculate the width and height of the map
        public int MapWidthInPixels = 0;
        public int MapHeightInPixels = 0;
        public Cell[,] MapGrid;

        public int MapID;

        public Transform LeftBotMap;
        public Vector2 LeftBotMapPoint;

        [Serializable]
        public class Warp
        {
            public MapCollision WarpStartPoint;
            public TiledMap WarpDestMap;
            public MapCollision WaprDestPoint;
        }

        public List<Warp> Warps;
        private List<MapCollision> EntryPoints; 

        /// <summary>
        /// Класс клетки для алгоритма поиска пути
        /// </summary>
        private class Vertex
        {
            int _f, _g, _h;
            /// <summary>
            /// предыдущий элемент в пути A*
            /// </summary>
            public Vertex parent;
            /// <summary>
            /// Текущая клетка
            /// </summary>
            public Cell cell;

            public int g
            {
                get { return _g; }
                set
                {
                    _g = value;
                    _f = _g + _h;
                }
            }

            public int h
            {
                get { return _h; }
                set
                {
                    _h = value;
                    _f = _g + _h;
                }
            }

            public int f { get { return _f; } }

            public Vertex(int g, int h, Cell cell, Vertex parent)
            {
                this.g = g;
                this.h = h;
                this.cell = cell;
                this.parent = parent;
            }
        }

        private class SordetVertexList
        {
            LinkedList<Vertex> _list = new LinkedList<Vertex>();

            public int count
            {
                get { return _list.Count; }
            }

            public void Push(Vertex vertex)
            {
                if (_list.Count == 0 || vertex.f < _list.First.Value.f)
                {
                    _list.AddFirst(vertex);
                    return;
                }

                var current = _list.First;
                while (vertex.f >= current.Value.f && current.Next != null)
                {
                    current = current.Next;
                }
                _list.AddAfter(current, vertex);
            }

            public Vertex Pop()
            {
                var result = _list.First;
                _list.RemoveFirst();

                return result.Value;
            }

            public Vertex Find(Cell cell)
            {
                var current = _list.First;
                while (current != null && cell != current.Value.cell)
                {
                    current = current.Next;
                }
                return current != null ? current.Value : null;
            }
        }

        // Максимальное число вершин, проверяемое в алгоритме
        const int MAX_PATH_VERTEX_COUNT = 600;

        /// <summary>
        /// Алгоритм поиска пути A*
        /// </summary>
        /// <param name="Unit"> Unit, который и начинает движение</param>
        /// <param name="end"> Конечная точка назначения </param>
        public List<Cell> PathFind(Unit unit, Cell end, int LengthToCheckUnits = 0)
        {
            //var s = DateTime.Now;
            Cell start = unit.Cell;
            var result = new List<Cell>();

            if (start == end)
            {
                return result;
            }

            Unit target = null;
            bool pathToTarget = false;
            if (end.HasUnit())
            {
                pathToTarget = true;
                target = end.Unit;
            }

            var open = new SordetVertexList();
            var closed = new List<Cell>(MAX_PATH_VERTEX_COUNT); //new List<Vertex>(MAX_PATH_VERTEX_COUNT);
            var root = new Vertex(0, 0, start, null);

            Vertex current;
            open.Push(root);
            closed.Add(start);
            var d = 0;
            while (open.count > 0 && d++ < MAX_PATH_VERTEX_COUNT)
            {
                current = open.Pop();
                var nabers = GetNabers(current.cell);
                foreach (var cell in nabers)
                {
                    var canEnter = cell.IsWalkable() && !cell.HasUnit();
                    //Мы уже нашли точку назначения, или вплотную подошли к юниту, который стоит на этой точке назначения.
                    if (cell == end)// || closed.Count > MAX_PATH_VERTEX_COUNT) //|| (pathToTarget && Overlap(Cell, unit, target)))
                    {
                        result.Add(cell);

                        while (current.parent != null)
                        {
                            result.Add(current.cell);
                            current = current.parent;
                        }

                        result.Reverse();

                        //var time = DateTime.Now - s;
                        //Debug.Log ("����� ���� " + time.TotalMilliseconds);

                        return result;
                    }

                    if (closed.FindIndex(x => x == cell) < 0)
                    {
                        closed.Add(cell);
                        int g = Math.Abs(end.X - cell.X) + Math.Abs(end.Y - cell.Y);
                        int h = current.h + 1;
   
                        open.Push(new Vertex(g, h, cell, current));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Волновой алгоритм поиска свободных клеток
        /// </summary>
        /// <param name="start"> Стартовая точка для поиска</param>
        /// <param name="needCells"> Необходимое количество свободных клеток </param>
        public List<Cell> FreeCells(Cell start, int needCells)
        {
            var result = new List<Cell>();

            var closed = new List<Cell>(MAX_PATH_VERTEX_COUNT);
            closed.Add(start);

            var currentWave = new List<Cell>();
            currentWave.Add(start);

            if (!start.HasUnit() && !start.IsObstacle)
            {
                result.Add(start);
            }

            while (result.Count < needCells && currentWave.Count != 0)
            {
                var nextWave = new List<Cell>();
                foreach (var cell in currentWave)
                {
                    var nabers = GetNabers(cell);
                    foreach (var naber in nabers)
                    {
                        if (closed.FindIndex(x => x == cell) < 0)
                        {
                            closed.Add(naber);
                            if (!naber.HasUnit() && !naber.IsObstacle)
                                result.Add(naber);
                        }
                    }
                }
            }

            return result;
        }

        private List<Cell> GetNabers(Cell cell)
        {
            List<Cell> nabers = new List<Cell>();
            //Правая
            if (cell.X + 1 < NumTilesWide && MapGrid[cell.X + 1, cell.Y].IsWalkable() && !MapGrid[cell.X + 1, cell.Y].HasUnit())
                nabers.Add(MapGrid[cell.X + 1, cell.Y]);
            //Левая
            if (cell.X - 1 >= 0 && MapGrid[cell.X - 1, cell.Y].IsWalkable() && !MapGrid[cell.X - 1, cell.Y].HasUnit())
                nabers.Add(MapGrid[cell.X - 1, cell.Y]);
            //Верхняя
            if (cell.Y + 1 < NumTilesHigh && MapGrid[cell.X, cell.Y + 1].IsWalkable() && !MapGrid[cell.X, cell.Y + 1].HasUnit())
                nabers.Add(MapGrid[cell.X, cell.Y + 1]);
            //Нижняя
            if (cell.Y - 1 >= 0 && MapGrid[cell.X, cell.Y - 1].IsWalkable() && !MapGrid[cell.X, cell.Y - 1].HasUnit())
                nabers.Add(MapGrid[cell.X, cell.Y - 1]);
            
            return nabers;
        }

        public void ChangeGlobalMap(Warp curWarp)
        {
            GameManager.CurrentMap = curWarp.WarpDestMap;
        }

        public float GetMapWidthInPixelsScaled()
        {
            return this.MapWidthInPixels * this.transform.lossyScale.x * this.ExportScale;
        }

        public float GetMapHeightInPixelsScaled()
        {
            return this.MapHeightInPixels * this.transform.lossyScale.y * this.ExportScale;
        }


        private void OnDrawGizmosSelected()
        {
            Vector2 pos_w = this.gameObject.transform.position;
            Vector2 topLeft = Vector2.zero + pos_w;
            Vector2 topRight = new Vector2(GetMapWidthInPixelsScaled(), 0) + pos_w;
            Vector2 bottomRight = new Vector2(GetMapWidthInPixelsScaled(), -GetMapHeightInPixelsScaled()) + pos_w;
            Vector2 bottomLeft = new Vector2(0, -GetMapHeightInPixelsScaled()) + pos_w;

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }

        public void Awake()
        {
            StartPoint = (Vector2)LeftBotMap.position + HalfCell;
            LeftBotMapPoint = LeftBotMap.position;

            MapGrid = new Cell[NumTilesWide, NumTilesHigh];
            for (int i = 0; i < NumTilesWide; i++)
                for (int j = 0; j < NumTilesHigh; j++)
                {
                    bool isObstacle = Colliders.OverlapPoint(new Vector2(StartPoint.x + i, StartPoint.y + j));

                    Cell cell = new Cell(i, j, isObstacle);
                    MapGrid[i, j] = cell;
                }

            DetectCollisions();
        }

        private void DetectCollisions()
        {
            EntryPoints = new List<MapCollision>();

            var destinations = transform.FindChild("Destinations");
            var collisions = destinations.GetComponentsInChildren<MapCollision>();

            if (collisions != null)
                foreach (var coll in collisions)
                {
                    int x = (int)(coll.transform.position.x - LeftBotMapPoint.x);
                    int y = (int)(coll.transform.position.y - LeftBotMapPoint.y);
                    coll.Cell = MapGrid[x, y];
                }


            var warps = transform.FindChild("WarpStarts");
            collisions = warps.GetComponentsInChildren<MapCollision>();

            if (collisions != null)
                foreach (var coll in collisions)
                {
                    int x = (int)(coll.transform.position.x - LeftBotMapPoint.x);
                    int y = (int)(coll.transform.position.y - LeftBotMapPoint.y);
                    coll.Cell = MapGrid[x, y];
                    EntryPoints.Add(coll);
                }


            warps = transform.FindChild("WarpEnds");
            collisions = warps.GetComponentsInChildren<MapCollision>();

            if (collisions != null)
                foreach (var coll in collisions)
                {
                    int x = (int)(coll.transform.position.x - LeftBotMapPoint.x);
                    int y = (int)(coll.transform.position.y - LeftBotMapPoint.y);
                    coll.Cell = MapGrid[x, y];
                    EntryPoints.Add(coll);
                }

        }





    }
}
