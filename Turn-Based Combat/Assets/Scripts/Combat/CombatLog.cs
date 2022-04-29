using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatLog : MonoBehaviour
{
    public static CombatLog instance;

    public Transform content;

    public GameObject prefab;

    void Awake()
    {
        instance = this;
    }

    public void CreateLog(string text)
    {
        GameObject textObject = Instantiate(prefab) as GameObject;
        textObject.transform.SetParent(content, false);

        Text textComponent = textObject.GetComponent<Text>();
        textComponent.text = text;

        Debug.Log(text);

        StartCoroutine(DestroyLog(textObject, 10f));
    }

    public void CreateLog(string text, Team team)
    {
        GameObject textObject = Instantiate(prefab) as GameObject;
        textObject.transform.SetParent(content, false);

        Text textComponent = textObject.GetComponent<Text>();
        textComponent.text = text;

        if (team == Team.Enemy)
        {
            textComponent.color = new Color(1, 0, 0, 1);
        }

        Debug.Log(text);

        StartCoroutine(DestroyLog(textObject, 10f));
    }

    private IEnumerator DestroyLog(GameObject log, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(log.gameObject);
    }
}