using UnityEngine;
using System.Collections;

public class SingletonInitialize : MonoBehaviour
{
    public virtual void Initialize() { }
}
public class Singleton<T> : SingletonInitialize where T : SingletonInitialize
{

    private static T _instance;
    private static object _lock = new object();
    public static T Instance
    {
        get {
            if (_applicationIsQuitting)
            {
                //Debug.LogWarning("[Singleton] Instance "+typeof(T)+" already destroyed on application quit."+
                //    " Won't create again - returning null.");
                return null;
            }
            lock(_lock)
            {
                if(_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));
                    if(FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopening the scene might fix it.");
                        return _instance;
                    }
                    if(_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = typeof(T).ToString() + "(singleton)";
                        DontDestroyOnLoad(singleton);
                        _instance.Initialize();
                        
#if UNITY_EDITOR || ACTIVE_LOG
                        Debug.Log("[Singleton] An instance of " + typeof(T) +
                            " is needed in the scene, so '" + singleton +
                            "' was created with DontDestroyOnLoad.");
#endif
                    }
                    else
                    {
#if UNITY_EDITOR || ACTIVE_LOG
                        Debug.Log("[Singleton] Using instance already created: " +
                            _instance.gameObject.name);
#endif
                    }
                }
                return _instance;
            }
        }
    }
    private static bool _applicationIsQuitting = false;
    public virtual void OnDestroy(){
        _applicationIsQuitting = true;
    }
	
}
