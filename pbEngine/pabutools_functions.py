import collections as cl
import pabutools.election as pbelec
import pabutools.rules as pbrule
from typing import List, Tuple
from Models import Election, Voter, Project
import random



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
    
    
def select_method(method_string):
    if(method_string == "equalShares"):
        return pbrule.method_of_equal_shares
    
def create_ballot(ballot_type_string, votes_gained: Voter):
    
    #1-Approval Voting. Only needs the list of the projects elected.
    # Assumes that no project have degree 0.
    if(ballot_type_string == "approval"):
        return pbelec.ApprovalBallot(votes_gained.selectedProjects)
    
def calculate_result(election:Election,method,ballot_type):
    method_to_use = select_method(method)
    voting_instance = pbelec.Instance([],election.totalBudget)
    # Create and add projects to instance
    projects = election.projects
    for project in projects:
        project_to_add = pbelec.Project(project.name,project.cost)
        voting_instance.add(project_to_add)

    # Create and add ballots to Profile
    ballots=[] 
    votes = election.votes
    for voter in votes:
        elected = create_ballot(ballot_type,voter)
        ballots.append(elected)
    
    profile = pbelec.ApprovalProfile(ballots)

    #Calculate Result
    outcome = method_to_use(voting_instance,profile,sat_class=pbelec.Cost_Sat)
    return outcome
    
    


