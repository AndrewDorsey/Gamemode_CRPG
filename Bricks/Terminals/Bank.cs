//+---------------------------------------------+
//| Title:	Bank				|
//| Author:	Jasa				|
//+---------------------------------------------+
//| Withdraw and deposit money.			|
//+---------------------------------------------+
datablock fxDtsBrickData(BankBrickData)
{
	category = "CRPG";
	subCategory = "Terminals";
	uiName = "Bank";
	bricktype = "Terminal";

	triggerDatablock = CRPGInputTriggerData;
	triggerSize = "8 9 6";
	trigger = 0;

	AdminOnly = true;
	
	brickSizeX = 2;
	brickSizeY = 3;
	brickSizeZ = 12;
	
	isProp = 1;
	
	brickRender = 0;
	brickCollide = 1;
	brickRaycast = 1;
	
	spawnModel = "ATMShape";
	modelOffset = "0 0 0";
	modelScale = "1 1 1";
	
	colorCount = 7;
	
	colorGroup[0] = "Red";
		colorMode[0] = "Fix";
		colorShift[0] = "1 0 0 1";
	
	colorGroup[1] = "Green";
		colorMode[1] = "Fix";
		colorShift[1]= "0 1 0 1";

	colorGroup[2] = "White";
		colorMode[2] = "Fix";
		colorShift[2] = "1 1 1 1";
	
	colorGroup[3] = "Black";
		colorMode[3] = "Fix";
		colorShift[3]= "0 0 0 1";

	colorGroup[4] = "Orange";
		colorMode[4] = "Fix";
		colorShift[4] = "1 0.5 0 1";
	
	colorGroup[5] = "Light Gray";
		colorMode[5] = "Fix";
		colorShift[5]= "0.8 0.8 0.8 1";

	colorGroup[6] = "Gray";
		colorMode[6] = "Fix";
		colorShift[6] = "0.5 0.5 0.5 1";
};

function BankBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus && %client.stage $= "")
		{
			if(!CRPGData.data[%client.bl_id].Value["Bank"] && !CRPGData.data[%client.bl_id].Value["Money"])
			{
				MessageClient(%client,'',"\c6You don't have any money to withdraw or deposite.");
				%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
				return;
			}
			MessageClient(%client,'', "\c6Please type the number corresponding to the options below.");
			if(CRPGData.data[%client.bl_id].Value["Bank"])
			{
				MessageClient(%client,'', "\c6 - You have \c3$" @ CRPGData.data[%client.bl_id].Value["Bank"] @ " \c6in the bank.");
				MessageClient(%client,'', "\c31 \c6- Withdraw Money");
			}
			if(CRPGData.data[%client.bl_id].Value["Money"])
			{
				MessageClient(%client,'', "\c32 \c6- Deposit Money");
				MessageClient(%client,'', "\c33 \c6- Deposit All Money [\c3$"@ CRPGData.data[%client.bl_id].Value["Money"]@"\c6]");
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
		if(%input == 2)
		{

			if(CRPGData.data[%client.bl_id].Value["Money"])
			{
				%client.stage = 1.2;
				MessageClient(%client,'', "\c6Please enter the amount of money you wish to deposit. You have \c3$"@ CRPGData.data[%client.bl_id].Value["Money"] @"\c6.");
				return;
			}
		}
		if(%input == 3)
		{

			if(CRPGData.data[%client.bl_id].Value["Money"])
			{
				%client.stage = 1.2;
				serverCmdMessageSent(%client, CRPGData.data[%client.bl_id].Value["Money"]);
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
			
			if(%input > CRPGData.data[%client.bl_id].Value["Bank"])
				%input = CRPGData.data[%client.bl_id].Value["Bank"];

			MessageClient(%client,'', "\c6You have withdrawn \c3$" @ %input @ "\c6.");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			
			CRPGData.data[%client.bl_id].Value["Bank"] -= %input;
			CRPGData.data[%client.bl_id].Value["Money"] += %input;
			
			commandtoclient(%client,'CRPGsetMoney',CRPGData.data[%client.bl_id].Value["Money"]);
			%client.setInfo();
			return;
		}
		
		if(%client.stage == 1.2)
		{
			if(%input < 1)
			{
				MessageClient(%client,'', "\c6Please enter a valid amount of money to deposit.");
				
				return;
			}
			
			if(%input > CRPGData.data[%client.bl_id].Value["Money"])
				%input = CRPGData.data[%client.bl_id].Value["Money"];
			
			MessageClient(%client,'', "\c6You have deposited \c3$" @ %input @ "\c6.");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			
			CRPGData.data[%client.bl_id].Value["Bank"] += %input;
			CRPGData.data[%client.bl_id].Value["Money"] -= %input;
			
			commandtoclient(%client, 'CRPGsetmoney', CRPGData.data[%client.bl_id].Value["Money"]);
			%client.setInfo();
		}
	}
}