using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsHero", menuName = "StatsBase/StatsHero")]

public class StatsHero : StatsBase
{
    public GameObject equipment;          // 장비
    public List<int> synergys;            // 시너지 리스트. 정수 or Enum으로 가지고 있다가
                                          // 공격대에 같은 시너지 index를 가지고 있는 영웅이 있으면 시너지 발동
}