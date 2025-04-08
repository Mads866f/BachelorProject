# Considerations made during the project development

## Architecturally considerations

- We have decided to use a monolithic project structure. Consisting of a Backend, Frontend, PbEngine, Postgress


- Needed to change the database ID's to strings instead of UUID to better support testing with a "mock" database with sqllite