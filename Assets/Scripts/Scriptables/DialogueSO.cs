using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/ Dialogue", fileName = "New Dialogue.")]
public class DialogueSO : ScriptableObject
{
    [TextArea(3,10)]
    public string[] sentences;

}
