//+---------------------------------------------+
//| Title:	Droppable			|
//| Author:	Jasa				|
//+---------------------------------------------+
//| Items dropped when a player dies.	 	|
//+---------------------------------------------+
datablock ItemData(cashItem)
{
	category = "Weapon";
	className = "Weapon";
	
	shapeFile = "Base/Data/Shapes/brickweapon.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.5;
	emap = true;
	
	doColorShift = false;
	colorShiftColor = "26 144 2 0";
	image = cashImage;
	candrop = true;
	canPickup = true;
};
datablock ShapeBaseImageData(cashImage)
{
	shapeFile = "Base/Data/Shapes/brickweapon.dts";
	emap = true;
	
	doColorShift = true;
	colorShiftColor = cashItem.colorShiftColor;
	canPickup = false;
};
function CashItem::onPickup(%this,%obj,%player)
{
	if(isObject(%player.client))
	{
		CRPGData.data[%player.client.bl_id].Value["Money"] += %obj.value;
		MessageClient(%player.client,'',"\c6You picked up \c3$"@ %obj.value @"\c6.");
		commandtoclient(%player.client,'setCRPGMoney',CRPGData.data[%player.client.bl_id].Value["Money"]);
		%player.client.setInfo();
		%obj.delete();
	}
}
datablock ItemData(LumberItem)
{
	category = "Weapon";
	className = "Weapon";
	
	shapeFile = "Add-Ons/Gamemode_CRPG/Shapes/Lumber.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.5;
	emap = true;
	
	doColorShift = false;
	colorShiftColor = "0 0 0 0";
	image = lumberImage;
	candrop = true;
	canPickup = true;
};
datablock ShapeBaseImageData(lumberImage)
{
	shapeFile = "Add-Ons/Gamemode_CRPG/Shapes/Lumber.dts";
};
function LumberItem::onPickup(%this,%obj,%player)
{
	if(isObject(%player.client))
	{
		CRPGData.data[%player.client.bl_id].Value["Lumber"] += %obj.value;
		MessageClient(%player.client,'',"\c6You picked up \c3"@ %obj.value @"\c6 lumber.");
		%obj.client.setInfo();
		%obj.delete();
	}
}
datablock ItemData(oreItem)
{
	category = "Weapon";
	className = "Weapon";
	
	shapeFile = "Add-Ons/Gamemode_CRPG/Shapes/Ore.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.5;
	emap = true;
	
	doColorShift = false;
	colorShiftColor = "0 0 0 0";
	image = oreImage;
	candrop = true;
	canPickup = true;
};
datablock ShapeBaseImageData(oreImage)
{
	shapeFile = "Add-Ons/Gamemode_CRPG/Shapes/Ore.dts";
};
function OreItem::onPickup(%this,%obj,%player)
{
	if(isObject(%player.client))
	{
		CRPGData.data[%player.client.bl_id].Value["Ore"] += %obj.value;
		MessageClient(%player.client,'',"\c6You picked up \c3"@ %obj.value @"\c6 ore.");
		%obj.client.setInfo();
		%obj.delete();
	}
}