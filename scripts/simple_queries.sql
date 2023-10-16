
-- Select all wines
select wine_id, Name from wine;

-- Select all users
select * from user

-- Select distinct wine in collection
select distinct wine_id from winecollection

select name from wine where wine_id = 2

select name from color where color like '_e_'

select username from user where id between 1 and 3

select name from food where food_id in (1,2,4) and name > 'd'
