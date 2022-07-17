using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class MessageManager : MonoBehaviour 
{
    public int maxMessages = 25;

    List<Message> MessageList = new List<Message>();

    public GameObject messageBoxPanel;
    public GameObject message_prefab;

    public static MessageManager instance;

    private ScrollRect ScrollRect;
    private float backupPosition;

    private void Awake()
    {
        instance = this;
        ScrollRect = GetComponentInChildren<ScrollRect>();
        ScrollRect.verticalScrollbar.value = 0f;
    }
    public void PrintMessage(string text, MessageType type)
    {
        if (MessageList.Count >= maxMessages)
        {
            Destroy(MessageList[0].TextObject.gameObject);
            MessageList.Remove(MessageList[0]);
        }

        Message newMessage = new Message(text, type);
        GameObject newTextObject = Instantiate(message_prefab, messageBoxPanel.transform);
        
        newMessage.TextObject = newTextObject.GetComponent<TMP_Text>();
        newMessage.TextObject.text = newMessage.text;
        newMessage.TextObject.color = GetColorForMessage(newMessage.type);
        MessageList.Add(newMessage);
        ScrollRect.verticalScrollbar.value = 0f;
    }
    Color32 GetColorForMessage(MessageType type)
    {
        if (type == MessageType.important)
        {
            return Settings.instance.ImportantMessageColor;
        }
        else
        {
            return Settings.instance.NotimportantMessageColor;
        }
    }

    

   


}
