using UnityEngine;

public static class HexMetrics
{
    public const float outerRadius = 1f;
    public const float innerRadius = outerRadius * 0.866025404f;
    public const float elevationStep = .25f;

    static Vector3[] corners =
    {
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(0f, 0f, outerRadius)
    };

    public static Vector3 GetFirstCorner (HexDirection direction)
    {
        return corners[(int)direction];
    }

    public static Vector3 GetSecondCorner (HexDirection direction)
    {
        return corners[(int)direction + 1];
    }

    public static string GetEdgeType (int elevation1, int elevation2)
    {
        if(elevation1 == elevation2)
        {
            return "flat";
        }
        int delta = elevation2 - elevation1;
        if(delta == 1 || delta == -1)
        {
            return "slope";
        }
        return "cliff";
    }
}
