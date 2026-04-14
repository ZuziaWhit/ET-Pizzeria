using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PizzaCutColor
{
    public virtual Color GetCutColor()
    {
        Debug.Log("Basic form");
        return Color.red;
    }
}

public class DefaultColor : PizzaCutColor
{
    public override Color GetCutColor()
    {
        Debug.Log("Override take over!");

        if (ColorUtility.TryParseHtmlString("#FCFAE3", out Color cutColor))
            return cutColor;

        return Color.white;
    }
}

public class GreenColor : PizzaCutColor
{
    public override Color GetCutColor()
    {
        Debug.Log("Override take over!");

        return Color.green;
    }
}