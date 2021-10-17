using UnityEngine;

public class SingletonInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = (T)FindObjectOfType<T>();
        }
        else
        {
            Destroy(this.gameObject);
        }

        OnInitialize();
    }

    public static T GetInstance()
    {
        return instance;
    }

    protected virtual void OnInitialize() { }
}