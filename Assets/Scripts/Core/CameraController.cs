using UnityEngine;

public class CameraController : MonoBehaviour
{
  public Transform Target = null;
  public float EffectTime = 1;
  private float alfha = 0;
  private MeshRenderer shadowMeshRenderer = null;
  private bool isShadowEffect = false;  
  private bool back = false;
    //DV{
    Camera Camera;
    float fieldWidth, fieldHeight;
    float cameraWidth, cameraHeight;
    float AvgVelocity;
    float UnitsPerPixel = 1f/32f;
    public GameObject Map;
    private Vector2 UpLeft;

    public float CameraZoom = 1;    //увеличение камеры
    public float traction = 0.0f;   //расстояние (в юнитах), через которое камера начинает двигаться за целью
    //DV}

    private void Start ()
  {    
    transform.position = new Vector3(Target.position.x, Target.position.y, transform.position.z);
    shadowMeshRenderer = GetComponentInChildren<MeshRenderer>();
        //DV{
        //transform.parent = Target;
        Camera = GetComponent<Camera>();

        Sprite Spr = Map.GetComponent<SpriteRenderer>().sprite;
        UpLeft = Map.transform.position;

        //Ширину и длину поля считаем в юнитах
        fieldWidth = Spr.rect.width * UnitsPerPixel;
        fieldHeight = Spr.rect.height * UnitsPerPixel;

        Camera.orthographicSize = Screen.height * 0.5f * UnitsPerPixel / CameraZoom;
        cameraHeight = Camera.orthographicSize;
        cameraWidth = Camera.orthographicSize * Camera.aspect;
        //DV}
    }

    //DV{
    void LateUpdate()
    {
        if (!Target) return;

        float newX = transform.position.x, newY = transform.position.y, newZ = transform.position.z;

        if (Target.position.x - transform.position.x > traction)
            newX = Target.position.x - traction;
        else
        if (Target.position.x - transform.position.x < -traction)
            newX = Target.position.x + traction;

        if (Target.position.y - transform.position.y > traction)
            newY = Target.position.y - traction;
        else
        if (Target.position.y - transform.position.y < -traction)
            newY = Target.position.y + traction;

        newZ = transform.position.z;

        //Проверка границ карты
        if (newX + cameraWidth > UpLeft.x + fieldWidth)
            newX = UpLeft.x + fieldWidth - cameraWidth;
        if (newX - cameraWidth < UpLeft.x)
            newX = UpLeft.x + cameraWidth;

        if (newY + cameraHeight > UpLeft.y)
            newY = UpLeft.y - cameraHeight;
        if (newY - cameraHeight < UpLeft.y - fieldHeight)
            newY = UpLeft.y - fieldHeight + cameraHeight;

        transform.position = new Vector3(newX, newY, newZ);
    }
    //DV}

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
