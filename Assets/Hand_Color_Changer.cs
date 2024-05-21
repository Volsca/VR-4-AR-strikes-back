using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand_Color_Changer : MonoBehaviour
{
    public GlobalVariables GV;

    private void Start()
    {
        //GV._HandColor = new Color(50, 50, 50, 1);
    }



    public void OnColorChange()
    {
        float value = GV._HandColorScrollbar.value;
        int v = (int)value; // 0, 1, 2 etc. to 9


        GV._DebugWindow.writeDebugMessage("v : " + v, 0, "Hand_Color_Changer");

        switch (v)
        {
            case 0:
                GV._HandColor = GV._ListOfHandColors[0];
                GV.l_handFade.ChangeHandColor(GV._ListOfHandColors[0]);
                GV._DebugWindow.writeDebugMessage("Hand color " + 0, 0, "Hand_Color_Changer");
                break;
            case 1:
                GV._HandColor = GV._ListOfHandColors[1];
                GV.l_handFade.ChangeHandColor(GV._ListOfHandColors[1]);
                GV._DebugWindow.writeDebugMessage("Hand color " + 1, 0, "Hand_Color_Changer");
                break;
            case 2:
                GV._HandColor = GV._ListOfHandColors[2];
                GV.l_handFade.ChangeHandColor(GV._ListOfHandColors[2]);
                GV._DebugWindow.writeDebugMessage("Hand color " + 2, 0, "Hand_Color_Changer");
                break;
            case 3:
                GV._HandColor = GV._ListOfHandColors[3];
                GV.l_handFade.ChangeHandColor(GV._ListOfHandColors[3]);
                GV._DebugWindow.writeDebugMessage("Hand color " + 3, 0, "Hand_Color_Changer");
                break;
            case 4:
                GV._HandColor = GV._ListOfHandColors[4];
                GV.l_handFade.ChangeHandColor(GV._ListOfHandColors[4]);
                GV._DebugWindow.writeDebugMessage("Hand color " + 4, 0, "Hand_Color_Changer");
                break;
            case 5:
                GV._HandColor = GV._ListOfHandColors[5];
                GV.l_handFade.ChangeHandColor(GV._ListOfHandColors[5]);
                GV._DebugWindow.writeDebugMessage("Hand color " + 5, 0, "Hand_Color_Changer");
                break;
            case 6:
                GV._HandColor = GV._ListOfHandColors[6];
                GV.l_handFade.ChangeHandColor(GV._ListOfHandColors[6]);
                GV._DebugWindow.writeDebugMessage("Hand color " + 6, 0, "Hand_Color_Changer");
                break;
            case 7:
                GV._HandColor = GV._ListOfHandColors[7];
                GV.l_handFade.ChangeHandColor(GV._ListOfHandColors[7]);
                GV._DebugWindow.writeDebugMessage("Hand color " + 7, 0, "Hand_Color_Changer");
                break;
            case 8:
                GV._HandColor = GV._ListOfHandColors[8];
                GV.l_handFade.ChangeHandColor(GV._ListOfHandColors[8]);
                GV._DebugWindow.writeDebugMessage("Hand color " + 8, 0, "Hand_Color_Changer");
                break;
            case 9:
                GV._HandColor = GV._ListOfHandColors[9];
                GV.l_handFade.ChangeHandColor(GV._ListOfHandColors[9]);
                GV._DebugWindow.writeDebugMessage("Hand color " + 9, 0, "Hand_Color_Changer");
                break;
            default:
                //GV._HandColor = GV._ListOfHandColors[0];
                break;
        }
    }
}
