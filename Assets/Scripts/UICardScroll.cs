using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UICardScroll : MonoBehaviour
{
    public RectTransform panel; // To hold the ScrollPanel
    public Button[] bttn;
    public RectTransform center; // Center to compare the distance for each button
    public Image searchBar;
    public Text searchText;
    public Sprite cardAddSprite;

    public float[] distance;    // All buttons' distance to the center
    public float[] distReposition;
    private bool dragging = false;  // Will be true, while we drag the panel
    private int bttnDistance;   // Will hold the distance between the buttons
    private int minButtonNum;   // To hold the number of the button closest to center
    private int bttnLength;
    private float cardScale = 200.0f;
    private float cardAngle = 1.0f;
    private string inputString;
    private string searchString = "";
    private string[] cardWords;
    private int cardIndex;
    private Coroutine waitForSearchReset;

    void Start()
    {
        bttnLength = bttn.Length;
        distance = new float[bttnLength];
        distReposition = new float[bttnLength];
        cardWords = new string[bttnLength];

        // Get distance between buttons
        bttnDistance = (int)Mathf.Abs(bttn[1].GetComponent<RectTransform>().anchoredPosition.x - bttn[0].GetComponent<RectTransform>().anchoredPosition.x);

        for (int i = 0; i<cardWords.Length; i++)
        {
            cardWords[i] = bttn[i].GetComponentInChildren<Text>().text;
            //Debug.Log(bttn[i].GetComponentInChildren<Text>().text);
        }
    }

    void Update()
    {
        for (int i = 0; i < bttn.Length; i++)
        {
            RectTransform rectTransform = bttn[i].GetComponent<RectTransform>();

            distReposition[i] = center.GetComponent<RectTransform>().position.x - bttn[i].GetComponent<RectTransform>().position.x;
            distance[i] = Mathf.Abs(distReposition[i]);

            if (distReposition[i] > 3000)
            {
                float curX = rectTransform.anchoredPosition.x;
                float curY = rectTransform.anchoredPosition.y;

                Vector2 newAnchoredPos = new Vector2(curX + (bttnLength * bttnDistance), curY);
                rectTransform.anchoredPosition = newAnchoredPos;
            }

            if (distReposition[i] < -3000)
            {
                float curX = rectTransform.anchoredPosition.x;
                float curY = rectTransform.anchoredPosition.y;

                Vector2 newAnchoredPos = new Vector2(curX - (bttnLength * bttnDistance), curY);
                rectTransform.anchoredPosition = newAnchoredPos;
            }

            float newScale = 1.0f - distance[i] / cardScale; // Mathf.Lerp(1.0f, 1.0f - distance[i] / 5.0f, Time.deltaTime); //0.16 per frame
            rectTransform.localScale = new Vector2(newScale, newScale);

            float newAngle = 0.0f - distReposition[i] * cardAngle; // Mathf.Lerp(0.0f, 0.0f - distReposition[i] * 35.0f, Time.deltaTime);
            rectTransform.rotation = Quaternion.Euler(rectTransform.rotation.x, Mathf.Max(newAngle, -100.0f), 0);

            //Change depths of card based on their distances
            bttn[i].transform.SetSiblingIndex(System.Convert.ToInt32(Mathf.Floor(distReposition[i] / -100.0f)));
            //Correct the depth of Center card
            if (distReposition[i] > -10.0f && distReposition[i] < 10.0f)
                bttn[i].transform.SetSiblingIndex(100);
        }

        if(cardIndex == minButtonNum)
        {
            float minDistance = Mathf.Min(distance);    // Get the min distance

            for (int i = 0; i < bttn.Length; i++)
            {
                if (minDistance == distance[i])
                {
                    minButtonNum = i;
                }
            }
        }

        if (!dragging)
        {

            //Search the cards
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (searchString.Length != 0)
                {
                    searchString = searchString.Remove(searchString.Length - 1);
                }
            }
            else if (Input.anyKeyDown)
            {
                inputString = Input.inputString;

                if(inputString != "")
                {
                    searchString += inputString;

                    if (SearchCards(searchString) != -1)
                    {
                        cardIndex = SearchCards(searchString);
                    }
                }

                //Reset the Search Bar
                waitForSearchReset = StartCoroutine(ClearSearchString());
            }

            Vector2 tempScale = searchBar.GetComponent<RectTransform>().sizeDelta;
            tempScale.x = 1.5f + 0.325f * searchString.Length;
            searchBar.GetComponent<RectTransform>().sizeDelta = tempScale;

            searchText.text = searchString;

            LerpToButton(-bttn[cardIndex].GetComponent<RectTransform>().anchoredPosition.x);
        }
        else
        {
            cardIndex = minButtonNum;
        }

        //Activate AddCard UI for the middle card
        for (int i = 0; i < bttn.Length; i++)
        {
            bttn[i].GetComponentsInChildren<Image>()[1].color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
        bttn[cardIndex].GetComponentsInChildren<Image>()[1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    void LerpToButton(float position)
    {
        float newX = Mathf.Lerp(panel.anchoredPosition.x, position, Time.deltaTime * 10.0f);
        Vector2 newPosition = new Vector2(newX, panel.anchoredPosition.y);
        panel.anchoredPosition = newPosition;
    }

    public void StartDrag()
    {
        dragging = true;
    }

    public void EndDrag()
    {
        dragging = false;
    }

    private void MoveInHierarchy(Button b, int delta)
    {
        int index = b.transform.GetSiblingIndex();
        b.transform.SetSiblingIndex(index + delta);
    }

    private int SearchCards(string i_string)
    {
        for(int i = 0; i<cardWords.Length; i++)
        {
            if(cardWords[i].StartsWith(i_string))
            {
                return i;
            }
        }
        return -1;
    }

    IEnumerator ClearSearchString()
    {
        if(waitForSearchReset != null)
        {
            StopCoroutine(waitForSearchReset);
        }

        yield return new WaitForSeconds(0.5f);
        searchString = "";
        //Debug.Log("Waiting finished");
    }

}
