create function EventDateCheck() RETURNS TRIGGER AS $EventDateCheck$
BEGIN
    --проверка даты
    IF NEW.date <= current_date THEN
		RAISE EXCEPTION USING
		errcode = 'INDAT',
		message = 'invalid event date',
		hint='event date must be grater then current date';
	END IF;
	RETURN NEW;
END;
$EventDateCheck$ LANGUAGE plpgsql;

CREATE TRIGGER EventDateCheckTrigger BEFORE INSERT OR UPDATE ON event
	FOR EACH ROW EXECUTE PROCEDURE EventDateCheck();
	


create function VintageYearCheck() RETURNS TRIGGER AS $VintageYearCheck$
declare
BEGIN
    --проверка даты
    IF new.year > EXTRACT('Year' FROM CURRENT_DATE) THEN
		RAISE EXCEPTION USING
		errcode = 'INDAT',
		message = 'invalid vintage date',
		hint='vintage date must be grater then current date';
	END IF;
	RETURN NEW;
END;
$VintageYearCheck$ LANGUAGE plpgsql;

CREATE TRIGGER VintageYearCheckTrigger BEFORE INSERT OR UPDATE ON vintage
	FOR EACH ROW EXECUTE PROCEDURE VintageYearCheck();
	


CREATE FUNCTION CollectionDateCheck() RETURNS TRIGGER AS $CollectionDateCheck$
BEGIN
    -- Проверка даты
    IF NEW.date > CURRENT_DATE THEN
        RAISE EXCEPTION USING
        errcode = 'INDAT',
        message = 'Invalid collection date',
        hint = 'Collection date must be less than or equal to the current date';
    END IF;

    RETURN NEW;
END;
$CollectionDateCheck$ LANGUAGE plpgsql;

CREATE TRIGGER CollectionDateCheckTrigger BEFORE INSERT OR UPDATE ON collection
    FOR EACH ROW EXECUTE PROCEDURE CollectionDateCheck();



CREATE FUNCTION RatingCheck() RETURNS TRIGGER AS $RatingCheck$
BEGIN
    -- Проверка рейтинга
    IF NEW.rating < 0 OR NEW.rating > 10 THEN
        RAISE EXCEPTION USING
        errcode = 'INRAT',
        message = 'Invalid rating',
        hint = 'Rating must be between 0 and 10';
    END IF;

    RETURN NEW;
END;
$RatingCheck$ LANGUAGE plpgsql;

CREATE TRIGGER RatingCheckTrigger BEFORE INSERT OR UPDATE ON winerating
    FOR EACH ROW EXECUTE PROCEDURE RatingCheck();
