using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAnim
{
    public AnimationClip idle;
    public AnimationClip runF;    
    public AnimationClip runB;    
    public AnimationClip runL;
    public AnimationClip runR;
}

public class PlayerCtrl : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float rotSpeed = 80.0f;

    public PlayerAnim playerAnim;

    private Animation animation;
    private Rigidbody rigidbody;

    private float h = 0.0f;
    private float v = 0.0f;
    private float r = 0.0f;

    private Vector3 moveDir;

    private void Awake() 
    {
        animation = GetComponent<Animation>();
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        GameManager.OnItemChange += UpdateSetup;
        moveSpeed = GameManager.instance.gameData.speed;
    }

    private void UpdateSetup()
    {
        moveSpeed = GameManager.instance.gameData.speed;
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        moveDir = (transform.forward * v) + (transform.right * h);

        if(moveDir.magnitude > 1.0f)
            moveDir.Normalize();

        //transform.Translate(moveDir * moveSpeed * Time.deltaTime);
        //rigidbody.MovePosition(moveDir * moveSpeed * Time.deltaTime + transform.position);

        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);        

        if(v >= 0.1f)
        {
            animation.CrossFade(playerAnim.runF.name, 0.3f);
        }else if(v <= -0.1f)
        {
            animation.CrossFade(playerAnim.runB.name, 0.3f);
        }else if(h <= -0.1f)
        {
            animation.CrossFade(playerAnim.runL.name, 0.3f);
        }else if(h >= 0.1f)
        {
            animation.CrossFade(playerAnim.runR.name, 0.3f);
        }else
        {
            animation.CrossFade(playerAnim.idle.name, 0.3f);
        }
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(moveDir * moveSpeed * Time.fixedDeltaTime + transform.position);
    }
}
