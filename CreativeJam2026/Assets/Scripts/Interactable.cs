using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string interactionText;
    [SerializeField] private string[] interactionMessage;

    [SerializeField] private KeyCode interactionKey;

    public KeyCode GetInteractionKey()
    {
        return interactionKey;
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
