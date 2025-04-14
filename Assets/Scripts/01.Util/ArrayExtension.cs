using System;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Util
{
    public static class ArrayExtension
    {
        public static T GetNear<T>(this T[] array, Vector3 position, bool isNullBreak = true) where T : Object
        {
            if (array == Array.Empty<T>())
                return null;
            
            T nearT = null;
            float minDis = float.MaxValue;
            if (array is Object[] objectArray)
            {
                foreach (var obj in objectArray)
                {
                    if (ReferenceEquals(obj, null))
                    {
                        if (isNullBreak) break;
                        else continue;
                    }
                    var go = obj.GameObject();
                    var dis = Vector3.Distance(go.transform.position, position);
                    if (dis < minDis)
                    {
                        minDis = dis;
                        nearT = (T)obj;
                    }
                }
            }

            return nearT;
        }
    }
}