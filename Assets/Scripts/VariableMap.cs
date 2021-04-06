using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VariableMap<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    public List<TKey> keys = new List<TKey>();
    public List<TValue> values = new List<TValue>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach(KeyValuePair<TKey,TValue> kv in this)
        {
            keys.Add(kv.Key);
            values.Add(kv.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();
        for(int i = 0; i < Math.Min(keys.Count, values.Count); ++i)
        {
            this.Add(keys[i], values[i]);
        }
    }
}