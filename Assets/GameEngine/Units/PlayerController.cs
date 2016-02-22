using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.GameEngine.BattleSystem;
using Tiled2Unity;
using UnityEngine.Assertions.Comparers;

namespace Assets.GameEngine.Units
{
    public class PlayerController : Unit
    {

        public Camera Camera;
        public GameManager GameManager;
        private TiledMap refCurMap;
        //public bool MovingToWeb = false;

        public void Start()
        {
            BaseStart();
            refCurMap = CurrentMap;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (BattleManager.BattleBegin)
                return;

            if (refCurMap != CurrentMap)
            {
                FastUpdateCamera();
                refCurMap = CurrentMap;
            }

            UpdateCamera();
      _action = Action.Stop;    
        

        if (Input.GetKey(KeyCode.A))
        {
          _action = Action.MoveLeft;
        }

        if (Input.GetKey(KeyCode.D))
        {
          _action = Action.MoveRight;
        }

        if (Input.GetKey(KeyCode.W))
        {
          _action = Action.MoveUp;
        }

        if (Input.GetKey(KeyCode.S))
        {
          _action = Action.MoveBottom;
        }
      
            if (Input.GetKeyDown(KeyCode.Space))
            {
              GameManager.StartBattle();
            }
      
        BaseUpdate();
      
          
        }

        private void UpdateCamera()
        {
            float dampTime = 0.15f;
            Vector3 velocity = Vector3.zero;

            Vector2 start = Camera.transform.position;
            Vector2 end = _transform.position;
            //Vector2 current = Vector2.Lerp(start, end, Time.deltaTime);
            Vector2 current = Vector3.SmoothDamp(start, end, ref velocity, dampTime);


            float left = CurrentMap.LeftBotMapPoint.x;
            float right = left + CurrentMap.NumTilesWide;
            float bottom = CurrentMap.LeftBotMapPoint.y;
            float top = bottom + CurrentMap.NumTilesHigh;

            Vector2 leftScreenCorner = Camera.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
            Vector2 rightScreenCorner = Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));
            float unitWidth = rightScreenCorner.x - leftScreenCorner.x;
            float unitHeight = rightScreenCorner.y - leftScreenCorner.y;


            if (unitWidth > right - left)
            {
                current.x = (left + right)/2;
            }
            else if (current.x + unitWidth / 2 > right)
            {
                current.x = right - unitWidth / 2;
            }
            else if (current.x - unitWidth / 2 < left)
            {
                current.x = left + unitWidth / 2;
            }


            if (unitHeight > top - bottom)
            {
                current.y = (top + bottom) / 2;
            }
            else if (current.y + unitHeight / 2 > top)
            {
                current.y = top - unitHeight / 2;
            }
            else if (current.y - unitHeight / 2 < bottom)
            {
                current.y = bottom + unitHeight / 2;
            }


            Camera.transform.position = new Vector3(current.x, current.y, Camera.transform.position.z);
        }

        private void FastUpdateCamera()
        {
            var current = _transform.position;

            float left = CurrentMap.LeftBotMapPoint.x;
            float right = left + CurrentMap.NumTilesWide;
            float bottom = CurrentMap.LeftBotMapPoint.y;
            float top = bottom + CurrentMap.NumTilesHigh;

            Vector2 leftScreenCorner = Camera.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
            Vector2 rightScreenCorner = Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            float unitWidth = rightScreenCorner.x - leftScreenCorner.x;
            float unitHeight = rightScreenCorner.y - leftScreenCorner.y;


            if (unitWidth > right - left)
            {
                current.x = (left + right) / 2;
            }
            else if (current.x + unitWidth / 2 > right)
            {
                current.x = right - unitWidth / 2;
            }
            else if (current.x - unitWidth / 2 < left)
            {
                current.x = left + unitWidth / 2;
            }


            if (unitHeight > top - bottom)
            {
                current.y = (top + bottom) / 2;
            }
            else if (current.y + unitHeight / 2 > top)
            {
                current.y = top - unitHeight / 2;
            }
            else if (current.y - unitHeight / 2 < bottom)
            {
                current.y = bottom + unitHeight / 2;
            }


            Camera.transform.position = new Vector3(current.x, current.y, Camera.transform.position.z);
        }

    }
}
