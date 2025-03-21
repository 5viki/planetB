using System.Collections.Generic;
using UnityEngine;

public class BodyInteractionHandler : MonoBehaviour
{
    CelestialBody myCelestialBody;
    PlayerPlanet myPlayerPlanet;
    [SerializeField] float maxMassDifference = 5f;
    public List<CelestialBody> attachedPlanets;
    
    void Start()
    {
        myCelestialBody = GetComponent<CelestialBody>();
        myPlayerPlanet = GetComponent<PlayerPlanet>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        CelestialBody otherCelestialBody = other.GetComponent<CelestialBody>();
      if (
        other.CompareTag("Stickable") && 
        otherCelestialBody.mass < maxMassDifference * myCelestialBody.mass
        )
        {
            // Check if the colliding object has a Rigidbody2D
            Rigidbody2D targetRb = collision.gameObject.GetComponent<Rigidbody2D>();
            
            if (targetRb != null)
            {
                // Add a FixedJoint2D to this object
                FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
                
                // Connect the joint to the colliding object's Rigidbody2D
                joint.connectedBody = targetRb;
                myCelestialBody.mass += otherCelestialBody.mass * 7/8;
                otherCelestialBody.mass = otherCelestialBody.mass / 8;
                otherCelestialBody.gameObject.layer = 6;
                if (otherCelestialBody == myPlayerPlanet.targetBody){myPlayerPlanet.RemoveSling(otherCelestialBody);}
                attachedPlanets.Add(otherCelestialBody);
            }
        }
    }  
    
}
