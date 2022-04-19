using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Team
{
    Player,
    Enemy
}

public class Unit : MonoBehaviour
{
    [Header("Team")]
    public Team team;

    [Header("Name")]
    public string unitName;
    public Color unitNameColor;

    [Header("Health")]
    public bool isDead;
    public int maxHealth;
    public int currentHealth;

    [Header("Stamina")]
    public int maxStamina;
    public int currentStamina;

    [Header("Damage")]
    public bool isDefending;
    public int damage;

    [Header("Abilities")]
    public Ability[] abilities;

    [Header("UI")]
    public Text unitNameText;

    public Button targetButton;

    public SpriteHandler spriteHandler;

    public Image unitImage;
    public Sprite[] unitSprites;
    
    public Text healthText;
    public Slider healthSlider;

    public Text staminaText;
    public Slider staminaSlider;

    public Animator animator;

    void Start()
    {
        unitNameText.text = unitName;
        unitNameText.color = unitNameColor;

        if (team == Team.Player)
        {
            unitImage.sprite = unitSprites[0];
        }

        currentHealth = maxHealth;
        currentStamina = maxStamina;

        SetUnitSlider(healthSlider, healthText, maxHealth, currentHealth);
        SetUnitSlider(staminaSlider, staminaText, maxStamina, currentStamina);
    }

    void Reset()
    {
        unitNameColor = new Color(1, 1, 1, 1);
    }

    public void CreateUnit(UnitData data)
    {
        unitName = data.unitName;
        unitNameColor = data.unitNameColor;
        maxHealth = data.maxHealth;
        maxStamina = data.maxStamina;
        damage = data.damage;
        abilities = data.abilities;
    }

    public int Attack()
    {
        Debug.Log($"{unitName} attacked for {damage} damage!");
        
        if (!IsDead())
        {
            return damage;
        }
        else
        {
            return 0;
        }
    }

    public void Damage(int damage)
    {
        if (isDefending && damage > 1)
        {
            damage = damage / 2;
        }

        int newHealth = currentHealth - damage;

        if (newHealth <= 0)
        {
            Dead();
        }
        else
        {
            currentHealth -= damage;
        }

        UpdateSlider(healthSlider, healthText, maxHealth, currentHealth);

        if (team == Team.Player)
        {
            animator.SetTrigger("Damage");

            SetUnitSprite();
        }
        else if (team == Team.Enemy)
        {
            animator.SetTrigger("Damage");
        }
    }

    public void ActivateAbility()
    {
        if (TurnBasedCombatSystem.instance.currentAbility != null)
        {
            TurnBasedCombatSystem.instance.currentAbility.Activate(this);
        }
    }

    public void Defend(bool isActive)
    {
        isDefending = isActive;
    }

    public void Heal(int amount)
    {
        int newHealth = currentHealth + amount;

        if (newHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += amount;
        }

        UpdateSlider(healthSlider, healthText, maxHealth, currentHealth);
    }

    public void RecoverStamina(int amount)
    {
        int newStamina = currentStamina + amount;

        if (newStamina >= maxStamina)
        {
            currentStamina = maxStamina;
        }
        else
        {
            currentStamina += amount;
        }

        UpdateSlider(staminaSlider, staminaText, maxStamina, currentStamina);
    }

    private void Dead()
    {
        currentHealth = 0;

        isDead = true;

        if (team == Team.Enemy)
        {
            animator.SetBool("IsDead", true);

            StartCoroutine(HideUnit());
        }
    }

    public bool IsDead()
    {
        if (isDead)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Revive()
    {
        isDead = false;
    }

    public void SetTargetButton(bool isActive)
    {
        targetButton.interactable = isActive;
    }

    private IEnumerator HideUnit()
    {
        yield return new WaitForSeconds(1f);

        spriteHandler.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }

    private void SetUnitSlider(Slider slider, Text text, int max, int current)
    {
        slider.maxValue = max;
        slider.value = slider.maxValue;

        text.text = $"{current} / {max}";
    }

    private void UpdateSlider(Slider slider, Text text, int max, int current)
    {
        slider.value = current;

        text.text = $"{current} / {max}";
    }

    private void SetUnitSprite()
    {
        float healthPercentage = ((float)currentHealth / (float)maxHealth) * 100f;

        if (healthPercentage > 75)
        {
            unitImage.sprite = unitSprites[0];
        }
        else if (healthPercentage > 50)
        {
            unitImage.sprite = unitSprites[1];
        }
        else if (healthPercentage > 25)
        {
            unitImage.sprite = unitSprites[2];
        }
        else if (healthPercentage <= 0 || IsDead())
        {
            unitImage.sprite = unitSprites[3];
        }
    }

    public void PlayEffect(string trigger)
    {
        if (team == Team.Enemy)
        {
            spriteHandler.effectsAnimator.Play(trigger);
        }
    }
}