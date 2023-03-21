using System.Collections;
using UnityEditor.Purchasing;
using UnityEngine;

public class RockBottomActive : MonoBehaviour
{
    private Transform transForm;
    public float jumpTime = 0.8f;
    private float divJumpTime;
    public float jumpHeigh = 2f;
    private void Awake()
    {
        transForm = GetComponent<Transform>();
        divJumpTime = 1 / 0.8f;
    }
    public void RockBottomJump()
    {
        StartCoroutine(CoRockBottomJump());
    }

    private IEnumerator CoRockBottomJump()
    {
        float timer = 0f;
        float radian = 0f;
        Vector3 posSave;
        while (true)
        {
            timer += Time.deltaTime;
            posSave = transForm.localPosition;
            if (timer > jumpTime)
            {
                posSave.y = 0f;
                transForm.localPosition = posSave;
                yield break;
            }
            radian = timer * divJumpTime * 2 * Mathf.PI;            
            var currHeight = (Mathf.Cos(radian) * -1 + 1f) * 0.5f;            
            posSave.y = Mathf.Lerp(0f, jumpHeigh, currHeight);
            transForm.localPosition = posSave;
            yield return null;
        }
    }
}
