//+---------------------------------------------+
//| Title:	Preferences			|
//| Author:	Jasa				|
//+---------------------------------------------+
//| Preferences for CRPG.		 	|
//+---------------------------------------------+
//if(isFile("Add-Ons/System_ReturnToBlockland/server.cs"))
//{
//	if(!$RTB::RTBR_ServerControl_Hook)
//		exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
//	RTB_registerPref("Tick Speed", "CRPG", "$CRPG::Pref::ClockSpeed", "int 1 20", "GameMode_CRPG", 10, 0, 0);
//	RTB_registerPref("Sun Position", "CRPG", "$CRPG::Pref::SunAzimuth", "int 0 359", "GameMode_CRPG", 65, 0, 0);
//	RTB_registerPref("Kick Without Client", "CRPG", "$CRPG::Pref::AutoKick", "bool", "GameMode_CRPG", 0, 0, 0);
//	RTB_registerPref("Max Lots", "CRPG", "$CRPG::Pref::Realestate::MaxLots", "int 1 5", "GameMode_CRPG", 3, 0, 0);
//	RTB_registerPref("Food Cost", "CRPG", "$CRPG::Pref::FoodPrice", "int 1 5", "GameMode_CRPG", 2, 0, 0);
//	RTB_registerPref("Lumber to Build", "CRPG", "$CRPG::Pref::BrickLumber", "bool", "GameMode_CRPG", 1, 0, 0);
//	RTB_registerPref("Water", "CRPG", "$CRPG::Pref::Water", "bool", "GameMode_CRPG", 0, 0, 0,"RebuildCRPGWater");
//	RTB_registerPref("Minumum Bounty", "CRPG", "$CRPG::Pref::MinumumBounty", "int 100 500", "GameMode_CRPG", 200, 0, 0);
//	RTB_registerPref("Chat in Jail", "CRPG", "$CRPG::Pref::JailChat", "bool", "GameMode_CRPG", 0, 0, 0);
//	RTB_registerPref("Demerit Cost", "CRPG", "$CRPG::Pref::Demerits::Price", "int 1 5", "GameMode_CRPG", 2, 0, 0);
//	RTB_registerPref("Demerit Reduce Rate", "CRPG", "$CRPG::Pref::Demerits::ReduceRate", "int 0 50", "GameMode_CRPG", 20, 0, 0);
//	RTB_registerPref("Murder Demerits", "CRPG", "$CRPG::Pref::Demerits::Murder", "int 100 400", "GameMode_CRPG", 200, 0, 0);
//	RTB_registerPref("Damging City Property", "CRPG", "$CRPG::Pref::Demerits::DCP", "int 0 20", "GameMode_CRPG", 10, 0, 0);
//	RTB_registerPref("Breaking And Entering", "CRPG", "$CRPG::Pref::Demerits::BaE", "int 0 200", "GameMode_CRPG", 50, 0, 0);
//	RTB_registerPref("Pardon Tick Cost", "CRPG", "$CRPG::Pref::Price::Pardon", "int 100 400", "GameMode_CRPG", 300, 0, 0);
//	RTB_registerPref("Record Clear Cost", "CRPG", "$CRPG::Pref::Price::ClearRecord", "int 2500 10000", "GameMode_CRPG", 5000, 0, 0);
//	RTB_registerPref("Reset Cost", "CRPG", "$CRPG::Pref::Price::Reset", "int 2500 10000", "GameMode_CRPG", 5000, 0, 0);
//	RTB_registerPref("Cellphone Cost", "CRPG", "$CRPG::Pref::Price::Cellphone", "int 0 300", "GameMode_CRPG", 200, 0, 0);
//	RTB_registerPref("Light Cost", "CRPG", "$CRPG::Pref::Price::Light", "int 0 100", "GameMode_CRPG", 50, 0, 0);
//	RTB_registerPref("Vehicle Spawn Cost", "CRPG", "$CRPG::Pref::Price::VehicleSpawn", "int 0 5000", "GameMode_CRPG", 1000, 0, 0,"UpdateVehicleSpawnCost");
//	RTB_registerPref("Music Brick Cost", "CRPG", "$CRPG::Pref::Price::MusicBrick", "int 0 1000", "GameMode_CRPG", 100, 0, 0,"UpdateMusicBrickCost");
//}
//else
//{
	$CRPG::Pref::ClockSpeed = "10";
	$CRPG::Pref::SunAzimuth = "65";
	$CRPG::Pref::AutoKick = 0;
	$CRPG::Pref::Realestate::MaxLots = "3";
	$CRPG::Pref::FoodPrice = "2";
	$CRPG::Pref::BrickLumber = 1;
	$CRPG::Pref::Water = 0;
	$CRPG::Pref::MinumumBounty = "200";
	$CRPG::Pref::JailChat = 0;
	$CRPG::Pref::Demerits::Murder = "200";
	$CRPG::Pref::Demerits::Price = "2";
	$CRPG::Pref::Demerits::ReduceRate = "20";
	$CRPG::Pref::Demerits::DCP = "10";
	$CRPG::Pref::Demerits::BaE = "50";
	$CRPG::Pref::Price::Pardon = "300";
	$CRPG::Pref::Price::ClearRecord = "5000";
	$CRPG::Pref::Price::Cellphone = "200";
	$CRPG::Pref::Price::Light = "50";
	$CRPG::Pref::Price::Reset = "5000";
	$CRPG::Pref::Price::MusicBrick = "100";
	$CRPG::Pref::Price::VehicleSpawn = "1000";
