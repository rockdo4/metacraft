using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsHero", menuName = "StatsBase/StatsHero")]

public class StatsHero : StatsBase
{
    public GameObject equipment;          // ���
    public List<int> synergys;            // �ó��� ����Ʈ. ���� or Enum���� ������ �ִٰ�
                                          // ���ݴ뿡 ���� �ó��� index�� ������ �ִ� ������ ������ �ó��� �ߵ�
}