using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetFade : MonoBehaviour
{
    public Scrollbar scrollbar;
    public HandsFade lHand;
    public HandsFade rHand;

    private void Start()
    {
        scrollbar.SetValueWithoutNotify(1.0f);
    }

    public void SetFadeScrollbar()
    {
        lHand.SetAlpha(scrollbar.value);
        rHand.SetAlpha(scrollbar.value);
    }
}
