using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform firePoint;
    public GameObject ammoType;

    public float shotSpeed;
    public float shotCounter, fireRate;


    //private Animator playerAnim;

    void Start()
    {
        //playerAnim = GetComponentInParent<Animator>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        
        UpdateFirePointDirection(moveHorizontal, moveVertical);


        if (Input.GetMouseButton(0))
        {
            shotCounter -= Time.deltaTime;
            if(shotCounter <= 0)
            {
                shotCounter = fireRate;
                Shoot();
            }

            //playerAnim.SetBoll("Shoot", true);
        }

        else
        {
            shotCounter = 0;
            //playerAnim.SetBool("shoot", false);
        }
    }

    void UpdateFirePointDirection(float horizontal, float vertical)
    {
        if (horizontal != 0 || vertical != 0)
        {
            
            Vector2 direction = new Vector2(horizontal, vertical).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0, 0, angle);
        }
    }



    void Shoot()
    {
        GameObject shot = Instantiate(ammoType, firePoint.position, firePoint.rotation);
        Rigidbody2D shotRB = shot.GetComponent<Rigidbody2D>();
        shotRB.AddForce(firePoint.right * shotSpeed, ForceMode2D.Impulse);
        Destroy(shot.gameObject, 1f);
    }
}
