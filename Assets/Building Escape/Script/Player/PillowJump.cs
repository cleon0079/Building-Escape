using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillowJump : MonoBehaviour
{
    [SerializeField] private float speed = 500f;
    private Transform playerPosition;
    private Vector3 dist;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerPosition = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collider)
    {
        Rigidbody otherRigidbody = collider.gameObject.GetComponent<Rigidbody>();
        dist = collider.transform.position - transform.position;
        Vector3 direction = dist.normalized;
        otherRigidbody.AddForce(direction*speed);
    }
}
