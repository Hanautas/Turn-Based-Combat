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
    public int damage;

    [Header("UI")]
    public Text unitNameText;

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
    }

    public int Attack()
    {
        Debug.Log(unitName + " attacked!");
        
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
}