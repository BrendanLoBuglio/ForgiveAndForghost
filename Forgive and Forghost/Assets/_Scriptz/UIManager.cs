using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI messageTextMesh;
    private string originalMessageEnglish = "";
    private string currentMessageEnglish = "";
    private string currentMessageX = "";

    [SerializeField] private TextMeshProUGUI railSelectTextMesh;

    [SerializeField] private TextMeshProUGUI portalMessageTextMesh;
    [SerializeField] private TextMeshProUGUI decipheringTextMesh;

    [SerializeField] private TextMeshProUGUI wotlRecieved;
    [SerializeField] private TextMeshProUGUI ghostRecieved;

	[SerializeField] private TextMeshProUGUI messagesDeliveredText;

    public static UIManager singleton => _singleton ?? (_singleton = FindObjectOfType<UIManager>());
    private static UIManager _singleton;
    
    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {

        /*if(Input.GetKeyDown(KeyCode.K)) {
            SetNewMessage("hey whats up im ghosty. check out my sick grinds", false);
        }

        if(Input.GetKeyDown(KeyCode.J)) {
            ShowMessageTextAtPortal();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            ShowWotlRecieved();
        }

        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            ShowGhostRecieved();
        }*/

    }

    public void SetNewMessage(string newMessage, bool allXs)
    {
        currentMessageEnglish = newMessage;
        
        string temp = "";
        foreach(char c in currentMessageEnglish) 
        {
            if(c != ' ') {
                temp += "x";
            }
            else {
                temp += " ";
            }
        }

        currentMessageX = temp;
        messageTextMesh.text = allXs ? currentMessageX : newMessage;
    }
    
    public void ShowRailMessage()
    {
        Color col = railSelectTextMesh.color;
        col.a = 1;
        railSelectTextMesh.color = col;
    }

    public void HideRailMessage()
    {
        Color col = railSelectTextMesh.color;
        col.a = 0;
        railSelectTextMesh.color = col;
    }

    public void ShowMessageTextAtPortal()
    {
        //StartCoroutine(DoShowMessageText(currentMessageEnglish, portalMessageTextMesh));
        StartCoroutine(WaitAndHideMessage(8f, decipheringTextMesh));
        StartCoroutine(ShowAndHideMessage(currentMessageEnglish, 8f, portalMessageTextMesh));
    }

    public void ShowWotlRecieved()
    {
        StartCoroutine(WaitAndHideMessage(6f, wotlRecieved));
    }

    public void ShowGhostRecieved()
    {
        StartCoroutine(WaitAndHideMessage(6f, ghostRecieved));
    }

    private IEnumerator DoShowMessageText(string text, TextMeshProUGUI textObject)
    {
        Color col = textObject.color;
        col.a = 1;
        textObject.color = col;

        float normalInterval = 0.08f;
        float spaceInterval = 0.2f;

        textObject.gameObject.SetActive(true);
        textObject.text = "";

        for (int i=0; i<text.Length; i++) {
            textObject.text += text[i];
            textObject.ForceMeshUpdate();

            if (text[i] == ' ') {
                yield return new WaitForSeconds(spaceInterval);
            }
            else {
                yield return new WaitForSeconds(normalInterval);
            }
        }
    }

    private IEnumerator WaitAndHideMessage(float time, TextMeshProUGUI textObject)
    {
        Color col = textObject.color;
        col.a = 1;
        textObject.color = col;
        yield return new WaitForSeconds(time);
        col.a = 0;
        textObject.color = col;
        //textObject.gameObject.SetActive(false);
    }

    private IEnumerator ShowAndHideMessage(string text, float time, TextMeshProUGUI textObject)
    {
        yield return StartCoroutine(DoShowMessageText(text, textObject));
        yield return StartCoroutine(WaitAndHideMessage(time, textObject));
    }

	public void SetMessagesDelieveredText(int messagesDelivered, int totalMessages)
	{
		messagesDeliveredText.text = string.Format("{0}/{1}", messagesDelivered, totalMessages);
	}
}
