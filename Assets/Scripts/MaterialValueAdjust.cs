using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialValueAdjust : MonoBehaviour
{
    public List<MeshRenderer> meshRenderer;
    public List<SkinnedMeshRenderer> skinnedMeshRenderer;
    public List<Material> materials;
    public Vector3 dissolveValue = new Vector3(1, 0, 0.5f); // current value, target value, timeFactor;
    public Vector3 highlightValue = new Vector3(1, 0, 10f); // current value, target value, timeFactor;

    void Start()
    {
        foreach(MeshRenderer mr in meshRenderer)
        {
            foreach(Material mat in mr.materials)
            {
                materials.Add(mat);
            }
        }
        foreach(SkinnedMeshRenderer mr in skinnedMeshRenderer)
        {
            foreach(Material mat in mr.materials)
            {
                materials.Add(mat);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        dissolveValue = new Vector3(
            Mathf.Lerp(dissolveValue.x, dissolveValue.y, Time.deltaTime * dissolveValue.z), 
            dissolveValue.y, 
            dissolveValue.z);

        highlightValue = new Vector3(
            Mathf.Lerp(highlightValue.x, highlightValue.y, Time.deltaTime * highlightValue.z),
            highlightValue.y,
            highlightValue.z);

        foreach (Material mat in materials)
        {
            mat.SetFloat("_dissolveValue", dissolveValue.x);
        }

        foreach (Material mat in materials)
        {
            mat.SetFloat("_HighlightValue", highlightValue.x);
        }
    }

    public void Dissolve(bool dissolve)
    {
        dissolveValue = new Vector3(
            dissolveValue.x, 
            dissolve ? 1 : 0, 
            dissolveValue.z);
    }

    public void SetColor(Color _color)
    {
        foreach(Material mat in materials)
        {
            mat.SetColor("_ColorHighlight", _color);
        }

        highlightValue = new Vector3(
            highlightValue.x,
            1,
            highlightValue.z);
    }

    public void HightlightFlash(Color _color)
    {
        foreach (Material mat in materials)
        {
            mat.SetColor("_ColorHighlight", _color);
        }

        highlightValue = new Vector3(
            4,
            0,
            highlightValue.z);
    }
}
