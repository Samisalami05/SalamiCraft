using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class camera : MonoBehaviour
{
    public float sensX = 240f;
    public float sensY = 240f;

   

    float xRotation;
    float yRotation;

    public Rigidbody rb;

    public Transform player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        float mouseX = Input.GetAxisRaw("Mouse X") * 0.01f * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * 0.01f * sensY;

        

        if (Pausemenu.menuOpen != true)
        {
            yRotation += mouseX;

            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            //player.rotation = Quaternion.Euler(0, yRotation, 0);
            transform.parent.rotation = Quaternion.Euler(0, yRotation, 0);
            //rb.MoveRotation(Quaternion.Euler(xRotation, yRotation, 0));
        }



    }

    //private void OnApplicationFocus(bool focus)
    //{
    //    if (focus)
    //    {
    //        Cursor.lockState = CursorLockMode.Locked;
    //        Cursor.visible = false;
    //    }
        
    //}
}