//}
$CRPG::Pref::DefaultTools = "hammerItem wrenchItem";
$CRPG::Pref::Drugs::CokeBuyPrice = "100";
$CRPG::Pref::Drugs::WeedBuyPrice	= "10";
$CRPG::Pref::Drugs::CokeSellPrice = "50";
$CRPG::Pref::Drugs::WeedSellPrice = "5";
//$CRPG::Pref::LumberBuyPrice = "1.5";
//$CRPG::Pref::OreBuyPrice	= "2";
//$CRPG::Pref::LumberSellPrice = "1";
//$CRPG::Pref::OreSellPrice = "1.5";
$CRPG::Pref::IronSellPrice = 1;
$CRPG::Pref::SilverSellPrice = 3;
$CRPG::Pref::PlatinumSellPrice = 6;
$CRPG::Pref::OakSellPrice = 1;
$CRPG::Pref::MapleSellPrice = 3;
$CRPG::Pref::MorningSellPrice = 6;
$CRPG::Pref::MatpackBuyPrice = "250";
$CRPG::Pref::Demerits::JailItem[0] = "PickaxeItem";
$CRPG::Pref::Demerits::Wanted = "215";

$CRPG::Pref::HungerRate = "0.5";
$CRPG::Hunger[0] = "Near Death";
$CRPG::Hunger[1] = "Starving";
$CRPG::Hunger[2] = "Very Hungry";
$CRPG::Hunger[3] = "Hungry";
$CRPG::Hunger[4] = "Underfed";
$CRPG::Hunger[5] = "Empty";
$CRPG::Hunger[6] = "Content";
$CRPG::Hunger[7] = "Satisfied";
$CRPG::Hunger[8] = "Full";
$CRPG::Hunger[9] = "Stuffed";
$CRPG::Hunger[10] = "Overfed";
$CRPG::Hunger[11] = "Engorged";
$CRPG::Hunger[12] = "Bloated";
$CRPG::Hunger[13] = "Bursting";
$CRPG::Hunger[14] = "Obese";

function UpdateMusicBrickCost()
{
	BrickMusicData.price = $CRPG::Pref::Price::MusicBrick;
}
function UpdateVehicleSpawnCost()
{
	BrickVehicleSpawnData.price = $CRPG::Pref::Price::VehicleSpawn;
}
BrickMusicData.price = $CRPG::Pref::Price::MusicBrick;
BrickVehicleSpawnData.price = $CRPG::Pref::Price::VehicleSpawn;
BrickSpawnPointData.adminOnly = "1";