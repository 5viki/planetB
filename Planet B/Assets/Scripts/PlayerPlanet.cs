using System.Collections.Generic;
using UnityEngine;

public class PlayerPlanet : MonoBehaviour
{
    CelestialBody myCelestialBody;
    BodyInteractionHandler myBodyInteractionHandler;
    public CelestialBody targetBody;
    [SerializeField] float slingFactor;
    bool isFree = true;
    void Start()
    {
        myCelestialBody = GetComponent<CelestialBody>();
        myBodyInteractionHandler = GetComponent<BodyInteractionHandler>();
    }
    public void SlingAround(CelestialBody targetPlanet)
    {
        if (isFree && !myBodyInteractionHandler.attachedPlanets.Contains(targetPlanet))
        {
            Debug.Log("Slinging around" + targetPlanet.name);
            myCelestialBody.SetSling(targetPlanet, slingFactor);
            targetPlanet.SetSling(myCelestialBody, slingFactor);
            Debug.DrawLine(transform.position, targetPlanet.transform.position, Color.white, 3);
            isFree = false;
            targetBody = targetPlanet;
        }
    }
    public void RemoveSling(CelestialBody targetPlanet)
    {
        if (!isFree)
        {
            Debug.Log("Removing Sling from" + targetPlanet.name);
            Debug.DrawLine(transform.position, targetPlanet.transform.position, Color.red, 3);
            myCelestialBody.SetSling(null, 0f);
            targetPlanet.SetSling(null, 0f);
            isFree = true;
            targetBody = null;
        }
        
    }
}
