using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// Makes the UI object it is attached to draggable.
/// </summary>
[RequireComponent(typeof(EventTrigger))]
public class Draggable : MonoBehaviour
{
    EventSystem FindEvent;
    Event Find2;
    Vector3 mouseOffset;

    /// <summary>
    /// Adds OnDrag and OnBEginDrag to the EventTrigger.
    /// </summary>
    public void Awake()
    {
        //setting up the drag trigger
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry drag = new EventTrigger.Entry();
        drag.eventID = EventTriggerType.Drag;
        drag.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
        trigger.triggers.Add(drag);

        //setting up the beginDrag trigger
        EventTrigger.Entry beginDrag = new EventTrigger.Entry();
        beginDrag.eventID = EventTriggerType.BeginDrag;
        beginDrag.callback.AddListener((data) => { OnBeginDrag((PointerEventData)data); });
        trigger.triggers.Add(beginDrag);
    }

    public void OnBeginDrag(PointerEventData data)
    {
        mouseOffset = new Vector3(data.position.x - this.transform.position.x, data.position.y - this.transform.position.y, 0);
    }

    public void OnDrag(PointerEventData data)
    {
        this.transform.position = new Vector3(data.position.x-mouseOffset.x, data.position.y - mouseOffset.y, 0);
    }
}