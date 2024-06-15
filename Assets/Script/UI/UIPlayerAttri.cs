using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIPlayerAttri : MonoBehaviour
{
    public GameObject attributePrefab;
    public Transform attributeContainer;

    // 定义属性显示顺序
    private string[] attributeOrder = { "health", "moveSpeedFactor", "armor", "attackSpeed" };
    private List<GameObject> attributeDisplays = new List<GameObject>();

    private void OnEnable()
    {
        Debug.Log("Subscribing to events");
        Player.instance.OnHealthRangeChanged += UpdateAttributesDisplay;
        Player.instance.OnMoveSpeedChanged += UpdateAttributesDisplay;
        Player.instance.OnAromoChanged += UpdateAttributesDisplay;
        Player.instance.OnAttackSpeedChanged += UpdateAttributesDisplay;
    }

    private void OnDisable()
    {
        Debug.Log("Unsubscribing from events");
        Player.instance.OnHealthRangeChanged -= UpdateAttributesDisplay;
        Player.instance.OnMoveSpeedChanged -= UpdateAttributesDisplay;
        Player.instance.OnAromoChanged -= UpdateAttributesDisplay;
        Player.instance.OnAttackSpeedChanged += UpdateAttributesDisplay;
    }

    void Start()
    {
        InitializeAttributeDisplays();
    }

    private void InitializeAttributeDisplays()
    {
        foreach (string attributeName in attributeOrder)
        {
            GameObject newAttribute = Instantiate(attributePrefab, attributeContainer);
            newAttribute.SetActive(false);
            attributeDisplays.Add(newAttribute);
        }
        UpdateAttributesDisplay();
    }

    public void UpdateAttributesDisplay()
    {
        //Debug.Log("Updating attributes display");
        for (int i = 0; i < attributeOrder.Length; i++)
        {
            var field = Player.instance.attributes.GetType().GetField(attributeOrder[i]);
            if (field == null)
            {
               // Debug.LogError("Field not found: " + attributeOrder[i]);
                continue;
            }
            int value = (int)field.GetValue(Player.instance.attributes);
            GameObject attributeDisplay = attributeDisplays[i];
            attributeDisplay.SetActive(true);
            attributeDisplay.GetComponent<AttributeDisplay>().Setup(field.Name, value);
        }
    }
}
