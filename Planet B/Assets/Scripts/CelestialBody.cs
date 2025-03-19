
using System.Collections.Generic;
using Unity.VisualScripting;
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
    float slingFactor;
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
                if (slingBody != null && body == slingBody) //Increase attraction
                {
                    gravitationalField.radius = slingFactor * gravitationalField.radius;
                    myRigidBody.AddForce(slingFactor*gravityScale*direction*mass*body.mass / (distance*distance), ForceMode2D.Force);
                    gravitationalField.radius = gravitationalField.radius / slingFactor;
                }
                else
                {
                    myRigidBody.AddForce(gravityScale*direction*mass*body.mass / (distance*distance), ForceMode2D.Force);
                }
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
            float initialSpeed = Mathf.Sqrt(gravityScale * body.mass / distance);

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
            Debug.DrawLine(transform.position, playerPlanet.transform.position, Color.white, 3);
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
        slingFactor = addedGravity;

    }
}

