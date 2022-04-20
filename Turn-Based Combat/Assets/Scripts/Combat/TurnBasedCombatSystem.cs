using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Turn
{
    Player,
    Enemy
}

public class TurnBasedCombatSystem : MonoBehaviour
{
    public static TurnBasedCombatSystem instance;

    [Header("Combat System")]
    public int round;
    public Turn turn;

    public bool canAttack;

    public int currentUnitIndex;
    public Unit currentUnit;

    public Ability currentAbility;

    public List<Unit> playerUnits;
    public List<Unit> enemyUnits;

    [Header("Combat Encounter")]
    public CombatEncounter combatEncounter;

    [Header("Prefabs")]
    public GameObject playerUnitPrefab;

    public GameObject enemyUnitPrefab;
    public GameObject enemyBoxPrefab;

    [Header("UI")]
    public Transform playerContent;

    public Transform enemyContent;
    public Transform enemyStationContent;

    public GameObject playerActions;
    public GameObject playerActionSelect;
    public GameObject playerAbilitySelect;
    public GameObject cancelButton;
    public List<GameObject> playerMenus;

    public GameObject buttonPrefab;

    public GameObject winScreen;
    public GameObject loseScreen;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PlayerActions(false);

        StartCombat();
    }

    public void StartCombat()
    {
        CreateUnits();

        currentUnitIndex = 0;
        currentUnit = playerUnits[currentUnitIndex];

        PlayerTurn();
    }

    private void CreateUnits()
    {
        for (int i = 0; i < PlayerData.instance.playableCharacters.Length; i++)
        {
            if (PlayerData.instance.GetIsRecruited(i))
            {
                CreatePlayerUnits(PlayerData.instance.GetUnitData(i), PlayerData.instance.GetUnitSprites(i));
            }
        }

        foreach (Encounter encounter in combatEncounter.encounters)
        {
            CreateEnemyUnits(encounter.enemyUnitData, encounter.enemySprite);
        }
    }

    private void CreatePlayerUnits(UnitData unitData, Sprite[] playerSprites)
    {
        GameObject UnitObject = Instantiate(playerUnitPrefab) as GameObject;
        UnitObject.transform.SetParent(playerContent, false);
        
        Unit playerUnit = UnitObject.GetComponent<Unit>();
        playerUnit.CreateUnit(unitData);
        playerUnit.unitSprites = playerSprites;

        playerUnits.Add(playerUnit);
    }

    private void CreateEnemyUnits(UnitData unitData, Sprite enemySprite)
    {
        GameObject enemyUnitObject = Instantiate(enemyUnitPrefab) as GameObject;
        enemyUnitObject.transform.SetParent(enemyContent, false);

        GameObject enemyBox = Instantiate(enemyBoxPrefab) as GameObject;
        enemyBox.transform.SetParent(enemyStationContent, false);

        SpriteHandler spriteHandler = enemyBox.GetComponent<SpriteHandler>();
        spriteHandler.entityImage.sprite = enemySprite;
        
        Unit enemyUnit = enemyUnitObject.GetComponent<Unit>();
        enemyUnit.CreateUnit(unitData);
        enemyUnit.spriteHandler = spriteHandler;
        enemyUnit.unitImage = spriteHandler.entityImage;
        enemyUnit.animator = spriteHandler.animator;

        Button enemyButtonComponent = enemyBox.transform.Find("Target Button").GetComponent<Button>();
        enemyButtonComponent.onClick.AddListener(() => AttackTarget(enemyUnit));
        enemyButtonComponent.onClick.AddListener(() => enemyUnit.ActivateAbility());

        enemyUnit.targetButton = enemyButtonComponent;

        enemyUnits.Add(enemyUnit);
    }

    private void GameOver()
    {
        int isGameOver = IsGameOver();

        if (isGameOver == 2)
        {
            StartCoroutine(DisplayGameOver(true));
        }
        else if (isGameOver == 1)
        {
            StartCoroutine(DisplayGameOver(false));
        }
    }

    private IEnumerator DisplayGameOver(bool isWin)
    {
        PlayerActions(false);

        yield return new WaitForSeconds(1f);

        if (isWin)
        {
            winScreen.SetActive(true);
        }
        else if (!isWin)
        {
            loseScreen.SetActive(true);
        }
    }

    private int IsGameOver()
    {
        if (IsUnitListDead(enemyUnits))
        {
            return 2;
        }
        else if (IsUnitListDead(playerUnits))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private bool IsUnitListDead(List<Unit> unitList)
    {
        foreach (Unit unit in unitList)
        {
            if (!unit.IsDead())
            {
                return false;
            }
        }

        return true;
    }

    private void SelectNextActiveUnit()
    {
        currentUnitIndex++;

        if (turn == Turn.Player)
        {
            if (currentUnitIndex < playerUnits.Count)
            {
                for (int i = currentUnitIndex; i < playerUnits.Count; i++)
                {
                    if (!playerUnits[i].IsDead())
                    {
                        currentUnitIndex = i;

                        currentUnit = playerUnits[i];

                        return;
                    }
                }
            }
            else
            {
                currentUnitIndex = 0;

                turn = Turn.Enemy;
            }
        }
        
        if (turn == Turn.Enemy)
        {
            if (currentUnitIndex < enemyUnits.Count)
            {
                for (int i = currentUnitIndex; i < enemyUnits.Count; i++)
                {
                    if (!enemyUnits[i].IsDead())
                    {
                        currentUnitIndex = i;

                        currentUnit = enemyUnits[i];

                        return;
                    }
                }
            }
            else
            {
                currentUnitIndex = -1;

                turn = Turn.Player;

                NextRound();

                SelectNextActiveUnit();
            }
        }
    }

    private void PlayerTurn()
    {
        PlayerActions(true);
    }

    private void EnemyTurn()
    {
        PlayerActions(false);

        GetRandomPlayerUnit().Damage(currentUnit.Attack());

        EndTurn();
    }

    public void EndTurn()
    {
        canAttack = false;

        PlayerActions(false);

        StartCoroutine(WaitEndTurn(1f));
    }

    private IEnumerator WaitEndTurn(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (IsGameOver() != 0)
        {
            GameOver();
        }
        else
        {
            PlayerActions(false);

            SelectNextActiveUnit();

            if (turn == Turn.Player)
            {
                PlayerTurn();
            }
            else if (turn == Turn.Enemy)
            {
                EnemyTurn();
            }
        }
    }

    private void NextRound()
    {
        round++;

        foreach (Unit unit in playerUnits)
        {
            unit.RecoverStamina(2);

            if (unit.isDefending)
            {
                unit.RecoverStamina(2);
                unit.Defend(false);
            }
        }

        foreach (Unit unit in enemyUnits)
        {
            unit.RecoverStamina(1);
        }
    }

    public Unit GetRandomPlayerUnit()
    {
        List<Unit> aliveUnits = new List<Unit>();

        foreach (Unit unit in playerUnits)
        {
            if (!unit.IsDead())
            {
                aliveUnits.Add(unit);
            }
        }

        return aliveUnits[Random.Range(0, aliveUnits.Count - 1)];
    }

    public Unit GetRandomEnemyUnit()
    {
        List<Unit> aliveUnits = new List<Unit>();

        foreach (Unit unit in enemyUnits)
        {
            if (!unit.IsDead())
            {
                aliveUnits.Add(unit);
            }
        }

        return aliveUnits[Random.Range(0, aliveUnits.Count - 1)];
    }

    public void AttackMode(bool isActive)
    {
        if (isActive)
        {
            canAttack = true;

            foreach (Unit unit in enemyUnits)
            {
                if (!unit.IsDead())
                {
                    unit.SetTargetButton(true);
                }
            }
        }
        else if (!isActive)
        {
            canAttack = false;

            foreach (Unit unit in enemyUnits)
            {
                if (!unit.IsDead())
                {
                    unit.SetTargetButton(false);
                }
            }
        }
    }

    public void AttackTarget(Unit target)
    {
        if (canAttack)
        {
            AttackMode(false);

            target.Damage(currentUnit.Attack());

            EndTurn();
        }
        else
        {
            Debug.Log("Can't attack!");
        }
    }

    public void AbilityMode(Ability ability, Team team)
    {
        currentAbility = ability;

        playerAbilitySelect.SetActive(false);
        cancelButton.SetActive(true);

        if (team == Team.Player)
        {
            foreach (Unit unit in playerUnits)
            {
                unit.SetTargetButton(true);
            }
        }
        else if (team == Team.Enemy)
        {
            foreach (Unit unit in enemyUnits)
            {
                unit.SetTargetButton(true);
            }
        }
    }

    public void ResetAbilityMode()
    {
        currentAbility = null;

        foreach (Unit unit in playerUnits)
        {
            unit.SetTargetButton(false);
        }

        foreach (Unit unit in enemyUnits)
        {
            unit.SetTargetButton(false);
        }
    }

    public void Defend()
    {
        currentUnit.Defend(true);

        EndTurn();
    }

    public void PlayerActions(bool isActive)
    {
        if (isActive)
        {
            playerActions.SetActive(true);
            playerActionSelect.SetActive(true);

            foreach (GameObject obj in playerMenus)
            {
                obj.SetActive(false);
            }
        }
        else if (!isActive)
        {
            playerActions.SetActive(false);
        }
    }
}