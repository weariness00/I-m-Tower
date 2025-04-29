using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PathFindDirection : IEnumerable<bool>
{
    [SerializeField]
    public bool[] directions; // 길이 8

    public void InitializeIfNeeded()
    {
        if (directions == null || directions.Length != 8)
        {
            directions = new bool[8];
        }
    }
    
    public IEnumerator<bool> GetEnumerator()
    {
        InitializeIfNeeded();
        foreach (var dir in directions)
        {
            yield return dir;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}