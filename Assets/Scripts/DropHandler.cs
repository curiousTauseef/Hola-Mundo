using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DropHandler : MonoBehaviour, IDropHandler
{

	GameObject child;

	public int index;

	public void ClearChild ()
	{
		child = null;

		GameObject.FindObjectOfType<DialogueManager>().UpdateResponse (index);
	}

	// Use this for initialization
	void Start () {
		child = null;


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnDrop (PointerEventData eventData)
	{
		Debug.Log ("drop");

		if (child == null) {
			child = eventData.pointerDrag;
			child.transform.parent = this.transform;
			child.transform.localPosition = new Vector3 (0, 0, child.transform.position.z);

			GameObject.FindObjectOfType<DialogueManager>().UpdateResponse (eventData.pointerDrag.GetComponentInChildren<Text>().text,index);
		} else {

			eventData.pointerDrag.GetComponent<DragNDrop>().EndDrag ();

		}
	}

}
