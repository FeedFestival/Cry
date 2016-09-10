using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BAD;

namespace Assets.Scripts.Utils
{
    public enum MainState
    {
        Calm,
        Alerted,
        Investigative,
        Aggressive
    }
    public enum MainAction
    {
        DoingNothing,
        DoingJob,
        CheckingAlert,
        ChasingEnemy,
        MoveTowardsPlayer,
        InvestigateLastKnownLocation,
        InvestigateSoundLocation,
        CheckHidingSpots
    }

    // alert
    public enum Alert
    {
        Hearing,
        Seeing
    }

    public enum AlertLevel
    {
        None,
        Suspicious,
        Talkative,
        Aggressive
    }

    public enum ActionTowardsAlert
    {
        NoAlert,
        FacingAlertPosition
    }

    // job
    public enum Job
    {
        Pupil,
        Guard
    }

    //--
    public enum NodeResult
    {
        Failure,
        Success,
        Continue,
        Restart
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


        public static string SplitCamelCase(string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }
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
}