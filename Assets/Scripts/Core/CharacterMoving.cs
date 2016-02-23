using UnityEngine;

[RequireComponent(typeof(Animator))]

public class CharacterMoving : MonoBehaviour
{
  public Sprite Portrait = null;
  [SerializeField] private float speed = 1;
  private Animator thisAnimator = null;
  [HideInInspector] public bool CanMove = true;

	private void Start ()
  {
    thisAnimator = GetComponent<Animator>();
  }
	
	private	void FixedUpdate ()
  {
    if (CanMove)
    {
      if (Input.GetKey(KeyCode.UpArrow))
      {
        transform.position += Vector3.up * Time.fixedDeltaTime * speed;
        thisAnimator.SetBool("IsMoving", true);
        thisAnimator.SetInteger("Direction", 0);
      }

      if (Input.GetKey(KeyCode.DownArrow))
      {
        transform.position -= Vector3.up * Time.fixedDeltaTime * speed;
        thisAnimator.SetBool("IsMoving", true);
        thisAnimator.SetInteger("Direction", 2);
      }
      if (Input.GetKey(KeyCode.RightArrow))
      {
        transform.position += Vector3.right * Time.fixedDeltaTime * speed;
        thisAnimator.SetBool("IsMoving", true);
        thisAnimator.SetInteger("Direction", 1);
      }
      if (Input.GetKey(KeyCode.LeftArrow))
      {
        transform.position -= Vector3.right * Time.fixedDeltaTime * speed;
        thisAnimator.SetBool("IsMoving", true);
        thisAnimator.SetInteger("Direction", 3);
      }
    }

    if ((!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.DownArrow)) || !CanMove)
        thisAnimator.SetBool("IsMoving", false);    
  }
}
