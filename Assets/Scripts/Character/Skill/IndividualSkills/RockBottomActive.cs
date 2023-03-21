using System.Collections;
using UnityEngine;

public class RockBottomActive : MonoBehaviour
{
    private Transform transForm;
    public float jumpTime = 0.8f;    
    public float jumpHeigh = 2f;
    private void Awake()
    {
        transForm = GetComponent<Transform>();        
    }
    public void RockBottomJump()
    {
        StartCoroutine(CoRockBottomJump());
    }

    private IEnumerator CoRockBottomJump()
    {
        float timer = 0f;
        float radian = 0f;
        float rangeZeroToDoublePieByTimer = 2 * Mathf.PI / jumpTime;
        Vector3 destPos;
        while (true)
        {
            timer += Time.deltaTime;
            destPos = transForm.localPosition;
            if (timer > jumpTime)
            {
                destPos.y = 0f;
                transForm.localPosition = destPos;
                yield break;
            }
            radian = timer * rangeZeroToDoublePieByTimer;            
            var currHeightRatio = Mathf.Cos(radian) * -0.5f + 0.5f;
            destPos.y = Mathf.Lerp(0f, jumpHeigh, currHeightRatio);
            transForm.localPosition = destPos;
            yield return null;
        }
    }
}
