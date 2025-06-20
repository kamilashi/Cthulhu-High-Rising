using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static void ProcessUninitializedText(ref string text)
    {
        if (text == "")
        {
            text = "Please set the text in the scriptable object.";
        }
    }
}
