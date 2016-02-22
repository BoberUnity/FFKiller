using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.GameEngine.Map;

namespace Assets.GameEngine.Cutscenes
{
    [Serializable]
    public class Sequence
    {
        [Serializable]
        public class Activity
        {
            public enum ActionType
            {
                Move,
                Speak,
                Wait
            }

            public ActionType actionType;
            public MapCollision Destination;
            public string Message;
            public float Time;
        }

        public bool IsLooped;
        //public Activity FirstActivity;
        public List<Activity> lActivities;

    }

}

