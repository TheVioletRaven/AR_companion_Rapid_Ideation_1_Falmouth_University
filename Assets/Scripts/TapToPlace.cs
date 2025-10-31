using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class TapToPlace : MonoBehaviour
{
    public GameObject companionPrefab;
    private ARRaycastManager raycastManager;
    private GameObject spawnedObject;
    static readonly List<ARRaycastHit> hits = new();

    void Awake()
    {
        raycastManager = FindFirstObjectByType<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount == 0) return;

        var touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began) return;

        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            var pose = hits[0].pose;

            if (spawnedObject == null)
                spawnedObject = Instantiate(companionPrefab, pose.position, pose.rotation);
            else
                spawnedObject.transform.SetPositionAndRotation(pose.position, pose.rotation);
        }
    }
}
