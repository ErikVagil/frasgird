using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();

        Material mat = mr.material;

        Vector2 offset = mat.mainTextureOffset;

        offset.x += Time.deltaTime / 25f;

        mat.mainTextureOffset = offset;
    }
}
