using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public GameObject character = null;
    public Transform talkingToCharacter = null;
    [TextArea(3, 10)]
    public string[] sentences = null;
    public int id = 0;
    public float dialogueboxHeight = 0.0f;
}
