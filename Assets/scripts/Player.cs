using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public float _speed = 6;
    public float _jumpForce = 6;
    private Rigidbody _rig;
    private float inputX;
    private float inputY;
    private Vector3 _movementVector;
    public float rotationSpeed = 100;

    public camera camerathing;
    public GameObject cube;
    public GameObject cameradir;

    public Vector3 targetpos;
    public GameObject lastcube;
    public float maxdistance;
    public LayerMask outlinelayer;

    public int slot = 0;
    public Image slotSelect;
    public Image sloticon1;
    private void Start()
    {
        _rig = GetComponent<Rigidbody>();
        lastcube = Instantiate(cube, new Vector3(0,0,0), new Quaternion(0, 0, 0, 0));
        lastcube.SetActive(false);
        

    }
    private void Update()
    {
        //Cleanerway to get input
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            _rig.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }

        if (Pausemenu.menuOpen != true)
        {
            slot += (int)Input.mouseScrollDelta.y;
        }
        if (slot < 0)
        {
            slot++;
        }
        else if (slot > 8)
        {
            slot--;
        }



        slotSelect.transform.position = new Vector2(slot * 70 + slotSelect.transform.parent.position.x - 278, 0 + slotSelect.transform.parent.position.y);
        





        lastcube.SetActive(false);
        RaycastHit hit;
        Debug.DrawRay(camerathing.transform.position, camerathing.transform.forward * maxdistance, Color.yellow);
        if (Physics.Raycast(camerathing.transform.position, camerathing.transform.forward, out hit, maxdistance, outlinelayer))
        {

            targetpos = hit.point - hit.normal * 0.5f;
            Vector3 targetblock = new Vector3(Mathf.Floor(targetpos.x), Mathf.Floor(targetpos.y), Mathf.Floor(targetpos.z));



            lastcube.SetActive(true);
            lastcube.transform.position = targetblock + new Vector3(0.5f,0.5f,0.5f);

            if (Pausemenu.menuOpen != true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    worldgen.Instance.SetBlock(targetblock, (char)Blocks.BlockType.Air);
                }
                if (Input.GetMouseButtonDown(1))
                {
                    worldgen.Instance.SetBlock(targetblock + hit.normal, (char)(slot + 1));
                }
            }
            
        }


        

    }
    private void FixedUpdate()
    {
        //Keep the movement vector aligned with the player rotation
        

        _movementVector = inputY * cameradir.transform.forward * _speed + inputX * cameradir.transform.right * _speed;
        //Apply the movement vector to the rigidbody without effecting gravity
        _rig.velocity = new Vector3(_movementVector.x, _rig.velocity.y, _movementVector.z);
    }
    private bool IsGrounded()
    {
        //Simple way to check for ground
        if (Physics.Raycast(transform.position, Vector3.down, 1.5f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //void OnCollisionStay(Collision collision)
    //{
    //    foreach (ContactPoint contact in collision.contacts)
    //    {
    //        print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
    //         Visualize the contact point
    //        Debug.DrawRay(contact.point, contact.normal, Color.white);
    //    }
    //}
}
