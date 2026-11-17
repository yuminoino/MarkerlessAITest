
using UnityEngine;

public class PlaneVisibilityBehaviour : MonoBehaviour
{
    public MeshRenderer MyMeshRenderer;
    public LineRenderer MyLineRenderer;

    void Start()
    {
        if (ARManager.Singleton != null)
        {
            ARManager.Singleton.CreatedPlanes.Add(this);
        }
    }

    void Update()
    {
        if (ARManager.Singleton == null)
            return;

        if (MyMeshRenderer != null)
            MyMeshRenderer.enabled = ARManager.Singleton.ShowPlanes;

        if (MyLineRenderer != null)
            MyLineRenderer.enabled = ARManager.Singleton.ShowPlanes;
    }

    private void OnDestroy()
    {
        if (ARManager.Singleton != null)
        {
            ARManager.Singleton.CreatedPlanes.Remove(this);
        }
    }
}
