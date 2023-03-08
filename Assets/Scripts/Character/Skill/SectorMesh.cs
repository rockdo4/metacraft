using UnityEngine;

public class SectorMesh : MonoBehaviour
{
    [SerializeField]
    private float radius = 5.0f;
    [SerializeField]
    private float angle = 60.0f;
    [SerializeField]
    private int segments = 16;

    public float Angle { get { return angle; } }
    public float Radius { get { return radius; } }

    private MeshFilter meshFilter;
    private Mesh mesh;    
    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;
    }
    private void Start()
    {
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero;
        float anglePerSegment = angle / segments;

        for (int i = 1; i <= segments + 1; i++)
        {
            float currentAngle = (i - 1) * anglePerSegment - angle / 2f;
            float x = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius;
            float z = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius;
            vertices[i] = new Vector3(x, 0.0f, z);
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
