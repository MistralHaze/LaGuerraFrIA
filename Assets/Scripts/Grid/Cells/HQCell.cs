using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQCell : HexCell {

    public float health, maxHealth = 100f;

    Renderer mat;

    public override void Awake()
    {
        base.Awake();
        health = maxHealth;
        type = CellType.HQ;
    }

    void Start()
    {
        mat = GetComponentInChildren<MeshCollider>().gameObject.GetComponent<Renderer>();
        mat.material.SetColor("_EmissionColor", new Color(0.25f, 0.25f, 0.25f));
    }

    public override void Highlight()
    {
        StartCoroutine(HighlightCell());
    }

    public override void ToneDown()
    {
        StartCoroutine(ToneDownCell());
    }

    public override void TurnToRed()
    {
        StartCoroutine(GoToRed());
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
        }
    }

    public IEnumerator GoToRed()
    {
        for (int i = 0; i < 17; i++)
        {
            yield return new WaitForSeconds(0.01f);
            Color aux = mat.material.GetColor("_EmissionColor");
            aux.r += 0.05f;
            mat.material.SetColor("_EmissionColor", aux);
        }
        for (int i = 0; i < 17; i++)
        {
            yield return new WaitForSeconds(0.01f);
            Color aux = mat.material.GetColor("_EmissionColor");
            aux.r -= 0.05f;
            mat.material.SetColor("_EmissionColor", aux);
        }
    }
}
