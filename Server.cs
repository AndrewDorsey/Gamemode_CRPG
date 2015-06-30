//+---------------------------------------------+
//| Title:	Server				|
//| Author:	Jasa				|
//+---------------------------------------------+
//| Executes everything and starts the mod up. 	|
//+---------------------------------------------+



echo("   Initializing CRPG Main");
exec("./Main.cs");
exec("./Prefs.cs");
exec("./Saving.cs");
exec("./Package.cs");
exec("./Events.cs");
exec("./Commands.cs");


echo("   Initializing CRPG Bricks");
exec("./Bricks.cs");

echo("   Initializing CRPG Jobs");
exec("./Jobs.cs");
Build();

echo("   Initializing CRPG Items");
exec("./Items/Baton.cs");
exec("./Items/LimitedBaton.cs");
exec("./Items/Handtaser.cs");
exec("./Items/Pickaxe.cs");
exec("./Items/LumberjackAxe.cs");
exec("./Items/Taser.cs");
//exec("./Items/Chainsaw.cs");
exec("./Items/Droppable.cs");

CRPGStartup();