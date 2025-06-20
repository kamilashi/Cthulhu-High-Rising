using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScreenUI : MonoBehaviour
{
    public GameManager GameManager;
    public GameObject VictoryScreen;
    public GameObject DefeatScreen;


    // Update is called once per frame
    void Update()
    {
        if (GameManager.combatResult == CombatResult.Won)
        {
            VictoryScreen.SetActive(true);
            DefeatScreen.SetActive(false);

        }
        else if (GameManager.combatResult == CombatResult.Lost)
        {
            VictoryScreen.SetActive(false);
            DefeatScreen.SetActive(true);
        }

    }
}
