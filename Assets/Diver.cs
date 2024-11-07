using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diver : MonoBehaviour
{

    public float speed = 3.0f;      // regular speed

    public float dashTime = 0.25f;  // duration time of each dash
    public float dashSpeed = 1.0f;  // bonus speed amount gained from dashing
    public float dashCooldown = 5.0f;
    public bool isCooldownForDash = false;
    public bool isCooldownForShoot = false;

    public bool isDashing = false; // ensures only subroutine for Dash() runs when the dash happens

    AudioManager am;

    private void Awake()
    {
        am = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        // Regular movement controls
        if (Input.GetKey("w"))
        {
            pos.y += speed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            pos.y -= speed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            pos.x += speed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            pos.x -= speed * Time.deltaTime;
        }

        // Dash mechanic, look at sub-function below
        if (Input.GetKey("space") && !isCooldownForDash)
        {
            am.PlaySFX(am.dashSound);
            StartCoroutine(Dash());
        }

        transform.position = pos;
    }

    private IEnumerator Cooldown()
    {
        isCooldownForDash = true;
        yield return new WaitForSeconds(dashCooldown);
        isCooldownForDash = false;
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        float startTime = Time.time;
        Vector3 dashDirection = Vector3.zero;

        // Determine dash direction based on current input
        if (Input.GetKey("w")) dashDirection.y = 1;
        if (Input.GetKey("s")) dashDirection.y = -1;
        if (Input.GetKey("d")) dashDirection.x = 1;
        if (Input.GetKey("a")) dashDirection.x = -1;

        // Normalize to ensure consistent dash speed
        dashDirection.Normalize();

        while (Time.time < startTime + dashTime)
        {
            transform.position += dashDirection * dashSpeed * Time.deltaTime;
            yield return null;
        }

        StartCoroutine(Cooldown());
        isDashing = false;
    }
}
