from fastapi import FastAPI
import pabutools_functions as pb
import pabutools.election as pbelec
from Models import Election, Voter, Project

app = FastAPI()




@app.get("/")
async def root():
    return {"message": "Pbengine says Hello :-)"}



@app.post("/getResult/")
async def root(election:Election,method:str, ballot_type:str):
    try:
        print(f"Received election data: {election}")
        print(f"Method: {method}, Ballot Type: {ballot_type}")
        return pb.calculate_result(election,method,ballot_type)
    except Exception as e:
        print(e)
        return{"error":str(e)}