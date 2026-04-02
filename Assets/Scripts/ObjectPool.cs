using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    bool IsActive { get; }
    void OnSpawn();
    void OnDespawn();
}

public class ObjectPool<T> where T : Component, IPoolable
{
    private readonly List<T> _pool = new();
    private readonly Func<T> _factory;

    public ObjectPool(Func<T> factory, int initialSize)
    {
        _factory = factory;
        for (int i = 0; i < initialSize; i++)
        {
            T obj = _factory();
            obj.OnDespawn();
            _pool.Add(obj);
        }
    }

    public T Get()
    {
        foreach (T obj in _pool)
        {
            if (!obj.IsActive)
            {
                obj.OnSpawn();
                return obj;
            }
        }

        T newObj = _factory();
        newObj.OnSpawn();
        _pool.Add(newObj);
        return newObj;
    }

    public void Release(T obj) => obj.OnDespawn();

    public void ReleaseAll()
    {
        foreach (T obj in _pool)
            if (obj.IsActive)
                obj.OnDespawn();
    }
}
