namespace Backend.Utilities.pbEnums;

public enum TieBreaking : uint
{
    App_Score_Tie = 1,
    Lexico_Score_Tie = 2,
    Max_Cost_Tie = 3,
    Min_Cost_Tie = 4,
    Refuse_Tie_Breaking = 5
}