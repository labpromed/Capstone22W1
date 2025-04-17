using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;


    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck; 
    public float groundDistance = 0.4f; 
    public LayerMask groundMask;

    AudioManager audioManager;
    //public AudioSource audioSource;
    //public AudioClip jumpSound;

    Vector3 velocity;

    bool isGrounded;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
       
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        controller.Move(velocity * Time.deltaTime);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        
        float x = Input.GetAxis("Horizontal"); //a d
        float z = Input.GetAxis("Vertical"); //w s

        Vector3 move = transform.right * x + transform.forward * z;

      
        controller.Move(move * speed * Time.deltaTime);

        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            audioManager.PlaySFX(audioManager.jumpSound);
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            
        }

       
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }



}