using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum MessageType
{ 
    important,
    not_important
}
public class Message 
{
    public string text;
    public TMP_Text TextObject;
    public MessageType type;

    public Message(string _text, MessageType _type)
    { 
        text = _text;
        type = _type;
    }
}
