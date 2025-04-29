using System;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Util
{
    public static class ArrayExtension
    {
        public static T Random<T>(this T[] array)
        {
            if (array == null || array == Array.Empty<T>())
            {
#if UNITY_EDITOR
                Debug.LogError($"{nameof(T)}의 Array가 비어있습니다.");
#endif
                return default;
            }
            return array[UnityEngine.Random.Range(0, array.Length)];
        }
        
        public static T GetNear<T>(this T[] array, Vector3 position, int length = -1, bool isNullBreak = true) where T : Object
        {
            if (array == Array.Empty<T>() || length == 0)
                return null;
            
            T nearT = null;
            float minDis = float.MaxValue;
            if (length == -1) length = array.Length;
            if (array is Object[] objectArray)
            {
                for (var i = 0; i < length; i++)
                {
                    var obj = objectArray[i];
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
        
        public static NativeArray<T> ToNativeArray<T>(this T[] array, Allocator allocator) where T : struct
        {
            var nativeArray = new NativeArray<T>(array.Length, allocator, NativeArrayOptions.UninitializedMemory);
            for (var i = 0; i < array.Length; i++)
            {
                nativeArray[i] = array[i];
            }
            return nativeArray;
        }
    }
}