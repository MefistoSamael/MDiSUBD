using Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace ScriptUsage.Services
{
    public class ConsoleInput
    {
        private User? user;

        private PGDBService db = new PGDBService();

        public void EnsureUserLoggedIn()
        {
            while (user == null)
            {
                Console.WriteLine("Please register or log in");
                Console.WriteLine("1 - Register\n2 - Log in\n");
                var answer = Console.ReadLine();
                switch (answer)
                {
                    case "1":
                        Register();
                        Log("registration");
                        return;
                    case "2":
                        Authorize();
                        Log("login");
                        return;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
        }

        public void MainMenu()
        {
            EnsureUserLoggedIn();

            if (user.RoleId == 6)
                UserMenu();
            else if (user.RoleId == 7)
                SommelierMenu();
            else if (user.RoleId == 8)
                WineryOwnerMenu();
            else if (user.RoleId == 9)
                OrganizerMenu();
            else if (user.RoleId == 10)
                AdminMenu();


            Console.Out.WriteLineAsync();
        }

        private void SommelierMenu()
        {
            Console.Clear();

            // получение роли 
            var role = db.GetRoles().Result.Where(r => r.Id == user.RoleId).First().Name;

            Console.WriteLine($"First Name: {user.FirstName}");
            Console.WriteLine($"Last name: {user.LastName}");
            Console.WriteLine($"email: {user.Email}");
            Console.WriteLine($"role: {role}");
            Console.WriteLine("========================================");
            Console.WriteLine();
            int input;
            do
            {
                Console.WriteLine("1. End program");
                Console.WriteLine("2. Change user");
                Console.WriteLine("3. Collections");
                Console.WriteLine("4. My wine rating");
                Console.WriteLine("5. Food");
                Console.WriteLine("6. Pairing");
                Console.WriteLine("7. My Events");

                input = CheckInput(7);

                switch (input)
                {
                    case 2:
                        GetUserParameters(out string username, out string password, out string firstName, out string lastName, out string email, out int roleId);
                        db.UpdateUser(user.Id, username, password, email, user.ProfilePicture, firstName, lastName, roleId).Wait();
                        Log("user change");
                        break;

                    case 3:
                        CollectionMenu();
                        break;
                    case 4:
                        WineRatingMenu();
                        break;
                    case 5:
                        FoodMenu();
                        break;
                    case 6:
                        PairingMenu();
                        break;
                    case 7:
                        UserEventsMenu();
                        break;
                }
                Console.Clear();
            } while (input != 1);

        }

        void UserMenu()
        {
            Console.Clear();

            // получение роли 
            var role = db.GetRoles().Result.Where(r => r.Id == user.RoleId).First().Name;

            Console.WriteLine($"First Name: {user.FirstName}");
            Console.WriteLine($"Last name: {user.LastName}");
            Console.WriteLine($"email: {user.Email}");
            Console.WriteLine($"role: {role}");
            Console.WriteLine("========================================");
            Console.WriteLine();
            int input;
            do
            {
                Console.WriteLine("1. End program");
                Console.WriteLine("2. Change user");
                Console.WriteLine("3. Collections");
                Console.WriteLine("4. My wine rating");
                Console.WriteLine("5. Events");
                Console.WriteLine();

                input = CheckInput(5);

                switch (input)
                {
                    case 2:
                        GetUserParameters(out string username, out string password, out string firstName, out string lastName, out string email, out int roleId);
                        db.UpdateUser(user.Id, username, password, email, user.ProfilePicture, firstName, lastName, roleId).Wait();
                        Log("user change");
                        break;

                    case 3:
                        CollectionMenu();
                        break;
                    case 4:
                        WineRatingMenu();
                        break;
                    case 5:
                        UserEventsMenu();
                        break;
                }
                Console.Clear();
            } while (input != 1);
        }

        private void UserEventsMenu()
        {
            Console.Clear();

            // получение роли 
            var role = db.GetRoles().Result.Where(r => r.Id == user.RoleId).First().Name;

            int input;
            do
            {
                Console.WriteLine($"My events:");
                WriteEventsForUser(user.Id);
                Console.WriteLine("========================================");
                Console.WriteLine();

                Console.WriteLine("1. Back");
                Console.WriteLine("2. Apply to new event");

                Console.WriteLine();

                input = CheckInput(2);

                switch (input)
                {
                    case 2:
                        WriteAllEvents();
                        Console.WriteLine();
                        Console.WriteLine("Enter event id");
                        var id = GetId();
                        db.AddUserParticipatorEvent(user.Id, id).Wait();
                        Log("apply to new event");
                        break;
                }
                Console.Clear();
            } while (input != 1);
        }

        private void WriteEventsForUser(int id)
        {
            var events = db.GetEventsForParticipator(id).Result;

            if (events.Any())
            {
                foreach (var ev in events)
                {
                    Console.WriteLine($"ID: {ev.Id}, Name: {ev.Name}, Date: {ev.Date}, Location: {ev.Location}");
                }
            }
            else
            {
                Console.WriteLine("No events found.");
            }
        }

        private void WineryOwnerMenu()
        {
            Console.Clear();

            // получение роли 
            var role = db.GetRoles().Result.Where(r => r.Id == user.RoleId).First().Name;

            Console.WriteLine($"First Name: {user.FirstName}");
            Console.WriteLine($"Last name: {user.LastName}");
            Console.WriteLine($"email: {user.Email}");
            Console.WriteLine($"role: {role}");
            Console.WriteLine("========================================");
            Console.WriteLine();
            int input;
            do
            {
                Console.WriteLine("1. End program");
                Console.WriteLine("2. Change user");
                Console.WriteLine("3. Collections");
                Console.WriteLine("4. My wine rating");
                Console.WriteLine("5. My wine wineries");
                Console.WriteLine("6. My events");
                Console.WriteLine();

                input = CheckInput(6);

                switch (input)
                {
                    case 2:
                        GetUserParameters(out string username, out string password, out string firstName, out string lastName, out string email, out int roleId);
                        db.UpdateUser(user.Id, username, password, email, user.ProfilePicture, firstName, lastName, roleId).Wait();
                        Log("user change");
                        break;

                    case 3:
                        CollectionMenu();
                        break;
                    case 4:
                        WineRatingMenu();
                        break;
                    case 5:
                        WineryMenu();
                        break;
                    case 6:
                        UserEventsMenu();
                        break;
                }
                Console.Clear();
            } while (input != 1);
        }

        private void OrganizerMenu()
        {
            Console.Clear();

            // получение роли 
            var role = db.GetRoles().Result.Where(r => r.Id == user.RoleId).First().Name;

            Console.WriteLine($"First Name: {user.FirstName}");
            Console.WriteLine($"Last name: {user.LastName}");
            Console.WriteLine($"email: {user.Email}");
            Console.WriteLine($"role: {role}");
            Console.WriteLine("========================================");
            Console.WriteLine();
            int input;
            do
            {
                Console.WriteLine("1. End program");
                Console.WriteLine("2. Change user");
                Console.WriteLine("3. Collections");
                Console.WriteLine("4. My wine rating");
                Console.WriteLine("5. Organizing events");
                Console.WriteLine("6. Participating events");
                Console.WriteLine();

                input = CheckInput(6);

                switch (input)
                {
                    case 2:
                        GetUserParameters(out string username, out string password, out string firstName, out string lastName, out string email, out int roleId);
                        db.UpdateUser(user.Id, username, password, email, user.ProfilePicture, firstName, lastName, roleId).Wait();
                        Log("user change");
                        break;

                    case 3:
                        CollectionMenu();
                        break;
                    case 4:
                        WineRatingMenu();
                        break;
                    case 5:
                        EventMenu();
                        break;
                    case 6:
                        UserEventsMenu();
                        break;
                }
                Console.Clear();
            } while (input != 1);
        }

        private void AdminMenu()
        {
            Console.Clear();

            // получение роли 
            var role = db.GetRoles().Result.Where(r => r.Id == user.RoleId).First().Name;

            Console.WriteLine($"First Name: {user.FirstName}");
            Console.WriteLine($"Last name: {user.LastName}");
            Console.WriteLine($"email: {user.Email}");
            Console.WriteLine($"role: {role}");
            Console.WriteLine("========================================");
            Console.WriteLine();
            int input;
            do
            {
                Console.WriteLine("1. End program");
                Console.WriteLine("2. Users");
                Console.WriteLine("3. Collections");
                Console.WriteLine("4. Wine ratings");
                Console.WriteLine("5. Events");
                Console.WriteLine("6. Food");
                Console.WriteLine("7. Pairing");
                Console.WriteLine("8. Wine type");
                Console.WriteLine("9. Color");
                Console.WriteLine("10. Sweetness");
                Console.WriteLine("11. Wine");
                Console.WriteLine("12. Vintages");
                Console.WriteLine("13. Wineries");
                Console.WriteLine("14. Roles");
                Console.WriteLine();

                input = CheckInput(14);

                switch (input)
                {
                    case 2:
                        AdminUserMenu();
                        break;

                    case 3:
                        AdminCollectionMenu();
                        break;

                    case 4:
                        AdminWineRatingMenu();
                        break;

                    case 5:
                        AdminEventMenu();
                        break;

                    case 6:
                        AdminFoodMenu();
                        break;

                    case 7:
                        AdminPairingMenu();
                        break;

                    case 8:
                        AdminWineTypeMenu();
                        break;

                    case 9:
                        AdminColorMenu();
                        break;

                    case 10:
                        AdminSweetnessMenu();
                        break;

                    case 11:
                        AdminWineMenu();
                        break;

                    case 12:
                        AdminVintageMenu();
                        break;

                    case 13:
                        AdminWineryMenu();
                        break;

                    case 14:
                        AdminRoleMenu();
                        break;

                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }

                Console.Clear();
            } while (input != 1);
        }

        void AdminEventMenu()
        {
            Console.Clear();
            int select;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Back");
                Console.WriteLine("2. All Events");
                Console.WriteLine("3. View Event Details");
                Console.WriteLine("4. Create Event");
                Console.WriteLine("5. Delete Event");

                select = CheckInput(5);

                switch (select)
                {
                    case 2:
                        WriteAllEvents();
                        Log("events check");
                        break;
                    case 3:
                        Console.WriteLine("Enter event id");
                        var eventId = GetId();
                        var selectedEvent = db.GetEvents().Result.FirstOrDefault(e => e.Id == eventId);
                        if (selectedEvent is null)
                            Console.WriteLine("Invalid event id");
                        else
                        {
                            Console.WriteLine($"Name: {selectedEvent.Name}");
                            Console.WriteLine($"Date: {selectedEvent.Date}");
                            Console.WriteLine($"Location: {selectedEvent.Location}");

                            Console.WriteLine("\nParticipants:");
                            WriteParticipantsForEvent(selectedEvent.Id);

                            Console.WriteLine("\nOrganisers:");
                            WriteOrganisersForEvent(selectedEvent.Id);

                            Console.WriteLine("\nWines:");
                            WriteWinesForEvent(selectedEvent.Id);
                        }
                        Log("event check");
                        break;
                    case 4:
                        GetEventParameters(out string eventName, out DateTime eventDate, out string eventLocation);
                        db.CreateEvent(eventName, eventDate, eventLocation).Wait();
                        
                        var createdEvent = db.GetEvents().Result.Where(e => e.Name == eventName && e.Location == eventLocation && e.Date == eventDate).FirstOrDefault();
                        Console.WriteLine($"Event {createdEvent.Id} created successfully.");
                        
                        WriteAllUsers();

                        Console.WriteLine("Add participants to the event (comma-separated user ids): ");
                        var participants = Console.ReadLine()?.Split(',').Select(int.Parse).ToList();
                        if (participants != null)
                        {
                            foreach (var participantId in participants)
                            {
                                db.AddUserParticipatorEvent(participantId, createdEvent.Id).Wait();
                            }
                        }

                        
                        Console.WriteLine("Add organisers to the event (comma-separated user ids): ");
                        var organisers = Console.ReadLine()?.Split(',').Select(int.Parse).ToList();
                        if (organisers != null)
                        {
                            foreach (var organiserId in organisers)
                            {
                                db.AddUserOrganiserEvent(organiserId, createdEvent.Id).Wait();
                            }
                        }

                        WriteAllWines();

                        Console.WriteLine("Add wines to the event (comma-separated wine ids): ");
                        var wines = Console.ReadLine()?.Split(',').Select(int.Parse).ToList();
                        if (wines != null)
                        {
                            foreach (var wineId in wines)
                            {
                                db.AddWineToEvent(createdEvent.Id, wineId).Wait();
                            }
                        }
                        Log("events creation");
                        break;
                    case 5:
                        Console.WriteLine("Enter event id");
                        int eventToDeleteId = GetId();
                        db.DeleteEvent(eventToDeleteId).Wait();
                        Log("events deleteion");
                        break;
                }
                Console.WriteLine("\n==========================================================================\n");
            } while (select != 1);
        }

        void GetEventParameters(out string name, out DateTime date, out string location)
        {
            Console.WriteLine("Enter event name: ");
            name = Console.ReadLine();

            Console.WriteLine("Enter event date (yyyy-MM-dd): ");
            while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                Console.WriteLine("Invalid date format. Please use yyyy-MM-dd.");
                Console.WriteLine("Enter event date (yyyy-MM-dd): ");
            }

            Console.WriteLine("Enter event location: ");
            location = Console.ReadLine();
        }


        void AdminFoodMenu()
        {
            Console.Clear();
            int select;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Back");
                Console.WriteLine("2. All Foods");
                Console.WriteLine("3. View Food Details");
                Console.WriteLine("4. Create Food");
                Console.WriteLine("5. Delete Food");
                Console.WriteLine("6. Update Food");

                select = CheckInput(6);

                switch (select)
                {
                    case 2:
                        WriteAllFoods();
                        Log("foods check");
                        break;
                    case 3:
                        Console.WriteLine("Enter food id");
                        var foodId = GetId();
                        var selectedFood = db.GetFoods().Result.FirstOrDefault(f => f.Id == foodId);
                        if (selectedFood is null)
                            Console.WriteLine("Invalid food id");
                        else
                        {
                            Console.WriteLine($"Name: {selectedFood.Name}");
                            Console.WriteLine($"Photo: {selectedFood.Photo}");
                            Console.WriteLine($"Description: {selectedFood.Description}");
                        }
                        Log("food check");
                        break;
                    case 4:
                        GetFoodParameters(out string foodName, out string foodPhoto, out string foodDescription);
                        db.CreateFood(foodName, foodPhoto, foodDescription).Wait();
                        Log("foods creation");
                        break;
                    case 5:
                        Console.WriteLine("Enter food id");
                        int foodIdToDelete = GetId();
                        db.DeleteFood(foodIdToDelete).Wait();
                        Log("foods deletion");
                        break;
                    case 6:
                        GetFoodParameters(out string newFoodName, out string newFoodPhoto, out string newFoodDescription);
                        Console.WriteLine("Enter food id");
                        int foodIdToUpdate = GetId();
                        db.UpdateFood(foodIdToUpdate, newFoodName, newFoodPhoto, newFoodDescription).Wait();
                        Log("foods updateion");
                        break;
                }
                Console.WriteLine("\n==========================================================================\n");
            } while (select != 1);
        }

        void WriteAllFoods()
        {
            var foods = db.GetFoods().Result;

            if (foods.Any())
            {
                foreach (var food in foods)
                {
                    Console.WriteLine($"ID: {food.Id}, Name: {food.Name}, Photo: {food.Photo}, Description: {food.Description}");
                }
            }
            else
            {
                Console.WriteLine("No foods found.");
            }
        }

        void GetFoodParameters(out string name, out string photo, out string description)
        {
            Console.WriteLine("Enter food name: ");
            name = Console.ReadLine();

            Console.WriteLine("Enter food photo (optional): ");
            photo = Console.ReadLine();

            Console.WriteLine("Enter food description: ");
            description = Console.ReadLine();
        }


        void AdminPairingMenu()
        {
            Console.Clear();
            int select;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Back");
                Console.WriteLine("2. All Pairings");
                Console.WriteLine("3. View Pairing Details");
                Console.WriteLine("4. Create Pairing");
                Console.WriteLine("5. Delete Pairing");

                select = CheckInput(5);

                switch (select)
                {
                    case 2:
                        WriteAllPairings();
                        Log("pairings check");
                        break;
                    case 3:
                        Console.WriteLine("Enter pairing id");
                        var pairingId = GetId();
                        var selectedPairing = db.GetPairings().Result.FirstOrDefault(p => p.Id == pairingId);
                        if (selectedPairing is null)
                            Console.WriteLine("Invalid pairing id");
                        else
                        {
                            Console.WriteLine($"Description: {selectedPairing.Description}");

                            Console.WriteLine("\nFoods:");
                            WriteFoodsForPairing(selectedPairing.Id);

                            var wine = db.GetWines().Result.Where(w => w.Id == selectedPairing.WineId).FirstOrDefault();
                            Console.WriteLine($"Wine: {wine.Name} (ID {wine.Id})");
                        }
                        Log("pairing check");
                        break;
                    case 4:
                        GetPairingParameters(out string pairingDescription, out int wineId);
                        db.CreatePairing(pairingDescription, wineId).Wait();

                        var createdPairing = db.GetPairings().Result.Where(p => p.Description == pairingDescription && p.WineId == wineId).FirstOrDefault();
                        Console.WriteLine($"Pairing {createdPairing.Id} created successfully.");

                        Console.WriteLine("Add foods to the pairing (comma-separated food ids): ");
                        var foods = Console.ReadLine()?.Split(',').Select(int.Parse).ToList();
                        if (foods != null)
                        {
                            foreach (var foodId in foods)
                            {
                                db.CreateFoodPairing(foodId, createdPairing.Id).Wait();
                            }
                        }

                        Log("pairings creation");
                        break;
                    case 5:
                        Console.WriteLine("Enter pairing id");
                        int pairingIdToDelete = GetId();
                        db.DeletePairing(pairingIdToDelete).Wait();
                        Log("pairings deletion");
                        break;
                }
                Console.WriteLine("\n==========================================================================\n");
            } while (select != 1);
        }

        void WriteAllPairings()
        {
            var pairings = db.GetPairings().Result;

            if (pairings.Any())
            {
                foreach (var pairing in pairings)
                {
                    Console.WriteLine($"ID: {pairing.Id}, Description: {pairing.Description}");
                }
            }
            else
            {
                Console.WriteLine("No pairings found.");
            }
        }

        void GetPairingParameters(out string description, out int wineId)
        {
            Console.WriteLine("Enter pairing description: ");
            description = Console.ReadLine();

            Console.WriteLine("Enter wine id: ");
            wineId = GetId();
        }


        void AdminWineTypeMenu()
        {
            Console.Clear();
            int select;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Back");
                Console.WriteLine("2. All Wine Types");
                Console.WriteLine("3. View Wine Type Details");
                Console.WriteLine("4. Create Wine Type");
                Console.WriteLine("5. Delete Wine Type");
                Console.WriteLine("6. Update Wine Type");

                select = CheckInput(6);

                switch (select)
                {
                    case 2:
                        WriteAllWineTypes();
                        Log("WineTypes check");
                        break;
                    case 3:
                        Console.WriteLine("Enter wine type id");
                        var wineTypeId = GetId();
                        var selectedWineType = db.GetWineTypes().Result.FirstOrDefault(wt => wt.Id == wineTypeId);
                        if (selectedWineType is null)
                            Console.WriteLine("Invalid wine type id");
                        else
                        {
                            Console.WriteLine($"Sparkling: {selectedWineType.Sparkling}");
                            Console.WriteLine($"Color: {selectedWineType.Color.Name}");
                            Console.WriteLine($"Sweetness: {selectedWineType.Sweetness.Name}");
                        }
                        Log("WineType check");
                        break;
                    case 4:
                        GetWineTypeParameters(out bool sparkling, out int colorId, out int sweetnessId);
                        db.CreateWineType(sparkling, colorId, sweetnessId).Wait();
                        Log("WineTypes creation");
                        break;
                    case 5:
                        Console.WriteLine("Enter wine type id");
                        int wineTypeIdToDelete = GetId();
                        db.DeleteWineType(wineTypeIdToDelete).Wait();
                        Log("WineTypes deletion");
                        break;
                    case 6:
                        GetWineTypeParameters(out bool newSparkling, out int newColorId, out int newSweetnessId);
                        Console.WriteLine("Enter wine type id");
                        int wineTypeIdToUpdate = GetId();
                        db.UpdateWineType(wineTypeIdToUpdate, newSparkling, newColorId, newSweetnessId).Wait();
                        Log("WineTypes updation");
                        break;
                }
                Console.WriteLine("\n==========================================================================\n");
            } while (select != 1);
        }

        void WriteAllWineTypes()
        {
            var wineTypes = db.GetWineTypes().Result;

            if (wineTypes.Any())
            {
                foreach (var wineType in wineTypes)
                {
                    var color = db.GetColors().Result.Where(c => c.Id == wineType.ColorId).FirstOrDefault();
                    var sweetness = db.GetSweetnesses().Result.Where(s => s.Id == wineType.SweetnessId).FirstOrDefault();
                    Console.WriteLine($"ID: {wineType.Id}, Sparkling: {wineType.Sparkling}, Color: {color.Name}, Sweetness: {sweetness.Name}");
                }
            }
            else
            {
                Console.WriteLine("No wine types found.");
            }
        }

        void GetWineTypeParameters(out bool sparkling, out int colorId, out int sweetnessId)
        {
            Console.WriteLine("Is the wine sparkling? (true/false): ");
            sparkling = Convert.ToBoolean(Console.ReadLine());

            Console.WriteLine("Enter color id: ");
            colorId = GetId();

            Console.WriteLine("Enter sweetness id: ");
            sweetnessId = GetId();
        }


        void AdminColorMenu()
        {
            Console.Clear();
            int select;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Back");
                Console.WriteLine("2. All Colors");
                Console.WriteLine("3. View Color Details");
                Console.WriteLine("4. Create Color");
                Console.WriteLine("5. Delete Color");
                Console.WriteLine("6. Update Color");

                select = CheckInput(6);

                switch (select)
                {
                    case 2:
                        WriteAllColors();
                        Log("colors check");
                        break;
                    case 3:
                        Console.WriteLine("Enter color id");
                        var colorId = GetId();
                        var selectedColor = db.GetColors().Result.FirstOrDefault(c => c.Id == colorId);
                        if (selectedColor is null)
                            Console.WriteLine("Invalid color id");
                        else
                        {
                            Console.WriteLine($"Name: {selectedColor.Name}");
                        }
                        Log("color check");
                        break;
                    case 4:
                        GetColorParameters(out string colorName);
                        db.CreateColor(colorName).Wait();
                        Log("colors creation");
                        break;
                    case 5:
                        Console.WriteLine("Enter color id");
                        int colorIdToDelete = GetId();
                        db.DeleteColor(colorIdToDelete).Wait();
                        Log("colors deletion");
                        break;
                    case 6:
                        GetColorParameters(out string newColorName);
                        Console.WriteLine("Enter color id");
                        int colorIdToUpdate = GetId();
                        db.UpdateColor(colorIdToUpdate, newColorName).Wait();
                        Log("colors updation");
                        break;
                }
                Console.WriteLine("\n==========================================================================\n");
            } while (select != 1);
        }

        void GetColorParameters(out string name)
        {
            Console.WriteLine("Enter color name: ");
            name = Console.ReadLine();
        }


        void AdminSweetnessMenu()
        {
            Console.Clear();
            int select;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Back");
                Console.WriteLine("2. All Sweetness Levels");
                Console.WriteLine("3. View Sweetness Level Details");
                Console.WriteLine("4. Create Sweetness Level");
                Console.WriteLine("5. Delete Sweetness Level");
                Console.WriteLine("6. Update Sweetness Level");

                select = CheckInput(6);

                switch (select)
                {
                    case 2:
                        WriteAllSweetnessLevels();
                        Log("sweetnesses check");
                        break;
                    case 3:
                        Console.WriteLine("Enter sweetness level id");
                        var sweetnessId = GetId();
                        var selectedSweetness = db.GetSweetnesses().Result.FirstOrDefault(s => s.Id == sweetnessId);
                        if (selectedSweetness is null)
                            Console.WriteLine("Invalid sweetness level id");
                        else
                        {
                            Console.WriteLine($"Name: {selectedSweetness.Name}");
                        }
                        Log("sweetness check");
                        break;
                    case 4:
                        GetSweetnessParameters(out string sweetnessName);
                        db.CreateSweetness(sweetnessName).Wait();
                        Log("sweetnesses creation");
                        break;
                    case 5:
                        Console.WriteLine("Enter sweetness level id");
                        int sweetnessIdToDelete = GetId();
                        db.DeleteSweetness(sweetnessIdToDelete).Wait();
                        Log("sweetnesses deletion");
                        break;
                    case 6:
                        GetSweetnessParameters(out string newSweetnessName);
                        Console.WriteLine("Enter sweetness level id");
                        int sweetnessIdToUpdate = GetId();
                        db.UpdateSweetness(sweetnessIdToUpdate, newSweetnessName).Wait();
                        Log("sweetnesses updation");
                        break;
                }
                Console.WriteLine("\n==========================================================================\n");
            } while (select != 1);
        }

        void WriteAllSweetnessLevels()
        {
            var sweetnessLevels = db.GetSweetnesses().Result;

            if (sweetnessLevels.Any())
            {
                foreach (var sweetnessLevel in sweetnessLevels)
                {
                    Console.WriteLine($"ID: {sweetnessLevel.Id}, Name: {sweetnessLevel.Name}");
                }
            }
            else
            {
                Console.WriteLine("No sweetness levels found.");
            }
        }

        void GetSweetnessParameters(out string name)
        {
            Console.WriteLine("Enter sweetness level name: ");
            name = Console.ReadLine();
        }


        void AdminWineMenu()
        {
            Console.Clear();
            int select;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Back");
                Console.WriteLine("2. All Wines");
                Console.WriteLine("3. View Wine Details");
                Console.WriteLine("4. Create Wine");
                Console.WriteLine("5. Delete Wine");
                Console.WriteLine("6. Update Wine");

                select = CheckInput(6);

                switch (select)
                {
                    case 2:
                        WriteAllWines();
                        Log("wines check");
                        break;
                    case 3:
                        Console.WriteLine("Enter wine id");
                        var wineId = GetId();
                        var selectedWine = db.GetWines().Result.FirstOrDefault(w => w.Id == wineId);
                        if (selectedWine is null)
                            Console.WriteLine("Invalid wine id");
                        else
                        {
                            var selectedVintage = db.GetVintages().Result.Where(v => v.Id == selectedWine.VintageId).FirstOrDefault();
                            var selectedWineType = db.GetWineTypes().Result.Where(w => w.Id ==  selectedWine.WineTypeId).FirstOrDefault();

                            Console.WriteLine($"Name: {selectedWine.Name}");
                            Console.WriteLine($"Photo: {selectedWine.Photo}");
                            Console.WriteLine($"Description: {selectedWine.Description}");
                            Console.WriteLine($"Vintage: {selectedVintage.Year}");
                            Console.WriteLine($"Wine Type: {selectedWineType.Color.Name} {selectedWineType.Sweetness.Name} sparkling? {selectedWineType.Sparkling}");
                            Console.WriteLine($"Winery: {selectedWine.Winery.Name}");
                        }
                        Log("wine check");
                        break;
                    case 4:
                        GetWineParameters(out string wineName, out string winePhoto, out string wineDescription, out int vintageId, out int wineTypeId, out int wineryId);
                        db.CreateWine(wineName, winePhoto, wineDescription, vintageId, wineTypeId, wineryId).Wait();
                        Log("wines creation");
                        break;
                    case 5:
                        Console.WriteLine("Enter wine id");
                        int wineIdToDelete = GetId();
                        db.DeleteWine(wineIdToDelete).Wait();
                        Log("wines deletion");
                        break;
                    case 6:
                        GetWineParameters(out string newWineName, out string newWinePhoto, out string newWineDescription, out int newVintageId, out int newWineTypeId, out int newWineryId);
                        Console.WriteLine("Enter wine id");
                        int wineIdToUpdate = GetId();
                        db.UpdateWine(wineIdToUpdate, newWineName, newWinePhoto, newWineDescription, newVintageId, newWineTypeId, newWineryId).Wait();
                        Log("wines updation");
                        break;
                }
                Console.WriteLine("\n==========================================================================\n");
            } while (select != 1);
        }

        void WriteAllWines()
        {
            var wines = db.GetWines().Result;

            if (wines.Any())
            {
                foreach (var wine in wines)
                {
                    var selectedVintage = db.GetVintages().Result.Where(v => v.Id == wine.VintageId).FirstOrDefault();
                    var selectedWineType = db.GetWineTypes().Result.Where(w => w.Id == wine.WineTypeId).FirstOrDefault();
                    var selectedWinery = db.GetWineries().Result.Where(w => w.Id == wine.WineryId).FirstOrDefault();
                    var color = db.GetColors().Result.Where(c => c.Id == selectedWineType.ColorId).FirstOrDefault();
                    var sweetness = db.GetSweetnesses().Result.Where(s => s.Id == selectedWineType.SweetnessId).FirstOrDefault();
                    Console.WriteLine($"ID: {wine.Id}, Name: {wine.Name}, Photo: {wine.Photo}, Description: {wine.Description}, Vintage: {selectedVintage.Year}, Wine Type: {color.Name} {sweetness.Name} is sparkling - {selectedWineType.Sparkling}, Winery: {selectedWinery.Name}");
                }
            }
            else
            {
                Console.WriteLine("No wines found.");
            }
        }

        void GetWineParameters(out string name, out string photo, out string description, out int vintageId, out int wineTypeId, out int wineryId)
        {
            Console.WriteLine("Enter wine name: ");
            name = Console.ReadLine();

            Console.WriteLine("Enter wine photo (optional): ");
            photo = Console.ReadLine();

            Console.WriteLine("Enter wine description: ");
            description = Console.ReadLine();

            Console.WriteLine("Enter vintage id: ");
            vintageId = GetId();

            Console.WriteLine("Enter wine type id: ");
            wineTypeId = GetId();

            Console.WriteLine("Enter winery id: ");
            wineryId = GetId();
        }


        void AdminVintageMenu()
        {
            Console.Clear();
            int select;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Back");
                Console.WriteLine("2. All Vintages");
                Console.WriteLine("3. View Vintage Details");
                Console.WriteLine("4. Create Vintage");
                Console.WriteLine("5. Delete Vintage");
                Console.WriteLine("6. Update Vintage");

                select = CheckInput(6);

                switch (select)
                {
                    case 2:
                        WriteAllVintages();
                        Log("vintsges check");
                        break;
                    case 3:
                        Console.WriteLine("Enter vintage id");
                        var vintageId = GetId();
                        var selectedVintage = db.GetVintages().Result.FirstOrDefault(v => v.Id == vintageId);
                        if (selectedVintage is null)
                            Console.WriteLine("Invalid vintage id");
                        else
                        {
                            Console.WriteLine($"Year: {selectedVintage.Year}");
                        }
                        Log("vintsge check");
                        break;
                    case 4:
                        GetVintageParameters(out short vintageYear);
                        db.CreateVintage(vintageYear).Wait();
                        Log("vintsges creation");
                        break;
                    case 5:
                        Console.WriteLine("Enter vintage id");
                        int vintageIdToDelete = GetId();
                        db.DeleteVintage(vintageIdToDelete).Wait();
                        Log("vintsges deletion");
                        break;
                    case 6:
                        GetVintageParameters(out short newVintageYear);
                        Console.WriteLine("Enter vintage id");
                        int vintageIdToUpdate = GetId();
                        db.UpdateVintage(vintageIdToUpdate, newVintageYear).Wait();
                        Log("vintsges updation");
                        break;
                }
                Console.WriteLine("\n==========================================================================\n");
            } while (select != 1);
        }

        void WriteAllVintages()
        {
            var vintages = db.GetVintages().Result;

            if (vintages.Any())
            {
                foreach (var vintage in vintages)
                {
                    Console.WriteLine($"ID: {vintage.Id}, Year: {vintage.Year}");
                }
            }
            else
            {
                Console.WriteLine("No vintages found.");
            }
        }

        void GetVintageParameters(out short year)
        {
            Console.WriteLine("Enter vintage year: ");
            year = Convert.ToInt16(Console.ReadLine());
        }


        void AdminWineryMenu()
        {
            Console.Clear();
            int select;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Back");
                Console.WriteLine("2. All Wineries");
                Console.WriteLine("3. View Winery Details");
                Console.WriteLine("4. Create Winery");
                Console.WriteLine("5. Delete Winery");
                Console.WriteLine("6. Update Winery");

                select = CheckInput(6);

                switch (select)
                {
                    case 2:
                        WriteAllWineries();
                        Log("wineries check");
                        break;
                    case 3:
                        Console.WriteLine("Enter winery id");
                        var wineryId = GetId();
                        var selectedWinery = db.GetWineries().Result.FirstOrDefault(w => w.Id == wineryId);
                        if (selectedWinery is null)
                            Console.WriteLine("Invalid winery id");
                        else
                        {
                            Console.WriteLine($"Name: {selectedWinery.Name}");
                            Console.WriteLine($"Phone Number: {selectedWinery.PhoneNumber}");
                            Console.WriteLine($"Location: {selectedWinery.Location}");
                            Console.WriteLine($"Region: {selectedWinery.Region}");
                            Console.WriteLine("\nUsers associated with this winery:");
                            WriteUsersForWinery(selectedWinery.Id);
                        }
                        Log("winery check");
                        break;
                    case 4:
                        GetWineryParameters(out string wineryName, out string phoneNumber, out string location, out string region);
                        db.CreateWinery(wineryName, phoneNumber, location, region).Wait();
                        Log("wineries creation");
                        break;
                    case 5:
                        Console.WriteLine("Enter winery id");
                        int wineryIdToDelete = GetId();
                        db.DeleteWinery(wineryIdToDelete).Wait();
                        Log("wineries deletion");
                        break;
                    case 6:
                        GetWineryParameters(out string newWineryName, out string newPhoneNumber, out string newLocation, out string newRegion);
                        Console.WriteLine("Enter winery id");
                        int wineryIdToUpdate = GetId();
                        db.UpdateWinery(wineryIdToUpdate, newWineryName, newPhoneNumber, newLocation, newRegion).Wait();
                        Log("wineries updations");
                        break;
                }
                Console.WriteLine("\n==========================================================================\n");
            } while (select != 1);
        }

        void WriteAllWineries()
        {
            var wineries = db.GetWineries().Result;

            if (wineries.Any())
            {
                foreach (var winery in wineries)
                {
                    Console.WriteLine($"ID: {winery.Id}, Name: {winery.Name}, Phone Number: {winery.PhoneNumber}, Location: {winery.Location}, Region: {winery.Region}");
                }
            }
            else
            {
                Console.WriteLine("No wineries found.");
            }
        }

        void WriteUsersForWinery(int wineryId)
        {
            var users = db.GetUsersForWinery(wineryId).Result;

            if (users.Any())
            {
                foreach (var user in users)
                {
                    Console.WriteLine($"- {user.FirstName} {user.LastName} (ID: {user.Id})");
                }
            }
            else
            {
                Console.WriteLine("No users associated with this winery.");
            }
        }

        void GetWineryParameters(out string name, out string phoneNumber, out string location, out string region)
        {
            Console.WriteLine("Enter winery name: ");
            name = Console.ReadLine();

            Console.WriteLine("Enter winery phone number (optional): ");
            phoneNumber = Console.ReadLine();

            Console.WriteLine("Enter winery location: ");
            location = Console.ReadLine();

            Console.WriteLine("Enter winery region: ");
            region = Console.ReadLine();
        }


        void AdminRoleMenu()
        {
            Console.Clear();
            int select;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Back");
                Console.WriteLine("2. All Roles");
                Console.WriteLine("3. View Role Details");
                Console.WriteLine("4. Create Role");
                Console.WriteLine("5. Delete Role");
                Console.WriteLine("6. Update Role");

                select = CheckInput(6);

                switch (select)
                {
                    case 2:
                        WriteAllRoles();
                        Log("roles check");
                        break;
                    case 3:
                        Console.WriteLine("Enter role id");
                        var roleId = GetId();
                        var selectedRole = db.GetRoles().Result.FirstOrDefault(r => r.Id == roleId);
                        if (selectedRole is null)
                            Console.WriteLine("Invalid role id");
                        else
                        {
                            Console.WriteLine($"Name: {selectedRole.Name}");
                        }
                        break;
                    case 4:
                        GetRoleParameters(out string roleName);
                        db.CreateRole(roleName).Wait();
                        Log("roles creation");
                        break;
                    case 5:
                        Console.WriteLine("Enter role id");
                        int roleIdToDelete = GetId();
                        db.DeleteRole(roleIdToDelete).Wait();
                        Log("roles deletion");
                        break;
                    case 6:
                        GetRoleParameters(out string newRoleName);
                        Console.WriteLine("Enter role id");
                        int roleIdToUpdate = GetId();
                        db.UpdateRole(roleIdToUpdate, newRoleName).Wait();
                        Log("roles updateoin");
                        break;
                }
                Console.WriteLine("\n==========================================================================\n");
            } while (select != 1);
        }

        void WriteAllRoles()
        {
            var roles = db.GetRoles().Result;

            if (roles.Any())
            {
                foreach (var role in roles)
                {
                    Console.WriteLine($"ID: {role.Id}, Name: {role.Name}");
                }
            }
            else
            {
                Console.WriteLine("No roles found.");
            }
        }

        void GetRoleParameters(out string name)
        {
            Console.WriteLine("Enter role name: ");
            name = Console.ReadLine();
        }


        void AdminWineRatingMenu()
        {
            Console.Clear();
            int select;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Back");
                Console.WriteLine("2. All Wine Ratings");
                Console.WriteLine("3. View Wine Rating Details");
                Console.WriteLine("4. Create Wine Rating");
                Console.WriteLine("5. Delete Wine Rating");
                Console.WriteLine("6. Update Wine Rating");

                select = CheckInput(6);

                switch (select)
                {
                    case 2:
                        WriteAllWineRatings();
                        Log("wine ratings check");
                        break;
                    case 3:
                        Console.WriteLine("Enter wine rating id");
                        var wineRatingId = GetId();
                        var selectedWineRating = db.GetWineRatings().Result.FirstOrDefault(wr => wr.Id == wineRatingId);
                        if (selectedWineRating is null)
                            Console.WriteLine("Invalid wine rating id");
                        else
                        {
                            var user = db.GetUsers().Result.FirstOrDefault(u => u.Id == selectedWineRating.UserId);
                            var wine = db.GetWines().Result.FirstOrDefault(w => w.Id == selectedWineRating.WineId);

                            if (user is null || wine is null)
                                Console.WriteLine("Invalid user id or wine id");
                            else
                            {
                                Console.WriteLine($"Rating: {selectedWineRating.Rating}");
                                Console.WriteLine($"Description: {selectedWineRating.Description}");
                                Console.WriteLine($"User: {user.FirstName} {user.LastName}");
                                Console.WriteLine($"Wine: {wine.Name}");
                            }
                        }
                        Log("wine rating check");
                        break;
                    case 4:
                        GetWineRatingParameters(out int rating, out string description, out int userId, out int wineId);
                        db.AddWineRating(rating, description, userId, wineId).Wait();
                        Log("wine ratings creation");
                        break;
                    case 5:
                        Console.WriteLine("Enter wine rating id");
                        int wineRatingIdToDelete = GetId();
                        db.RemoveWineRating(wineRatingIdToDelete).Wait();
                        Log("wine ratings deletion");
                        break;
                    case 6:
                        GetWineRatingParameters(out int newRating, out string newDescription, out int newUserId, out int newWineId);
                        Console.WriteLine("Enter wine rating id");
                        int wineRatingIdToUpdate = GetId();
                        db.UpdateWineRating(wineRatingIdToUpdate, newRating, newDescription, newUserId, newWineId).Wait();
                        Log("wine ratings updation");
                        break;
                }
                Console.WriteLine("\n==========================================================================\n");
            } while (select != 1);
        }

        void GetWineRatingParameters(out int rating, out string description, out int userId, out int wineId)
        {
            Console.WriteLine("Enter wine rating (1-5): ");
            rating = CheckInputRange(1, 5);

            Console.WriteLine("Enter wine rating description: ");
            description = Console.ReadLine();

            Console.WriteLine("Enter user id: ");
            userId = GetId();

            Console.WriteLine("Enter wine id: ");
            wineId = GetId();
        }

        private int CheckInputRange(int v1, int v2)
        {
            int input;
            while (!int.TryParse(Console.ReadLine(), out input) && input <= v1 && input >= v2)
            {
                Console.WriteLine("Invalid input. try again: ");
            }

            return input;
        }

        void AdminCollectionMenu()
        {
            Console.Clear();
            int select;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Back");
                Console.WriteLine("2. All Collections");
                Console.WriteLine("3. View Collection Details");
                Console.WriteLine("4. Create Collection");
                Console.WriteLine("5. Delete Collection");
                Console.WriteLine("6. Update Collection");

                select = CheckInput(6);

                switch (select)
                {
                    case 2:
                        WriteAllCollections();
                        Log("collections check");
                        break;
                    case 3:
                        Console.WriteLine("Enter collection id");
                        var collectionId = GetId();
                        var selectedCollection = db.GetCollection().Result.FirstOrDefault(c => c.Id == collectionId);
                        if (selectedCollection is null)
                            Console.WriteLine("Invalid collection id");
                        else
                        {
                            var user = db.GetUsers().Result.FirstOrDefault(u => u.Id == selectedCollection.UserId);
                            if (user is null)
                                Console.WriteLine("Invalid user id");
                            else
                            {
                                Console.WriteLine($"Date: {selectedCollection.Date}");
                                Console.WriteLine($"Note: {selectedCollection.Note}");
                                Console.WriteLine($"User: {user.Username} {user.FirstName} {user.LastName}");
                            }
                        }
                        Log("collection check");
                        break;
                    case 4:
                        GetCollectionParameters(out DateTime date, out string note, out int userId);
                        db.AddToCollection(date, note, userId).Wait();
                        Log("collections creation");
                        break;
                    case 5:
                        Console.WriteLine("Enter collection id");
                        int collectionIdToDelete = GetId();
                        db.RemoveCollection(collectionIdToDelete).Wait();
                        Log("collections deletion");
                        break;
                    case 6:
                        GetCollectionParameters(out DateTime date1, out string note1, out int userId1);
                        int collectionIdToUpdate= GetId();
                        db.UpdateCollection(collectionIdToUpdate, date1, note1, userId1).Wait();
                        Log("collections updation");
                        break;

                }
                Console.WriteLine("\n==========================================================================\n");
            } while (select != 1);
        }

        void GetCollectionParameters(out DateTime date, out string note, out int userId)
        {
            Console.WriteLine("Enter collection date (yyyy-MM-dd): ");
            while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                Console.WriteLine("Invalid date format. Please use yyyy-MM-dd.");
                Console.WriteLine("Enter collection date (yyyy-MM-dd): ");
            }

            Console.WriteLine("Enter collection note: ");
            note = Console.ReadLine();

            Console.WriteLine("Enter user id: ");
            userId = GetId();
        }

        void AdminUserMenu()
        {
            Console.Clear();
            int select;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Back");
                Console.WriteLine("2. All Users");
                Console.WriteLine("3. Certain user");
                Console.WriteLine("4. Create user");
                Console.WriteLine("5. Delete user");
                Console.WriteLine("6. Update user");

                select = CheckInput(6);

                switch (select)
                {
                    case 2:
                        WriteAllUsers();
                        Log("users check");
                        break;
                    case 3:
                        Console.WriteLine("Enter user id");
                        var id = GetId();
                        var selectedUser = db.GetUsers().Result.Where(u => u.Id == id).FirstOrDefault();
                        if (selectedUser is null)
                            Console.WriteLine("Invalid user id");
                        else
                        {
                            var role = db.GetRoles().Result.Where(r => r.Id == selectedUser.RoleId).First().Name;

                            Console.WriteLine($"First Name: {selectedUser.FirstName}");
                            Console.WriteLine($"Last name: {selectedUser.LastName}");
                            Console.WriteLine($"email: {selectedUser.Email}");
                            Console.WriteLine($"role: {role}");
                        }
                        Log("user check");
                        break;
                    case 4:
                        GetUserParameters(out string username, out string password, out string firstName, out string lastName, out string email, out int roleId);
                        db.CreateUser(username, password, email, "", firstName, lastName, roleId).Wait();
                        Log("users creation");
                        break;
                    case 5:
                        Console.WriteLine("Enter user id");
                        int id2 = GetId();
                        db.DeleteUser(id2).Wait();
                        Log("users deletion");
                        break;
                    case 6:
                        GetUserParameters(out string username1, out string password1, out string firstName1, out string lastName1, out string email1, out int roleId1);
                        Console.WriteLine("Enter user id");
                        int id1 = GetId();
                        db.UpdateUser(id1, username1, password1, email1, "", firstName1, lastName1, roleId1).Wait();
                        Log("users updation");
                        break;
                }
                Console.WriteLine("\n==========================================================================\n");
            } while (select != 1);
        }
        void EventMenu()
        {
            Console.Clear();
            int select;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Back");
                Console.WriteLine("2. My events");
                Console.WriteLine("3. Certain event");
                Console.WriteLine("4. Create event");
                Console.WriteLine("5. Delete event");
                Console.WriteLine("6. Update event");

                select = CheckInput(7);

                switch (select)
                {
                    case 2:
                        var events = db.GetEventsForOrganizer(user.Id).Result;
                        foreach (var item in events)
                        {
                            Console.WriteLine($"Name - {item.Name}\n" +
                                $"Date - {item.Date}\n" +
                                $"Location - {item.Location}\n" +
                                $"Id - {item.Id}\n");
                        }
                        Log("its events check");
                        break;
                    case 3:
                        var id = GetId();
                        var selectedEvent = db.GetEvents().Result.Where(e => e.Id == id).FirstOrDefault();
                        if (selectedEvent is null)
                            Console.WriteLine("Invalid event id");
                        else
                        {
                            Console.WriteLine($"Name - {selectedEvent.Name}\n" +
                                $"Date - {selectedEvent.Date}\n" +
                                $"Location - {selectedEvent.Location}\n" +
                                $"Id - {selectedEvent.Id}\n");
                            Console.WriteLine("Event users");
                            foreach (User item in db.GetParticipantsForEvent(id).Result)
                                Console.WriteLine($"Id - {item.Id}, Name - {item.Username}");
                        }
                        Log("event check");
                        break;
                    case 4:
                        GetEventParams(out string name, out DateTime date, out string location);
                        db.CreateEvent(name, date, location).Wait();
                        var eventId1 = db.GetEvents().Result.Where(e => e.Name == name && e.Date == date && e.Location == location).FirstOrDefault().Id;
                        db.AddUserOrganiserEvent(user.Id, eventId1).Wait();
                        Log("event creation");
                        break;
                    case 5:
                        db.DeleteEvent(GetId()).Wait();
                        Log("event deletion");
                        break;
                    case 6:
                        Console.WriteLine("Enter event id");
                        int id1 = GetId();
                        GetEventParams(out string name1, out DateTime date1, out string location1);
                        db.UpdateEvent(id1, name1, date1, location1).Wait();
                        Log("event updation");
                        break;

                }
                Console.WriteLine("\n==========================================================================\n");
            } while (select != 1);
        }

        private void GetEventParams(out string name, out DateTime date, out string location)
        {
            Console.WriteLine("Enter event name:");
            name = Console.ReadLine();

            Console.WriteLine("Enter event date (yyyy-MM-dd):");
            while (!DateTime.TryParse(Console.ReadLine(), out date))
                Console.WriteLine("Invalid date. Try again");

            Console.WriteLine("Enter event location:");
            location = Console.ReadLine();
        }


        void WineryMenu()
        {
            Console.Clear();
            int select;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Back");
                Console.WriteLine("2. All wineries");
                Console.WriteLine("3. Certain winery");
                Console.WriteLine("4. Create winery");
                Console.WriteLine("5. Delete winery");
                Console.WriteLine("6. Update winery");
                Console.WriteLine("7. Add user to winery");

                select = CheckInput(7);

                switch (select)
                {
                    case 2:
                        var wineries = db.GetUserWinaries(user.Id).Result;
                        foreach (var item in wineries)
                        {
                            Console.WriteLine($"Name - {item.Name}\n" +
                                $"Phone Number - {item.PhoneNumber}\n" +
                                $"Location - {item.Location}\n" +
                                $"Region - {item.Region}\n" +
                                $"Id - {item.Id}\n");
                        }
                        Log("wineries check");
                        break;
                    case 3:
                        var id = GetId();
                        var winery = db.GetWineries().Result.Where(w => w.Id == id).FirstOrDefault();
                        if (winery is null)
                            Console.WriteLine("Invalid winery id");
                        else
                        {
                            Console.WriteLine($"Name - {winery.Name}\n" +
                                $"Phone Number - {winery.PhoneNumber}\n" +
                                $"Location - {winery.Location}\n" +
                                $"Region - {winery.Region}\n" +
                                $"Id - {winery.Id}\n");
                            foreach(Wine item in db.GetWines().Result.Where(w => w.WineryId == winery.Id))
                            {
                                Console.WriteLine("Winery wines:");
                                Console.WriteLine($"Id - {item.Id}, Name - {item.Name}");
                            }
                        }
                        Log("winery check");
                        break;
                    case 4:
                        GetWineryParams(out string name, out string phoneNumber, out string location, out string region);
                        db.CreateWinery(name, phoneNumber, location, region).Wait();
                        Log("winery creation");
                        break;
                    case 5:
                        db.DeleteWinery(GetId()).Wait();
                        Console.WriteLine("Winery deleted");
                        Log("winery deletion");
                        break;
                    case 6:
                        int id1 = GetId();
                        GetWineryParams(out string name1, out string phoneNumber1, out string location1, out string region1);
                        db.UpdateWinery(id1, name1, phoneNumber1, location1, region1).Wait();
                        Log("winery updation");
                        break;
                    case 7:
                        WriteAllWinaries();
                        Console.WriteLine("Enter winery id");
                        int wineryId = GetId();
                        db.AddUserToWinery(user.Id, wineryId).Wait();
                        break;
                }
                Console.WriteLine("\n==========================================================================\n");
            } while (select != 1);
        }

        private void GetWineryParams(out string name, out string phoneNumber, out string location, out string region)
        {
            Console.WriteLine("Enter name:");
            name = Console.ReadLine();

            Console.WriteLine("Enter phone number:");
            phoneNumber = Console.ReadLine();

            Console.WriteLine("Enter location:");
            location = Console.ReadLine();

            Console.WriteLine("Enter region:");
            region = Console.ReadLine();
        }

        void PairingMenu()
        {
            Console.Clear();
            int select;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Back");
                Console.WriteLine("2. All pairings");
                Console.WriteLine("3. Certain pairing");
                Console.WriteLine("4. Create pairing");
                Console.WriteLine("5. Delete pairing");
                Console.WriteLine("6. Update pairing");
                Console.WriteLine("7. Add food to pairing");

                select = CheckInput(7);

                switch (select)
                {
                    case 2:
                        var pairings = db.GetPairings().Result;
                        foreach (var item in pairings)
                        {
                            Console.WriteLine($"Description - {item.Description}\n" +
                                $"Id - {item.Id}\n");
                        }
                        break;
                    case 3:
                        var id = GetId();
                        var pairing = db.GetPairings().Result.Where(p => p.Id == id).FirstOrDefault();
                        if (pairing is null)
                            Console.WriteLine("Invalid pairing id");
                        else
                        {
                            Console.WriteLine($"Description - {pairing.Description}\n" +
                                $"Id - {pairing.Id}\n" +
                                $"Wine - {db.GetWines().Result.Where(w => w.Id == pairing.WineId).FirstOrDefault().Name}");
                            foreach (Food item in db.GetFoodOfPairing(pairing.Id).Result)
                                Console.WriteLine($"Id - {item.Id}, Name - {item.Name}");
                        }
                        break;
                    case 4:
                        GetPairingParams(out string description, out int wineId);
                        db.CreatePairing(description, wineId).Wait();
                        break;
                    case 5:
                        db.DeletePairing(GetId()).Wait();
                        Console.WriteLine("Pairing deleted");
                        break;
                    case 6:
                        int id1 = GetId();
                        GetPairingParams(out string description1, out int wineId1);
                        db.UpdatePairing(id1, description1, wineId1).Wait();
                        break;
                    case 7:
                        WriteAllFood();
                        Console.WriteLine("Enter pairing id");
                        int pairingId = GetId();
                        Console.WriteLine("Enter food id");
                        int foodId = GetId();
                        db.CreateFoodPairing(foodId, pairingId).Wait();
                        break;
                }
                Console.WriteLine("\n==========================================================================\n");
            } while (select != 1);
        }

        private void GetPairingParams(out string description, out int wineId)
        {
            Console.WriteLine("Enter description:");
            description = Console.ReadLine();

            WriteAllWines();
            Console.WriteLine("Enter wine id");
            wineId = GetId();
        }


        void FoodMenu()
        {
            Console.Clear();
            int select;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Back");
                Console.WriteLine("2. All foods");
                Console.WriteLine("3. Certain food");
                Console.WriteLine("4. Create food");
                Console.WriteLine("5. Delete food");
                Console.WriteLine("6. Update food");

                select = CheckInput(6);

                switch (select)
                {
                    case 2:
                        var foods = db.GetFoods().Result;
                        foreach (var item in foods)
                        {
                            Console.WriteLine($"Name - {item.Name}\n" +
                                $"Description - {item.Description}\n" +
                                $"Id - {item.Id}\n");
                        }
                        break;
                    case 3:
                        var id = GetId();
                        var food = db.GetFoods().Result.Where(f => f.Id == id).FirstOrDefault();
                        if (food is null)
                            Console.WriteLine("Invalid food id");
                        else
                        {
                            Console.WriteLine($"Name - {food.Name}\n" +
                                $"Description - {food.Description}\n" +
                                $"Id - {food.Id}\n");
                        }
                        break;
                    case 4:
                        GetFoodParams(out string name, out string description);
                        db.CreateFood(name, "", description).Wait();
                        break;
                    case 5:
                        db.DeleteFood(GetId()).Wait();
                        Console.WriteLine("Food deleted");
                        break;
                    case 6:
                        int id1 = GetId();
                        GetFoodParams(out string name1, out string description1);
                        db.UpdateFood(id1, name1, "", description1).Wait();
                        break;
                }
                Console.WriteLine("\n==========================================================================\n");
            } while (select != 1);
        }

        private void GetFoodParams(out string name, out string description)
        {
            Console.WriteLine("Enter name:");
            name = Console.ReadLine();

            Console.WriteLine("Enter description:");
            description = Console.ReadLine();
        }


        // возращает созданного пользователя
        private async Task<User> CreateUser()
        {
            while (true)
            {
                Console.WriteLine("Enter username");
                var username = Console.ReadLine();
                
                if (username.Trim() == "") 
                {
                    await Console.Out.WriteLineAsync("username mustnt be empty. try again");
                    continue;
                }
                Console.WriteLine("Enter password");
                var password = Console.ReadLine();
                if (password.Trim() == "")
                {
                    await Console.Out.WriteLineAsync("password mustnt be empty. try again");
                    continue;
                }
                Console.WriteLine("Enter email");
                var email = Console.ReadLine();
                if (email.Trim() == "")
                {
                    await Console.Out.WriteLineAsync("email mustnt be empty. try again");
                    continue;
                }
                Console.WriteLine("Enter path to profile picture");
                var profilePicture = Console.ReadLine();
                Console.WriteLine("Enter first name");
                var firstName = Console.ReadLine();
                if (firstName.Trim() == "")
                {
                    await Console.Out.WriteLineAsync("firstName mustnt be empty. try again");
                    continue;
                }
                Console.WriteLine("Enter last name");
                var lastName = Console.ReadLine();
                if (lastName.Trim() == "")
                {
                    await Console.Out.WriteLineAsync("lastName mustnt be empty. try again");
                    continue;
                }

                int roleId = 8;
                // если пользователь админ позволяем ввести роль
                if (user is not null && user.RoleId == 10)
                {
                    Console.WriteLine("Enter role id");
                    if (int.TryParse(Console.ReadLine(), out roleId))
                    {
                        await Console.Out.WriteLineAsync("enter valid role id. try again");
                        continue;
                    }
                }
                if (!db.CreateUser(username, password, email, profilePicture, firstName, lastName, roleId).Result)
                {
                    await Console.Out.WriteLineAsync("try again");
                    continue;
                }
                return (await db.GetUsers()).Where(u => u.Username == username).FirstOrDefault();
            }
        }

        private void GetUserParameters(out string username, out string password, out string firstName, out string lastName, out string email, out int roleId)
        {
            if (user.RoleId == 10)
            {
                Console.WriteLine("Enter username");
                username = Console.ReadLine();

                Console.WriteLine("Enter password");
                password = Console.ReadLine();

                foreach (var role in db.GetRoles().Result)
                    Console.WriteLine($"Id - {role.Id}, Name - {role.Name}");

                Console.WriteLine("Enter role id");
                roleId = GetId();
            }
            else
            {
                username = user.Username;

                password = user.Password;

                roleId = user.RoleId;
            }

            Console.WriteLine("Enter first name");
            firstName = Console.ReadLine();

            Console.WriteLine("Enter last name");
            lastName = Console.ReadLine();

            Console.WriteLine("Enter email");
            email = Console.ReadLine();
        }

        void WineRatingMenu()
        {
            Console.Clear();
            int select;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Back");
                Console.WriteLine("2. All wine ratings");
                Console.WriteLine("3. Certain wine rating");
                Console.WriteLine("4. Create wine rating");
                Console.WriteLine("5. Delete wine rating");
                Console.WriteLine("6. Update wine rating");

                select = CheckInput(6);

                switch (select)
                {
                    case 2:
                        var wineRatings = db.GetWineRatings().Result.Where(wr => wr.UserId == user.Id);
                        foreach (var item in wineRatings)
                        {
                            Console.WriteLine($"Rating - {item.Rating}\n" +
                                $"Description - {item.Description}\n" +
                                $"Id - {item.Id}\n");
                        }
                        break;
                    case 3:
                        var id = GetId();
                        var wineRating = db.GetWineRatings().Result.Where(wr => wr.Id == id).FirstOrDefault();
                        if (wineRating is null)
                            Console.WriteLine("Invalid wine rating id");
                        else
                        {
                            Console.WriteLine($"Rating - {wineRating.Rating}\n" +
                                $"Description - {wineRating.Description}\n" +
                                $"Id - {wineRating.Id}\n" +
                                $"Wine - {db.GetWines().Result.Where(w => w.Id == wineRating.WineId).FirstOrDefault()}");
                        }
                        break;
                    case 4:
                        GetWineRatingParams(out int rating, out string description, out int userId, out int wineId);
                        db.AddWineRating(rating, description, userId, wineId).Wait();
                        break;
                    case 5:
                        db.RemoveWineRating(GetId()).Wait();
                        Console.WriteLine("Wine rating deleted");
                        break;
                    case 6:
                        Console.WriteLine("Enter rating id");
                        int id1 = GetId();
                        GetWineRatingParams(out int rating1, out string description1, out int userId1, out int wineId1);
                        db.UpdateWineRating(id1, rating1, description1, userId1, wineId1).Wait();
                        break;
                }
                Console.WriteLine("\n==========================================================================\n");
            } while (select != 1);
        }

        private void GetWineRatingParams(out int rating, out string description, out int userId, out int wineId)
        {
            Console.WriteLine("Enter rating:");
            while (!int.TryParse(Console.ReadLine(), out rating))
                Console.WriteLine("Invalid rating. Try again");

            Console.WriteLine("Enter description");
            description = Console.ReadLine();

            WriteAllWines();
            Console.WriteLine("Enter wine id");
            wineId = GetId();

            if (user.RoleId == 10)
            {
                WriteAllUsers();
                Console.WriteLine("Enter user id");
                userId = GetId();
            }
            else
            {
                userId = user.Id;
            }

        }

        void CollectionMenu()
        {
            Console.Clear();
            int select;

            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Back");
                Console.WriteLine("2. All collections");
                Console.WriteLine("3. Certain collection");
                Console.WriteLine("4. Create collection");
                Console.WriteLine("5. Delete collection");
                Console.WriteLine("6. Update collection");
                Console.WriteLine("7. Add wine to collection");

                select = CheckInput(7);

                switch (select) 
                {
                    case 2:
                        var collections = db.GetCollection().Result.Where(c => c.UserId == user.Id);
                        foreach (var item in collections)
                        {
                            Console.WriteLine($"Creation date - {item.Date}\n" +
                            $"Description - {item.Note}\n" +
                            $"Id - {item.Id}\n");
                        }
                        break;
                    case 3:
                        var id = GetId();
                        var collection = db.GetCollection().Result.Where(c => c.Id == id).FirstOrDefault();
                        if (collection is null)
                            Console.WriteLine("invalid colleciton id");
                        else
                        {
                            Console.WriteLine($"Creation date - {collection.Date}\n" +
                            $"Description - {collection.Note}\n" +
                            $"Id - {collection.Id}\n" +
                            $"Wines: ");
                            foreach (var wine in db.GetWinesCollection(collection.Id).Result)
                                Console.Write($"{wine.Name}, ");
                            Console.WriteLine();
                        }
                        break;
                    case 4:
                        GetCollectionParams(out DateTime date, out string note);
                        db.AddToCollection(date, note, user.Id).Wait();
                        break;
                    case 5:
                        db.RemoveCollection(GetId()).Wait();
                        Console.WriteLine("Collection deleted");
                        break;
                    case 6:
                        int id1 = GetId();
                        GetCollectionParams(out DateTime date1, out string note1);
                        db.UpdateCollection(id1, date1, note1, user.Id).Wait();
                        break;
                    case 7:
                        WriteAllWines();
                        Console.WriteLine();
                        Console.WriteLine("Enter wine id");
                        int wineId = GetId();
                        Console.WriteLine("Enter collection id");
                        int collectionId = GetId();
                        db.AddWineToCollection(wineId, collectionId).Wait();
                        break;
                }
                Console.WriteLine("\n==========================================================================\n");
            } while (select != 1);
        }

        private void GetCollectionParams(out DateTime date, out string note)
        {
            Console.WriteLine("Enter date:");
            while (!DateTime.TryParse(Console.ReadLine(), out date))
                Console.WriteLine("Inalid date. try again");

            Console.WriteLine("Enter note");
            note = Console.ReadLine();
        }




        private int CheckInput(int max)
        {
            int change;
            Console.WriteLine("make choice: ");
            while (!int.TryParse(Console.ReadLine(), out change) || (change < 1 || change > max))
            {
                Console.WriteLine("Invalid input. Try again: ");
            }

            return change;
        }

        int GetId()
        {
            int change;
            Console.WriteLine("Enter object id: ");
            while (!int.TryParse(Console.ReadLine(), out change))
            {
                Console.WriteLine("Invalid input. try again: ");
            }

            return change;
        }

        private async void Register()
        {
            var user = await CreateUser();
            if (user is null)
                throw new Exception("user is null. But he dont have to be null");

            this.user = user;
        }

        private async void Authorize()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Enter username");
                var username = Console.ReadLine();

                if (username.Trim() == "")
                {
                    await Console.Out.WriteLineAsync("username mustnt be empty. try again");
                    continue;
                }
                Console.WriteLine("Enter password");
                var password = Console.ReadLine();
                if (password.Trim() == "")
                {
                    await Console.Out.WriteLineAsync("password mustnt be empty. try again");
                    continue;
                }

                // если не смогли найти такое сочетание имени и пароля, значит
                // или такого пользователя нет или пароль не правильный
                var user = (await db.GetUsers()).Where(u => u.Username == username && u.Password == password)
                    .FirstOrDefault();

                if (user is null)
                {
                    await Console.Out.WriteLineAsync("wrong username or password. try again");
                    continue;
                }
                else
                {
                    this.user = user;
                    break;
                }
            }
        }

        private void WriteAllUsers()
        {
            var roles = db.GetRoles().Result;
            Console.WriteLine("Users:");
            foreach (var user in db.GetUsers().Result)
            {
                
                Console.WriteLine($"Id - {user.Id}, Name - {user.Username}, Role Id- {roles.Where(r => r.Id == user.RoleId).FirstOrDefault().Id}");
            }
        }

        private void WriteAllFood()
        {
            Console.WriteLine("Food:");
            foreach (var food in db.GetFoods().Result)
            {
                Console.WriteLine($"Id - {food.Id}, Name - {food.Name}");
            }
        }

        private void WriteAllWinaries()
        {
            Console.WriteLine("Winaries:");
            foreach (var winary in db.GetWineries().Result)
            {
                Console.WriteLine($"Id - {winary.Id}, Name - {winary.Name}");
            }

        }

        void WriteAllCollections()
        {
            var collections = db.GetCollection().Result;

            if (collections.Any())
            {
                foreach (var collection in collections)
                {
                    var user = db.GetUsers().Result.FirstOrDefault(u => u.Id == collection.UserId);
                    Console.WriteLine($"ID: {collection.Id}, Date: {collection.Date}, Note: {collection.Note}, User: {user?.FirstName} {user?.LastName}");
                }
            }
            else
            {
                Console.WriteLine("No collections found.");
            }
        }

        void WriteAllWineRatings()
        {
            var wineRatings = db.GetWineRatings().Result;

            if (wineRatings.Any())
            {
                foreach (var wineRating in wineRatings)
                {
                    var user = db.GetUsers().Result.FirstOrDefault(u => u.Id == wineRating.UserId);
                    var wine = db.GetWines().Result.FirstOrDefault(w => w.Id == wineRating.WineId);

                    Console.WriteLine($"ID: {wineRating.Id}, Rating: {wineRating.Rating}, Description: {wineRating.Description}, User: {user?.FirstName} {user?.LastName}, Wine: {wine?.Name}");
                }
            }
            else
            {
                Console.WriteLine("No wine ratings found.");
            }
        }

        void WriteAllEvents()
        {
            var events = db.GetEvents().Result;

            if (events.Any())
            {
                foreach (var ev in events)
                {
                    Console.WriteLine($"ID: {ev.Id}, Name: {ev.Name}, Date: {ev.Date}, Location: {ev.Location}");
                }
            }
            else
            {
                Console.WriteLine("No events found.");
            }
        }

        void WriteParticipantsForEvent(int eventId)
        {
            var participants = db.GetParticipantsForEvent(eventId).Result;

            if (participants.Any())
            {
                foreach (var participant in participants)
                {
                    var user = db.GetUsers().Result.FirstOrDefault(u => u.Id == participant.Id);
                    Console.WriteLine($"- {user?.FirstName} {user?.LastName} (ID: {user?.Id})");
                }
            }
            else
            {
                Console.WriteLine("No participants found for this event.");
            }
        }

        void WriteOrganisersForEvent(int eventId)
        {
            var organisers = db.GetOrganisersForEvent(eventId).Result;

            if (organisers.Any())
            {
                foreach (var organiser in organisers)
                {
                    var user = db.GetUsers().Result.FirstOrDefault(u => u.Id == organiser.Id);
                    Console.WriteLine($"- {user?.FirstName} {user?.LastName} (ID: {user?.Id})");
                }
            }
            else
            {
                Console.WriteLine("No organisers found for this event.");
            }
        }

        void WriteWinesForEvent(int eventId)
        {
            var wines = db.GetWinesForEvent(eventId).Result;

            if (wines.Any())
            {
                foreach (var wine in wines)
                {
                    Console.WriteLine($"- {wine.Name} (ID: {wine.Id})");
                }
            }
            else
            {
                Console.WriteLine("No wines found for this event.");
            }
        }

        void WriteFoodsForPairing(int pairingId)
        {
            var foods = db.GetFoodOfPairing(pairingId).Result;

            if (foods.Any())
            {
                foreach (var food in foods)
                {
                    Console.WriteLine($"- {food.Name} (ID: {food.Id})");
                }
            }
            else
            {
                Console.WriteLine("No foods found for this pairing.");
            }
        }

        void WriteAllColors()
        {
            var colors = db.GetColors().Result;

            if (colors.Any())
            {
                foreach (var color in colors)
                {
                    Console.WriteLine($"ID: {color.Id}, Name: {color.Name}");
                }
            }
            else
            {
                Console.WriteLine("No colors found.");
            }
        }

        void Log(string message)
        {
            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = File.AppendText(Path.Combine(Environment.CurrentDirectory, "Logs.txt")))
            {   
                    outputFile.WriteLine($"User - {user.Username} ({user.Id}) did {message}\n");
            }
        }
    }
}
