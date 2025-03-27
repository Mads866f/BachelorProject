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
    Additive_Cost_Square_Say = 7
    Cost_Log_Sat = 8
    Additive_Cost_Log_sat = 9
    Cardinality_Sat = 10
    Relative_Cardinality_Sat = 11
    Effort_Sat = 12
    Additive_Cardinal_Sat = 13
    Relative_Cardinal_Sat = 14
    Additive_Borda_Sat = 15



class TieBreaking(Enum):
    App_Score_Tie = 1
    Lexico_Score_Tie = 2
    Max_Cost_Tie = 3
    Min_Cost_Tie = 4
    Refuse_Tie_Breaking = 5

class InstanceAnalysis(Enum):
    Avg_project_cost = 1
    Funding_scarcity = 2
    median_project_cost = 3
    Std_dev_project_cost = 4
    sum_project_cost = 5

class InstanceBallotAnalysis(Enum):
    Avg_ballot_cost = 1
    Avg_ballot_length = 2
    Avg_total_score = 3
    Median_approval_score = 4
    Median_ballot_cost = 5
    Median_ballot_length = 6
    Median_total_cost = 7



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


def number_to_InstanceAnalysis(number):
    try:
        return InstanceAnalysis(number)
    except ValueError:
        print("ERROR CONVERTING ",number, "To InstanceAnalysis")


def number_to_InstanceBallotAnalysis(number):
    try:
        return InstanceBallotAnalysis(number)
    except ValueError:
        print("ERROR CONVERTING ",number, "To InstanceBallotAnalysis")