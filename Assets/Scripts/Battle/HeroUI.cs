﻿using UnityEngine;
using UnityEngine.UI;

public class HeroUI : MonoBehaviour
{
  [SerializeField] private Hero hero = null;
  [SerializeField] private Text NameText = null;
  [SerializeField] private Text mhpText = null;
  [SerializeField] private Text hpText = null;
  [SerializeField] private Image aglImage = null;

  public void UpdateUI ()
  {
    NameText.text = hero.Name;
    mhpText.text = hero.Mhp.ToString("f0");
    hpText.text = hero.Hp.ToString("f0");
  }
}
