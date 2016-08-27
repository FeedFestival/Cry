using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace BAD
{
    public class ComponentMethodLookup
    {
        public string Class;
        public string Method;

        private Component _componentClass;
        private MethodInfo _methodInfo;

        private object[] _parameters;

        private const int FuncIndex = 2;

        // [Guard][CheckBehaviourState][enum.BehaviourState.Suspicious]
        public ComponentMethodLookup(string[] functionParts)
        {
            functionParts[0] = functionParts[0].Replace("[", "");
            functionParts[functionParts.Length - 1] = functionParts[functionParts.Length - 1].Replace("]", "");

            Class = functionParts[0];
            Method = functionParts[1];
            
            if (functionParts.Length > FuncIndex)
            {
                // has parameters
                int parametersCount = functionParts.Length - FuncIndex;
                int parametersIndex = 0;
                
                _parameters = InitializeArray<object>(parametersCount);
                
                for (int i = FuncIndex; i < (_parameters.Length + FuncIndex); i++)
                {
                    string[] parameterParts = functionParts[i].Split(new[] { "." }, StringSplitOptions.None);

                    if (parameterParts.Length > 2)
                    {
                        // its an enum
                        var enumType = GetEnumType("Assets.Scripts.Utils." + parameterParts[1]);
                        var enumValue = Enum.Parse(enumType, parameterParts[2], true);
                        _parameters[parametersIndex] = enumValue;
                    }
                    else
                    {
                        var type = Type.GetType(parameterParts[0], true, true);

                    }

                    parametersIndex++;
                }
            }
        }

        public object Invoke()
        {
            return _methodInfo.Invoke(_componentClass, _parameters);
        }

        public void Resolve(GameObject go)
        {
            var componentType = Type.GetType(Class, true, true);
            _componentClass = go.GetComponent(componentType);
            if (_componentClass == null)
            {
                Debug.LogWarning("Adding " + componentType + " to gameobject at runtime.");
                _componentClass = go.AddComponent(componentType);
            }
            if (_componentClass == null) throw new System.Exception("No componentClass named: " + Class);
            _methodInfo = componentType.GetMethod(Method);
            if (_methodInfo == null) throw new System.Exception("No method named: " + Method + " on " + Class + " (Is it public?)");

        }

        public override string ToString()
        {
            string s = string.Empty;
            s = string.Format("{0}.{1}", Class, Method);

            if (_parameters != null && _parameters.Length > 0)
            {
                s += "(";
                for (int i = 0; i < _parameters.Length; i++)
                {
                    s += _parameters[i].ToString();

                    if (i != _parameters.Length - 1)
                        s += ", ";
                }
                s += ")";
            }
            return s;
        }

        //---

        private static Type GetEnumType(string enumName)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var type = assembly.GetType(enumName);
                if (type == null)
                    continue;
                if (type.IsEnum)
                    return type;
            }
            return null;
        }

        T[] InitializeArray<T>(int length) where T : new()
        {
            T[] array = new T[length];
            for (int i = 0; i < length; ++i)
            {
                array[i] = new T();
            }

            return array;
        }
    }
}