using UnityEngine;

public class CustomerRandomiser: MonoBehaviour
{
    private Color newColor;
    public Color colorPicker;
    public float randomizer = 0.1f;

    private void Awake()
    {
        float randomColor = Random.Range(-randomizer, randomizer);
        newColor.r = colorPicker.r + randomColor;
        newColor.g = colorPicker.g + randomColor;
        newColor.b = colorPicker.b + randomColor;

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Color[] colors = new Color[vertices.Length];
        int i = 0;
        while (i < vertices.Length)
        {
            colors[i] *= newColor;
            i++;
        }
        mesh.colors = colors;
    }
}
