using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugWindow : MonoBehaviour
{
    public class DebugMessage
    {
        public int type; // 1 for warning, 0 for error, 2 for gobblin or smth idk, 99 for incorrect message
        public string message;
       // public int messageSize;
        public string source;
        // Constructeurs
        public DebugMessage() { type = 0; message = null; source = null; }

        public DebugMessage(int t, string m, string s)
        {
            type = (t < 3) ? t : 99;
            message = m;
            source = s;
        }

    }
    // public attributes
    public TextMeshProUGUI _Text;
    public int MaxMessages = 20;
    // private attributes
    private List<DebugMessage> _DebugMessage;
    private int previousSize;
    private int buttonInt;

    void Start()
    {
        _DebugMessage = new List<DebugMessage>();
        MaxMessages = 20;
        buttonInt = 0;
    }

    void FixedUpdate()
    {
        if (previousSize != _DebugMessage.Count)
        {
            //writeDebugMessage("Debug count : " + _DebugMessage.Count, 1, "itself");
            if (_DebugMessage.Count >= MaxMessages)
            {
                clearDebug();
            }
            writeDebug();
        }
        
        previousSize = _DebugMessage.Count;
    }

    public void writeDebugMessage(string message, int type, string source)
    {
        previousSize = _DebugMessage.Count;
        if (message != null)
        {
            DebugMessage newMessage = new DebugMessage(type, message, source);
            _DebugMessage.Add(newMessage);
        }
        else
        {
            DebugMessage newMessage = new DebugMessage(99, "Erroneous message", source);
            _DebugMessage.Add(newMessage);
        }
    }

    // clear the list to reset it to 19 messages
    private void clearDebug()
    {
        while(_DebugMessage.Count > 19)
        {
            _DebugMessage.RemoveAt(0);
            //for (int i = 0; i < _DebugMessage.Count - 1; i++)
            //{
            //    _DebugMessage[i] = _DebugMessage[i + 1];
            //}
        }
    }
    private void writeDebug()
    {
        _Text.text = "";
        foreach (DebugMessage dm in _DebugMessage)
        {
            _Text.text += "\n<color=red>" + dm.message;// + " ----- Debug current count : " + _DebugMessage.Count;
        }
    }
    public void debugButton()
    {
        buttonInt++;
        writeDebugMessage("Button Debug Pressed : " + buttonInt , 1, "button");
    }
}
