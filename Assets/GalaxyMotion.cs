using UnityEngine;

public class GalaxyMotion : MonoBehaviour
{

    public float rotationSpeed = 1f;
    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }
}
