using System.Collections.Generic;
using UnityEngine;

namespace Util.UniqueID
{
    public static class UniquePrefabIdentifierGenerator
    {
        private static int nextId = 1;
        private static readonly HashSet<int> used = new();
        private static readonly Stack<int> recycled = new();

        public static int Generate()
        {
            int id = recycled.Count > 0 ? recycled.Pop() : nextId++;
            used.Add(id);
            return id;
        }

        public static void Release(int id)
        {
            if (used.Remove(id))
                recycled.Push(id);
        }

        public static void Clear()
        {
            used.Clear();
            recycled.Clear();
            nextId = 1;
        }
    }
}