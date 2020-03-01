using UnityEngine;
using UnityEngine.UI;

public class LabelPopup : MonoBehaviour
{
    [SerializeField]
    private Text text;

    public void SetText(string label)
    {
        text.text = label;
    }
}
