using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed on application quit.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    // Tìm instance trong scene
                    _instance = FindFirstObjectByType<T>();

                    if (_instance == null)
                    {
                        // Tạo mới nếu chưa có
                        GameObject singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return _instance;
            }
        }
    }

    /// <summary>
    /// Gán cờ khi app thoát để tránh lỗi singleton bị gọi sau khi hủy
    /// </summary>
    protected virtual void OnApplicationQuit()
    {
        _applicationIsQuitting = true;
    }
}