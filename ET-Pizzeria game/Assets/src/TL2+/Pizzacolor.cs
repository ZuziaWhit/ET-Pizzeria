using UnityEngine;

public class PizzaCutManager : CutScreenManager
{
    public override Color GetCutColor()
    {
        Debug.Log("Override take over!");

        if (ColorUtility.TryParseHtmlString("#FCFAE3", out Color cutColor))
            return cutColor;

        return Color.white;
    }
}
