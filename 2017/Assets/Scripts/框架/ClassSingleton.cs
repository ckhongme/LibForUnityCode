using UnityEngine;
using System.Collections;
using System;

public class ClassSingleton<T> where T : new()
{
    private static T instance = default(T);
    public static T Instance
    {
        get
        {
            return instance == null ? _Create() : instance;
        }
    }

    private static T _Create()
    {
        if (instance == null)
        {
            instance = new T();
        }
        return instance;
    }

    public static void Destroy()
    {
        if (instance != null)
        {
            if (instance is IDisposable)
            {
                ((IDisposable)instance).Dispose();
                GC.SuppressFinalize(instance);
            }
            instance = default(T);
        }
    }


}
