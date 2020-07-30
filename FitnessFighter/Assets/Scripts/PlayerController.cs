using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {

        Vector3 mouse = Input.mousePosition;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3( mouse.x, mouse.y, transform.position.y));
        Vector3 forward = mouseWorld - transform.position;
        transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    }
}
