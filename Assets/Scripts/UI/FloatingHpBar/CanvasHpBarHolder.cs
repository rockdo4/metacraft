using UnityEngine;
public class CanvasHpBarHolder : MonoBehaviour
{
    public static CanvasHpBarHolder Instance { get; private set;}
    private void Awake() => Instance = this;
}
