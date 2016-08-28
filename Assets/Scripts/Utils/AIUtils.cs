using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using BAD;

namespace Assets.Scripts.Utils
{
    public enum Job
    {
        Pupil,
        Guard
    }

    public enum DoAction
    {
        LookThroughThingsToDo,
        CheckAnomally,
        SitAndDoShit,
        ReduceDistance,
        CatchFoe,
        TryToSeeEnemy,
        TurnToAlert,
    }

    public enum AlertType
    {
        Hearing,
        InFieldOfView,
        Seeing
    }

    public enum MainState
    {
        Calm,
        Alerted
    }

    public enum BehaviourState
    {
        Idle,
        Suspicious,
        Aggressive,
        DoingJob
    }

    public enum ActionTowardsAlert
    {
        NoAlert,
        FacingAlertPosition
    }

    public enum EnemyState
    {
        NoEnemy,
        SeeingEnemy
    }

    public enum AggressionLevel
    {
        NotAggressive,
        Attentionate,
        Threatening,
        Violent
    }

    public enum ActionTowardsEnemy
    {
        None,
        LookAtEnemy,
        ReduceDistance
    }

    public class AIUtils : MonoBehaviour
    {
        private TextAsset _guardNeurons;
        public TextAsset GuardNeurons
        {
            get
            {
                if (_guardNeurons == null)
                    _guardNeurons = Resources.Load("AI/Guard") as TextAsset;
                return _guardNeurons;
            }
        }

        private TextAsset _childNeurons;
        public TextAsset ChildNeurons
        {
            get
            {
                if (_childNeurons == null)
                    _childNeurons = Resources.Load("AI/Child") as TextAsset;
                return _childNeurons;
            }
        }

        private MethodInfo GetMethodInfo(string methodName)
        {
            Type ourType = this.GetType();
            MethodInfo mi = ourType.GetMethod(methodName,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            return mi;
        }
        public Action<Unit> SetupNeuronAction(string method)
        {
            Action<Unit> action = (Unit unit) => { };

            var mi = GetMethodInfo(method);
            if (mi != null)
            {
                action = (Action<Unit>)Delegate.CreateDelegate(typeof(Action<Unit>), this, mi);
            }

            return action;
        }

    }
}