//+---------------------------------------------+
//| Title:	Police				|
//| Author:	Jasa				|
//+---------------------------------------------+
//| View criminals and pay demerits.		|
//+---------------------------------------------+
datablock fxDtsBrickData(PoliceBrickData)
{
	category = "CRPG";
	subCategory = "Terminals";
	uiName = "Police";
	bricktype = "Terminal";
	
	brickFile   = "Add-ons/Gamemode_CRPG/Shapes/Bricks/TerminalBlue.blb";
	
	triggerDatablock = CRPGInputTriggerData;
	triggerSize = "7 10 4";
	trigger = 0;
	
	adminOnly = true;

	brickSizeX = 1;
	brickSizeY = 4;
	brickSizeZ = 8;
};
function PoliceBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	for(%a = 0; %a < $Server::PlayerCount; %a++)
	{
		if(CRPGData.data[clientGroup.getObject(%a).bl_id].Value["Demerits"] >= 200)
		{
			%Criminals = true;
			break;
		}
	}
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus == true)
		{
			if(!%criminals && !CRPGData.data[%client.bl_id].Value["Demerits"])
			{
				MessageClient(%client,'',"\c6There are no criminals online and you don't have any demerits.");
				%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
				return;
			}

			MessageClient(%client,'', "\c6Please type the number corresponding to the options below.");

			if(CRPGData.data[%client.bl_id].Value["Demerits"])
				MessageClient(%client,'', "\c6 - You have \c3"@ CRPGData.data[%client.bl_id].Value["Demerits"] @"\c6 demerits.");
			if(%Criminals)
				MessageClient(%client,'', "\c3 1 \c6- View Online Criminals");
			if(CRPGData.data[%client.bl_id].Value["Money"] >= $CRPG::Pref::Demerits::Price)
			{
				%yourDemerits = CRPGData.data[%client.bl_id].Value["Demerits"];
				%totalPrice = mFloor(CRPGData.data[%client.bl_id].Value["Demerits"] * $CRPG::Pref::Demerits::Price);
				%demsYouCanAfford = mFloor(CRPGData.data[%client.bl_id].Value["Money"] / $CRPG::Pref::Demerits::Price);
				%demsYouCanBuy = (%demsYouCanAfford > %yourDemerits ? %yourDemerits : %demsYouCanAfford);
				%demCost = mFloor(%demsYouCanBuy * $CRPG::Pref::Demerits::Price);

				MessageClient(%client,'', "\c3 2 \c6- Pay Demerits [\c3"@ %demsYouCanBuy @"\c6 out of \c3"@ %yourDemerits @"\c6 for \c3$"@ %demCost @"\c6]");
			}
			return;
		}
		MessageClient(%client,'', "\c6Thanks, come again.");
		%client.stage = "";
		return;
	}
	%input = mfloor(%text);
	if(%input == 1)
	{
		if(%Criminals)
		{
			for(%a = 0; %a < $Server::PlayerCount; %a++)
			{
				%criminal = clientGroup.getObject(%a);
				
				if(CRPGData.data[%criminal.bl_id].Value["Demerits"] >= 200)
					MessageClient(%client,'', "\c3"@ %criminal.name @"\c6 - \c3"@ %criminal.getWantedStars());
			}
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			return;
		}
	}
	if(%input == 2)
	{
		if(CRPGData.data[%client.bl_id].Value["Money"] >= $CRPG::Pref::Demerits::Price)
		{
			%yourDemerits = CRPGData.data[%client.bl_id].Value["Demerits"];
			%totalPrice = mFloor(CRPGData.data[%client.bl_id].Value["Demerits"] * $CRPG::Pref::Demerits::Price);
			%demsYouCanAfford = mFloor(CRPGData.data[%client.bl_id].Value["Money"] / $CRPG::Pref::Demerits::Price);
			%demsYouCanBuy = (%demsYouCanAfford > %yourDemerits ? %yourDemerits : %demsYouCanAfford);
			%demCost = mFloor(%demsYouCanBuy * $CRPG::Pref::Demerits::Price);
			
			if(%demsYouCanBuy <= 0)
			{
				MessageClient(%client,'', "\c6You cant afford to pay off any demerits.");
				
				%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
				
				return;
			}
			
			if(CRPGData.data[%client.bl_id].Value["Money"] < %demCost)
			{
				MessageClient(%client,'', "\c6You don't have enough money pay off your demerits.");
				
				return;
			}
			
			CRPGData.data[%client.bl_id].Value["Money"] -= %demCost;
			CRPGData.data[%client.bl_id].Value["Demerits"] -= %demsYouCanBuy;
			
			MessageClient(%client,'', "\c6You have paid \c3$"@ %demCost @"\c6 and now have \c3"@ CRPGData.data[%client.bl_id].Value["Demerits"] @"\c6 demerits.");
			
			commandtoclient(%client,'CRPGsetdems',CRPGData.data[%client.bl_id].Value["Demerits"]);
			commandtoclient(%client,'CRPGsetMoney',CRPGData.data[%client.bl_id].Value["Money"]);
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			return;
		}
	}
	MessageClient(%client,'', "\c3" @ %text @ " \c6is not a valid option.");
}