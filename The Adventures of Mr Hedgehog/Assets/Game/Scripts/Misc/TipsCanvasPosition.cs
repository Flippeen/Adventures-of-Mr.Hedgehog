using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsCanvasPosition : MonoBehaviour
{
    [SerializeField]
    private float tip_E_Height = 1.0f, tip_E_Width = 1.0f,
                  tip_RightClick_Height = 1.0f, tip_RightClick_Width = 1.0f;

    public Vector2 Get_Tip_E()
    {
        return new Vector2(tip_E_Height, tip_E_Width);
    }

    public Vector2 Get_Tip_RightClick()
    {
        return new Vector2(tip_RightClick_Width, tip_RightClick_Height);
    }
}
