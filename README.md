Bychko Vassiliy 153505<br />
Virtual Wine Cellar Manager<br />
Chosen DMS - PostgreSQL<br />

To run program ScriptUsage, db must be created with queries presented in scripts/table_creation.sql'<br />
To populate db with data use srcipts/table_data_population.sql. But IDs in intermediate tables must be changed<br />
Connection configuration are located in Services/PGDBService.cs of ScriptUsage project<br />


This database where created for subject "data models and database management system".<br /> 

Program folder contains simple console application, that works with created database. Program was written with .NET 7.0<br />
Documentation folder consists of files, that describe my database.<br />

documentation<br />
	|<br />
	+--L1<br />
	|  |<br />
	|  + Entities.txt - describes entities of database<br />
	|  |<br />
	|  + estimation.txt - contains data about predicted<br />
	|  | and real time of completing laboratory works<br />
	|  |<br />
	|  + FunctionalRequirements.txt - describes functions<br />
	|  | that will have application which where created at the end of term<br />
	|  |<br />
	|  + InfologicalDBModel.svg - represents infological<br />
	|    scheme of database<br />
	|<br />
	|<br />
	|<br />
	+--L2<br />
	   |<br />
	   + DatalogicalDBModel.drawio.svg - represents data-<br />
	     logical scheme of database (but it has no intermediate tables)<br />


Scripts folder contains of files with different sql queries that was created during course<br />


	   