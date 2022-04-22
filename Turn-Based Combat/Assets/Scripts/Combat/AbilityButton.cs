using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text buttonText;
    public Image buttonImage;

    public Ability ability;

    public AbilityHandler abilityHandler;

    public void CreateButton(Ability ability, AbilityHandler abilityHandler)
    {
        this.ability = ability;
        this.abilityHandler = abilityHandler;

        buttonText.text = ability.abilityName;
        buttonImage.sprite = ability.icon;
    }

    public void SetAbilityMode()
    {
        if (TurnBasedCombatSystem.instance.currentUnit.CheckStaminaCost(ability.cost))
        {
            ability.SetMode();
        }
        else
        {
            Debug.Log("Too expensive!");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        abilityHandler.DisplayText($"{ability.abilityName} - {ability.abilityDescription}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        abilityHandler.DisplayText("");
    }
}