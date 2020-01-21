
public interface IInteractable
{
    string InteractabilityInfo { get; }

    void ShowInteractability();

    void RpcInteract();

    void Interact();
}
