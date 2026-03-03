using UnityEngine;

public class Sphere : MonoBehaviour
{
    [Header("Line Material")]
    public Material material;
    
    [Header("Dimensions")]
    public Vector2 position;
    public float zPosition;
    public float size;
    public int segments = 16;
    
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
        
        //Horizontal Circles
        for (int i = 0; i <= segments; i++)
        {
            float phi = Mathf.PI * i / segments;
            for (int j = 0; j < segments; j++)
            {
                float theta = 2.0f * Mathf.PI * j / segments;
                float nextTheta = 2.0f * Mathf.PI * (j + 1) / segments;
                
                GL.Vertex(GetPerspectivePoint(radius, phi, theta));
                GL.Vertex(GetPerspectivePoint(radius, phi, nextTheta));
            }
        }

        // Vertical Circles
        for (int i = 0; i < segments; i++)
        {
            float theta = 2.0f * Mathf.PI * i / segments;
            for (int j = 0; j < segments; j++)
            {
                float phi = Mathf.PI * j / segments;
                float nextPhi = Mathf.PI * (j + 1) / segments;

                GL.Vertex(GetPerspectivePoint(radius, phi, theta));
                GL.Vertex(GetPerspectivePoint(radius, nextPhi, theta));
            }
        }

        GL.End();
        GL.PopMatrix();
    }
    
    private Vector2 GetPerspectivePoint(float r, float phi, float theta)
    {
        float x = r * Mathf.Sin(phi) * Mathf.Cos(theta);
        float y = r * Mathf.Cos(phi);
        float z = r * Mathf.Sin(phi) * Mathf.Sin(theta);
        
        float perspectiveFactor = PerspectiveCamera.Instance.GetPerspective(zPosition + z);
        
        return (position + new Vector2(x, y)) * perspectiveFactor;
    }
}
