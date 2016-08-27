using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using Assets.Scripts.Utils;
using System.Collections.Generic;

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

    public enum BehaviourState
    {
        Idle,
        Suspicious,
        Alerted,
        Agressive
    }

    public class AIUtils : MonoBehaviour
    {
        private MethodInfo GetMethodInfo(string methodName)
        {
            Type ourType = this.GetType();
            MethodInfo mi = ourType.GetMethod(methodName,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            return mi;
        }
        /*
     * 
    //public Neuron SetupNeuronAction(Neuron neuron)
    //{
    //    var mi = GetMethodInfo(neuron.Method);
    //    if (mi != null)
    //    {
    //        neuron.Action = (Action)Delegate.CreateDelegate(typeof(Action), this, mi);
    //    }

    //    return neuron;
    //}

    //public Action SetupNeuronAction(string method)
    //{
    //    Action action = () => { };

    //    var mi = GetMethodInfo(method);
    //    if (mi != null)
    //    {
    //        action = (Action)Delegate.CreateDelegate(typeof(Action), this, mi);
    //    }

    //    return action;
    //}

    //public Action<object, object> SetupNeuronAction(string method)
    //{
    //    Action<object, object> action = (object param1, object param2) => { };

    //    var mi = GetMethodInfo(method);
    //    if (mi != null)
    //    {
    //        action = (Action<object, object>)Delegate.CreateDelegate(typeof(Action<object, object>), this, mi);
    //    }

    //    return action;
    //}

    */

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