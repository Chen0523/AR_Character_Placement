using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public Event gEvent;
    public UnityEvent response;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Debug.Log("Registered" + this.tag);
        gEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        response.Invoke();
    }
}
