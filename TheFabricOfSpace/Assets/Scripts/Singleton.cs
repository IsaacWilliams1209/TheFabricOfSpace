using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This singleton design pattern is set up to ensure that when running the game there is ever only one instance of the derived class created during run time.
/// The 'where' keyword makes sure that T that derives from this class needs to be a unity component type and not something else.
/// </summary>
/// <typeparam name="T"> T allows for the type to be set on initlization. For example it can be set as a type GameObject.</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T: Component
{
    /// <summary> T is a template generic type variable that always for the type of variable to be set on initilization. For example T can be a GameObject,
    /// int, float, etc. It can be whatever the class initlizing it needs to be.
    /// The variable will make sure that it is only initilized once through out gameplay and won't be deleted when changing scenes.
    /// </summary>
    private static T instance; 

    private static bool applicationIsQuitting = false; //Checks to see if unity is quitting so we can do some extra work

    /// <summary> This is what is known as a accessor function that we provide to the user as Unity handles the constructors internally. The method will 
    /// ensure that when called it will construct an object if one does not already exist. If a instance/object already exists than the method will return
    /// that instead.
    /// </summary>
    /// <returns> Returns the type of instance to the user when called. For example T as a GameObject.</returns>
    public static T GetInstance()
    {
        if (applicationIsQuitting) { return null; }

        if (instance == null)
        {
            instance = FindObjectOfType<T>();

            if (instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                instance = obj.AddComponent<T>();
            }
        }

        return instance;
    }

    /// <summary> The class that dervies from this class in one of its methods should call the 'base.Awake()' function to override this method and 
    /// become the type of T that this class is set up to only allow one instance of.
    /// </summary>
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this as T)
        {
            Destroy(gameObject);
        }
        else { DontDestroyOnLoad(gameObject); }
    }

    private void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }
}
