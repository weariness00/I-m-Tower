using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Util
{
    [System.Serializable]
    [JsonConverter(typeof(MinMaxJsonConverter))]
    public class MinMax<T>
    {
        [SerializeField] private T _min;
        [SerializeField] private T _max;
        
        public T Min
        {
            get => _min;
            set => _min = value;
        }

        public T Max
        {
            get => _max;
            set => _max = value;
        }
        
        public MinMax(T min, T max)
        {
            _min = min;
            _max = max;
        }
        
        public bool IsInRange(T value, IComparer<T> comparer = null)
        {
            comparer ??= Comparer<T>.Default;
            return comparer.Compare(value, _min) >= 0 && comparer.Compare(value, _max) <= 0;
        }
    }
    
    public class MinMaxJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(MinMax<int>);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var minMax = (MinMax<int>)value;
            writer.WriteStartObject();
            writer.WritePropertyName("Min");
            writer.WriteValue(minMax.Min);
            writer.WritePropertyName("Max");
            writer.WriteValue(minMax.Max);
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var minMax = new MinMax<int>(0, 0);
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                    break;

                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var propertyName = reader.Value.ToString();
                    reader.Read();
                    if (propertyName == "Min")
                        minMax.Min = Convert.ToInt32(reader.Value);
                    else if (propertyName == "Max")
                        minMax.Max = Convert.ToInt32(reader.Value);
                }
            }
            return minMax;
        }
    }
    
    public static class MinMaxIntExtension
    {
        public static int Length(this MinMax<int> value)
        {
            return Mathf.Abs(value.Min) + Mathf.Abs(value.Max);
        }

        public static int Random(this MinMax<int> value, bool includeMax =false)
        {
            return UnityEngine.Random.Range(value.Min, value.Max + (includeMax ? 1 : 0));
        }
    }

    public static class MinMaxFloatExtension
    {
        public static int Length(this MinMax<float> value)
        {
            return (int)(Mathf.Abs(value.Min) + Mathf.Abs(value.Max));
        }
    }
}