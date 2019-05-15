using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityCell : HexCell
{

    public enum ResourceType { ENERGY, MATERIAL, MOON }

    public ResourceType resourceType = ResourceType.ENERGY;

    //public Faction currentFaction;

    Renderer mat;
    Color originalColor;
    Color currentColor;
    

    public override void Awake()
    {
        switch (resourceType)
        {
            case ResourceType.ENERGY: type = CellType.CITYENERGY; break;
            case ResourceType.MATERIAL: type = CellType.CITYMATS; break;
            case ResourceType.MOON: type = CellType.CITYMOON; break;
        }
    }

    void Start()
    {
        if (this.gameObject.tag != "Moon")
        {
            mat = GetComponentInChildren<MeshCollider>().gameObject.GetComponent<Renderer>();
            mat.material.SetColor("_EmissionColor", new Color(0.25f, 0.25f, 0.25f));
            originalColor = mat.material.GetColor("_EmissionColor");
        }
    }

    public override void Highlight()
    {
        if (this.gameObject.tag != "Moon")
        {
            StartCoroutine(HighlightCell());
        }
    }

    public override void ToneDown()
    {
        if (this.gameObject.tag != "Moon")
        {
            StartCoroutine(ToneDownCell());
        }
    }

    public override void TurnToRed()
    {
        StartCoroutine(GoAndBackFromRed());
    }

    public override void TurnToColor(string color)
    {
        base.TurnToColor(color);
        switch (color)
        {
            case "Red":
                StartCoroutine(GoToRed());
                break;
            case "Blue":
                StartCoroutine(GoToBlue());
                break;
            case "Purple":
                StartCoroutine(GoToPurple());
                break;
            default:
                print("No has escrito bien el color para cambiar la celda ciudad");
                break;
        }
    }

    public IEnumerator HighlightCell()
    {
        for (int i = 0; i < 17; i++)
        {
            yield return new WaitForSeconds(0.01f);
            Color aux = mat.material.GetColor("_EmissionColor");
            aux.r += 0.05f;
            aux.g += 0.05f;
            aux.b += 0.05f;
            mat.material.SetColor("_EmissionColor", aux);
            currentColor = mat.material.GetColor("_EmissionColor");
        }        
    }

    public IEnumerator ToneDownCell()
    {
        for (int i = 0; i < 17; i++)
        {
            yield return new WaitForSeconds(0.01f);
            Color aux = mat.material.GetColor("_EmissionColor");
            aux.r -= 0.05f;
            aux.g -= 0.05f;
            aux.b -= 0.05f;
            mat.material.SetColor("_EmissionColor", aux);
            currentColor = mat.material.GetColor("_EmissionColor");
        }        
    }

    public IEnumerator GoToPurple()
    {
        float t = 0;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.01f);
            t += 0.1f;
            mat.material.SetColor("_EmissionColor", Color.Lerp(currentColor, Color.magenta, t));
        }
    }

    public IEnumerator GoToBlue()
    {
        float t = 0;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.01f);
            t += 0.1f;
            mat.material.SetColor("_EmissionColor", Color.Lerp(currentColor, Color.blue, t));
        }
    }

    public IEnumerator GoToRed()
    {
        float t = 0;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.01f);
            t += 0.1f;
            mat.material.SetColor("_EmissionColor", Color.Lerp(currentColor, Color.red, t));
        }
    }

    public IEnumerator GoAndBackFromRed()
    {
        float t = 0;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.01f);
            t += 0.1f;
            mat.material.SetColor("_EmissionColor", Color.Lerp(currentColor, Color.red, t));
        }
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.01f);
            t -= 0.1f;
            mat.material.SetColor("_EmissionColor", Color.Lerp(Color.red, originalColor, t));
        }
    }
}