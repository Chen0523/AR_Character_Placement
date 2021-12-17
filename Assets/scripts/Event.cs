using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/Event", order = 2)]
public class Event : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise()
    {
        foreach (GameEventListener lis in listeners)
        {
            lis.OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
        else
        {
            Debug.Log("Already in Listeners");
        }
    }
    public void UnregisterListener (GameEventListener listener)
    {
        listeners.Remove(listener);
    }



}
