
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARManager : MonoBehaviour
{
    public static ARManager Singleton;

    [Header("AR Settings")]
    public ARPlaneManager PlaneManager;

    public bool ShowPlanes = true;

    public List<PlaneVisibilityBehaviour> CreatedPlanes =
        new List<PlaneVisibilityBehaviour>();

    [Header("Notes Settings")]
    public GameObject PrefabToInstantiate;

    public int MinNotes = 3;
    public int MaxNotes = 5;

    [Header("Placement Settings")]
    public float FloorHeightOffset = 0.25f;
    public float TableHeightOffset = 0.25f;
    public float TableHeightThreshold = 0.35f;

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

    void Start()
    {
        StartCoroutine(WaitAndSpawnNotes());
    }

    IEnumerator WaitAndSpawnNotes()
    {
        if (PlaneManager == null)
            yield break;

        // Aspetta che vengano rilevati i piani.
        while (PlaneManager.trackables.count == 0)
        {
            yield return null;
        }

        // Aspetta che il telefono analizzi meglio la stanza.
        yield return new WaitForSeconds(8f);

        SpawnNotes();
    }

    void SpawnNotes()
    {
        if (NotesSpawned || PrefabToInstantiate == null)
            return;

        List<ARPlane> detectedPlanes = new List<ARPlane>();

        foreach (ARPlane plane in PlaneManager.trackables)
        {
            if (plane.trackingState == TrackingState.Tracking &&
                plane.alignment == PlaneAlignment.HorizontalUp &&
                plane.size.x > 0.3f &&
                plane.size.y > 0.3f)
            {
                detectedPlanes.Add(plane);
            }
        }

        if (detectedPlanes.Count == 0)
            return;

        // Trova il piano più basso: lo consideriamo il pavimento.
        float floorHeight = float.MaxValue;

        foreach (ARPlane plane in detectedPlanes)
        {
            float height = plane.transform.TransformPoint(
                plane.center
            ).y;

            if (height < floorHeight)
                floorHeight = height;
        }

        List<ARPlane> floorPlanes = new List<ARPlane>();
        List<ARPlane> raisedPlanes = new List<ARPlane>();

        foreach (ARPlane plane in detectedPlanes)
        {
            float height = plane.transform.TransformPoint(
                plane.center
            ).y;

            if (height <= floorHeight + TableHeightThreshold)
                floorPlanes.Add(plane);
            else
                raisedPlanes.Add(plane);
        }

        int numberOfNotes = Random.Range(MinNotes, MaxNotes + 1);

        for (int i = 0; i < numberOfNotes; i++)
        {
            ARPlane selectedPlane;
            float heightOffset;

            if (raisedPlanes.Count > 0 &&
                floorPlanes.Count > 0 &&
                i % 2 == 1)
            {
                selectedPlane = raisedPlanes[
                    Random.Range(0, raisedPlanes.Count)
                ];

                heightOffset = TableHeightOffset;
            }
            else
            {
                List<ARPlane> availablePlanes =
                    floorPlanes.Count > 0 ? floorPlanes : raisedPlanes;

                selectedPlane = availablePlanes[
                    Random.Range(0, availablePlanes.Count)
                ];

                heightOffset = FloorHeightOffset;
            }

            PlaceNote(selectedPlane, heightOffset);
        }

        NotesSpawned = true;
    }

    void PlaceNote(ARPlane plane, float heightOffset)
    {
        Vector3 localPosition = plane.center;

        localPosition.x += Random.Range(
            -plane.size.x / 2f,
            plane.size.x / 2f
        );

        localPosition.z += Random.Range(
            -plane.size.y / 2f,
            plane.size.y / 2f
        );

        Vector3 worldPosition =
            plane.transform.TransformPoint(localPosition);

        worldPosition += Vector3.up * heightOffset;

        GameObject note = Instantiate(
            PrefabToInstantiate,
            worldPosition,
            Quaternion.identity
        );

        if (Camera.main != null)
        {
            Vector3 direction =
                Camera.main.transform.position - worldPosition;

            note.transform.rotation =
                Quaternion.LookRotation(direction);
        }
    }
}
