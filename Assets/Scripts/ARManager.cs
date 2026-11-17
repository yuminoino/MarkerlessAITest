
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARManager : MonoBehaviour
{
    public static ARManager Singleton;

    public ARPlaneManager PlaneManager;
    public GameObject PrefabToInstantiate;
    public bool ShowPlanes = true;

    public List<PlaneVisibilityBehaviour> CreatedPlanes =
        new List<PlaneVisibilityBehaviour>();

    public int NumberOfNotes = 5;

    public float SpawnRadius = 2f;
    public float MinHeight = 0.5f;
    public float MaxHeight = 1.5f;

    private bool NotesSpawned = false;

    void Awake()
    {
        if (Singleton != null && Singleton != this)
        {
            Destroy(gameObject);
            return;
        }

        Singleton = this;
    }

    void Update()
    {
        if (!NotesSpawned && PlaneManager != null)
        {
            foreach (ARPlane plane in PlaneManager.trackables)
            {
                if (plane.trackingState == TrackingState.Tracking &&
                    plane.alignment == PlaneAlignment.HorizontalUp &&
                    plane.size.x > 0.5f &&
                    plane.size.y > 0.5f)
                {
                    SpawnNotes(plane);
                    NotesSpawned = true;
                    break;
                }
            }
        }
    }

    void SpawnNotes(ARPlane plane)
    {
        for (int i = 0; i < NumberOfNotes; i++)
        {
            Vector3 localPosition = plane.center;

            localPosition.x += Random.Range(
                -SpawnRadius, SpawnRadius);

            localPosition.z += Random.Range(
                -SpawnRadius, SpawnRadius);

            localPosition.y += Random.Range(
                MinHeight, MaxHeight);

            Vector3 worldPosition =
                plane.transform.TransformPoint(localPosition);

            GameObject note = Instantiate(
                PrefabToInstantiate,
                worldPosition,
                Quaternion.identity
            );

            note.transform.rotation =
                Quaternion.LookRotation(
                    Camera.main.transform.position - worldPosition
                );
        }
    }
}
