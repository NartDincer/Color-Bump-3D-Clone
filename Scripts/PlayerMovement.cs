using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Touch touch;
    [SerializeField] private float speedModifier;
    [SerializeField] private float wallDistance = 5f;
    [SerializeField] private float minCamDistance = 3f;
    public float ballSpeed;
    public float cameraSpeed;
    public float tapCount;
    public float doubleTapTimer;
    public float expForce = 700f;
    public float radius = 5f;
    public Rigidbody CameraRb;
    Rigidbody rb;


    public TimeManager timeManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speedModifier = 0.01f;
    }

    private void FixedUpdate()
    {
        if (!GameManager.singleton.isStarted)
            return;
        ForwardMovement();
        CameraMovement();

    }

    void Update()
    {
        //check the situation of game
        if (!GameManager.singleton.isStarted)
            return;

        if (Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);


            if (touch.phase == TouchPhase.Moved)
            {
                transform.position = new Vector3(
                    transform.position.x + touch.deltaPosition.x * speedModifier,
                    transform.position.y,
                    transform.position.z + touch.deltaPosition.y * speedModifier);

            }

        }


    }

    private void LateUpdate()
    {
        // keep the ball from falling
        Vector3 pos = transform.position;

        if (transform.position.x < -wallDistance)
        {
            pos.x = -wallDistance;
        }
        else if (transform.position.x > wallDistance)
        {
            pos.x = wallDistance;
        }

        if (transform.position.z < Camera.main.transform.position.z + minCamDistance)
        {
            pos.z = transform.position.z + minCamDistance;
        }

        transform.position = pos;

        /*double tap and jump
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            tapCount++;
        }
        if (tapCount > 0)
        {
            doubleTapTimer += Time.deltaTime;
        }
        if (tapCount >= 2)
        {

            rb.velocity = Vector3.up * force;
            //rb.AddForce(new Vector3(0, force, 0));
            //rb.AddForce(new Vector3(0, force, 0), ForceMode.VelocityChange);

            Debug.Log("doubletap başarılı");
            doubleTapTimer = 0.0f;
            tapCount = 0;
        }
        if (doubleTapTimer > 0.5f)
        {
            doubleTapTimer = 0f;
            tapCount = 0;
        }*/

    }

    //Find the enemy objects
    private void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy");
            timeManager.SlowMotion();
            GameManager.singleton.gameOverPanel.SetActive(true);
            GameManager.singleton.isStarted = false;
            CameraRb.velocity = Vector3.zero;
        }
        if (collision.gameObject.tag == "bomb")
        {
            Debug.Log("bum!");
            Explode();
        }

    }

    private void ForwardMovement()
    {
        rb.velocity = Vector3.forward * ballSpeed;
    }

    public void CameraMovement()
    {
        CameraRb.velocity = Vector3.forward * cameraSpeed;
    }


    public void OnTriggerEnter(Collider other)
    {//check the finish line and end the game
        if (other.tag.Equals("FinishLine"))
        {
            Debug.Log("Done");
            timeManager.SlowMotion();
            GameManager.singleton.winPanel.SetActive(true);
            CameraRb.velocity = Vector3.zero;
            GameManager.singleton.isStarted = false;
        }
    }

    public void Explode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rig = nearbyObject.GetComponent<Rigidbody>();
            if (rig != null)
            {
                rig.AddExplosionForce(expForce, transform.position, radius);
            }
        }
    }
}
