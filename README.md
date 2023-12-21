<pre>
Bychko Vassiliy 153505
Virtual Wine Cellar Manager
Chosen DMS - PostgreSQL

To run program ScriptUsage, db must be created with queries presented in scripts/table_creation.sql'
To populate db with data use srcipts/table_data_population.sql. But IDs in intermediate tables must be changed
Connection configuration are located in Services/PGDBService.cs of ScriptUsage project


This database where created for subject "data models and database management system". 

Program folder contains simple console application, that works with created database. Program was written with .NET 7.0
Documentation folder consists of files, that describe my database.

documentation
	|
	+--L1
	|  |
	|  + Entities.txt - describes entities of database
	|  |
	|  + estimation.txt - contains data about predicted
	|  | and real time of completing laboratory works
	|  |
	|  + FunctionalRequirements.txt - describes functions
	|  | that will have application which where created at the end of term
	|  |
	|  + InfologicalDBModel.svg - represents infological
	|    scheme of database
	|
	|
	|
	+--L2
	   |
	   + DatalogicalDBModel.drawio.svg - represents data-
	     logical scheme of database (but it has no intermediate tables)


Scripts folder contains of files with different sql queries that was created during course
</pre>

	   