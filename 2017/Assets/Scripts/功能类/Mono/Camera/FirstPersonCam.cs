using UnityEngine;
using System.Collections;


[RequireComponent(typeof(CharacterController))]
public class FirstPersonCam : MonoBehaviour 
{
    public Transform camTf;                 
    public float moveSpeed = 20;            
    public float roateSpeed = 20;
    public float yMinLimit = -60;
    public float yMaxLimit = 40;
    public float ySpeed;                    
    public float jumpSpeed = 10;            
    public float gravityMultiplier = 2;

    private float mouseX, mouseY;

    private CharacterController controller;//角色控制器

    void Start () 
    {
        mouseX = this.transform.eulerAngles.y;
        controller = this.GetComponent<CharacterController>();
	}
	 
	void Update () 
    { 
        RoateView();
        Movement();
        Jump();
	}
        
    /// <summary>
    /// 移动角色
    /// </summary>
    void Movement()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * moveSpeed;
        move.y = ySpeed;
        Vector3 moveDirection = this.transform.TransformDirection(move);
        controller.Move(moveDirection * Time.deltaTime );
    }

    /// <summary>
    /// 旋转视野 
    /// </summary>
    void RoateView()
    {
        mouseX += Input.GetAxis("Mouse X") * Time.deltaTime * roateSpeed;
        transform.eulerAngles = new Vector3(0, mouseX, 0);

        mouseY += Input.GetAxis("Mouse Y") * Time.deltaTime * roateSpeed;
        mouseY = Mathf.Clamp(mouseY, yMinLimit, yMaxLimit); 
        camTf.localEulerAngles = new Vector3(-mouseY, 0, 0);
    }

    /// <summary>
    /// 跳跃
    /// </summary>
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            ySpeed = jumpSpeed;
        }
        //获取默认的重力 Physics.gravity  ==>{0,-9.8,0}
        ySpeed += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
    }
}
