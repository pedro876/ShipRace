using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualScreen : GraphicRaycaster
{
    public Camera screenCamera; // Reference to the camera responsible for rendering the virtual screen's rendertexture

    public GraphicRaycaster screenCaster; // Reference to the GraphicRaycaster of the canvas displayed on the virtual screen
    public LayerMask layerMask;
    Ray ray;

    // Called by Unity when a Raycaster should raycast because it extends BaseRaycaster.
    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
        //eventData.
        eventData.position = ToScreenCamera(eventData.position);
        //eventData.position /= targetRes;
        ray = screenCamera.ScreenPointToRay(eventData.position); // Mouse
        //Debug.Log(eventData.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            // Figure out where the pointer would be in the second camera based on texture position or RenderTexture.
            Vector3 virtualPos = new Vector3(hit.textureCoord.x, hit.textureCoord.y);
            virtualPos.x *= screenCamera.targetTexture.width;
            virtualPos.y *= screenCamera.targetTexture.height;

            eventData.position = virtualPos;

            screenCaster.Raycast(eventData, resultAppendList);
        }
    }

    private Vector2 ToScreenCamera(Vector2 eventDataPos)
    {
        eventDataPos = new Vector2(
            eventDataPos.x / Screen.width * screenCamera.targetTexture.width,
            eventDataPos.y / Screen.height * screenCamera.targetTexture.height);
        return eventDataPos;
    }


    private void OnDrawGizmos()
    {
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
    }
}