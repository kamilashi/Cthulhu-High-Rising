using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialValueAdjust : MonoBehaviour
{
    public List<MeshRenderer> meshRenderer;
    public List<Material> materials;
    public Vector3 dissolveValue = new Vector3(1, 0, 0.5f); // current value, target value, timeFactor;

    void Start()
    {
        foreach(MeshRenderer mr in meshRenderer)
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

        foreach(Material mat in materials)
        {
            mat.SetFloat("_dissolveValue", dissolveValue.x);
        }
    }

    public void Dissolve(bool dissolve)
    {
        dissolveValue = new Vector3(
            dissolveValue.x, 
            dissolve ? 1 : 0, 
            dissolveValue.z);
    }
}
