using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshCombiner : MonoBehaviour {
	// Use this for initialization
	void Start () {
        
        //transform.GetComponent<MeshFilter>().mesh = new Mesh();
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        /*
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        List<Material> materials = new List<Material>();

        foreach (MeshRenderer render in renderers) {
            foreach (Material mat in render.sharedMaterials) {
                materials.Add(mat);
            }
        }

        List<Mesh> submesh = new List<Mesh>();
        foreach (Material mat in materials) {
            List<CombineInstance> combiners = new List<CombineInstance>();
            foreach(MeshFilter filter in meshFilters) {
                MeshRenderer render = filter.gameObject.GetComponent<MeshRenderer>();
                if(render != null && render != GetComponent<MeshRenderer>()) {
                    Material[] localMat = render.sharedMaterials;
                    for (int i = 0; i < localMat.Length; i++) {
                        if(localMat[i] != mat) {
                            continue;
                        }
                        CombineInstance com = new CombineInstance();
                        com.mesh = filter.sharedMesh;
                        com.subMeshIndex = i;
                        com.transform = filter.transform.localToWorldMatrix;
                        combiners.Add(com);
                    }

                }

            }

            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combiners.ToArray());
            submesh.Add(mesh);
        }
        List<CombineInstance> combineAll = new List<CombineInstance>();
        foreach(Mesh mesh in submesh) {
            CombineInstance com = new CombineInstance();
            com.mesh = mesh;
            com.subMeshIndex = 0;
            com.transform = transform.worldToLocalMatrix;
            combineAll.Add(com);
        }

        Mesh finalMesh = new Mesh();
        finalMesh.CombineMeshes(combineAll.ToArray());
        GetComponent<MeshFilter>().sharedMesh = finalMesh;
        */

        AdvancedMerge();
        for (int i = 0; i < meshFilters.Length; i++) {
            meshFilters[i].gameObject.SetActive(false);
        }
        transform.gameObject.SetActive(true);
        
    }

    public void AdvancedMerge() {
        MeshFilter myMeshFilter = GetComponent<MeshFilter>();
        // All our children (and us)
        MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();

        // All the meshes in our children (just a big list)
        List<Material> materials = new List<Material>();
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>(); // <-- you can optimize this
        foreach (MeshRenderer renderer in renderers) {
            if (renderer.transform == transform) {
                continue;
            }
            Material[] localMats = renderer.sharedMaterials;
            foreach (Material localMat in localMats) {
                if (!materials.Contains(localMat)) {
                    materials.Add(localMat);
                }
            }
        }

        // Each material will have a mesh for it.
        List<Mesh> submeshes = new List<Mesh>();
        foreach (Material material in materials) {
            // Make a combiner for each (sub)mesh that is mapped to the right material.
            List<CombineInstance> combiners = new List<CombineInstance>();
            foreach (MeshFilter filter in filters) {
                if (filter.transform == transform) continue;
                // The filter doesn't know what materials are involved, get the renderer.
                MeshRenderer renderer = filter.GetComponent<MeshRenderer>();  // <-- (Easy optimization is possible here, give it a try!)
                if (renderer == null) {
                    Debug.LogError(filter.name + " has no MeshRenderer");
                    continue;
                }

                // Let's see if their materials are the one we want right now.
                Material[] localMaterials = renderer.sharedMaterials;
                for (int materialIndex = 0; materialIndex < localMaterials.Length; materialIndex++) {
                    if (localMaterials[materialIndex] != material)
                        continue;
                    // This submesh is the material we're looking for right now.
                    CombineInstance ci = new CombineInstance();
                    ci.mesh = filter.sharedMesh;
                    ci.subMeshIndex = materialIndex;
                    ci.transform = filter.transform.localToWorldMatrix;
                    combiners.Add(ci);
                }
            }
            // Flatten into a single mesh.
            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combiners.ToArray(), true);
            submeshes.Add(mesh);
        }

        // The final mesh: combine all the material-specific meshes as independent submeshes.
        List<CombineInstance> finalCombiners = new List<CombineInstance>();
        foreach (Mesh mesh in submeshes) {
            CombineInstance ci = new CombineInstance();
            ci.mesh = mesh;
            ci.subMeshIndex = 0;
            ci.transform = transform.worldToLocalMatrix;
            finalCombiners.Add(ci);
        }
        Mesh finalMesh = new Mesh();
        finalMesh.CombineMeshes(finalCombiners.ToArray(), false);
        myMeshFilter.sharedMesh = finalMesh;
        GetComponent<MeshRenderer>().materials = materials.ToArray();
        Debug.Log("Final mesh has " + submeshes.Count + " materials.");
    }
}
