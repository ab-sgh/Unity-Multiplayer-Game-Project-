using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletforce= 20f;
    public LayerMask playerLayer;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.TransformPoint(Vector3.zero), firePoint.rotation);
        bullet.layer = gameObject.layer;
    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
    rb.AddForce(firePoint.up * bulletforce, ForceMode2D.Impulse);
    Debug.Log($"Bullet layer: {bullet.layer} Player Layer: {gameObject.layer}");

    }
}
