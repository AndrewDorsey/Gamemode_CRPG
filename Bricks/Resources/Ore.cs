datablock fxDTSBrickData(CRPGIronOreData)
{
	brickFile   = "Add-ons/Gamemode_CRPG/Shapes/Bricks/smallOre.blb";
	iconName    = "Add-ons/Gamemode_CRPG/Shapes/BrickIcons/smallOre";
	uiName      = "Iron Ore";
	category    = "CRPG";
	subCategory = "Resources";
	
	bricktype = "Resource";
	adminOnly = true;
	
	CRPG_RequiredLevel = "0";
	CRPG_isOre = 1;
	CRPG_gives = 1;
	CRPG_life = 15;
	CRPG_exp = 0.38;
};

datablock fxDTSBrickData(CRPGSilverOreData)
{
	brickFile   = "Add-ons/Gamemode_CRPG/Shapes/Bricks/smallOre.blb";
	iconName    = "Add-ons/Gamemode_CRPG/Shapes/BrickIcons/smallOre";
	uiName      = "Silver Ore";
	category    = "CRPG";
	subCategory = "Resources";
	
	bricktype = "Resource";
	adminOnly = true;
	
	CRPG_RequiredLevel = "10";
	CRPG_isOre = 1;
	CRPG_gives = 2;
	CRPG_life = 30;
	CRPG_exp = 1.55;
};

datablock fxDTSBrickData(CRPGPlatinumOreData)
{
	brickFile   = "Add-ons/Gamemode_CRPG/Shapes/Bricks/smallOre.blb";
	iconName    = "Add-ons/Gamemode_CRPG/Shapes/BrickIcons/smallOre";
	uiName      = "Platinum Ore";
	category    = "CRPG";
	subCategory = "Resources";
	
	bricktype = "Resource";
	adminOnly = true;
	
	CRPG_RequiredLevel = "20";
	CRPG_isOre = 1;
	CRPG_gives = 3;
	CRPG_life = 60;
	CRPG_exp = 2.00;
};

