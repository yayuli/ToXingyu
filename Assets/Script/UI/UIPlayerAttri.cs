using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPlayerAttri : MonoBehaviour
{
    public GameObject attributePrefab;
    public Transform attributeContainer;

    // 定义属性显示顺序
    private string[] attributeOrder = { "Health", "Energy","Strength", "Dexterity", "Intelligence" };

    void Start()
    {
        UpdateAttributesDisplay();
    }

    public void UpdateAttributesDisplay()
    {
        // 清除现有的子对象
        foreach (Transform child in attributeContainer)
        {
            Destroy(child.gameObject);
        }

        // 根据预定义的顺序创建属性显示
        foreach (string attributeName in attributeOrder)
        {
            var field = Player.instance.attributes.GetType().GetField(attributeName);
            int value = (int)field.GetValue(Player.instance.attributes);
            GameObject newAttribute = Instantiate(attributePrefab, attributeContainer);
            newAttribute.GetComponent<AttributeDisplay>().Setup(field.Name, value); 
        }
    }
}
