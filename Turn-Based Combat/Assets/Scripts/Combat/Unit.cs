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
    public Team team;

    public UnitData unitData;

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
    public int damage;

    [Header("UI")]
    public Text unitNameText;

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

        SetUnitSlider(healthText, healthSlider, maxHealth, currentHealth);
        SetUnitSlider(staminaText, staminaSlider, maxStamina, currentStamina);
    }

    void Reset()
    {
        unitData = new UnitData();
        unitNameColor = new Color(1, 1, 1, 1);
    }

    public int Attack()
    {
        return damage;
    }

    public void Damage(int damage)
    {
        int newHealth = currentHealth - damage;

        if (newHealth <= 0)
        {
            Dead();
        }
        else
        {
            currentHealth -= damage;
        }

        UpdateSlider(healthSlider, currentHealth);
        UpdateSliderText(healthText, maxHealth, currentHealth);

        if (team == Team.Player)
        {
            SetUnitSprite();
        }
        else if (team == Team.Enemy)
        {
            animator.SetTrigger("Damage");
        }
    }

    private void Dead()
    {
        currentHealth = 0;

        isDead = true;

        if (team == Team.Enemy)
        {
            animator.SetBool("IsDead", true);
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

    private void SetUnitSlider(Text text, Slider slider, int max, int current)
    {
        slider.maxValue = max;
        slider.value = slider.maxValue;

        UpdateSliderText(text, max, current);

    }

    private void UpdateSlider(Slider slider, int current)
    {
        slider.value = current;
    }

    private void UpdateSliderText(Text text, int max, int current)
    {
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
}