using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] float MoveSpeed = 10f;
    [SerializeField] float xPadding = 0.6f;
    [SerializeField] float yPaddingTopPercentage = 0.3f;
    [SerializeField] float yPaddingBottom = 0.5f;
    [SerializeField] GameObject missilePrefab;
    [SerializeField] float projectileSpeed = 10f;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity) as GameObject;
            missile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * MoveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + xPadding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - xPadding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + yPaddingBottom;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y * yPaddingTopPercentage;
    }
}
