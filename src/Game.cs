using System;
using System.Runtime.InteropServices;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;

	// Constructor
	public Game()
	{
		parser = new Parser();
		player = new Player();
		CreateRooms();
	}

	// Initialise the Rooms (and the Items)
	private void CreateRooms()
	{
		// Create the rooms
		Room outside = new Room("outside the main entrance of the university");
		Room theatre = new Room("in a lecture theatre");
		Room pub = new Room("in the campus pub");
		Room lab = new Room("in a computing lab");
		Room office = new Room("in the computing admin office");
		Room basement = new Room("in the basement. There is a secret path to somewhere but it is blocked.");
		Room secondFloor = new Room("on the 2. floor hallway");
		Room boysWC = new Room("in the boys bathroom");
		Room girlsWC = new Room("in the girls bathroom");
		Room secretRoom = new Room("in the secret room");
		Room exit = new Room("in the exit of the university.");

		basement.Block("bookshelf", true);
		outside.Block("padlock", true);

		// Initialise room exits
		outside.AddExit("north", exit);
		outside.AddExit("east", theatre);
		outside.AddExit("south", lab);
		outside.AddExit("west", pub);
		outside.AddExit("down", basement);

		theatre.AddExit("west", outside);
		theatre.AddExit("up", secondFloor);

		secondFloor.AddExit("down", theatre);
		secondFloor.AddExit("left", boysWC);
		secondFloor.AddExit("right", girlsWC);

		boysWC.AddExit("back", secondFloor);

		girlsWC.AddExit("back", secondFloor);

		pub.AddExit("east", outside);

		basement.AddExit("up", outside);
		basement.AddExit("forward", secretRoom);

		secretRoom.AddExit("back", basement);

		lab.AddExit("north", outside);
		lab.AddExit("east", office);

		office.AddExit("west", lab);

		// Create your Items here
		Item knife = new Item(2);
		Item baseballBat = new Item(5);
		Item medkit = new Item(8);
		Item key = new Item(1);
		// And add them to the Rooms
		lab.Chest.Put("medkit", medkit);
		theatre.Chest.Put("knife", knife);
		pub.Chest.Put("baseballBat", baseballBat);
		boysWC.Chest.Put("medkit", medkit);
		pub.Chest.Put("medkit", medkit);
		secretRoom.Chest.Put("key", key);
		// Start game outside
		player.CurrentRoom = outside;
	}

	//  Main play routine. Loops until end of play.
	public void Play()
	{
		PrintWelcome();

		// Enter the main command loop. Here we repeatedly read commands and
		// execute them until the player wants to quit.
		bool finished = false;
		while (!finished)
		{
			Command command = parser.GetCommand();
			finished = ProcessCommand(command);
		}
		Console.WriteLine("Thank you for playing.");
		Console.WriteLine("Press [Enter] to continue.");
		Console.ReadLine();
	}

	// Print out the opening message for the player.
	private void PrintWelcome()
	{
		Console.WriteLine();
		Console.WriteLine("Welcome to Zuul!");
		Console.WriteLine("Zuul is a new, incredibly boring adventure game.");
		Console.WriteLine("Type 'help' if you need help.");
		Console.WriteLine();
		Look();
	}

	// Given a command, process (that is: execute) the command.
	// If this command ends the game, it returns true.
	// Otherwise false is returned.
	private bool ProcessCommand(Command command)
	{
		bool wantToQuit = false;

		if (command.IsUnknown())
		{
			Console.WriteLine("I don't know what you mean...");
			return wantToQuit; // false
		}

		switch (command.CommandWord)
		{
			case "help":
				PrintHelp();
				break;
			case "go":
				GoRoom(command);
				break;
			case "quit":
				wantToQuit = true;
				break;
			case "look":
				Look();
				break;
			case "status":
				Status(player);
				break;
			case "take":
				Take(command);
				break;
			case "drop":
				Drop(command);
				break;
			case "use":
				player.Use(command.SecondWord);
				break;
		}

		return wantToQuit;
	}

	// ######################################
	// implementations of user commands:
	// ######################################

	// Print out some help information.
	// Here we print the mission and a list of the command words.
	private void PrintHelp()
	{
		Console.WriteLine("You are lost. You are alone.");
		Console.WriteLine("You wander around at the university.");
		Console.WriteLine();
		// let the parser print the commands
		parser.PrintValidCommands();
	}

	// Try to go to one direction. If there is an exit, enter the new
	// room, otherwise print an error message.
	private void GoRoom(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Go where?");
			return;
		}

		string direction = command.SecondWord;

		// Try to go to the next room.
		Room nextRoom = player.CurrentRoom.GetExit(direction);
		if (nextRoom == null)
		{
			Console.WriteLine("There is no door to " + direction + "!");
			return;
		}

		if (player.CurrentRoom.HasBlock())
		{
			if (direction == "forward")
			{
				Console.WriteLine("The path is blocked by bookshelf. You need to destroy it.");
				return;
			}
			else if (direction == "north")
			{
				Console.WriteLine($"You need a key to unlock it.");
				return;
			}
		}

		// Move to the next room
		player.CurrentRoom = nextRoom;
		Look();
		player.Damage(10);

		if (nextRoom.GetShortDescription() == "in the exit of the university.")
		{
			Console.WriteLine("You have succesfully beat the game. Congratulations!");
			Environment.Exit(0);
		}
	}

	private void Look()
	{
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
		if (player.CurrentRoom.Chest.TotalWeight() <= 0)
		{
			Console.WriteLine("There is nothing to be found");
		}
		else
		{
			player.CurrentRoom.Chest.ShowItems();
		}
	}

	private void Status(Player player)
	{
		Console.WriteLine($"Health is {player.GetHealth()}/100");
		Console.WriteLine($"Capacity: {player.Backpack.TotalWeight()}/10");
		Console.WriteLine($"Inventory: {player.GetInventory()}");
	}

	private void Take(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("What do you want to take?");
			return;
		}

		string itemName = command.SecondWord;
		bool success = player.TakeFromChest(itemName);

		if (success)
		{
			Console.WriteLine($"{itemName} is added to your inventory.");
		}
	}

	private void Drop(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("What do you want to drop?");
		}

		string itemName = command.SecondWord;
		bool success = player.DropToChest(itemName);

		if (success)
		{
			Console.WriteLine($"{itemName} is deleted from your inventory.");
		}
	}
}