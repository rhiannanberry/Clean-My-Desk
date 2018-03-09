using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class Kuwahara : MonoBehaviour
{
    public int radius;

    private Material material;

	void Awake() {
		material = new Material(Shader.Find("Custom/Kuwahara"));
	}

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
		material.SetInt("_Radius", radius);
        Graphics.Blit(source, destination, material);
    }
}
