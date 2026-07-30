using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scripts.Injector
{
    public class ServiceLocator
    {
        static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void Register<T>(T service)
        {
            if (!_services.ContainsKey(typeof(T)))
                _services[typeof(T)] = service;
        }

        public static T Get<T>() where T : class
        {
            if (!_services.ContainsKey(typeof(T)))
            {
                Debug.LogError("Service of type " + typeof(T).Name + " not registered!");
                return null;
            }
            return _services[typeof(T)] as T;
        }
    }
}