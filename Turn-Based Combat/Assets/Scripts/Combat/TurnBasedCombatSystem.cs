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
    public Turn turn;

    public int currentUnitIndex;
    public Unit currentUnit;

    public Unit target;

    public List<Unit> playerUnits;
    public List<Unit> enemyUnits;

    [Header("Combat Encounter")]
    public CombatEncounter combatEncounter;
    public List<UnitData> enemyUnitsData;

    [Header("Prefabs")]
    public GameObject enemyUnitPrefab;
    public GameObject enemyBoxPrefab;

    [Header("UI")]
    public Transform enemyContent;
    public Transform enemyStationContent;

    public GameObject playerActions;
    public GameObject playerActionSelect;
    public GameObject playerTargetSelect;
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
        enemyUnitsData = combatEncounter.enemyUnits;

        CreateUnits();

        currentUnitIndex = 0;
        currentUnit = playerUnits[currentUnitIndex];

        PlayerTurn();
    }

    private void CreateUnits()
    {
        //CreatePlayerUnits()

        foreach (UnitData data in enemyUnitsData)
        {
            CreateEnemyUnits(data);
        }
    }

    private void CreatePlayerUnits()
    {
        
    }

    private void CreateEnemyUnits(UnitData data)
    {
        GameObject enemyUnitObject = Instantiate(enemyUnitPrefab) as GameObject;
        enemyUnitObject.transform.SetParent(enemyContent, false);

        GameObject enemyBox = Instantiate(enemyBoxPrefab) as GameObject;
        enemyBox.transform.SetParent(enemyStationContent, false);

        Enemy enemy = enemyBox.GetComponent<Enemy>();
        
        Unit enemyUnit = enemyUnitObject.GetComponent<Unit>();
        enemyUnit.unitData = data;
        enemyUnit.unitImage = enemy.enemyImage;
        enemyUnit.animator = enemy.animator;

        enemyUnits.Add(enemyUnit);
    }

    private void GameOver()
    {
        int isGameOver = IsGameOver();

        if (isGameOver == 2)
        {
            PlayerActions(false);

            StartCoroutine(DisplayGameOver(true));
        }
        else if (isGameOver == 1)
        {
            PlayerActions(false);

            StartCoroutine(DisplayGameOver(false));
        }
    }

    private IEnumerator DisplayGameOver(bool isWin)
    {
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
            if (currentUnitIndex != playerUnits.Count)
            {
                for (int i = currentUnitIndex; i < playerUnits.Count; i++)
                {
                    if (!playerUnits[i].IsDead())
                    {
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
            if (currentUnitIndex != enemyUnits.Count)
            {
                for (int i = currentUnitIndex; i < enemyUnits.Count; i++)
                {
                    if (!enemyUnits[i].IsDead())
                    {
                        currentUnit = enemyUnits[i];

                        return;
                    }
                }
            }
            else
            {
                currentUnitIndex = -1;

                turn = Turn.Player;

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

        RandomPlayerUnit().Damage(currentUnit.Attack());

        StartCoroutine(WaitEndTurn(1f));
    }

    public void EndTurn()
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

        GameOver();
    }

    private IEnumerator WaitEndTurn(float delay)
    {
        PlayerActions(false);

        yield return new WaitForSeconds(delay);

        EndTurn();
    }

    public void SetTarget(int index)
    {
        target = enemyUnits[index];
    }

    public void SetTarget(Unit unit)
    {
        target = unit;
    }

    public Unit RandomPlayerUnit()
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

    public void AttackTarget()
    {
        target.Damage(currentUnit.Attack());

        StartCoroutine(WaitEndTurn(1f));
    }

    public void PlayerActions(bool isActive)
    {
        if (isActive)
        {
            playerActions.SetActive(true);

            CreateSelectButtons();
        }
        else if (!isActive)
        {
            playerActions.SetActive(false);
            playerActionSelect.SetActive(true);
            playerTargetSelect.SetActive(false);
        }
    }

    private void CreateSelectButtons()
    {
        foreach (Transform child in playerTargetSelect.transform)
        {
            Destroy(child.gameObject);
        }

        GameObject backButtonObject = Instantiate(buttonPrefab) as GameObject;
        backButtonObject.transform.SetParent(playerTargetSelect.transform, false);
        
        Button backButton = backButtonObject.GetComponent<Button>();
        backButton.onClick.AddListener(() => playerActionSelect.SetActive(true));
        backButton.onClick.AddListener(() => playerTargetSelect.SetActive(false));

        foreach (Unit unit in enemyUnits)
        {
            if (!unit.IsDead())
            {
                GameObject buttonObject = Instantiate(buttonPrefab) as GameObject;
                buttonObject.transform.SetParent(playerTargetSelect.transform, false);
                
                Button button = buttonObject.GetComponent<Button>();
                button.onClick.AddListener(() => SetTarget(unit));
                button.onClick.AddListener(() => AttackTarget());
            }
        }
    }
}