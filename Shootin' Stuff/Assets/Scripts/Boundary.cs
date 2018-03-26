using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour {

    public GameObject canvasObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Time.timeScale = 0;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            canvasObject.SetActive(true);
        }
    }
}
