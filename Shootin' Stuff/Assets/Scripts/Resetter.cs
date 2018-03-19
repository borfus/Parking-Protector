using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resetter : MonoBehaviour
{
    public Rigidbody2D projectile;
    public float ResetSpeed = 0.025f;
    public GameObject canvasObject;

    private float resetSpeedSqr;
    private SpringJoint2D spring;

    // Use this for initialization
    void Start()
    {
        resetSpeedSqr = ResetSpeed * ResetSpeed;
        spring = projectile.GetComponent<SpringJoint2D>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "Enemy")
        {
            Destroy(col.gameObject);
        }
    }

    void Reset()
    {
        canvasObject.SetActive(false);
        Application.LoadLevel(Application.loadedLevel);
        Time.timeScale = 1;
    }
}
