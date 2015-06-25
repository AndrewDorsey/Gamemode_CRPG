//+---------------------------------------------+
//| Title:	Bricks				|
//| Author:	Jasa				|
//+---------------------------------------------+
//| Bricks.				 	|     DANNU WAS HERE
//+---------------------------------------------+
datablock triggerData(CRPGLotTriggerData)
{
	tickPeriodMS = 500;
	parent = 0;
};
datablock triggerData(CRPGInputTriggerData)
{
	tickPeriodMS = 500;
	parent = 0;
};
datablock fxDTSBrickData(SmallResidentialLotBrickData : brick16x16FData)
{
	category = "CRPG";
	subCategory = "Residential Lots";
	
	uiName = "Small Residential";
	lotType = "Residential";
	
	BrickType = "Lot";
	AdminOnly = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "16 16 60";
	trigger = 0;
	
	lotPrice = "250";
	taxAmount = "8";
};
datablock fxDTSBrickData(HalfMediumResidentialLotBrickData : brick16x32FData)
{
	category = "CRPG";
	subCategory = "Residential Lots";
	
	uiName = "Half Medium Residential";
	lotType = "Residential";
	
	BrickType = "Lot";
	AdminOnly = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "16 32 60";
	trigger = 0;
	
	lotPrice = "500";
	taxAmount = "12";
};
datablock fxDTSBrickData(MediumResidentialLotBrickData : brick32x32FData)
{
	category = "CRPG";
	subCategory = "Residential Lots";
	
	uiName = "Medium Residential";
	lotType = "Residential";
	
	BrickType = "Lot";
	AdminOnly = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "32 32 80";
	trigger = 0;
	
	lotPrice = "1000";
	taxAmount = "20";
};
datablock fxDTSBrickData(HalfLargeResidentialLotBrickData)
{
	category = "CRPG";
	subCategory = "Residential Lots";
	
	uiName = "Half Large Residential";
	lotType = "Residential";
	
	BrickType = "Lot";
	AdminOnly = false;

	brickSizeX = 32;
	brickSizeY = 64;
	brickSizeZ = 1;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "32 64 120";
	trigger = 0;
	
	lotPrice = "2000";
	taxAmount = "36";
};
datablock fxDTSBrickData(LargeResidentialLotBrickData : brick64x64FData)
{
	category = "CRPG";
	subCategory = "Residential Lots";
	
	uiName = "Large Residential";
	lotType = "Residential";
	
	BrickType = "Lot";
	AdminOnly = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "64 64 160";
	trigger = 0;
	
	lotPrice = "4000";
	taxAmount = "68";
};
datablock fxDTSBrickData(SmallCommercialLotBrickData : brick16x16FData)
{
	category = "CRPG";
	subCategory = "Commercial Lots";
	
	uiName = "Small Commercial";
	lotType = "Commercial";
	
	BrickType = "Lot";
	AdminOnly = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "16 16 60";
	trigger = 0;
	
	lotPrice = "400";
	taxAmount = "10";
};
datablock fxDTSBrickData(HalfMediumCommercialLotBrickData : brick16x32FData)
{
	category = "CRPG";
	subCategory = "Commercial Lots";
	
	uiName = "Half Medium Commercial";
	lotType = "Commercial";
	
	BrickType = "Lot";
	AdminOnly = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "16 32 60";
	trigger = 0;
	
	lotPrice = "800";
	taxAmount = "15";
};
datablock fxDTSBrickData(MediumCommercialLotBrickData : brick32x32FData)
{
	category = "CRPG";
	subCategory = "Commercial Lots";
	
	uiName = "Medium Commercial";
	lotType = "Commercial";
	
	BrickType = "Lot";
	AdminOnly = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "32 32 80";
	trigger = 0;
	
	lotPrice = "1600";
	taxAmount = "25";
};
datablock fxDTSBrickData(HalfLargeCommercialLotBrickData)
{
	category = "CRPG";
	subCategory = "Commercial Lots";
	
	uiName = "Half Large Commercial";
	lotType = "Commercial";
	
	BrickType = "Lot";
	AdminOnly = false;

	brickSizeX = 32;
	brickSizeY = 64;
	brickSizeZ = 1;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "32 64 120";
	trigger = 0;
	
	lotPrice = "3200";
	taxAmount = "45";
};
datablock fxDTSBrickData(LargeCommercialLotBrickData : brick64x64FData)
{
	category = "CRPG";
	subCategory = "Commercial Lots";
	
	uiName = "Large Commercial";
	lotType = "Commercial";
	
	BrickType = "Lot";
	AdminOnly = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "64 64 160";
	trigger = 0;
	
	lotPrice = "6400";
	taxAmount = "85";
};

datablock fxDTSBrickData(SmallIndustrialLotBrickData : brick16x16FData)
{
	category = "CRPG";
	subCategory = "Industrial Lots";
	
	uiName = "Small Industrial";
	lotType = "Industrial";
	
	BrickType = "Lot";
	AdminOnly = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "16 16 60";
	trigger = 0;
	
	lotPrice = "400";
	taxAmount = "-5";
};

