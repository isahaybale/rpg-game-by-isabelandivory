using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedInteractable : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool playerInRange;
    private bool dialogShown;

    private void Update()
    {
        if (!playerInRange) return;

        // Press E to open dialog (recommended for RPG)
        if (Input.GetKeyDown(interactKey) && !dialogShown)
        {
            dialogShown = true;

            UIManager.Instance.ShowSleepDialog(
                onConfirm: Sleep,
                onCancel: () => { dialogShown = false; }
            );
        }
    }

    private void Sleep()
    {
        // Run fade to black, do "sleep effects" during black
        UIManager.Instance.RunSleepFade(duringBlack: () =>
        {
            // Put your sleep logic here:
            // - Advance time/day
            // - Heal player
            // - Save game
            // - Move player to wake position
            // Example debug:
            Debug.Log("Player slept. Apply healing/time here.");
        });

        // Allow dialog again after sleeping
        dialogShown = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        dialogShown = false;

        // If the player walks away while dialog is open, hide it
        if (UIManager.Instance != null)
            UIManager.Instance.HideSleepDialog();
    }
}