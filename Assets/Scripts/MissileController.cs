using UnityEngine;

public class MissileController : MonoBehaviour
{
    public float missileSpeed = 25f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * missileSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
            GameObject gy = Instantiate(GameManager.instance.explosionEffect, transform.position, transform.rotation);
            Destroy(gy, 2f);
        }
    }
}
