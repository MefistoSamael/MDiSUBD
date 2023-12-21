Bychko Vassiliy 153505__
Virtual Wine Cellar Manager__
Chosen DMS - PostgreSQL__

To run program ScriptUsage, db must be created with queries presented in scripts/table_creation.sql'__
To populate db with data use srcipts/table_data_population.sql. But IDs in intermediate tables must be changed__
Connection configuration are located in Services/PGDBService.cs of ScriptUsage project__


This database where created for subject "data models and database management system".__ 

Program folder contains simple console application, that works with created database. Program was written with .NET 7.0__
Documentation folder consists of files, that describe my database.__

documentation__
	|__
	+--L1__
	|  |__
	|  + Entities.txt - describes entities of database__
	|  |__
	|  + estimation.txt - contains data about predicted__
	|  | and real time of completing laboratory works__
	|  |__
	|  + FunctionalRequirements.txt - describes functions__
	|  | that will have application which where created at the end of term__
	|  |__
	|  + InfologicalDBModel.svg - represents infological__
	|    scheme of database__
	|__
	|__
	|__
	+--L2__
	   |__
	   + DatalogicalDBModel.drawio.svg - represents data-__
	     logical scheme of database (but it has no intermediate tables)__


Scripts folder contains of files with different sql queries that was created during course__


	   