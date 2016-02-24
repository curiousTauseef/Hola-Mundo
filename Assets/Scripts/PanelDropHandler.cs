using UnityEngine;
using System.Collections;

using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PanelDropHandler : MonoBehaviour, IDropHandler {




	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnDrop (PointerEventData eventData)
	{
		Debug.Log ("panel drop");

		GameObject go = eventData.pointerDrag;

		if (go.transform.parent != this.transform) {
					go.transform.parent = this.transform;
		}

	}



}
