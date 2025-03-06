using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    private static T instance = null;
    private static readonly object Lock = new object();
    public static T Instance
    {
        get
        {
            lock (Lock)
            {

                if (instance != null)
                    return instance;

                // Search for existing instance.
                instance = (T)FindObjectOfType(typeof(T));
                // Create new instance if one doesn't already exist.
                if (instance != null) return instance;
                // Need to create a new GameObject to attach the singleton to.
                var singletonObject = new GameObject();
                instance = singletonObject.AddComponent<T>();
                singletonObject.name = typeof(T).ToString();

                // Make instance persistent.
                //DontDestroyOnLoad(singletonObject);

                return instance;
            }
        }
    }
    protected virtual void Awake()
    {
        if (instance != null)
        {
            Debug.LogError(string.Format("허용되지 않은 중복 인스턴스 => {0}", typeof(T)));
            Destroy(this);
            return;
        }

        instance = (T)this;
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

}
