
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    public float mass;
    public float gravityScale = 100f;
    Collider2D[] myColliders;
    public Collider2D gravitationalField;
    public Collider2D surfaceCollider;
    Rigidbody2D myRigidBody;
    List<CelestialBody> nearbyBodies;
    bool givenInitialVelocity = false;
    void Start()
    {
        gravityScale = 5f;
        nearbyBodies = new List<CelestialBody>();
        myColliders = GetComponents<CircleCollider2D>();
        myRigidBody = GetComponent<Rigidbody2D>();
        mass = myRigidBody.mass;
        foreach (Collider2D collider in myColliders)
        {
            if(collider.isTrigger)
            {
                gravitationalField = collider;
            }
            else
            {
                surfaceCollider = collider;
            }
        }
        
    }
    void Update()
    {
        if (!givenInitialVelocity)
        {
            InitialVelocity();
            givenInitialVelocity = true;
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        CelestialBody otherCelestialBody = collision.GetComponent<CelestialBody>();
        if (otherCelestialBody != null)
        {
            nearbyBodies.Add(otherCelestialBody);
        }

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        CelestialBody otherCelestialBody = collision.GetComponent<CelestialBody>();
        if (otherCelestialBody != null)
        {
            nearbyBodies.Remove(otherCelestialBody);
        }

    }

    void FixedUpdate()
    {
        if (nearbyBodies != null)
        {
            foreach (CelestialBody body in nearbyBodies)
            {
                float distance = Vector2.Distance(transform.position, body.transform.position);
                Vector2 direction = (body.transform.position - transform.position).normalized;
                myRigidBody.AddForce((gravityScale*(direction*mass*body.mass) / (distance*distance)), ForceMode2D.Force);
            }
        }
    }
    private void InitialVelocity()
    {
        foreach (CelestialBody body in nearbyBodies)
            {
                float distance = Vector2.Distance(transform.position, body.transform.position);
                Vector2 direction = (body.transform.position - transform.position).normalized;
                transform.LookAt(body.transform);
                myRigidBody.linearVelocity += new Vector2(transform.right.x, transform.right.y) * Mathf.Sqrt(gravityScale * body.mass / distance);
            }
    }
}

