﻿using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target = null;
    public float EffectTime = 1;
    public GameObject Map;
    public float CameraZoom = 1;    //увеличение камеры
    public float traction = 0.0f;   //расстояние (в юнитах), через которое камера начинает двигаться за целью

    Camera Camera;
    Animator Animator;
    float fieldWidth, fieldHeight;      //размеры карты
    float cameraWidth, cameraHeight;    //половинные размеры камеры
    float UnitsPerPixel = 1f/32f;
    private Vector2 UpLeft;

    bool ConstX, ConstY;    //Камера вмещает карту целиком и не двигается по Х и/или У.

    private void Start ()
    {    
        //DV{
        //transform.parent = Target;
        Camera = GetComponent<Camera>();

        Sprite Spr = Map.GetComponent<SpriteRenderer>().sprite;
        UpLeft = Map.transform.position;

        TuneMap();

        Camera.orthographicSize = Screen.height * 0.5f * UnitsPerPixel / CameraZoom;
        cameraHeight = Camera.orthographicSize;
        cameraWidth = Screen.width * 0.5f * UnitsPerPixel / CameraZoom;
        Camera.aspect = cameraWidth / cameraHeight;

        Animator = GetComponentInChildren<Animator>();
        Animator.SetFloat("Speed", 0.5f/EffectTime);
        //DV}
    }

    void LateUpdate()
    {
        if (!Target) return;
        if (ConstX && ConstY) return;

        float newX = transform.position.x, newY = transform.position.y, newZ = transform.position.z;

        //Если Ширина камеры больше ширины карты, камеру по горизонтали не двигаем.
        if (!ConstX)
        {
            if (Target.position.x - transform.position.x > traction)
                newX = Target.position.x - traction;
            if (Target.position.x - transform.position.x < -traction)
                newX = Target.position.x + traction;

            //Проверка границ карты по Х
            if (newX + cameraWidth > UpLeft.x + fieldWidth)
                newX = UpLeft.x + fieldWidth - cameraWidth;
            if (newX - cameraWidth < UpLeft.x)
                newX = UpLeft.x + cameraWidth;
        }

        //Если Высота камеры больше высоты карты, камеру по вертикали не двигаем
        if (!ConstY)
        {
            if (Target.position.y - transform.position.y > traction)
                newY = Target.position.y - traction;
            if (Target.position.y - transform.position.y < -traction)
                newY = Target.position.y + traction;

            //Проверка границ карты по Y
            if (newY + cameraHeight > UpLeft.y)
                newY = UpLeft.y - cameraHeight;
            if (newY - cameraHeight < UpLeft.y - fieldHeight)
                newY = UpLeft.y - fieldHeight + cameraHeight;
        }

        transform.position = new Vector3(newX, newY, newZ);
    }


  public void StartEffect()
  {
        Animator.SetTrigger("Play");
  }

    public void TuneMap()
    {
        Sprite Spr = Map.GetComponent<SpriteRenderer>().sprite;
        UpLeft = Map.transform.position;

        //Ширину и длину поля считаем в юнитах
        fieldWidth = Spr.rect.width * UnitsPerPixel;
        fieldHeight = Spr.rect.height * UnitsPerPixel;

        ConstY = (2 * Camera.orthographicSize >= fieldHeight);
        ConstX = (2 * Camera.orthographicSize * Camera.aspect >= fieldHeight);

        //Если размер камеры больше размера карты, устанавливаем камеру в центр карты. Если камера меньше устанавливаем её на цель.
        if(ConstX && ConstY)
            transform.position = Map.transform.position + new Vector3(fieldWidth * 0.5f, -fieldHeight * 0.5f, transform.position.z);
        else
            transform.position = new Vector3(Target.position.x, Target.position.y, transform.position.z);

    }
}
