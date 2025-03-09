using System;
using Unity.VisualScripting;
using UnityEngine;

namespace BehaviorTree
{
    [Serializable]
    public class WaitNode : ActionNode
    {
        /// <summary>
        /// The Duration that the Node waits before returning success.
        /// </summary>
        private float EndTime;
        private float StartTime;

        private TimeManager timeManager;

        public WaitNode Init(float startTime, float endTime)
        {
            timeManager = FindObjectOfType<TimeManager>();
            StartTime = startTime;
            EndTime = endTime;

            return this;
        }

        /// <inheritdoc />
        protected override void OnStart() { }

        /// <inheritdoc />
        protected override void OnStop() { }

        /// <inheritdoc />
        protected override State OnUpdate()
        {
            string currentTime = timeManager.GetCurrentTime();

            //Determine the current location based on the time of day
            float.TryParse(currentTime.Split(':')[0], out float currentHour);

            if (currentHour >= StartTime && currentHour < EndTime)
            {
                return State.Running;
            }

            return State.Success;
        }
    }
}