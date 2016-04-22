using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Conditions
{
    public int ifCurrStep = 0;
    public Quest quest = null;
    public int newQuestStep = 0;
}

public class QuestStepCondition : MonoBehaviour
{
    [SerializeField]
    private int currentStep = 0;
    [SerializeField]
    private Conditions[] ListOfConditions = null;

    public void OnEventAction()
    {
        foreach (var item in ListOfConditions)
        {
            if (item.quest == null)
            {
                Debug.LogWarning("Ошибка !!! Объект " + gameObject.name + " Есть незаполненные поля!");
                return;
            }
            if (item.ifCurrStep == currentStep)
                item.quest.CurrentStep = item.newQuestStep;
        }
    }
}
