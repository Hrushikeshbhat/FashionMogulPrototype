using System;
using UnityEngine;

public class Utils
{
    public enum ShirtColors
    {
        RED = 0, DARK_BLUE, LIGHT_BLUE, BLACK
    }

    public static Color GetShirtColor(ShirtColors color)
    {
        switch (color)
        {
            case ShirtColors.RED: return Color.red;
            case ShirtColors.DARK_BLUE: return Color.blue;
            case ShirtColors.LIGHT_BLUE: return Color.cyan;
            case ShirtColors.BLACK: return Color.black;
        }

        return Color.red;
    }
}