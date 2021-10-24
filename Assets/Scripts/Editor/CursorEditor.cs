using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CursorEditor
{
    static void EditorPlaying()
    {
        if (EditorApplication.isPaused)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
    }
}
