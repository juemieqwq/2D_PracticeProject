using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;


[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class InteractionButton : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer image;
    private TextMeshProUGUI text;
    public PlayerController playerController;
    public Player player { get; private set; }
    private bool isPressed = false;

    void Start()
    {
        player = transform.parent.parent.GetComponentInChildren<Player>();
        playerController = transform.parent.GetComponentInChildren<PlayerController>();
        if (playerController == null)
            playerController = transform.parent.parent.GetComponentInChildren<PlayerController>();
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        image = GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = "";
        image.enabled = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isPressed)
            return;
        if (collision.tag == "CanInteraction" && image.enabled == false)
        {
            image.enabled = true;
            var tragetText = playerController.inputActions.PlayingGame.Interaction.GetBindingDisplayString();
            tragetText = tragetText.Replace("Tap or Long Tap ", "");
            text.text = tragetText;
        }
        if (playerController.interaction.isPressed)
        {

            image.enabled = false;
            text.text = "";
            isPressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag == "CanInteraction")
        {
            image.enabled = false;
            text.text = "";
            isPressed = false;
        }
    }

}
