//+---------------------------------------------+
//| Title:	Bounty				|
//| Author:	Jasa				|
//+---------------------------------------------+
//| Place and view bounties.			|
//+---------------------------------------------+
datablock fxDtsBrickData(BountyBrickData)
{
	category = "CRPG";
	subCategory = "Terminals";
	uiName = "Bounty";
	bricktype = "Terminal";
	
	triggerDatablock = CRPGInputTriggerData;
	triggerSize = "7 10 4";
	trigger = 0;
	
	adminOnly = true;

	brickSizeX = 1;
	brickSizeY = 4;
	brickSizeZ = 8;
	
	isProp = 1;
	
	brickRender = 0;
	brickCollide = 1;
	brickRaycast = 1;
	
	spawnModel = "MoniterShape";
	modelOffset = "0 0 0";
	modelScale = "1 1 1";
	
	colorCount = 2;
	
	colorGroup[0] = "Gray";
		colorMode[0] = "Fix";
		colorShift[0] = "0.5 0.5 0.5 1";
	
	colorGroup[1] = "Cyan";
		colorMode[1] = "Fix";
		colorShift[1]= "0 1 1 1";
};
function BountyBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	for(%a = 0; %a < $Server::PlayerCount; %a++)
	{
		if(CRPGData.data[clientGroup.getObject(%a).bl_id].value["Bounty"])
		{
			%bounties = true;
			break;
		}
	}
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus && %client.stage $= "")
		{
			if(!%bounties && !CRPGData.data[%client.bl_id].value["JobID"].PlaceBounties && CRPGData.data[%client.bl_id].value["Money"]<$CRPG::Pref::MinimumBounty)
			{
				MessageClient(%client,'',"\c6There are no bounties and you cannot place bounties.");
				%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
				return;
			}
			MessageClient(%client,'', "\c6Please type the number corresponding to the options below.");
			if(%bounties)
				messageClient(%client, '', "\c31 \c6- View bounties.");
			if(CRPGData.data[%client.bl_id].value["JobID"].PlaceBounties && CRPGData.data[%client.bl_id].value["Money"] >= $CRPG::Pref::MinimumBounty)
				messageClient(%client, '', "\c32 \c6- Place a bounty.");
			return;
		}
		messageClient(%client, '', "\c6Thanks, come again.");
		%client.stage = "";
		return;
	}
	%input = mfloor(%text);
	%stage = mfloor(%client.stage);
	if(!%stage)
	{
		if(%input == 1)
		{
			if(%bounties)
			{
				for(%a = 0; %a < $Server::PlayerCount; %a++)
				{
					%wanted = clientGroup.getObject(%a);
					
					if(CRPGData.data[%wanted.bl_id].value["Bounty"])
						messageClient(%client,'',"\c3"@ %wanted.name @"\c6 - \c3$"@ CRPGData.data[%wanted.bl_id].value["Bounty"]);
				}
				%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
				return;
			}
		}
		if(%input == 2)
		{
			if(CRPGData.data[%client.bl_id].value["JobID"].PlaceBounties && CRPGData.data[%client.bl_id].value["Money"] >= $CRPG::Pref::MinimumBounty)
			{
				%client.stage = 1.1;
				messageClient(%client, '', "\c6Who do you want to place a bounty on? (ID or Name)");
				return;
			}
		}
		MessageClient(%client,'', "\c3"@ %text @" \c6is not a valid option.");
		return;
	}
	if(%stage)
	{
		if(%client.stage == 1.1)
		{
			%id = findClientByBl_id(%text);
			%name = findClientByName(%text);
			if(isObject(%id))
				%client.player.hunted = %id;
			if(isObject(%name))
				%client.player.hunted = %name;
			if(!isObject(%client.player.hunted))
			{
				messageClient(%client,'',"\c6Target not found.");
				return;
			}
			if(!%client.player.hunted == %client)
			{
				messageClient(%client,'',"\c6You cannot place a bounty on yourself.");
				return;
			}
			if(CRPGData.data[%client.player.hunted.bl_id].value["Bounty"])
			{
				messageClient(%client,'',"\c3"@ %client.player.hunted.name @"\c6 already has a bounty.");
				return;
			}
			%client.stage = 1.2;
			messageclient(%client,'',"\c6How much money would you like to place on \c3"@ %client.player.hunted.name @"\c6?");
			return;
		}
		if(%client.stage == 1.2)
		{
			if(%input < $CRPG::Pref::MinimumBounty)
			{
				messageClient(%client,'',"\c6Please enter a valid bounty of at least \c3$"@ $CRPG::Pref::MinimumBounty @"\c6.");
				return;
			}
			if(%input > CRPGData.data[%client.bl_id].Value["Money"])
				%input = CRPGData.data[%client.bl_id].Value["Money"];

			MessageClient(%client,'', "\c6You have placed a \c3$" @ %input @ "\c6 bounty on \c3"@ %client.player.hunted.name @"\c6.");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			
			CRPGData.data[%client.player.hunted.bl_id].Value["Bounty"] = %input;
			CRPGData.data[%client.bl_id].Value["Money"] -= %input;
			
			commandtoclient(%client, 'CRPGsetMoney', CRPGData.data[%client.bl_id].Value["Money"]);
		}
	}
}