using UnityEngine;

public class PlaneVisibilityBehaviour : MonoBehaviour
{
    public MeshRenderer MyMeshRenderer;
    public LineRenderer MyLineRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ARManager.Singleton.CreatedPlanes.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        MyMeshRenderer.enabled = ARManager.Singleton.ShowPlanes;
        MyLineRenderer.enabled = ARManager.Singleton.ShowPlanes;
    }
    private void OnDestroy()
    {
        ARManager.Singleton.CreatedPlanes.Remove(this);
    }
}
