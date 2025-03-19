using UnityEngine;

public class PlayerPlanet : MonoBehaviour
{
    CelestialBody myCelestialBody;
    [SerializeField] float slingFactor;
    void Start()
    {
        myCelestialBody = GetComponent<CelestialBody>();
    }
    public void SlingAround(CelestialBody targetPlanet)
    {
        Debug.Log("Slinging around" + targetPlanet.name);
        myCelestialBody.SetSling(targetPlanet, slingFactor);
        targetPlanet.SetSling(myCelestialBody, slingFactor);
    }
    public void RemoveSling(CelestialBody targetPlanet)
    {
        Debug.Log("Removing Sling from" + targetPlanet.name);
        myCelestialBody.SetSling(null, 0f);
        targetPlanet.SetSling(null, 0f);
    }
}
