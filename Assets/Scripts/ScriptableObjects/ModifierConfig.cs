using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ModifierConfig : BaseConfig<ModifierConfig>
{
    [SerializeField] private List<Model> _models;

#if UNITY_EDITOR

    /// <summary> ИСПОЛЬЗОВАТЬ ТОЛЬКО ДЛЯ Editor </summary>
    public List<Model> Models => _models;

#endif

    /// <summary> Получить модификаторы </summary>
    /// <param name="count"> сколько модификаторов</param>
    public static List<Model> GetModifiers(int count = 1)
    {
        if (count <= 0 || count > Instance._models.Count)
        {
            Debug.LogError("Incorrect value!");
            return null;
        }
        
        var modifiers = new List<Model>();

        while (modifiers.Count < count)
        {
            var randomIndex = Random.Range(0, Instance._models.Count);
            var model = Instance._models[randomIndex];
            if (modifiers.Contains(model))
                continue;
            
            modifiers.Add(model);
        }
        
        return modifiers;
    }
    
    [Serializable]
    public struct Model
    {
        public string Name;
        public int Value;
        public Sprite Icon;
    }
}