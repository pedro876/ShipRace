using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

public class ServiceLocator : IServiceLocator
{
    private readonly Dictionary<Type, object> services;

    public ServiceLocator()
    {
        services = new Dictionary<Type, object>();
    }

    public void RegisterService<T>(T service)
    {
        Type type = typeof(T);
        if (services.ContainsKey(type))
        {
            Debug.LogWarning($"Service {type} already registered");
        }
        services[type] = service;
    }

    public T GetService<T>()
    {
        var type = typeof(T);
        bool serviceFound = services.TryGetValue(type, out var service);
        Assert.IsTrue(serviceFound, $"Service {type} not registered");
        return (T)service;
    }
}
