﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    public GameObject bulletPrefab;
    public Transform firePoint;
    public GameObject mousePointA;
    public GameObject mousePointB;
    public GameObject arrow;
    public GameObject circle;

    public int playerHealth = 50;

    // calc distance
    private float currentDistance;
    private float maxDistance = 3f;
    private float safeSpace;
    private float shootPower;

    private Vector3 shootDirection;
    private Vector3 push;


    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Virus" || collision.tag == "Bakteri") {
            playerHealth -= 2;
            Debug.Log(playerHealth);
        }
        if (collision.tag == "Food" ) {
            playerHealth += 5;
            Debug.Log(playerHealth);
        }
    }

    private void OnMouseDrag() {
        Time.timeScale = 0.05f;
        

        currentDistance = Vector3.Distance(mousePointA.transform.position, transform.position);

        if(currentDistance <= maxDistance) {
            safeSpace = currentDistance;
        } else {
            safeSpace = maxDistance;
        }

        doArrowAndCircleStuff();

        // calc power and direction
        shootPower = Mathf.Abs(safeSpace) * 10;

        Vector3 dimxy = mousePointA.transform.position - transform.position;
        float difference = dimxy.magnitude;
        mousePointB.transform.position = transform.position + ((dimxy / difference) * currentDistance * -1);
        mousePointB.transform.position = new Vector3(mousePointB.transform.position.x, mousePointB.transform.position.y, -0.8f);

        shootDirection = Vector3.Normalize(mousePointA.transform.position - transform.position);
    }

 

    private void OnMouseUp() {
        Time.timeScale = 1f;
        arrow.GetComponent<Renderer>().enabled = false;
        circle.GetComponent<Renderer>().enabled = false;

        push = shootDirection * shootPower * -1;
        Shoot();
       // Debug.Log(push);
        //GetComponent<Rigidbody2D>().AddForce(push, ForceMode2D.Impulse);
    }

    void Shoot() {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(push, ForceMode2D.Impulse);
    }

    private void doArrowAndCircleStuff() {
        arrow.GetComponent<Renderer>().enabled = true;
       // circle.GetComponent<Renderer>().enabled = true;

        if(currentDistance <= maxDistance) {
            arrow.transform.position = new Vector3((2 * transform.position.x) - mousePointA.transform.position.x,
                (2 * transform.position.y) - mousePointA.transform.position.y, -1.8f);
        } else {
            Vector3 dimxy = mousePointA.transform.position - transform.position;
            float difference = dimxy.magnitude;
            arrow.transform.position = transform.position + ((dimxy / difference) * maxDistance * -1);
            arrow.transform.position = new Vector3(arrow.transform.position.x, arrow.transform.position.y, -1.8f);
        }
    }
}
