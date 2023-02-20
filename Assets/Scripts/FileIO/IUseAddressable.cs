using UnityEngine.ResourceManagement.AsyncOperations;

public interface IUseAddressable
{
    AsyncOperationHandle Handle { get; set; }

    public void LoadAddressable(string address);
    public void ReleaseAddressable();
}