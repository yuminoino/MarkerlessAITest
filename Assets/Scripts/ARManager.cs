using UnityEngine;
using System.Collections.Generic;

public class ARManager : MonoBehaviour
{
    public static ARManager Singleton;
    public Camera MainCamera;
    public GameObject PrefabToInstantiate;
    public bool ShowPlanes = true;
    public List<PlaneVisibilityBehaviour> CreatedPlanes = new List<PlaneVisibilityBehaviour>();
    void OnEnable()
    {
        if (Singleton!= null)
        {
            Destroy(this);
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
   
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Instantiate(PrefabToInstantiate, hit.point, Quaternion.identity);
            }
        }
    }

}
        void TouchObjectpositioning()
     {
        if (Input.touchCount > 0)
        {
            if(Input.GetTouch)
        }
        
    }

