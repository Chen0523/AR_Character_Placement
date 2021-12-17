using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrigger : MonoBehaviour
{
    [SerializeField] Event gEvent;
    [SerializeField] float min_change = 0.1f;
    Vector3 last_pos;
    Vector3 last_for;
    private void Start()
    {
        last_pos = this.transform.position;
        last_for = this.transform.forward;
    }

    private void Update()
    {
        if(Vector3.Distance(this.transform.position, last_pos)>min_change)
        {
            gEvent.Raise();
            last_pos = this.transform.position;
            last_for = this.transform.forward;

            return;
        }
        if (Vector3.Angle(this.transform.forward, last_for) > 5)
        {
            gEvent.Raise();
            last_pos = this.transform.position;
            last_for = this.transform.forward;
            return;
        }
    }
}
