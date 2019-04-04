 using UnityEngine;
 using UnityEngine.UI;
 using System.Collections;
 using UnityEngine.EventSystems;
 
 public class Draggable : MonoBehaviour {
     EventSystem FindEvent;
     Event Find2;
	 RectTransform parent;
	 
	 public void Awake() {
		EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
        trigger.triggers.Add(entry);
		
		parent = transform.parent.GetComponent<RectTransform>();
	 }
	 public void OnDrag(PointerEventData data) {
		 print("test");
		 print(parent.anchoredPosition);
		 print(Input.mousePosition);
		 parent.anchoredPosition = Input.mousePosition;
	 }
 }