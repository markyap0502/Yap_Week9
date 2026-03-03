using System;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = System.Numerics.Vector3;


public class Pyramid : MonoBehaviour
{
    [Header("Line Material")]
    public Material material;
    
    [Header("Dimensions")]
    public Vector2 position;
    public float zPosition;
    public float size;
    public Vector2 apex = new Vector2(0, 1);
    
    private void OnPostRender()
    {
        DrawShape();
    }

    public void DrawShape()
    {
        if (material == null)
        {
            Debug.LogError("You need to add a material");
            return;
        }
        GL.PushMatrix();

        GL.Begin(GL.LINES);
        material.SetPass(0);
        
        var frontZ = PerspectiveCamera.Instance.GetPerspective(zPosition + size * .5f);
        var backZ = PerspectiveCamera.Instance.GetPerspective(zPosition - size * .5f);
        var apexZ = PerspectiveCamera.Instance.GetPerspective(zPosition);
        
        var transformedApex = ((position + apex) * apexZ) * size;
        var baseSquare = MakeBase(position, frontZ, backZ);

        for (int i = 0; i < baseSquare.Length; i++)
        {
            GL.Vertex(baseSquare[i]);
            GL.Vertex(baseSquare[(i+1) % baseSquare.Length]);
        }

        for (int i = 0; i < baseSquare.Length; i++)
        {
            GL.Vertex(transformedApex);
            GL.Vertex(baseSquare[i]);
        }

        GL.End();
        GL.PopMatrix();
    }

    public Vector2[] MakeBase(Vector2 pos, float front, float back)
    {
        var baseVertices = new Vector2[]
        {
            new Vector2 (-1f, 0f),
            new Vector2 (1f, 0f),
            new Vector2 (1f, 0f),
            new Vector2 (-1f, 0f),
        };

        for(var i = 0; i <baseVertices.Length; i++)
        {
            if (i < 2)
            {
                baseVertices[i] = new Vector2(pos.x + baseVertices[i].x, pos.y + baseVertices[i].y) * back;
                
            }
            else
            {
                baseVertices[i] = new Vector2(pos.x + baseVertices[i].x, pos.y + baseVertices[i].y) * front;
            }
            baseVertices[i] *= size;
        }
        return  baseVertices;
    }
    
    
}
