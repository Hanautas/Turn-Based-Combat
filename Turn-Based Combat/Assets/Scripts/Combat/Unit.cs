using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Weapon weapon;

    public Ability[] abilities;

    public List<StatusEffectObject> statusEffects;

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
        weapon = data.weapon;
        abilities = data.abilities;
    }

    public int Attack()
    {        
        if (!IsDead())
        {
            int damage = weapon.Damage();

            CombatLog.instance.CreateLog($"{unitName} attacked for {damage} damage!");

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
            StartCoroutine(Dead());
        }
        else
        {
            currentHealth -= damage;
        }

        AudioManager.instance.PlaySound(AudioManager.instance.damageClip);

        UpdateSlider(healthSlider, healthText, maxHealth, currentHealth);

        if (team == Team.Player)
        {
            animator.SetTrigger("Damage");

            SetUnitSprite();
        }
        else if (team == Team.Enemy)
        {
            animator.SetTrigger("Damage");

            PlayEffect("Damage");
        }
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

        if (team == Team.Player)
        {
            SetUnitSprite();
        }
    }

    private IEnumerator Dead()
    {
        currentHealth = 0;

        isDead = true;

        if (team == Team.Enemy)
        {
            yield return new WaitForSeconds(0.5f);

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

    public void SetStamina(int amount)
    {
        int newStamina = currentStamina + amount;

        if (newStamina >= maxStamina)
        {
            currentStamina = maxStamina;
        }
        else if (newStamina <= 0)
        {
            currentStamina = 0;
        }
        else
        {
            currentStamina += amount;
        }

        UpdateSlider(staminaSlider, staminaText, maxStamina, currentStamina);
    }

    public bool CheckStaminaCost(int cost)
    {
        if (currentStamina >= Mathf.Abs(cost))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Defend(bool isActive)
    {
        isDefending = isActive;

        if (isDefending)
        {
            CombatLog.instance.CreateLog($"{unitName} is defending!");
        }
    }

    public void ActivateAbility()
    {
        Ability currentAbility = TurnBasedCombatSystem.instance.GetCurrentAbility();

        if (currentAbility != null)
        {
            currentAbility.Activate(this);
        }
    }

    public void AddStatusEffect(StatusEffect statusEffect)
    {
        if (CheckStatusEffect(statusEffect))
        {
            statusEffects.Add(new StatusEffectObject(statusEffect));
        }
    }

    private bool CheckStatusEffect(StatusEffect statusEffect)
    {
        foreach (StatusEffectObject statusEffectObject in statusEffects)
        {
            if (statusEffectObject.statusEffect == statusEffect)
            {
                statusEffectObject.duration = statusEffect.duration;

                return false;
            }
        }

        return true;
    }

    public void CheckStatusEffects()
    {
        for (int i = statusEffects.Count - 1; i >= 0; i--)
        {
            if (statusEffects[i].duration > 0)
            {
                statusEffects[i].Activate(this);
            }
            else
            {
                statusEffects.RemoveAt(i);
            }
        }
    }

    public void SetTargetButton(bool isActive)
    {
        targetButton.interactable = isActive;

        if (isActive)
        {
            PlayEffect("Select");
        }
        else if (!isActive)
        {
            PlayEffect("Hide");
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

    public void PlayEffect(string trigger)
    {
        if (team == Team.Enemy && !IsDead())
        {
            spriteHandler.effectsAnimator.Play(trigger);
        }
    }
}