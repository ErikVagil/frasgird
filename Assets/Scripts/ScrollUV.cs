using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        Material material = meshRenderer.material;

        Vector2 offset = material.mainTextureOffset;

        offset.x += Time.deltaTime / 25f;

        // material.mainTextureOffset = offset;
    }
}
