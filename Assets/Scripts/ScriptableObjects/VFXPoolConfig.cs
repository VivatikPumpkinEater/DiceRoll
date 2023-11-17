using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class VFXPoolConfig : BaseConfig<VFXPoolConfig>
{
    [SerializeField] private List<Model> _models;

#if UNITY_EDITOR

    /// <summary> ИСПОЛЬЗОВАТЬ ТОЛЬКО ДЛЯ Editor </summary>
    public List<Model> Models => _models;

#endif
    
    private readonly Dictionary<VFXType, VFXPoolObject> _vfxObjects = new();

    protected override void OnInit()
    {
        base.OnInit();
        
        foreach (var model in _models)
            _vfxObjects.Add(model.Type, model.VFXPoolObject);
    }

    public static VFXPoolObject GetVfx(VFXType type)
    {
        return Instance._vfxObjects[type];
    }

    [Serializable]
    public struct Model
    {
        public VFXType Type;
        [FormerlySerializedAs("PoolObject")] public VFXPoolObject VFXPoolObject;
    }
}