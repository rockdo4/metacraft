using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    //Non-duplicate random number generator
    public static List<int> DistinctRandomNumbers(int totalNumber, int requiredNumber)
    {
        List<int> values = new();
        List<int> spwanIndexes = new();
        for (int i = 0; i < totalNumber; i++)
        {
            values.Add(i);
        }

        for (int i = 0; i < requiredNumber; ++i)
        {
            int a = Random.Range(0, values.Count);
            spwanIndexes.Add(values[a]);
            values.RemoveAt(a);
        }
        return spwanIndexes;
    }

    public static void CopyTransform(GameObject gameObject, Transform dest)
    {
        gameObject.transform.SetPositionAndRotation(dest.position, dest.rotation);
        gameObject.transform.localScale = dest.localScale;
    }

    public static void CopyPositionAndRotation(GameObject gameObject, Transform dest)
    {
        gameObject.transform.SetPositionAndRotation(dest.position, dest.rotation);
    }

    //�� �������� �ٱ����� ���� ���� ���� �����ϴ� ����
    public static Vector3 IntersectPointCircleCenterToOut(Vector3 center, float radius, Vector3 point)
    {
        Vector3 direction = point - center;
        Vector3 normalizedDirection = direction.normalized;
        Vector3 intersectionPoint = center + normalizedDirection * radius;
        return intersectionPoint;
    }

    //�Ϲ����� math, mathf abs �Լ����� ���� ����.
    public static float Abs(float x)
    {
        return (x >= 0) ? x : -x;
    }
    public static int Abs(int x)
    {
        return (x >= 0) ? x : -x;
    }

}