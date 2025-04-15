from pydantic import BaseModel
from typing import Optional

###
#  Note Names is with CamelCase and not snake case due to communication over http
####

class Project(BaseModel):
    name: str
    cost: int
    categories: list[str]
    target: list[str]


class Voter(BaseModel):
    selectedProjects: list[str]
    selectedDegree: list[int]

class Election(BaseModel):
    totalBudget: int
    method: Optional[str] = None
    ballot_type: Optional[str] = None
    name: Optional[str] = None
    projects: list[Project]
    votes: list[Voter]

