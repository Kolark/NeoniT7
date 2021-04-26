using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] Vector2 parallaxMultiplier;
    [SerializeField] float deltaMultiplier;
    [SerializeField] bool infiniteHorizontal;
    float textureUnitSizeX;

    Transform cameraTransform, player;
    Vector3 lastCameraPosition, lastPlayerPosition;
    Sprite sprite;
    Texture2D texture;

    void Start() {
        //cameraTransform = Camera.main.transform; //Buscar cámara
        //lastCameraPosition = cameraTransform.position;
        sprite = GetComponent<SpriteRenderer>().sprite;
        texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        Invoke("GetCharacter", 0.1f);
    }

    void LateUpdate() {
        if (player)
        {
            Vector3 deltaMovement = (player.position - lastPlayerPosition) * deltaMultiplier;
            transform.position += new Vector3(deltaMovement.x * parallaxMultiplier.x, deltaMovement.y * parallaxMultiplier.y);
            lastPlayerPosition = player.position;

            if (infiniteHorizontal)
            {
                if (Mathf.Abs(player.position.x - transform.position.x) >= textureUnitSizeX)
                {
                    float offsetPositionX = (player.position.x - transform.position.x) % textureUnitSizeX;
                    transform.position = new Vector3(player.position.x + offsetPositionX, transform.position.y);
                }
            } 
        }
    }

    void GetCharacter()
    {
        player = BasicCharacter.Instance.transform;
        lastPlayerPosition = player.position;
    }
}
