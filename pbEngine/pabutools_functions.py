import collections as cl
import pabutools.election as pbelec
import pabutools.rules as pbrule
import pabutools.analysis as pban
from typing import List, Tuple
from Models import Election, Voter, Project
import random
from Constants import Rules,Ballot,TieBreaking,Satisfaction,InstanceAnalysis,InstanceBallotAnalysis, number_to_Ballot,number_to_Rule, number_to_Satisfaction, number_to_tiebreak,number_to_InstanceAnalysis,number_to_InstanceBallotAnalysis



def initialTest():
    ##
    # Simple Function for testing fastapi and pabutools connection
    # returns instance, profile, outcome
    ##

    # Creation of projects
    p1 = pbelec.Project("Road",10)
    p2 = pbelec.Project("Clean Street",4)
    p3 = pbelec.Project("Public Bathroom",6)
    p4 = pbelec.Project("Fang Munken",1)

    # Creation of the voting instance
    voting_instance = pbelec.Instance(budget_limit=14)
    voting_instance.add(p1)
    voting_instance.update([p2,p3,p4])

    # Ballots, Each simulating a voter
    b1 = pbelec.ApprovalBallot([p2,p3])
    b2 = pbelec.ApprovalBallot([p3,p2])
    b3 = pbelec.ApprovalBallot([p1])
    b4 = pbelec.ApprovalBallot([p2,p4])

    # Creating the profile
    approval_profile = pbelec.ApprovalProfile([b1,b2,b3,b4])

    outcome_greedy = pbrule.greedy_utilitarian_welfare(voting_instance,approval_profile,sat_class = pbelec.Cost_Sat )


    return voting_instance,approval_profile, outcome_greedy

def instance_to_array_of_projects(instance):
    result = []
    for i in instance:
        result.append(i)
    return result

def random_t_approval_ballot(projects,t):
    chosen_projects = []
    for i in range(0,t):
        choice = random.randint(0,len(projects)-1)
        chosen_projects.append(projects[choice])
    b1 = pbelec.ApprovalBallot(init=chosen_projects)
    return b1

def random_project(min_cost,max_cost,name=""):
    name = name
    cost = random.randint(min_cost,max_cost)
    if name == "":
        name = "p"+str(cost)+":"+str(random.randint(0,100))
    return pbelec.Project(name,cost)


def profile_vote_counter(profile):
    project_votes = cl.defaultdict(int)
    for ballot in profile:
        for project in list(ballot):
            project_votes[project] += 1
    return project_votes
    
    
def select_method(rule:int):
    rule_dictonary = {
        Rules.Equal_shares: pbrule.method_of_equal_shares,
        Rules.Greedy_Utilitarian: pbrule.greedy_utilitarian_welfare,
        Rules.Additive_Utilitarian: pbrule.max_additive_utilitarian_welfare,
        Rules.Sequential_Phragmen:pbrule.sequential_phragmen,

    }
    try:
        return rule_dictonary[number_to_Rule(rule)]
    except  KeyError:
        print("Rule Does Not Exist.")

def determine_satisfaction(sat_number):
   sat_map = {
    1 :pbelec.CC_Sat ,
    2:pbelec.Cost_Sqrt_Sat ,
    3:pbelec.Cost_Log_Sat,
    4:pbelec.AdditiveSatisfaction,
    5:pbelec.Relative_Cost_Sat ,
    6:pbelec.Relative_Cost_Approx_Normaliser_Sat,
    7:pbelec.Additive_Cost_Sqrt_Sat ,
    8:pbelec.Cost_Log_Sat ,
    9:pbelec.Additive_Cost_Log_Sat ,
    10:pbelec.Cardinality_Sat ,
    11:pbelec.Relative_Cardinality_Sat ,
    12:pbelec.Effort_Sat ,
    13:pbelec.Additive_Cardinal_Sat ,
    14:pbelec.Relative_Cardinality_Sat ,
    15:pbelec.Additive_Borda_Sat 
   }

   return sat_map[sat_number]

