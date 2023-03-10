public enum MapEventEnum
{
    None = -3,
    Normal = -2,        // 아무 이벤트 X, 맵 출력용
    Defense = -1,       // 아무 이벤트 X, 맵 출력용
    CivilianRescue,     // 시민구조
    NewbieHeroRescue,   // 히어로 구조
    Roadblock,          // 길막
    FloodRescue,        // 침수 구조
    BombTrap,           // 폭탄 함정
    BlockDoor,          // 차단문
    VillainsSafe,       // 빌런의 금고
    LostChild,          // 길 잃은 아이
    MagicJean,          // 마법진
    OperationBoard,     // 작전보드
    Count,
}
