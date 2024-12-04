using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // Try to find an existing instance in the scene
                instance = FindFirstObjectByType<T>();

                // If no instance is found, log an error for clarity
                if (instance == null)
                {
                    Debug.LogError($"Instance of {typeof(T)} not found. Ensure it exists in the scene.");
                }
            }

            return instance;
        }
    }

    public static bool IsInitialized
    {
        get => instance != null;
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            // Assign the singleton instance to this object
            instance = this as T;

            // Optionally persist across scenes (if desired)
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another instance exists, destroy this one to enforce the singleton pattern
            Destroy(gameObject);
        }
    }

    // Optional: Clear the singleton reference when the object is destroyed
    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
