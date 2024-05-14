using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;

public class setSize : MonoBehaviour
{
    public Scrollbar scrollbar;
    public HandsFade lHand;
    public HandsFade rHand;

    private void Start()
    {
        scrollbar.SetValueWithoutNotify(0.5f);
    }

    public void SetHandSize()
    {
        lHand.SetSize(scrollbar.value + 0.5f);
        rHand.SetSize(scrollbar.value + 0.5f);
    }
}
