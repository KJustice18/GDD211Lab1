using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovePlayer : MonoBehaviour
{

    private float SpeedMultiplier = 1f;
    private float MovemntSpeed = 5f;
    private CharacterController cc;
    [SerializeField] private float PlayerVelocity;
    private float JumpSpeed = 0.09f;
    private float VertSpeed;
    private float Gravity = -.25f;
    private bool Grounded;
    private Animator ani;
    private bool isSprinting = false;
    private Camera mainCam;
    [SerializeField] private TMP_Text winTXT;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        ani = GetComponent<Animator>();
        mainCam = Camera.main;
        winTXT.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        float sideMovement = Input.GetAxisRaw("Horizontal") * MovemntSpeed * SpeedMultiplier * Time.deltaTime;

        if (sideMovement < 0) 
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
        if (sideMovement > 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }


        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            SpeedMultiplier = 1.75f;
            isSprinting = true;

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            SpeedMultiplier = 1f;
            isSprinting = false;
        }
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            SceneManager.LoadScene(0);
        }

        if (cc.velocity.y < 0.01f && cc.velocity.y > -0.01f)
        {
            Grounded = true;
        }
        else
        {
            Grounded = false;
        }

        if (Grounded)
        {
            VertSpeed = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                VertSpeed += JumpSpeed;
            }
        }

        mainCam.transform.position = new Vector3 (gameObject.transform.position.x, 2.25f, gameObject.transform.position.z - 8f);

        VertSpeed += (Gravity * Time.deltaTime);

        cc.Move(new Vector3 (sideMovement,VertSpeed,0));
        PlayerVelocity = Mathf.Abs(sideMovement);

        ani.SetFloat("PlayerVelocity", PlayerVelocity);
        ani.SetBool("IsSprinting", isSprinting);
        


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish") 
        {
            winTXT.enabled = true;
        }
    }
}
