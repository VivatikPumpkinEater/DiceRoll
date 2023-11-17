using System.Linq;
using UnityEngine;

[ConfigAttribute(FilePath = "ScriptableObjects")]
public abstract class BaseConfig<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance;

    protected static T Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            var name = $"{typeof(T).Name}";
            var path = GetFilePath();

            _instance = Resources.Load<T>($"{path}/{name}");
            if (_instance != null)
            {
                (_instance as BaseConfig<T>)?.OnInit();
                return _instance;
            }

            _instance = CreateInstance<T>();
#if UNITY_EDITOR

            var assetDirectory = $"Assets/Resources/{path}";
            if (!System.IO.Directory.Exists(assetDirectory))
                System.IO.Directory.CreateDirectory(assetDirectory);

            UnityEditor.AssetDatabase.CreateAsset(_instance, $"{assetDirectory}/{name}.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

#else
                Debug.LogError("Created new name");

#endif

            return _instance;
        }
    }

    /// <summary> Инициализация конфига </summary>
    protected virtual void OnInit()
    {
    }

    /// <summary> Получить путь конфига </summary>
    private static string GetFilePath()
    {
        var attr = typeof(T).GetCustomAttributes(true)
            .OfType<ConfigAttribute>()
            .FirstOrDefault();
        return attr?.FilePath;
    }
}