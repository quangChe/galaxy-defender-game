using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] float health = 100;
    [SerializeField] float shotCounter;
    [SerializeField] float explosionDuration = 1f;
    [SerializeField] GameObject explosionParticles;
    [SerializeField] int scoreValue = 150;

    [Header("Projectile")]
    [SerializeField] GameObject missilePrefab;
    [SerializeField] float projectileSpeed = 8f;
    [SerializeField] float minTimebetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;

    [Header("Audio")]
    [SerializeField] AudioClip deathAudio;
    [SerializeField] [Range (0, 1)] float deathAudioVolume = 0.7f;
    [SerializeField] AudioClip shootingAudio;
    [SerializeField] [Range(0, 1)] float shootingAudioVolume = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimebetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimebetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject missile = Instantiate(
            missilePrefab,
            transform.position,
            Quaternion.identity) as GameObject;
        missile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootingAudio, Camera.main.transform.position, shootingAudioVolume);
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
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(explosionParticles, transform.position, transform.rotation) as GameObject;
        Destroy(explosion, explosionDuration);
        AudioSource.PlayClipAtPoint(deathAudio, Camera.main.transform.position, deathAudioVolume);
    }
}
