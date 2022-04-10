using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHandler : MonoBehaviour
{
    public Text nameText;
    public Text descriptionText;

    public Transform content;

    public GameObject buttonPrefab;

    public Ability[] abilities;

    public void CreateButtons()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        abilities = TurnBasedCombatSystem.instance.currentUnit.abilities;

        foreach (Ability ability in abilities)
        {
            GameObject buttonObject = Instantiate(buttonPrefab) as GameObject;
            buttonObject.transform.SetParent(content, false);

            AbilityButton abilityButtonComponent = buttonObject.GetComponent<AbilityButton>();
            abilityButtonComponent.CreateButton(ability, this);
        }
    }

    public void ShowPlayerActions()
    {
        TurnBasedCombatSystem.instance.PlayerActions(true);
    }

    public void DisplayText(string text)
    {
        descriptionText.text = text;
    }
}