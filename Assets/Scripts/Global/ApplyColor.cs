using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class ApplyColor : MonoBehaviour
{
    bool updateOnce = true;
    Image img;
    SpriteRenderer sr;
    public bool applyToAllChildren = false;
    //Define our text files that we will pull names from
    TextAsset colorList = null;

    //Enables a drop down in Unity Editor for us to select the
    //  clip names from a list
    public static List<string> allColors = new();
    [ListToDropDown(typeof(ApplyColor), "allColors")]
    public string targetColor;


    //Called when the scene is loaded and when a change is made to a property in the Inspector
    //   this allows us to catch and update any changes
    void updateColors(TextAsset file, string fileName, List<string> colorNames)
    {
        if (file == null) file = Resources.Load(fileName) as TextAsset;

        colorNames.Clear();

        string fs = file.text.Trim();

        string[] fLines = Regex.Split(fs, "\n");

        for (int i = 0; i < fLines.Length; i++) colorNames.Add(fLines[i].Trim());
    }

    //Update our lists with potentially any new values
    //    as values shouldnt change, this probably could be called just 1x with a boolean
    private void OnValidate()
    {
        if (updateOnce)
        {
            updateColors(colorList, "colorNames", allColors);
            updateOnce = false;
        }

        if (sr == null && GetComponent<SpriteRenderer>()) sr = GetComponent<SpriteRenderer>();
        if (img == null && GetComponent<Image>()) img = GetComponent<Image>();

        Color newCol;
        ColorUtility.TryParseHtmlString('#' + targetColor, out newCol);

        Debug.Log(img);
        if (img != null) img.color = newCol;
        if (sr != null) sr.color = newCol;

        if (applyToAllChildren)
        {
            SpriteRenderer[] allChildren = GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < allChildren.Length; i++)
            {
                allChildren[i].color = newCol;
            }

            Image[] allImages = GetComponentsInChildren<Image>();
            for (int i = 0; i < allImages.Length; i++)
            {
                allImages[i].color = newCol;
            }
        }

        if (GetComponent<Camera>()) GetComponent<Camera>().backgroundColor = newCol;
        //set color
    }


    private void Start()
    {
        // Tools.Instance.ColorSwatch[0];
    }
}
