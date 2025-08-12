using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class DialogueTree : MonoBehaviour
{
    public LocalizedStringTable table;

    public string[] keys;
    public string[] namekeys;
    public Sprite[] icon;
    public bool[] side;
    public int[] nextId;
    public int[] panels;
}
