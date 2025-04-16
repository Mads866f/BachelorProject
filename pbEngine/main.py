from fastapi import FastAPI
from fastapi.responses import FileResponse
import pabutools_functions as pb
import pabutools.election as pbelec
from Models import Election, Voter, Project
from pathlib import Path

app = FastAPI()


@app.get("/")
async def root():
    return {"message": "Pbengine says Hello :-)"}



@app.post("/getResult/")
async def root(election:Election,method:int, ballot_type:int):
    try:
        method = int(method)
        ballot_type = int(ballot_type)
        print(f"Method: {method}, Ballot Type: {ballot_type}")
        return pb.calculate_result(election,method,ballot_type)
    except Exception as e:
        print(e)
        return{"error":str(e)}

@app.post("/analyze/avgSatisfaction")
async def root(election:Election,outcome:list[Project],satisfactions:list[int]):
    try:
        result = []
        for sat in satisfactions:
            sat_number = pb.calculate_satisfaction(election,outcome,sat)
            result.append(sat_number)
        return {"result":result}

    except Exception as e:
        print(e)
        return{"error":str(e)}


@app.post("/analyze/instanceAnalysis")
async def root(election:Election,options:list[int]):
    #Options is the constants "InstanceAnalysis"
    try:
        result = []
        for option in options:
            opt = pb.calculate_analyze_instance(election,option)
            result.append(opt)
        return {"result":result}
    except Exception as e:
        print(e)
        return {"error":str(e)}

@app.post("/analyze/instanceBallotAnalysis")
async def root(election:Election,options:list[int]):
    #options is the constants "InstanceBallotAnalysis"
    try:
        result = []
        for option in options:
            opt = pb.calculate_analyze_instance_ballot(election,option)
            result.append(opt)
        return {"result":result}
    except Exception as e:
        print(e)
        return {"error":str(e)}

    return

@app.get("/realElections")
async def root(file_name:str):
    #filepath = Path(__file__).resolve().parent.parent /"real-elections"/str(file_name) # Used when fastapi is alone
    filepath = "real-elections/"+str(file_name)
    print("FILEPATH:",filepath)
    instance  = pb.real_election_converter(filepath)
    name_nice = file_name.replace("_"," ").replace(".pb","")
    instance.name = name_nice
    return instance

@app.post("/downloads")
async def root(instance:Election):
    file_name = instance.name + "_custom"
    real_file = pb.election_to_file(instance,file_name)
    return FileResponse(
        path = real_file,
        filename=file_name+"_custom.pb",
        media_type="application/octet-stream"
    )