def determine_instance_analysis(inst_an_number :int):
    an_map = {
        1: pban.avg_project_cost,
        2: pban.funding_scarcity,
        3: pban.median_project_cost,
        4: pban.std_dev_project_cost,
        5: pban.sum_project_cost
    }
    return an_map[inst_an_number]

def determine_instance_ballot_analysis(inst_ballot_an_number: int):
    an_map = {
        1: pban.avg_ballot_cost,
        2: pban.avg_ballot_length,
        3: pban.avg_total_score,
        4: pban.median_approval_score,
        5: pban.median_ballot_cost,
        6: pban.median_ballot_length,
    }
    return an_map[inst_ballot_an_number]


def create_ballot(ballot_type, votes_gained: Voter,election:Election):
    voters_project_models_to_projects = [
    project for project in election.projects if project.name in votes_gained.selectedProjects
    ]
    ballot_dictonary = {
        Ballot.Approval: pbelec.ApprovalBallot(map(project_model_to_project_pabu,voters_project_models_to_projects)),
        Ballot.Cardinal: None,
        Ballot.Cumulative: None,
        Ballot.Ordinal: None,
    }

    try:
        return ballot_dictonary[ballot_type]
    except KeyError:
        print("Ballot does not exist:",ballot_type)
        

def create_profile(ballot_type,ballots):
    profile_dictonary = {
        Ballot.Approval : pbelec.ApprovalProfile(ballots)
    }

    try:
        return profile_dictonary[ballot_type]
    except KeyError:
        print("Profile Does Not Exist")

def project_model_to_project_pabu(project_model: Project):
    return pbelec.Project(project_model.name,project_model.cost,project_model.categories,project_model.target)

def profile_from_voter_list(votes:list[Voter],ballot_type,election):
    ballots=[] 
    for voter in votes:
        elected = create_ballot(ballot_type,voter,election)
        ballots.append(elected)

    profile = create_profile(ballot_type,ballots)
    return profile
    

def calculate_result(election:Election,method:int,ballot_type:int):
    method_to_use = select_method(method)
    ballot_type = number_to_Ballot(ballot_type)
    voting_instance = pbelec.Instance([],election.totalBudget)
    # Create and add projects to instance
    projects = election.projects
    for project in projects:
        project_to_add = pbelec.Project(project.name,project.cost,project.categories,project.target)
        voting_instance.add(project_to_add)

    # Create and add ballots to Profile
    profile = profile_from_voter_list(election.votes,ballot_type,election)

    #Calculate Result
    outcome = method_to_use(voting_instance,profile,sat_class=pbelec.Cost_Sat)
    return outcome
    



def calculate_satisfaction(election:Election,outcome:list[Project],satisfaction):
    sat = determine_satisfaction(satisfaction)
    projects = map(project_model_to_project_pabu,election.projects)
    voting_instance = pbelec.Instance(projects,election.totalBudget)
    profile = profile_from_voter_list(election.votes,Ballot.Approval,election) #Change such that the ballot type is not hardcoded
    print("PROFILE:",profile)
    outcome = map(project_model_to_project_pabu,outcome)
    return float(pban.avg_satisfaction(voting_instance,profile,outcome,sat_class=sat))



def calculate_analyze_instance(election:Election,option:int):
    voter_instance = pbelec.Instance(map(project_model_to_project_pabu,election.projects),election.totalBudget)
    analysis_to_perform = determine_instance_analysis(option)
    return float(analysis_to_perform(voter_instance))

def calculate_analyze_instance_ballot(election:Election,option:int):
    voter_instance = pbelec.Instance(map(project_model_to_project_pabu,election.projects),election.totalBudget)
    profile = profile_from_voter_list(election.votes,Ballot.Approval,election) #FIX SO BALLOT TYPE IS NOT HARDCODED
    analysis_to_perform = determine_instance_ballot_analysis(option)
    return float(analysis_to_perform(voter_instance,profile))