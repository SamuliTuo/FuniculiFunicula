using UnityEngine;

[RequireComponent(typeof(PlatformController))]
public class PlatformWaypointFeeder : MonoBehaviour
{
    [Header("Oscillation Settings")]
    public Vector3 direction = Vector3.down;
    public float distance = 2f; 
    public float waitTime = 0.5f;

    private PlatformWaypoint waypointA;
    private PlatformWaypoint waypointB;
    private PlatformController platform;

    void Start()
    {
        platform = GetComponent<PlatformController>();
        platform.waitTime = waitTime;

        Vector3 startPos = transform.position;
        waypointA = CreateWaypoint(startPos, "Waypoint_A");
        waypointB = CreateWaypoint(startPos + direction.normalized * distance, "Waypoint_B");
        waypointA.nextWaipoint = waypointB;
        waypointB.nextWaipoint = waypointA;
        platform.currentWaypoint = waypointA;
    }

    private PlatformWaypoint CreateWaypoint(Vector3 position, string name)
    {
        GameObject wpObj = new GameObject(name);
        wpObj.transform.position = position;
        PlatformWaypoint wp = wpObj.AddComponent<PlatformWaypoint>();
        return wp;
    }
}