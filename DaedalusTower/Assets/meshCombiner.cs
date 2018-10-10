using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshCombiner : MonoBehaviour {
	// Use this for initialization
	void Start () {
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
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
                MeshRenderer render = filter.GetComponent<MeshRenderer>();
                if(render != null) {
                    Material[] localMat = render.sharedMaterials;
                    for (int i = 0; i < localMat.Length; i++) {
                        if(localMat[i] != mat) {
                            continue;
                        }
                        CombineInstance com = new CombineInstance();
                        com.mesh = filter.sharedMesh;
                        com.subMeshIndex = i;
                        com.transform = filter.transform.position - transform.position;
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
            com.transform = Matrix4x4.identity;
            combineAll.Add(com);
        }

        Mesh finalMesh = new Mesh();
        finalMesh.CombineMeshes(combineAll.ToArray());
        GetComponent<MeshFilter>().sharedMesh = finalMesh;

        /*
        for(int i = 0; i < meshFilters.Length; i++) {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = Matrix4x4.identity;
            meshFilters[i].gameObject.SetActive(false);
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.gameObject.SetActive(true);
        */
    }
}
