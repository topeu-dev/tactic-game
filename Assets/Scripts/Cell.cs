using UnityEngine;

public class Cell : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    public int x;
    public int y;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.enabled = true;
    }
    
    internal void EnableMeshRenderer() {
        _meshRenderer.enabled = true;
    }


    public void DisableMeshRenderer() {
        _meshRenderer.enabled = false;
    }
}