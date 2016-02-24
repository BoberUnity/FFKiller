using UnityEngine;

public class CameraController : MonoBehaviour
{
  public Transform Target = null;
  public float EffectTime = 1;
  private float alfha = 0;
  private MeshRenderer shadowMeshRenderer = null;
  private bool isShadowEffect = false;  
  private bool back = false;

  private void Start ()
  {    
    transform.position = new Vector3(Target.position.x, Target.position.y, transform.position.z);
    transform.parent = Target;
    shadowMeshRenderer = GetComponentInChildren<MeshRenderer>();
  }

  private void Update()
  {
    if (isShadowEffect)
      ShadowEffect();
  }

  public void StartEffect()
  {
    alfha = 0;
    back = false;
    isShadowEffect = true;
  }

  private void ShadowEffect()
  {
    if (back)
    {
      alfha -= Time.deltaTime / EffectTime;
      if (alfha < 0)
      {
        alfha = 0;
        shadowMeshRenderer.material.color = new Color(1, 1, 1, alfha);
        isShadowEffect = false;
      }
    }
    else
    {
      alfha += Time.deltaTime / EffectTime;
      if (alfha > 1)
      {
        alfha = 1;
        back = true;
      }
    }
    shadowMeshRenderer.material.color = new Color(1, 1, 1, alfha);
  }
}
