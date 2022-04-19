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
        ability.SetMode();
    }

    public void HidePlayerActions()
    {
        TurnBasedCombatSystem.instance.PlayerActions(false);
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