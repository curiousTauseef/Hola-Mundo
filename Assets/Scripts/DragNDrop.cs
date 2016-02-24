using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragNDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{

	private Transform optionsPanel;

	private Vector3 startDragPos;

	// Use this for initialization
	void Start () {
		optionsPanel = null;
	}
	
	// Update is called once per frame
	void Update () {

		if (optionsPanel == null)
			optionsPanel = this.transform.parent;



	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if (eventData.clickCount == 2) {
			if (this.transform.parent != optionsPanel) {

				this.transform.parent.GetComponent<DropHandler>().ClearChild();

				this.transform.parent = optionsPanel;

				Vector2 psize = optionsPanel.gameObject.GetComponent<RectTransform> ().sizeDelta;
				Vector2 size = this.GetComponent<RectTransform> ().sizeDelta;

				Vector3 pos = new Vector3 (Random.Range(0,psize.x - size.x),-1 * Random.Range (size.y, psize.y),this.transform.localPosition.z);

				this.transform.localPosition = pos;
			}
		}


	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (optionsPanel != this.transform.parent)
			return;

		startDragPos = eventData.position; 
		startDragPos.z = transform.position.z;

		transform.position = startDragPos + new Vector3(-50.0f,-12.5f,0.0f);
	}
	
	
	public void OnDrag(PointerEventData eventData)
	{
		if (optionsPanel != this.transform.parent)
			return;


		Vector3 pos = eventData.position;//Camera.main.ScreenToWorldPoint(eventData.position);
		pos.z = transform.position.z;
		transform.position = pos + new Vector3(-50.0f,-12.5f,0.0f); //replace using height and width
		//if(controller!=null)controller.eat ();
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (optionsPanel != this.transform.parent)
			return;


		//Debug.Log ("end drag");
	}

	public void EndDrag ()
	{
		transform.position = startDragPos;

	}

}
