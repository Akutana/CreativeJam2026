using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string interactionText;
    [SerializeField] private string[] interactionMessage;

    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private KeyCode nextMessageKey = KeyCode.Space;

    public KeyCode GetInteractionKey()
    {
        return interactionKey;
    }

    public KeyCode GetNextMessageKey()
    {
        return nextMessageKey;
    }

    public string GetInteractionText()
    {
        return interactionText;
    }

    public string[] GetInteractionMessage()
    {
        return interactionMessage;
    }
}
