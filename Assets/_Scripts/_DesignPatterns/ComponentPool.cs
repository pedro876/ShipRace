using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentPool<T> where T : Component
{
    int maxSize = 100;
    int tail = 0;
    T[] prototypes;
    T[] pool;
    Transform poolParent;

    public ComponentPool(string poolName, int maxSize, params T[] prototypes)
    {
        this.maxSize = maxSize;
        this.prototypes = prototypes;
        poolParent = new GameObject(poolName).transform;
        pool = new T[maxSize];
        for(int i = 0; i < maxSize; i++)
        {
            T instance = CreateInstance(i % prototypes.Length);
            Release(instance);
        }
        Shuffle();
    }

    private T CreateInstance(int protoIdx)
    {
        T instance = Object.Instantiate(prototypes[protoIdx], poolParent);
        return instance;
    }

    private void Shuffle()
    {
        if (prototypes.Length <= 1) return;

        for(int i = 0; i < tail-1; i++)
        {
            int j = Random.Range(i+1, tail);
            var objJ = pool[j];
            pool[j] = pool[i];
            pool[i] = objJ;
        }
    }

    public T Get(bool setActive = true)
    {
        T obj;
        if(tail <= 0)
        {
            obj = CreateInstance(Random.Range(0, prototypes.Length));
            obj.gameObject.SetActive(setActive);
        }
        else
        {
            obj = GetAt(tail-1, setActive);
        }
        return obj;
    }

    public T GetAt(int idx, bool setActive = true)
    {
        T obj = pool[idx];
        tail--;
        pool[idx] = pool[tail];
        pool[tail] = null;
        obj.gameObject.SetActive(setActive);
        return obj;
    }

    public T GetAtRandom(bool setActive = true)
    {
        int rnd = Random.Range(0, tail);
        return GetAt(rnd);
    }

    public void Release(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(poolParent);
        if(tail < maxSize)
        {
            pool[tail] = obj;
            tail++;
        }
        else
        {
            Object.Destroy(obj.gameObject);
        }
    }

    public bool IsEmpty()
    {
        return tail <= 0;
    }
}
