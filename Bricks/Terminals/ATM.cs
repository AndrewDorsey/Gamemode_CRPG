//+---------------------------------------------+
//| Title:	ATM				|
//| Author:	Jasa				|
//+---------------------------------------------+
//| An ATM.					|
//+---------------------------------------------+
datablock fxDtsBrickData(ATMBrickData)
{
	category = "CRPG";
	subCategory = "Player Bricks";
	uiName = "ATM";
	bricktype = "Terminal";
	
	brickFile   = "Add-ons/Gamemode_CRPG/Shapes/Bricks/ATM.blb";

	triggerDatablock = CRPGInputTriggerData;
	triggerSize = "8 9 6";
	trigger = 0;

	AdminOnly = false;
	Price = "5000";
	
	brickSizeX = 2;
	brickSizeY = 3;
	brickSizeZ = 12;
};

function ATMBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus && %client.stage $= "")
		{
			if(!CRPGData.data[%client.bl_id].Value["Bank"])
			{
				MessageClient(%client,'',"\c6You don't have any money to withdraw.");
				%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
				return;
			}
			MessageClient(%client,'', "\c6Please type the number corresponding to the options below.");
			if(CRPGData.data[%client.bl_id].Value["Bank"])
			{
				MessageClient(%client,'', "\c6 - You have \c3$" @ CRPGData.data[%client.bl_id].Value["Bank"] @ " \c6in the bank.");
				MessageClient(%client,'', "\c31 \c6- Withdraw money. (ATM)");
			}
			return;
		}
		MessageClient(%client,'', "\c6Thanks, come again.");
		%client.stage = "";
		return;
	}
	%input = mfloor(%text);
	%stage = mfloor(%client.stage);
	if(!%stage)
	{
		if(%input == 1)
		{
			if(CRPGData.data[%client.bl_id].Value["Bank"])
			{
				%client.stage = 1.1;
				MessageClient(%client,'', "\c6Please enter the amount of money you wish to withdraw.");
				return;
			}
		}
		MessageClient(%client,'', "\c3" @ %text @ " \c6is not a valid option.");
		return;
	}
	if(%stage)
	{
		if(%client.stage == 1.1)
		{
			if(%input < 1)
			{
				MessageClient(%client,'', "\c6Please enter a valid amount of money to withdraw.");
				
				return;
			}
			
			if(CRPGData.data[%client.bl_id].Value["Bank"] - %input < 0)
				%input = CRPGData.data[%client.bl_id].Value["Bank"];

			MessageClient(%client,'', "\c6You have withdrawn \c3$" @ %input @ "\c6.");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			
			CRPGData.data[%client.bl_id].Value["Bank"] -= %input;
			CRPGData.data[%client.bl_id].Value["Money"] += %input;
			
			%client.setScore(CRPGData.data[%client.bl_id].Value["Bank"]);
		}
	}
}