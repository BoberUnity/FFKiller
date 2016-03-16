using UnityEngine;
// Плавно уменьшает громкость на установленном AudioSource
public class OnTriggerActionSoundFade : MonoBehaviour
{
  [SerializeField] private AudioSource targetAudioSource;
  [SerializeField] private float effectTime = 3;
  [SerializeField] private int numTrigger = 1;
  [SerializeField] private float time = 0;  
  TriggerBase thisTrigger = null;
  [SerializeField]
  private bool isEffectOn = false;
  [SerializeField]
  private float currentEffectTime = 0;

  private void Start ()
  {
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
      Invoke("StartSoundFade", time);
    }
  }

  private void StartSoundFade()
  {
    isEffectOn = true;
  }

  private void Update()
  {
    if (isEffectOn)
    {
      targetAudioSource.volume -= currentEffectTime / effectTime;
      currentEffectTime += Time.deltaTime;
      if (currentEffectTime > effectTime)
      {
        targetAudioSource.volume = 0;
        isEffectOn = false;
      }
    }
  }
}
