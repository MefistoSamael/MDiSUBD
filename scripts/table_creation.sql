CREATE TABLE Color (
    id		 SERIAL PRIMARY KEY,
    name     VARCHAR (100) NOT NULL
);

CREATE TABLE Event (
    id       SERIAL PRIMARY KEY,
    Name     VARCHAR (100) NOT NULL,
    Date     DATE          NOT NULL,
    Location VARCHAR       NOT NULL
);

CREATE TABLE Food (
    id     		SERIAL PRIMARY KEY,
    Name        VARCHAR (100)  NOT NULL,
    Photo       VARCHAR,
    description VARCHAR (1000) NOT NULL
);


CREATE TABLE Role (
    id   	SERIAL PRIMARY KEY,
    Name    VARCHAR (100) NOT NULL
);

CREATE TABLE Sweetness (
    id SERIAL PRIMARY KEY,
    name         VARCHAR (100) NOT NULL
);

CREATE TABLE MUser (
    id			   SERIAL PRIMARY KEY,
    Username       VARCHAR (100) NOT NULL
                                 UNIQUE,
    Email          VARCHAR       NOT NULL,
    Password       VARCHAR       NOT NULL,
    ProfilePicture VARCHAR,
    FirstName      VARCHAR       NOT NULL,
    LastName       VARCHAR       NOT NULL,
    role_id        INTEGER       NOT NULL,
    FOREIGN KEY (
        role_id
    )
    REFERENCES role (id) 
);

CREATE TABLE Vintage (
    id 		   SERIAL PRIMARY KEY,
    Year       SMALLINT   NOT NULL
);

CREATE TABLE Winery (
    id   		SERIAL PRIMARY KEY,
    Name        VARCHAR (100) NOT NULL,
    Phonenumber VARCHAR,
    Location    VARCHAR       NOT NULL,
    Region      VARCHAR       NOT NULL
);

CREATE TABLE WineType (
    id  SERIAL PRIMARY KEY,
    sparkling    BOOL    NOT NULL,
    color_id     INTEGER NOT NULL,
    sweetness_id INTEGER NOT NULL,
    FOREIGN KEY (
        color_id
    )
    REFERENCES Color (id),
    FOREIGN KEY (
        sweetness_id
    )
    REFERENCES sweetness (id) 
);

CREATE TABLE Wine (
    id 			SERIAL PRIMARY KEY,
    Name        VARCHAR (100),
    Photo       VARCHAR,
    Description TEXT,
    vintage_id  INTEGER       NOT NULL,
    wineType_id INTEGER       NOT NULL,
    winery_id   INTEGER       NOT NULL,
    FOREIGN KEY (
        vintage_id
    )
    REFERENCES vintage (id),
    FOREIGN KEY (
        wineType_id
    )
    REFERENCES winetype (id),
    FOREIGN KEY (
        winery_id
    )
    REFERENCES winery (id) 
);

CREATE TABLE Pairing (
    id  SERIAL PRIMARY KEY,
    Description TEXT,
    wine_id     INTEGER        NOT NULL,
	FOREIGN KEY (
        wine_id
    )
    REFERENCES Wine (id) 
);

CREATE TABLE FoodPairing (
    food_id        INTEGER NOT NULL,
    pairing_id     INTEGER NOT NULL,
    PRIMARY KEY(food_id, pairing_id),
    FOREIGN KEY (
        food_id
    )
    REFERENCES food (id),
    FOREIGN KEY (
        pairing_id
    )
    REFERENCES Pairing (id) 
);

CREATE TABLE Collection (
    id 			  SERIAL PRIMARY KEY,
    Date          DATE        NOT NULL,
    Note          TEXT,
    user_id       INTEGER     NOT NULL,
    FOREIGN KEY (
        user_id
    )
    REFERENCES MUser (id) 
);

CREATE TABLE UserOrganiserEvent (
    user_id               INTEGER NOT NULL,
    event_id              INTEGER NOT NULL,
    PRIMARY KEY(user_id, event_id),
    FOREIGN KEY (
        user_id
    )
    REFERENCES MUser (id),
    FOREIGN KEY (
        event_id
    )
    REFERENCES event (id) 
);

CREATE TABLE UserParticipatorEvent (
    user_id                  INTEGER NOT NULL,
    event_id                 INTEGER NOT NULL,
    PRIMARY KEY(user_id, event_id),
    FOREIGN KEY (
        user_id
    )
    REFERENCES MUser (id),
    FOREIGN KEY (
        event_id
    )
    REFERENCES event (id) 
);

CREATE TABLE WineCollection (
    wine_id           INTEGER NOT NULL,
    collection_id     INTEGER NOT NULL,
    PRIMARY KEY(wine_id, collection_id),
    FOREIGN KEY (
        wine_id
    )
    REFERENCES wine (id),
    FOREIGN KEY (
        collection_id
    )
    REFERENCES collection (id) 
);

CREATE TABLE WineEvent (
    event_id     INTEGER NOT NULL,
    wine_id      INTEGER NOT NULL,
    PRIMARY KEY(wine_id, event_id),
    FOREIGN KEY (
        event_id
    )
    REFERENCES event (id),
    FOREIGN KEY (
        wine_id
    )
    REFERENCES wine (id) 
);

CREATE TABLE WineRating (
    id   SERIAL PRIMARY KEY,
    Rating      INTEGER     NOT NULL,
    Description TEXT,
    user_id     INTEGER     NOT NULL,
    wine_id     INTEGER     NOT NULL,
    FOREIGN KEY (
        user_id
    )
    REFERENCES MUser (id),
    FOREIGN KEY (
        wine_id
    )
    REFERENCES wine (id) 
);

CREATE TABLE WineryUser (
    user_id       INTEGER NOT NULL,
    winery_id     INTEGER NOT NULL,
    PRIMARY KEY(user_id, winery_id),
    FOREIGN KEY (
        user_id
    )
    REFERENCES MUser (id),
    FOREIGN KEY (
        winery_id
    )
    REFERENCES Winery (id) 
);
