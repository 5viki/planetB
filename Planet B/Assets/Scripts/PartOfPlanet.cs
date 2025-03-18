using UnityEngine;

namespace PlanetProperties{
    public class PartOfPlanet : MonoBehaviour
    {
        //Ovdje se nalazi logika koja olakšava utvrđivanje kojoj vrsti pripada objekt.
        [Tooltip("Vrsta Dijela Planeta")]
        [SerializeField] public PlanetComponent myType;

        void Awake()
        {
            if(gameObject.name.Contains("Base", System.StringComparison.CurrentCultureIgnoreCase))
            {
                myType = PlanetComponent.Base;
                return;
            }
            if(gameObject.name.Contains("Inside", System.StringComparison.CurrentCultureIgnoreCase))
            {
                myType = PlanetComponent.Inside;
                return;
            }
            if(gameObject.name.Contains("Outside", System.StringComparison.CurrentCultureIgnoreCase))
            {
                myType = PlanetComponent.Outside;
                return;
            }
        }
    }
}
