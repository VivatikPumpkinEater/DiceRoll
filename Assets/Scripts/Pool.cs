using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private readonly VFXPoolObject _prefab;
    private readonly Transform _container;
    private readonly int _startCapacity;

    private List<VFXPoolObject> _pool = new();

    public Pool(VFXPoolObject prefab, Transform container, int startCapacity = 5)
    {
        _prefab = prefab;
        _container = container;
        _startCapacity = startCapacity;
        
        _pool.Clear();
        CreatePool();
    }

    private void CreatePool()
    {
        _pool = new List<VFXPoolObject>();

        for (var i = 0; i < _startCapacity; i++)
            CreateElement();
    }

    private VFXPoolObject CreateElement()
    {
        var createObject = Object.Instantiate(_prefab, _container);
        createObject.gameObject.SetActive(false);

        _pool.Add(createObject);

        return createObject;
    }

    private void TryGetElement(out VFXPoolObject element)
    {
        foreach (var i in _pool)
        {
            if (i.gameObject.activeInHierarchy)
                continue;
            
            element = i;
            i.gameObject.SetActive(true);
            return;
        }

        element = CreateElement();
        element.gameObject.SetActive(true);
    }

    public VFXPoolObject GetFreeElement()
    {
        TryGetElement(out var element);
        return element;
    }

    public VFXPoolObject GetFreeElement(Vector2 position)
    {
        var element = GetFreeElement();
        element.transform.position = position;
        return element;
    }
}