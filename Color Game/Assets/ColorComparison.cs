using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ComparisonMode
{
    Analagous,
    Complimentary,
    Triadic
}

public class ColorComparison : MonoBehaviour
{
    public ComparisonMode mode;
    public static ColorComparison singleton;
    // Start is called before the first frame update
    void Start()
    {
        if (singleton != this && singleton != null)
        {
            Destroy(singleton.gameObject);
        }
        singleton = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float CompareColors(Color one, Color two)
    {
        Color full = new Color(1.0f, 1.0f, 1.0f);
        switch (mode)
        {
            case ComparisonMode.Analagous:
                return Similarity(one, two);
            case ComparisonMode.Complimentary:
                return Similarity(full - one, two);
            case ComparisonMode.Triadic:
                return GetHSVOffset(one, two, 0.333f, 0.0416f);
            default:
                Debug.Log($"No code available for mode {mode}!");
                return 1.0f;
        }
    }

    public float GetHSVOffset(Color one, Color two, float goal, float wiggle)
    {
        //Generate angle distance
        float angleDiff = GetHSVAngleDiff(one, two);

        if (angleDiff > .5f)
        {
            angleDiff = 1.0f - angleDiff;
        }

        Debug.Log("Angle diff is " + angleDiff);
        
        //See how close we are to it!
        float totalDiff = Mathf.Abs(angleDiff - goal);

        if (totalDiff < wiggle)
        {
            return 1.0f;
        }
        return wiggle / totalDiff;
    }

    public float GetHSVAngleDiff(Color one, Color two)
    {
        float H1, H2, S, V;
        Color.RGBToHSV(one, out H1, out S, out V);
        Color.RGBToHSV(two, out H2, out S, out V);
        return Mathf.Abs(H1 - H2);
    }    

    public float Similarity(Color one, Color two)
    {
        return 1.0f - ColorVal(Diff(one, two));
    }

    public Color Diff(Color one, Color two)
    {
        return new Color(Mathf.Abs(one.r - two.r), Mathf.Abs(one.g - two.g), Mathf.Abs(one.b - two.b));
    }

    public float ColorVal(Color color)
    {
        return (color.r + color.g + color.b) / 3.0f;
    }
}
