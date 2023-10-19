CREATE TABLE Collection (
    collection_id INTEGER     NOT NULL
                              PRIMARY KEY,
    Date          DATE        NOT NULL,
    Note          TEXT (1000),
    user_id       INTEGER     NOT NULL,
    FOREIGN KEY (
        user_id
    )
    REFERENCES user (user_id) 
);

CREATE TABLE Color (
    color_id INTEGER       PRIMARY KEY,
    name     VARCHAR (100) NOT NULL
);

CREATE TABLE Event (
    event_id INTEGER       NOT NULL
                           PRIMARY KEY,
    Name     VARCHAR (100) NOT NULL,
    Date     DATE          NOT NULL,
    Location VARCHAR       NOT NULL
);

CREATE TABLE Food (
    food_id     INTEGER        NOT NULL
                               PRIMARY KEY,
    Name        VARCHAR (100)  NOT NULL,
    Photo       VARCHAR,
    description VARCHAR (1000) NOT NULL
);

CREATE TABLE FoodPairing (
    food_id        INTEGER NOT NULL,
    pairing_id     INTEGER NOT NULL,
    PRIMARY KEY(food_id, pairing_id),
    FOREIGN KEY (
        food_id
    )
    REFERENCES food (food_id),
    FOREIGN KEY (
        pairing_id
    )
    REFERENCES Pairing (pairing_id) 
) WITHOUT ROWID;

CREATE TABLE Pairing (
    pairing_id  INTEGER        NOT NULL
                               PRIMARY KEY,
    Description TEXT (1000),
    wine_id     INTEGER        NOT NULL
                               REFERENCES Wine (wine_id) 
);

CREATE TABLE Role (
    role_id INTEGER       NOT NULL
                          PRIMARY KEY,
    Name    VARCHAR (100) NOT NULL
);

CREATE TABLE Sweetness (
    sweetness_id INTEGER       PRIMARY KEY,
    name         VARCHAR (100) NOT NULL
);

CREATE TABLE User (
    user_id        INTEGER       NOT NULL
                                 PRIMARY KEY,
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
    REFERENCES role (role_id) 
);

CREATE TABLE UserOrganiserEvent (
    user_id               INTEGER NOT NULL,
    event_id              INTEGER NOT NULL,
    primary key(user_id, event_id),
    FOREIGN KEY (
        user_id
    )
    REFERENCES user (user_id),
    FOREIGN KEY (
        event_id
    )
    REFERENCES event (event_id) 
) without rowid;

CREATE TABLE UserParticipatorEvent (
    user_id                  INTEGER NOT NULL,
    event_id                 INTEGER NOT NULL,
    primary key(user_id, event_id),
    FOREIGN KEY (
        user_id
    )
    REFERENCES user (user_id),
    FOREIGN KEY (
        event_id
    )
    REFERENCES event (event_id) 
)without rowid;

CREATE TABLE Vintage (
    vintage_id INTEGER    NOT NULL
                          PRIMARY KEY,
    Year       NUMBER (4) NOT NULL
);

CREATE TABLE Wine (
    wine_id     INTEGER       NOT NULL
                              PRIMARY KEY,
    Name        VARCHAR (100),
    Photo       VARCHAR,
    Description TEXT (1000),
    vintage_id  INTEGER       NOT NULL,
    wineType_id INTEGER       NOT NULL,
    winery_id   INTEGER       NOT NULL,
    FOREIGN KEY (
        vintage_id
    )
    REFERENCES vintage (vintage_id),
    FOREIGN KEY (
        wineType_id
    )
    REFERENCES winetype (wineType_id),
    FOREIGN KEY (
        winery_id
    )
    REFERENCES winery (winery_id) 
);

CREATE TABLE WineCollection (
    wine_id           INTEGER NOT NULL,
    collection_id     INTEGER NOT NULL,
    primary key(wine_id, collection_id),
    FOREIGN KEY (
        wine_id
    )
    REFERENCES wine (wine_id),
    FOREIGN KEY (
        collection_id
    )
    REFERENCES collection (collection_id) 
)without rowid;

CREATE TABLE WineEvent (
    event_id     INTEGER NOT NULL,
    wine_id      INTEGER NOT NULL,
    primary key(wine_id, event_id),
    FOREIGN KEY (
        event_id
    )
    REFERENCES event (event_id),
    FOREIGN KEY (
        wine_id
    )
    REFERENCES wine (wine_id) 
)without rowid;

CREATE TABLE WineRating (
    rating_id   INTEGER     NOT NULL
                            PRIMARY KEY,
    Rating      INTEGER     NOT NULL,
    Description TEXT (1000),
    user_id     INTEGER     NOT NULL,
    wine_id     INTEGER     NOT NULL,
    FOREIGN KEY (
        user_id
    )
    REFERENCES user (user_id),
    FOREIGN KEY (
        wine_id
    )
    REFERENCES wine (wine_id) 
);

CREATE TABLE Winery (
    winery_id   INTEGER       NOT NULL
                              PRIMARY KEY,
    Name        VARCHAR (100) NOT NULL,
    PhoneNumber VARCHAR,
    Location    VARCHAR       NOT NULL,
    Region      VARCHAR       NOT NULL
);

CREATE TABLE WineryUser (
    user_id       INTEGER NOT NULL,
    winery_id     INTEGER NOT NULL,
    primary key(user_id, winery_id),
    FOREIGN KEY (
        user_id
    )
    REFERENCES user (user_id),
    FOREIGN KEY (
        winery_id
    )
    REFERENCES Winery (winery_id) 
)without rowid;

CREATE TABLE WineType (
    wineType_id  INTEGER NOT NULL
                         PRIMARY KEY,
    sparkling    BOOL    NOT NULL,
    color_id     INTEGER NOT NULL,
    sweetness_id INTEGER NOT NULL,
    FOREIGN KEY (
        color_id
    )
    REFERENCES Color (color_id),
    FOREIGN KEY (
        sweetness_id
    )
    REFERENCES sweetness (sweetness_id) 
);