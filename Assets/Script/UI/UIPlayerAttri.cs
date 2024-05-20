using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPlayerAttri : MonoBehaviour
{
    public GameObject attributePrefab;
    public Transform attributeContainer;

    // 定义属性显示顺序
    private string[] attributeOrder = { "Health", "Energy", "Strength", "Dexterity", "Intelligence" };

    private void OnEnable()
    {
        Debug.Log("Subscribing to events");
        Player.instance.OnHealthChanged += UpdateAttributesDisplay;
        Player.instance.OnStrengthChanged += UpdateAttributesDisplay;
        Player.instance.OnEnergyChanged += UpdateAttributesDisplay;
    }

    private void OnDisable()
    {
        Player.instance.OnHealthChanged -= UpdateAttributesDisplay;
        Player.instance.OnStrengthChanged -= UpdateAttributesDisplay;
        Player.instance.OnEnergyChanged -= UpdateAttributesDisplay;
    }


    void Start()
    {
        UpdateAttributesDisplay();
        OnEnable();
    }

    public void UpdateAttributesDisplay()
    {
        Debug.Log("Updating attributes display");
        foreach (Transform child in attributeContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (string attributeName in attributeOrder)
        {
            var field = Player.instance.attributes.GetType().GetField(attributeName);
            if (field == null)
            {
                Debug.LogError("Field not found: " + attributeName);
                continue;
            }
            int value = (int)field.GetValue(Player.instance.attributes);
            GameObject newAttribute = Instantiate(attributePrefab, attributeContainer);
            newAttribute.GetComponent<AttributeDisplay>().Setup(field.Name, value);
        }
    }
}