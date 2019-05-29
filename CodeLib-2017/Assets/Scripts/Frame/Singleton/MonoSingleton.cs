using UnityEngine;
using System.Collections;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            return instance == null ? _Create() : instance;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this as T;
            Init();
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }

    private static T _Create()
    {
        instance = GameObject.FindObjectOfType(typeof(T)) as T;
        if (instance == null)
        {
            GameObject go = new GameObject(typeof(T).ToString());
            instance = go.AddComponent<T>();
            instance.Init();
        }
        return instance;
    }

    public virtual void Init()
    { }
}
