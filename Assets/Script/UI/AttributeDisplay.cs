using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttributeDisplay : MonoBehaviour
{
    public TMP_Text nameLabel;
    public TMP_Text valueLabel;

    public void Setup(string name, int value)
    {
        nameLabel.text = name;
        valueLabel.text = value.ToString();
       
    }
}
