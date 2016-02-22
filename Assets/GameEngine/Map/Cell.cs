using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using Assets.GameEngine.Units;

namespace Assets.GameEngine.Map
{
    public class Cell
    {
        public Unit Unit { get; set; }

        public readonly int X;
        public readonly int Y;

        public bool IsObstacle;

        public Cell(int x, int y, bool obs)
        {
            X = x;
            Y = y;
            IsObstacle = obs;
        }

        public bool IsWalkable()
        {
            return !IsObstacle;
        }

        public bool HasUnit()
        {
            return Unit != null;
        }

        public static bool operator ==(Cell left, Cell right)
        {
            //if (left == null || right == null)
            //    return false;
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(Cell left, Cell right)
        {
            //if (left == null || right == null)
            //    return true;
            return left.X != right.X || left.Y != right.Y;
        }

    }
}

