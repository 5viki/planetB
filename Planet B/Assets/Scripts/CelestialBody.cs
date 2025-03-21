
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CelestialBody : MonoBehaviour, IPointerDownHandler
{
    public float mass;
    public float gravityScale = 0.5f;
    Collider2D[] myColliders;
    public CircleCollider2D gravitationalField;
    public CircleCollider2D surfaceCollider;
    Rigidbody2D myRigidBody;
    [SerializeField] List<CelestialBody> nearbyBodies;
    bool givenInitialVelocity = false;
    CelestialBody slingBody;
    float slingAttractionFactor;
    bool planetClicked;
    void Start()
    {
        gravityScale = 1f;
        nearbyBodies = new List<CelestialBody>();
        myColliders = GetComponents<CircleCollider2D>();
        myRigidBody = GetComponent<Rigidbody2D>();
        mass = myRigidBody.mass;
        
        foreach (CircleCollider2D collider in myColliders)
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


    void OnTriggerEnter2D(Collider2D collision)
    {
        CelestialBody otherCelestialBody = collision.GetComponent<CelestialBody>();
        if (otherCelestialBody != null && !nearbyBodies.Contains(otherCelestialBody))
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
        myRigidBody.mass = mass;
        if (slingBody != null && GetComponent<PlayerPlanet>() != null) //Increase attraction
        {
            float distance = Vector2.Distance(slingBody.transform.position, transform.position);
            Vector2 direction = (slingBody.transform.position - transform.position).normalized;
            Vector2 forceVector = gravityScale*direction*slingBody.mass / (distance*distance);
            //gravitationalField.radius = slingAttractionFactor * gravitationalField.radius;
            //myRigidBody.AddForce(slingAttractionFactor*forceVector, ForceMode2D.Force);
            myRigidBody.AddForce(slingAttractionFactor* distance * direction, ForceMode2D.Force);
            //gravitationalField.radius = gravitationalField.radius / slingAttractionFactor;
        }
        if (nearbyBodies != null)
        {
            if (!givenInitialVelocity)
            {
                InitialVelocity();
            }
            foreach (CelestialBody body in nearbyBodies)
            {
                float distance = Vector2.Distance(body.transform.position, transform.position);
                Vector2 direction = (body.transform.position - transform.position).normalized;
                Vector2 forceVector = gravityScale*direction*body.mass / (distance*distance);
                myRigidBody.AddForce(forceVector, ForceMode2D.Force);
            }
        }
    }
    private void InitialVelocity()
    {
        foreach (CelestialBody body in nearbyBodies)
        {
            float distance = Vector2.Distance(transform.position, body.transform.position);
            Vector2 direction = (body.transform.position - transform.position).normalized;
            Vector2 perpendicularDirection = new Vector2(-direction.y , direction.x);
            float initialSpeed = Mathf.Sqrt(gravityScale * body.mass / (distance*mass));

            myRigidBody.linearVelocity += perpendicularDirection * initialSpeed; 
            givenInitialVelocity = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!planetClicked)
        {
            PlayerPlanet playerPlanet = FindFirstObjectByType<PlayerPlanet>();
            if (playerPlanet.gameObject == this.gameObject){return;}
            playerPlanet.SlingAround(this);
            planetClicked = true;
            return;
        }
        if(planetClicked)
        {
            PlayerPlanet playerPlanet = FindFirstObjectByType<PlayerPlanet>();
            if (playerPlanet.gameObject == this.gameObject){return;}
            playerPlanet.RemoveSling(this);
            planetClicked = false;
        }

    }

    public void SetSling(CelestialBody planet, float addedGravity)
    {
        slingBody = planet;
        slingAttractionFactor = addedGravity;

    }
}

