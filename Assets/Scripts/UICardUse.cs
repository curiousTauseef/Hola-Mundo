using UnityEngine;
using System.Collections;

public class UICardUse : MonoBehaviour
{
    public RectTransform cardTransform;

	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    public void FollowMouse()
    {
        cardTransform.position = Input.mousePosition;//new Vector2(Input.GetAxis("Mouse X") * 1.0f, Input.GetAxis("Mouse Y") * 1.0f);
        //Debug.Log(cardTransform.position);
    }
}
