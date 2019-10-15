using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Configuration parameters
    [Header("Viewport")]
    [SerializeField] float xPadding = 0.6f;
    [SerializeField] float yPaddingTopPercentage = 0.3f;
    [SerializeField] float yPaddingBottom = 0.5f;

    [Header("Space Craft")]
    [SerializeField] float health = 100;
    [SerializeField] float MoveSpeed = 10f;
    
    [Header("Projectile")]
    [SerializeField] GameObject missilePrefab;
    [SerializeField] float projectileSpeed = 12f;
    [SerializeField] float projectileFiringPeriod = 0.3f;

    [Header("Audio")]
    [SerializeField] AudioClip shootingAudio;
    [SerializeField] [Range(0, 1)] float shootingAudioVolume = 0.2f;
    [SerializeField] AudioClip deathAudio;
    [SerializeField] [Range(0, 1)] float deathAudioVolume = 0.8f;

    Coroutine firingCoroutine;

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
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject missile = Instantiate(
                missilePrefab,
                transform.position,
                Quaternion.identity) as GameObject;
            missile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootingAudio, Camera.main.transform.position, shootingAudioVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
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
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - yPaddingTopPercentage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer) { ProcessHit(damageDealer); }
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Destroy(gameObject);
        //GameObject explosion = Instantiate(explosionParticles, transform.position, transform.rotation) as GameObject;
        //Destroy(explosion, explosionDuration);
        AudioSource.PlayClipAtPoint(deathAudio, Camera.main.transform.position, deathAudioVolume);
        Time.timeScale = 0;
    }
}
