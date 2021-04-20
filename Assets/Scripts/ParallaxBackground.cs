using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] Vector2 parallaxMultiplier;
    [SerializeField] float deltaMultiplier;
    [SerializeField] bool infiniteHorizontal;
    float textureUnitSizeX;

    Transform cameraTransform;
    Vector3 lastCameraPosition;
    Sprite sprite;
    Texture2D texture;

    void Start() {
        cameraTransform = Camera.main.transform; //Buscar cámara
        lastCameraPosition = cameraTransform.position;
        sprite = GetComponent<SpriteRenderer>().sprite;
        texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    void LateUpdate() {
        Vector3 deltaMovement = (cameraTransform.position - lastCameraPosition) * deltaMultiplier;
        transform.position += new Vector3(deltaMovement.x * parallaxMultiplier.x, deltaMovement.y * parallaxMultiplier.y);
        lastCameraPosition = cameraTransform.position;

        if (infiniteHorizontal) {
            if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX) {
                float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
            }
        }
    }
}
