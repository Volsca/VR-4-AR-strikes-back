using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using OVR.OpenVR;
using Unity.VisualScripting;

public class NewDebugWindow : MonoBehaviour
{
    #region Attributes$
    // Little singleton
    private static NewDebugWindow _GlobalDebug;

    public static NewDebugWindow GetInstance()
    {
        if (_GlobalDebug != null)
        {
            return _GlobalDebug;
        }
        else
        {
            return null;
        }

    }

    [SerializeField] private TextMeshProUGUI _Text;
    [SerializeField] private int MaxMessages = 20;
    private List<DebugMessage> _DebugMessage;
    private int previousSize;
    private int buttonInt;

    // Defining the structure for debug messages
    public class DebugMessage
    {
        public string _Message { get; private set; }
        public string _MessageSource { get; private set; }
        public int _Type { get; private set; }

        public DebugMessage() { _Type = 0; _Message = null; _MessageSource = null; }

        public DebugMessage(int t, string m, string s)
        {
            _Type = (t < 3) ? t : 99;
            _Message = m;
            _MessageSource = s;
        }
    }

    private NewDebugWindow() { } // Private to prevent instantiation
    #endregion

    void Start()
    {
        _DebugMessage = new List<DebugMessage>();
        MaxMessages = 20;
        buttonInt = 0;
    }
    void Awake()
    {
        if (_GlobalDebug == null)
        {
            _GlobalDebug = this;
        }
    }

    void Update()
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
        while (_DebugMessage.Count > 19)
        {
            _DebugMessage.RemoveAt(0);
        }
    }
    private void writeDebug()
    {
        _Text.text = "";
        foreach (DebugMessage dm in _DebugMessage)
        {
            if (dm._Type == 0)
            {
                _Text.text += dm._MessageSource + "\n<color=white>" + dm._Message;// + " ----- Debug current count : " + _DebugMessage.Count;
                Debug.Log(dm._MessageSource + " : " + dm._Message);
            }
            else
            {
                _Text.text += dm._MessageSource + "\n<color=red>" + dm._Message;// + " ----- Debug current count : " + _DebugMessage.Count;
                Debug.Log(dm._MessageSource + " : " + dm._Message);
            }

        }
    }
    public void debugButton()
    {
        buttonInt++;
        writeDebugMessage("Button Debug Pressed : " + buttonInt, 1, "button");
    }
}
