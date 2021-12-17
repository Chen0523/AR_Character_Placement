using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[Serializable]
public class LabelCollection
{
    public List<Label> OneRuntimeLabels;
    public LabelCollection()
    {
        OneRuntimeLabels = new List<Label>();
    }    
    public void Add(Label OneLabel)
    {
        OneRuntimeLabels.Add(OneLabel);
    }
    public int GetCount()
    {
        return OneRuntimeLabels.Count;
    }
}
