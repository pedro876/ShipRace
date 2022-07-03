using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IServiceLocator
{
    public void RegisterService<T>(T service);
    public T GetService<T>();
}