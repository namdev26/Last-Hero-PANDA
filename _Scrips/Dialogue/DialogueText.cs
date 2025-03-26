using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/DialogueText")]

public class DialogueText : ScriptableObject
{
    public string NPCName;

    [TextArea(3, 10)]
    public string[] paragraps;
}
