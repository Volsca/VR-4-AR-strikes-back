using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugWindow : MonoBehaviour
{
    public class DebugMessage
    {
        public int type; // 0 for warning, 1 for error, 2 for gobblin or smth idk, 99 for incorrect message
        public string message;
        public int messageSize;
        public string source;
        // Constructeurs
        public DebugMessage() { type = 0; message = null; messageSize = 0; source = null; }

        public DebugMessage(int t, string m, int ms, string s)
        {
            type = (t < 3) ? t : 99;
            message = m;
            messageSize = ms;
            source = s;
        }

    }
    // public attributes
    public TMP_Text _Text;
    // private attributes
    private List<DebugMessage> _DebugMessage;
    private int previousSize;

    void Start()
    {
        _DebugMessage = new List<DebugMessage>();
    }

    void FixedUpdate()
    {
        if (previousSize != _DebugMessage.Count)
        {
            if (_DebugMessage[_DebugMessage.Count].type == 0)
            {
                _Text.text += "\n" + "<color=red>" + _DebugMessage[_DebugMessage.Count].message + "</Color>";
            }
            else
            {
                _Text.text += "\n" + "<color=yellow>" + _DebugMessage[_DebugMessage.Count].message + "</Color>";
            }

        }

        previousSize = _DebugMessage.Count;
    }

    public void writeDebugMessage(string message, int type, string source)
    {
        previousSize = _DebugMessage.Count;
        int size;
        if ((size = message.Length) > 1)
        {
            DebugMessage newMessage = new DebugMessage(type, message, size, source);
            _DebugMessage.Add(newMessage);
        }
        else
        {
            DebugMessage newMessage = new DebugMessage(99, "Erronious message", "Erronious message".Length, source);
            _DebugMessage.Add(newMessage);
        }
    }
}
