using UnityEngine;
using System.Collections.Generic;

public abstract class ClassSingleton<T> where T : new()
{
    protected static readonly T _instance = new T();
    protected static readonly object _padlock = new object();

    public static T Instance
    {
        get
        {
            lock (_padlock)
            {
                return _instance;
            }
        }
    }

    protected virtual void Init() { }

    protected ClassSingleton()
    {
        Init();
    }
}


public abstract class ClassSingletonInDictionary<T> where T : class
{
    protected static readonly Dictionary<int, T> _instance = new Dictionary<int, T>();
    protected static readonly object _padlock = new object();

    public static T Instance(int __iID = -999)
    {
        lock (_padlock)
        {
            if (__iID.Equals(-999))
            {
                if (GameManager.Instance != null)
                {
                    //Debug.LogWarning("GameManager.Instance.ActiveServerID:" + GameManager.Instance.ActiveServerID);
                    if (GameManager.Instance.ActiveServerID > 0)
                    {
                        __iID = GameManager.Instance.ActiveServerID;
                    }
                    else
                    {
                        Debug.LogWarning("because GameServer is disconnected. " + typeof(T).Name + "'s order was skip. ActiveServerID:" + GameManager.Instance.ActiveServerID);
                    }
                }
            }
            if (_instance.ContainsKey(__iID) == false)
            {
                _instance.Add(__iID, (T)System.Activator.CreateInstance(typeof(T)));
            }

            return _instance[__iID];
        }
    }

    protected virtual void Init() { }

    protected ClassSingletonInDictionary()
    {
        Init();
    }
}