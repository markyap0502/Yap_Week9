using UnityEngine;

public class Capsule : MonoBehaviour
{
    [Header("Line Material")]
    public Material material;
    
    [Header("Dimensions")]
    public Vector2 position;
    public float zPosition;
    public float size;
    public int segments = 16;
    public float height = 2.0f;

    private void OnPostRender()
    {
        DrawShape();
    }
    
    public void DrawShape()
    {
        if (material == null) return;

        GL.PushMatrix();
        material.SetPass(0);
        GL.Begin(GL.LINES);

        float radius = size;
        float halfHeight = height * 0.5f;
        
        for (int i = 0; i <= segments; i++)
        {
            float phi = Mathf.PI * i / segments;
            
            float yOffset = (phi < Mathf.PI * 0.5f) ? halfHeight : -halfHeight;

            for (int j = 0; j < segments; j++)
            {
                float theta = 2.0f * Mathf.PI * j / segments;
                float nextTheta = 2.0f * Mathf.PI * (j + 1) / segments;
                
                GL.Vertex(GetPerspectivePoint(radius, phi, theta, yOffset));
                GL.Vertex(GetPerspectivePoint(radius, phi, nextTheta, yOffset));
                
                if (i < segments)
                {
                    float nextPhi = Mathf.PI * (i + 1) / segments;
                    float nextYOffset = (nextPhi < Mathf.PI * 0.5f) ? halfHeight : -halfHeight;
                    
                    GL.Vertex(GetPerspectivePoint(radius, phi, theta, yOffset));
                    GL.Vertex(GetPerspectivePoint(radius, nextPhi, theta, nextYOffset));
                }
            }
        }
        
        for (int j = 0; j < segments; j++)
        {
            float theta = 2.0f * Mathf.PI * j / segments;
            
            GL.Vertex(GetPerspectivePoint(radius, Mathf.PI * 0.5f, theta, halfHeight));
            GL.Vertex(GetPerspectivePoint(radius, Mathf.PI * 0.5f, theta, -halfHeight));
        }

        GL.End();
        GL.PopMatrix();
    }

    private Vector2 GetPerspectivePoint(float r, float phi, float theta, float yOffset)
    {
        float x = r * Mathf.Sin(phi) * Mathf.Cos(theta);
        float y = r * Mathf.Cos(phi) + yOffset;
        float z = r * Mathf.Sin(phi) * Mathf.Sin(theta);

        float perspectiveFactor = PerspectiveCamera.Instance.GetPerspective(zPosition + z);
        return (position + new Vector2(x, y)) * perspectiveFactor;
    }
}
