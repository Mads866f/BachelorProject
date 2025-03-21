from fastapi import FastAPI
import pabutools_functions as pb
import pabutools.election as pbelec

app = FastAPI()




@app.get("/")
async def root():
    return {"message": "Hello World"}

@app.get("/test")
async def root():
    instance, profile, outcome = pb.initialTest()
    result = {"instance":instance,"profile":profile,"outcome":outcome}
    return result



@app.post("/getResult/")
async def root(election:pb.Election,method:str, ballot_type:str):
    return pb.calculate_result(election,method,ballot_type)