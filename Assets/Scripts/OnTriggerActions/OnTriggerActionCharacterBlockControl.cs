﻿using UnityEngine;

public class OnTriggerActionCharacterBlockControl : MonoBehaviour
{
  [SerializeField] private float blockControlTime = 5;
  [SerializeField] private int numTrigger = 1;
  [SerializeField] private float time = 0;
  TriggerBase thisTrigger = null;
  private CharacterMoving characterMoving = null;

  private void Start()
  {
    characterMoving = FindObjectOfType<CharacterMoving>();
    thisTrigger = GetComponent<TriggerBase>();
    thisTrigger.OnTriggerAction += OnTriggerAction;
  }

  private void OnDestroy()
  {
    thisTrigger.OnTriggerAction -= OnTriggerAction;
  }

  private void OnTriggerAction(int currTrigger)
  {
    if (currTrigger == numTrigger)
    {
      Invoke("BlockControl", time);
    }
  }

  private void BlockControl()
  {
    characterMoving.CanMove = false;
    Invoke("ReBlockControl", blockControlTime);
  }

  private void ReBlockControl()
  {
    characterMoving.CanMove = true;
  }
}
