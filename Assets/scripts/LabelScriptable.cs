using UnityEngine;
using System.Collections.Generic;
using System;
[CreateAssetMenu(menuName = "ScriptableObjects/LabelInfo", order = 1)]
public class LabelScriptable : ScriptableObject
{
    public bool Collecting = false;
    public bool Adjusting = false;
    public LabelCollection OneRunTimeLabels;
}
