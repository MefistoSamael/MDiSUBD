---Some huge querry---
CREATE OR REPLACE PROCEDURE FindWineryByName(IN wineryName varchar)
as $$
declare
	wineryInfo RECORD;
	wineInfo RECORD;
	vintageYear integer;
	wineTypeInfo record;
	ownerInfo record;
begin
	SELECT * INTO wineryInfo FROM winery WHERE name = wineryName;
 
 	IF FOUND THEN
		RAISE NOTICE '
						  winery name - %, 
						  location - %,
						  region - %,
						  phone number - %,',
						  wineryInfo.name, wineryInfo.location,
						  wineryInfo.region, wineryInfo.phonenumber;

		FOR ownerInfo in 
			SELECT muser.username, muser.firstname, muser.lastname, role.name as role from wineryuser 
			join muser on muser.id = user_id
			join role on muser.role_id = role.id
			WHERE winery_id = wineryInfo.id
		loop
			RAISE NOTICE '
						  winery owner username - %,
						  owner name - % %,
						  owner role - %
						  ', 
						  ownerInfo.username, ownerInfo.firstname, 
						  ownerInfo.lastname, ownerInfo.role;
		end loop;
		
		
		for wineInfo in 
			select * from wine where winery_id = wineryInfo.Id
		loop
			select year into vintageYear from vintage where vintage.Id = wineInfo.vintage_id;
			
			select color.name as color, sweetness.name as sweetness, sparkling into wineTypeInfo
				from winetype
				join color on winetype.color_id = color.id
				join sweetness on winetype.sweetness_id = sweetness.id
				where winetype.Id = wineInfo.winetype_id;
			RAISE NOTICE '
						  wine name - %,
						  wine description - %,
						  wine vintage - %,
						  wine color - %,
						  wine sweetness - %,
						  is wine sparkling? - %
						  ', 
						  wineInfo.name, wineInfo.description,
						  vintageYear, wineTypeInfo.color,
						  wineTypeInfo.sweetness, wineTypeInfo.sparkling;
		end loop;
		
		
	ELSE
		RAISE NOTICE 'winery not founded';
	END IF;
 	
end;
$$ language plpgsql;

CALL FindWineryByName('Brilliant winery');

---User selection by id---

CREATE OR REPLACE PROCEDURE getUserById(
    IN user_id INT
)
AS $$
DECLARE
    user_record record;
    role_name VARCHAR;
BEGIN
    SELECT * INTO user_record FROM MUser WHERE id = user_id;
    
    SELECT name INTO role_name FROM role WHERE id = user_record.role_id;

    
    RAISE NOTICE '
				  Role - %,
				  Username - %,
				  Email - %,
				  Password - %,
				  First name - %,
				  Last name - %', 
                  role_name, user_record.username, user_record.email, 
                  user_record.password, user_record.firstname, 
				  user_record.lastname;
END;
$$ LANGUAGE plpgsql;

CALL getUserById(8);

---User selextion by username---

CREATE OR REPLACE PROCEDURE getUserByUsername(
    IN new_username varchar
)
AS $$
DECLARE
    user_record record;
    role_name VARCHAR;
BEGIN
    SELECT * INTO user_record FROM MUser WHERE username = new_username;
    
    SELECT name INTO role_name FROM role WHERE id = user_record.role_id;

    
    RAISE NOTICE '
				  Role - %,
				  Username - %,
				  Email - %,
				  Password - %,
				  First name - %,
				  Last name - %', 
                  role_name, user_record.username, user_record.email, 
                  user_record.password, user_record.firstname, 
				  user_record.lastname;
END;
$$ LANGUAGE plpgsql;

CALL getUserByUsername('SiMple_Customer228');

---User creation---

CREATE OR REPLACE PROCEDURE createUser(
    IN new_role_id INT,
    IN new_username VARCHAR,
    IN new_email VARCHAR,
    IN new_password VARCHAR,
	IN new_firstname VARCHAR,
	IN new_lastname VARCHAR,
	IN new_profilepicture VARCHAR
)
AS $$
BEGIN
    INSERT INTO MUser (username, email, password, profilePicture, FirstName, LastName, role_id)
    VALUES (new_username, new_email, new_password, new_profilePicture, new_FirstName, new_LastName, new_role_id);
END;
$$ LANGUAGE plpgsql;

CALL createUser(8, 'a', 'ak@a.com', '123123123', 'a', 'a', null);

---User updation---

CREATE OR REPLACE PROCEDURE updateUser(
    IN user_id INT,
    IN updated_username VARCHAR,
    IN updated_email VARCHAR,
    IN updated_password VARCHAR,
	IN updated_firstname VARCHAR,
	IN updated_lastname VARCHAR
)
AS $$
BEGIN
    UPDATE muser
    SET Username = updated_username, 
        Email = updated_email, 
        password = updated_password,
		firstname = updated_firstname,
		lastname = updated_lastname
    WHERE id = user_id;
END;
$$ LANGUAGE plpgsql;

CALL updateUser(20, 'c', 'c@c.com', '123321123', 'c', 'c')

---User deletion---

CREATE OR REPLACE PROCEDURE deleteUserByUserName(
    IN del_username VARCHAR
)
AS $$
BEGIN
    DELETE FROM muser where username = del_username;
END;
$$ LANGUAGE plpgsql;

CALL deleteUserByUserName('a');




