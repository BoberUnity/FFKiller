﻿using UnityEngine;

public class Vagon : TriggerNPC
{
  private bool isConnected = false;

  protected override void EndDialog()
  {
    base.EndDialog();    
    if (!isConnected)
      Connect();
  }

  private void Connect()
  {
    isConnected = true;
    thisAnimator.enabled = false;
    FindObjectOfType<Party>().Connect(transform);
  }
}
