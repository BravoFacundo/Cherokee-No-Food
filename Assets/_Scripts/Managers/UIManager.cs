using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private int enemyCount;

    [Header("Config")]
    [SerializeField] private List<GameObject> bars = new();
    [SerializeField] private List<GameObject> coins = new();
    [SerializeField] private GameObject enemiesBar;
    private Animator enemiesBarAnimator;

    [Header("References")]
    [SerializeField] private Transform enemiesBarParent;
    //private float enemiesBarYpos;
    //private float enemiesBarWidth;

    [Header("Prefabs")]
    [SerializeField] private GameObject enemiesBarPrefab;
    [SerializeField] private GameObject coinPrefab;


    private void Start()
    {
        InstantiateBars(enemyCount);
    }

    public void InstantiateBars(int enemyCount)
    {
        float enemiesBarYpos = enemiesBarPrefab.GetComponent<RectTransform>().anchoredPosition.y;
        float enemiesBarWidth = enemiesBarPrefab.GetComponent<RectTransform>().rect.width - 12;

        List<RectTransform> coinTransforms = enemiesBarPrefab.GetComponentsInChildren<RectTransform>().ToList();
        coinTransforms.RemoveAt(0);

        int barsNeeded = CalculateEnemyBars(enemyCount);
        float barPosX = CalculateBarStartPosX(barsNeeded, enemiesBarWidth);

        EraseCanvasElementsChilds(enemiesBarParent.gameObject);
        for (int i = 0; i < barsNeeded; i++)
        {
            GameObject newBar = Instantiate(enemiesBarPrefab, enemiesBarParent);

            RectTransform newBarRT = newBar.GetComponent<RectTransform>();
            newBarRT.anchoredPosition = new Vector2(barPosX, enemiesBarYpos);
            barPosX += Mathf.Abs(enemiesBarWidth / 2) * 2;

            for (int j = 0; j < 5; j++)
            {
                Destroy(newBar.transform.GetChild(j).gameObject);
                GameObject newCoin = Instantiate(coinPrefab, newBar.transform);
                RectTransform newCoinRT = newBar.GetComponent<RectTransform>();
                //newCoinRT = coinTransforms[i];
            }
        }
    }


    private int CalculateEnemyBars(int enemyCount)
    {
        if (enemyCount <= 0) return 0;
        else
        {
            int barsNeeded = enemyCount / 5;
            if (enemyCount % 5 != 0) barsNeeded++;
            return barsNeeded;
        }
    }
    private float CalculateBarStartPosX(float barsNeeded, float enemiesBarWidth)
    {
        if (barsNeeded == 0) return 0;
        else
        {
            float barPosX = (enemiesBarWidth / 2) * (barsNeeded - 1);

            return barPosX * -1;
        }
    }
    private void EraseCanvasElementsChilds(GameObject parentObject)
    {
        foreach (Transform child in parentObject.transform)
        {
            Destroy(child.gameObject);
        }
    }

}


