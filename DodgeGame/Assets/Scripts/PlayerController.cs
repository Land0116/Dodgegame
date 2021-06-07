using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    public float speed = 8f;
    public int hp = 100;

    public Transform tr;

    public HPbar hpbar;

    public float spawnRate = 0.2f;
    public float timeAfterSpawn;
    public GameObject playerBulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        float xSpeed = xInput * speed;
        float zSpeed = zInput * speed;

        Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);
        playerRigidbody.velocity = newVelocity;

        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            Vector3 projectedPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            Vector3 currentPos = transform.position;
            Vector3 rotation = projectedPos - currentPos;
            tr.forward = rotation;
        }

        timeAfterSpawn += Time.deltaTime;
        if(Input.GetButton("Fire1") && timeAfterSpawn >= spawnRate)
        {
            timeAfterSpawn = 0;
            GameObject bullt = Instantiate(playerBulletPrefab, transform.position, transform.rotation);
        }
    }

    public void GetDamage(int _damage)
    {
        hp -= _damage;
        hpbar.SetHP(hp);
        if(hp <= 0)
        {
            Die();
        }
    }

    public void GetHeal(int _heal)
    {
        hp += _heal;
        if (hp > 100)
        {
            hp = 100;
        }
        hpbar.SetHP(hp);
    }

    void Die()
    {
        gameObject.SetActive(false);
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.EndGame();

        GameManager2 gameManager2 = FindObjectOfType<GameManager2>();
        gameManager2.EndGame2();
    }
}
