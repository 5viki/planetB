using UnityEditor.Animations;
using UnityEngine;
using PlanetProperties;
using Unity.VisualScripting;

public class Planet : MonoBehaviour
{
    [SerializeField] public Color outlineColor;
    [SerializeField] public Color baseColor;
    [SerializeField] public Color insideColor;

    void Start()
    {
        SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
        GeneratePlanet();
    }

    private void GeneratePlanet()
    {
        CalculateColors();
        ApplyColors();
    }

    private void ApplyColors()
    {
        foreach (SpriteRenderer childSprite in GetComponentsInChildren<SpriteRenderer>())
        {
            PartOfPlanet childProperty = childSprite.gameObject.GetComponent<PartOfPlanet>();
            if (childProperty == null){Debug.LogWarning("ChildPROP NULL!");}
            if (childSprite == null){Debug.LogWarning("ChildSPR NULL!");}
            PlanetComponent childType = childProperty.myType;
            Debug.Log(childType);
            if (childType == PlanetComponent.Base){childSprite.color = baseColor;}
            if (childType == PlanetComponent.Inside){childSprite.color = insideColor;}
            if (childType == PlanetComponent.Outside){childSprite.color = outlineColor;}
                    
        }
    }

    private Color[] CalculateColors()
    {
        baseColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        insideColor = new Color
        ( //Boje Slične bazi, donekle
        Random.Range(baseColor.r-0.3f, baseColor.r+0.3f), 
        Random.Range(baseColor.g-0.3f, baseColor.g+0.3f),
        Random.Range(baseColor.b - 0.3f, baseColor.b + 0.3f)
        );
        Color[] myColors = {outlineColor, baseColor, insideColor};

        return myColors;
    }
}
