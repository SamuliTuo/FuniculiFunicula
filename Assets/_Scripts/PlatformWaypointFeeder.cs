using UnityEngine;

[RequireComponent(typeof(PlatformController))]
public class PlatformWaypointFeeder : MonoBehaviour {

     public enum MovementPattern {
        Oscillation,
        Loop,
        Circle,
        Random
    }

    [Header("SSettings")]
    public MovementPattern pattern = MovementPattern.Oscillation;
    public Vector3 direction = Vector3.down;
    public float distance = 2f; 
    public float waitTime = 0.5f;
    public int pointCount = 4;

    private PlatformController platform;
    private readonly System.Collections.Generic.List<PlatformWaypoint> waypoints = new();

    private PlatformWaypoint waypointA;
    private PlatformWaypoint waypointB;
    

    void Start() {
      platform = GetComponent<PlatformController>();
      platform.waitTime = waitTime;

      /* Vector3 startPos = transform.position;
      waypointA = CreateWaypoint(startPos, "Waypoint_A");
      waypointB = CreateWaypoint(startPos + direction.normalized * distance, "Waypoint_B");
      waypointA.nextWaipoint = waypointB;
      waypointB.nextWaipoint = waypointA;
      platform.currentWaypoint = waypointA; */

      switch (pattern) {
        case MovementPattern.Oscillation:
          CreateOscillation();
          break;
        case MovementPattern.Loop:
            CreateLoop();
            break;
        case MovementPattern.Circle:
            CreateCircle();
            break;
      }

      // assign first waypoint if any were created
      if (waypoints.Count > 0)
          platform.currentWaypoint = waypoints[0];
    }

    private void CreateOscillation() {
        Vector3 start = transform.position;
        var a = CreateWaypoint(start, "A");
        var b = CreateWaypoint(start + direction.normalized * distance, "B");

        a.nextWaipoint = b;
        b.nextWaipoint = a;

        waypoints.Add(a);
        waypoints.Add(b);
    }

    private void CreateLoop() {
      Vector3 start = transform.position;
        Vector3 right = Vector3.right * distance;
        Vector3 up = Vector3.up * distance;

        var w1 = CreateWaypoint(start, "1");
        var w2 = CreateWaypoint(start + right, "2");
        var w3 = CreateWaypoint(start + right + up, "3");
        var w4 = CreateWaypoint(start + up, "4");

        w1.nextWaipoint = w2;
        w2.nextWaipoint = w3;
        w3.nextWaipoint = w4;
        w4.nextWaipoint = w1; // loop

        waypoints.AddRange(new[] { w1, w2, w3, w4 });
    }

    private void CreateCircle() {
       Vector3 center = transform.position;
        float radius = distance;
        int count = Mathf.Max(3, pointCount);
        PlatformWaypoint first = null;
        PlatformWaypoint prev = null;

        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2f / count;
            Vector3 pos = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            var wp = CreateWaypoint(pos, $"C{i}");
            if (prev != null) prev.nextWaipoint = wp;
            else first = wp;
            prev = wp;
            waypoints.Add(wp);
        }

        // close the circle
        if (prev && first) prev.nextWaipoint = first;
    }

    private PlatformWaypoint CreateWaypoint(Vector3 position, string name) {
        GameObject wpObj = new GameObject(name);
        wpObj.transform.position = position;
        PlatformWaypoint wp = wpObj.AddComponent<PlatformWaypoint>();
        return wp;
    }
}