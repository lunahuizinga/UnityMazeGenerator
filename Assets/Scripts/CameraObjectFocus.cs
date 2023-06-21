using UnityEngine;
public class CameraObjectFocus : MonoBehaviour{
    
    // Offset our camera by a constant factor
    public float CameraHeightOffset = 10;
    
    /// <summary>
    /// This method will position the camera to look down so that it has the specified object in frame.
    /// </summary>
    /// <param name="focusObject">The object to put in frame.</param>
    public void FocusOnObject(GameObject focusObject){
        // Check for null
        if (focusObject == null) return;
        // We need the MeshRenderer in order to access MeshRenderer.bounds
        MeshRenderer objectRenderer = focusObject.GetComponent<MeshRenderer>();
        if (objectRenderer == null) return;
        
        // Get the camera from the current GameObject
        Camera localCamera = GetComponent<Camera>();
        // Get the MeshRender bounds these are based on the mesh itself
        Bounds objectBounds = objectRenderer.bounds;
        // Get the center position of the mesh
        Vector3 objectCenter = objectBounds.center;
        
        // Camera.fieldOfView only returns the vertical field of view be default
        float verticalFieldOfView = localCamera.fieldOfView;
        // Calculate the horizontal field of view
        float horizontalFieldOfView = GetHorizontalFOV(localCamera);
        
        // Calculate both the horizontal and vertical fitting distance
        float verticalFittingDistance = GetFittingDistance(objectBounds.extents.z, verticalFieldOfView);
        float horizontalFittingDistance = GetFittingDistance(objectBounds.extents.x, horizontalFieldOfView);
        
        // Get the distance from the camera to the object to have the object in frame of the camera
        float cameraDistance = Mathf.Max(verticalFittingDistance, horizontalFittingDistance);
        // Add our offset to more nicely frame the object
        cameraDistance += CameraHeightOffset;
        // Set the camera to the correct height
        transform.position = new Vector3(objectCenter.x, cameraDistance, objectCenter.z);
        // Make the camera centre on the maze
        transform.LookAt(objectCenter);
    }

    /// <summary>
    /// This method returns the corresponding horizontal field of view of the specified camera.
    /// </summary>
    /// <param name="camera">The Camera to get the horizontal field of view of.</param>
    /// <returns>The horizontal field of view.</returns>
    private static float GetHorizontalFOV(Camera camera){
        // We first calculate the "camera height" using trigonometry
        float cameraHeight = Mathf.Tan(camera.fieldOfView * .5f * Mathf.Deg2Rad);
        // We scale the "camera height" using the aspect ratio of the camera to get the "camera width"
        // After this we can simply get the angle of the "camera width" to get the horizontal field of view
        return Mathf.Atan(cameraHeight * camera.aspect) * 2 * Mathf.Rad2Deg;
    }
    
    /// <summary>
    /// This method will calculate the height a camera needs to be at in order to have an object in focus.
    /// </summary>
    /// <param name="halfPerpendicularSize">
    /// Half of one of the sizes perpendicular to the camera view.
    /// This is ideally used with Bounds.extents.
    /// </param>
    /// <param name="fieldOfView">The field of view of the camera.</param>
    /// <returns>The height that the camera needs to be at to have the object in frame.</returns>
    private static float GetFittingDistance(float halfPerpendicularSize, float fieldOfView){
        // This is a simple trigonometric calculation to calculate the adjacent side of "the triangle"
        return Mathf.Abs(halfPerpendicularSize) / Mathf.Tan(fieldOfView * .5f * Mathf.Deg2Rad);
    }
}