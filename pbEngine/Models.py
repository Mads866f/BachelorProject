from pydantic import BaseModel

###
#  Note Names is with CamelCase and not snake case due to communication over http
####

class Project(BaseModel):
    name: str
    cost: int

class Voter(BaseModel):
    selectedProjects: list[str]
    selectedDegree: list[int]

class Election(BaseModel):
    totalBudget: int
    projects: list[Project]
    votes: list[Voter]

