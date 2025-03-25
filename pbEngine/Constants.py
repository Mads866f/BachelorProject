from enum import Enum

class Rules(Enum):
    Equal_shares = 1
    Greedy_Utilitarian = 2
    Additive_Utilitarian = 3
    Sequential_Phragmen = 4
    Cumulative_Support_Transfer_Voting = 5


class Ballot(Enum):
    Approval = 1
    Cardinal = 2
    Cumulative = 3
    Ordinal = 4


class Satisfaction(Enum):
    CC_Sat = 1
    Cost_Sqrt_sat = 2
    Cost_Log_sat = 3
    Additive_sat = 4
    Relative_Cost_Sat = 5
    Approx_Normaliser_Relative_Cost_Sat = 6
    Cost_Square_Sat = 7
    Additive_Cost_Square_Say = 8
    Cost_Log_Sat = 9
    Additive_Cost_Log_sat = 10
    Cardinality_Sat = 11
    Relative_Cardinality_Sat = 12
    Effort_Sat = 13
    Additive_Cardinal_Sat = 14
    Relative_Cardinal_Sat = 15
    Additive_Borda_Sat = 16



class TieBreaking(Enum):
    App_Score_Tie = 1
    Lexico_Score_Tie = 2
    Max_Cost_Tie = 3
    Min_Cost_Tie = 4
    Refuse_Tie_Breaking = 5


def number_to_Rule(number):
    try:
        return Rules(number)
    except ValueError:
        print("ERROR CONVERTING ",number, "To Rule")


def number_to_Ballot(number):
    try:
        return Ballot(number)
    except ValueError:
        print("ERROR CONVERTING ",number, "To Ballot")


def number_to_Satisfaction(number):
    try:
        return Satisfaction(number)
    except ValueError:
        print("ERROR CONVERTING ",number, "To Satisfaction")



def number_to_tiebreak(number):
    try:
        return TieBreaking(number)
    except ValueError:
        print("ERROR CONVERTING ",number, "To TieBreakin")