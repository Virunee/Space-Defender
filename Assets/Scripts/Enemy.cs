using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 1f;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float explosionDuration = 1f;

    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathVolume = 0.3f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        countdownAndShoot();
    }

    private void countdownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    private void OnTriggerEnter2D(Collider2D otherObject)
    {
        DamageDealer damageDealer = otherObject.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);

        if(!otherObject.name.Contains("Enemy"))
        {
            Destroy(otherObject.gameObject);
        }
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.getDamage();
        if (health <= 0)
        {
            StartCoroutine(PlayExplosionVFX());
            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathVolume);
            Destroy(gameObject);
            
        }
    }

    IEnumerator PlayExplosionVFX()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(explosionDuration);
        Destroy(explosion);

    }
}
