using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPlayerController : MonoBehaviour
{


    private const float gravityScale = 9.8f, speedScale = 5f, jumpForce = 8f, turnSpeed = 90f;
    private float verticalSpeed = 0f, mouseX = 0f, mouseY = 0f, currentAngleX = 0f;
    private CharacterController controller;
    [SerializeField] private Camera goCamera;
    [SerializeField] private GameObject particlesPrefab, currentEquipItem;
    private float hitLastTime = 0f;
    private const float hitScaleSpeed = 15f;


    private Transform itemParent;
    private bool canMove = true;
    public RaycastHit hit;

    [SerializeField] private GameObject[] equiptableItems;
    public const string EQUIPE_NOT_SELECTED_TEXT = "EquipeNotSelected";
    [HideInInspector] public string itemYouCanEquipName = EQUIPE_NOT_SELECTED_TEXT;
   
   

  

    void Update()
    {
        if (canMove)
        {
            RotateCharacter();
            MoveCharacter();
            
        }
      
    }
    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();

       
    }

    private void RotateCharacter()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        transform.Rotate(new Vector3(0f, mouseX * turnSpeed * Time.deltaTime, 0f));
        currentAngleX += mouseY * turnSpeed * Time.deltaTime * -1f;
        currentAngleX = Mathf.Clamp(currentAngleX, -60f, 60f);
        goCamera.transform.localEulerAngles = new Vector3(currentAngleX, 0f, 0f);
    }

    private void MoveCharacter()
    {
        Vector3 velocity = new Vector3(
            Input.GetAxis("Horizontal"), //(x) -1 ..0.. 1 * 5
            0,
            Input.GetAxis("Vertical")); //(z) -1 ..0.. 1
        velocity = transform.TransformDirection(velocity) * speedScale; // velocity * 5
        transform.hasChanged = true;
        if (controller.isGrounded)
        {
            verticalSpeed = 0;
            if (Input.GetButton("Jump"))
            {
                verticalSpeed = jumpForce;
            }
        }
        verticalSpeed -= gravityScale * Time.deltaTime;
        velocity.y = verticalSpeed;
        controller.Move(velocity * Time.deltaTime);
    }
   
    
  

    
   
   
   
}