datablock fxDTSBrickData(HalfMediumIndustrialLotBrickData : brick16x32FData)
{
	category = "CRPG";
	subCategory = "Industrial Lots";
	
	uiName = "Half Medium Industrial";
	lotType = "Industrial";
	
	BrickType = "Lot";
	AdminOnly = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "16 32 60";
	trigger = 0;
	
	lotPrice = "800";
	taxAmount = "-10";
};

datablock fxDTSBrickData(MediumIndustrialLotBrickData : brick32x32FData)
{
	category = "CRPG";
	subCategory = "Industrial Lots";
	
	uiName = "Medium Industrial";
	lotType = "Industrial";
	
	BrickType = "Lot";
	AdminOnly = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "32 32 80";
	trigger = 0;
	
	lotPrice = "1600";
	taxAmount = "-20";
};

datablock fxDTSBrickData(HalfLargeIndustrialLotBrickData)
{
	category = "CRPG";
	subCategory = "Industrial Lots";
	
	uiName = "Half Large Industrial";
	lotType = "Industrial";
	
	BrickType = "Lot";
	AdminOnly = false;

	brickSizeX = 32;
	brickSizeY = 64;
	brickSizeZ = 1;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "32 64 120";
	trigger = 0;
	
	lotPrice = "3200";
	taxAmount = "-40";
};

datablock fxDTSBrickData(LargeIndustrialLotBrickData : brick64x64FData)
{
	category = "CRPG";
	subCategory = "Industrial Lots";
	
	uiName = "Large Industrial";
	lotType = "Industrial";
	
	BrickType = "Lot";
	AdminOnly = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "64 64 160";
	trigger = 0;
	
	lotPrice = "6400";
	taxAmount = "-80";
};

datablock fxDTSBrickData(SmallAdminLotBrickData : brick16x16FData)
{
	category = "CRPG";
	subCategory = "Admin Lots";
	
	uiName = "Small Admin";
	lotType = "Admin";
	
	BrickType = "Lot";
	AdminOnly = true;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "16 16 60";
	trigger = 0;
	
	lotPrice = "0";
	taxAmount = "0";
};

datablock fxDTSBrickData(HalfMediumAdminLotBrickData : brick16x32FData)
{
	category = "CRPG";
	subCategory = "Admin Lots";
	
	uiName = "Half Medium Admin";
	lotType = "Admin";
	
	BrickType = "Lot";
	AdminOnly = true;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "16 32 60";
	trigger = 0;
	
	lotPrice = "0";
	taxAmount = "0";
};

datablock fxDTSBrickData(MediumAdminLotBrickData : brick32x32FData)
{
	category = "CRPG";
	subCategory = "Admin Lots";
	
	uiName = "Medium Admin";
	lotType = "Admin";
	
	BrickType = "Lot";
	AdminOnly = true;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "32 32 80";
	trigger = 0;
	
	lotPrice = "0";
	taxAmount = "0";
};

datablock fxDTSBrickData(HalfLargeAdminLotBrickData)
{
	category = "CRPG";
	subCategory = "Admin Lots";
	
	uiName = "Half Large Admin";
	lotType = "Admin";
	
	BrickType = "Lot";
	AdminOnly = true;

	brickSizeX = 32;
	brickSizeY = 64;
	brickSizeZ = 1;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "32 64 120";
	trigger = 0;
	
	lotPrice = "0";
	taxAmount = "0";
};

datablock fxDTSBrickData(LargeAdminLotBrickData : brick64x64FData)
{
	category = "CRPG";
	subCategory = "Admin Lots";
	
	uiName = "Large Admin";
	lotType = "Admin";
	
	BrickType = "Lot";
	AdminOnly = true;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "64 64 160";
	trigger = 0;
	
	lotPrice = "0";
	taxAmount = "0";
};

datablock fxDtsBrickData(PersonalSpawnBrickData : brickSpawnPointData)
{
	category = "CRPG";
	subCategory = "Player Bricks";
	
	uiName = "Personal Spawn";

	BrickType = "Spawn";
	AdminOnly = false;
	
	spawnData = "personalSpawn";
};

datablock fxDtsBrickData(JailSpawnBrickData : brickSpawnPointData)
{
	category = "CRPG";
	subCategory = "Spawns";
	
	uiName = "Jail Spawn";

	BrickType = "Spawn";
	AdminOnly = true;
	
	spawnData = "jailSpawn";
};

exec("./Bricks/Terminals/Bank.cs");
exec("./Bricks/Terminals/Police.cs");
exec("./Bricks/Terminals/Bounty.cs");
exec("./Bricks/Terminals/Labor.cs");
exec("./Bricks/Terminals/Drug.cs");
exec("./Bricks/Terminals/Safe.cs");
exec("./Bricks/Terminals/ATM.cs");

exec("./Bricks/Resources/Lumber.cs");
exec("./Bricks/Resources/Ore.cs");
exec("./Bricks/Resources/CityTree.cs");
exec("./Bricks/Resources/PlayerTree.cs");
