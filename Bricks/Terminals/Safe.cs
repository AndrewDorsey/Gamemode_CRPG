//+---------------------------------------------+
//| Title:	Safe				|
//| Author:	Jasa				|
//+---------------------------------------------+
//| Stores stuff in a brick.			|
//+---------------------------------------------+
datablock fxDtsBrickData(SafeBrickData)
{
	category = "CRPG";
	subCategory = "Player Bricks";
	uiName = "Safe";
	bricktype = "Terminal";
	
	brickFile   = "Add-ons/Gamemode_CRPG/Shapes/Bricks/Safe.blb";

	triggerDatablock = CRPGInputTriggerData;
	triggerSize = "10 10 6";
	trigger = 0;
	
	brickSizeX = 4;
	brickSizeY = 4;
	brickSizeZ = 10;
	
	price = "5000";
};
function SafeBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	%group = %brick.getGroup();
	%money = getWord(CRPGData.data[%group.bl_id].Value["SafeMoney"],%brick.safeID);
	%lumber = getWord(CRPGData.data[%group.bl_id].Value["SafeLumber"],%brick.safeID);
	%ore = getWord(CRPGData.data[%group.bl_id].Value["SafeOre"],%brick.safeID);
	%item = getWord(CRPGData.data[%group.bl_id].Value["SafeItems"],%brick.safeID);
	%itemname = trim(%item.UIname);
	%trust = getTrustLevel(%client,%group);
	%itemexist = isObject(%item);
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus == true && %client.stage $= "")
		{
			if(%trust)
			{
				MessageClient(%client,'', "\c6Please type the number corresponding to the options below.");
				if(%money && %trust > 1)
					MessageClient(%client,'', "\c31 \c6- Withdraw Money [\c3$"@ %money @"\c6]");
				if(CRPGData.data[%client.bl_id].Value["Money"])
					MessageClient(%client,'', "\c32 \c6- Deposit Money");

				if(%lumber && %trust > 1)
					MessageClient(%client,'', "\c33 \c6- Withdraw Lumber [\c3"@ %lumber @"\c6]");
				if(CRPGData.data[%client.bl_id].Value["Lumber"])
					MessageClient(%client,'', "\c34 \c6- Deposit Lumber");

				if(%ore && %trust > 1)
					MessageClient(%client,'', "\c35 \c6- Withdraw Ore [\c3"@ %ore @"\c6]");
				if(CRPGData.data[%client.bl_id].Value["Ore"])
					MessageClient(%client,'', "\c36 \c6- Deposit Ore");

				if(%itemexist && %trust > 1)
					MessageClient(%client,'', "\c37 \c6- Take Item [\c3"@ %itemname @"\c6]");
				if(!%itemexist)
					MessageClient(%client,'', "\c37 \c6- Deposit Item");
			}
			else
			{
				MessageClient(%client,'', "\c6You do not trust \c3"@ CRPGData.data[%group.bl_id].Value["Name"] @"\c6 enough to use this safe.");
				%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
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
		if(%trust)
		{
			if(%input == 1 && %trust > 1)
			{
				if(%money)
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
			if(%input == 3 && %trust > 1)
			{
				if(%lumber)
				{
					%client.stage = 1.3;
					MessageClient(%client,'', "\c6Please enter the amount of lumber you wish to withdraw.");
					return;
				}
			}
			if(%input == 4)
			{
				if(CRPGData.data[%client.bl_id].Value["Lumber"])
				{
					%client.stage = 1.4;
					MessageClient(%client,'', "\c6Please enter the amount of lumber you wish to deposit. You have \c3"@ CRPGData.data[%client.bl_id].Value["Lumber"] @"\c6 lumber.");
					return;
				}
			}
			if(%input == 5 && %trust > 1)
			{
					if(%ore)
				{
					%client.stage = 1.5;
					MessageClient(%client,'', "\c6Please enter the amount of ore you wish to withdraw.");
					return;
				}
			}
			if(%input == 6)
			{
				if(CRPGData.data[%client.bl_id].Value["Ore"])
				{
					%client.stage = 1.6;
					MessageClient(%client,'', "\c6Please enter the amount of ore you wish to deposit. You have \c3"@ CRPGData.data[%client.bl_id].Value["Ore"] @"\c6 ore.");
					return;
				}
			}
			if(%input == 7)
			{
				if(%itemexist && %trust > 1)
				{
					for(%i = 0; %i < $CRPG::Pref::MaxTools; %i++)
					{
						if(!%client.player.tool[%i])
						{
							%client.player.tool[%i] = nameToId(%item);
							messageClient(%client,'MsgItemPickup','',%i,nameToId(%item));
							CRPGData.data[%group.bl_id].Value["SafeItems"] = setWord(CRPGData.data[%group.bl_id].Value["SafeItems"],%brick.safeID,0);
							if(isObject(%group.client))
								MessageClient(%group.client,'',"\c3"@ %client.name @"\c6 has withdrawn a \c3"@ %itemname @"\c6 out of your safe.");
							return;
						}
					}
					MessageClient(%client,'',"\c6You did not have an open slot for a \c3"@ %itemname @"\c6.");
					return;
				}
				if(!%itemexist)
				{
					%client.stage = 1.7;
					MessageClient(%client,'', "\c6Please enter the item you wish to deposit.");
					for(%i = 0; %i < $CRPG::Pref::MaxTools; %i++)
					{
						if(%client.player.tool[%i])
							MessageClient(%client,'',"\c3"@ %i+1 @"\c6 - "@ %client.player.tool[%i].uiName);
					}
					return;
				}
			}
		}
		MessageClient(%client,'', "\c3"@ %text @" \c6is not a valid option.");
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
			if(%input > %money)
				%input = %money;

			MessageClient(%client,'', "\c6You have withdrawn \c3$" @ %input @ "\c6 out of the safe.");
			if(isObject(%group.client))
				MessageClient(%group.client,'',"\c3"@ %client.name @"\c6 has withdrawn \c3$"@ %input @"\c6 out of your safe.");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));

			CRPGData.data[%group.bl_id].Value["SafeMoney"] = setWord(CRPGData.data[%group.bl_id].Value["SafeMoney"],%brick.safeID,%money-%input);
			CRPGData.data[%client.bl_id].Value["Money"] += %input;
			
			commandtoclient(%client,'CRPGsetMoney',CRPGData.data[%client.bl_id].Value["Money"]);
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
			
			MessageClient(%client,'', "\c6You have deposited \c3$" @ %input @ "\c6 into the safe.");
			if(isObject(%group.client))
				MessageClient(%group.client,'',"\c3"@ %client.name @"\c6 has deposited \c3$"@ %input @"\c6 into your safe.");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			
			CRPGData.data[%group.bl_id].Value["SafeMoney"] = setWord(CRPGData.data[%group.bl_id].Value["SafeMoney"],%brick.safeID,%money+%input);
			CRPGData.data[%client.bl_id].Value["Money"] -= %input;
			
			commandtoclient(%client,'setCRPGMoney',CRPGData.data[%client.bl_id].Value["Money"]);
			return;
		}
		if(%client.stage == 1.3)
		{
			if(%input < 1)
			{
				MessageClient(%client,'', "\c6Please enter a valid amount of lumber to withdraw.");
				return;
			}
			if(%input > %Lumber)
				%input = %Lumber;

			MessageClient(%client,'', "\c6You have withdrawn \c3" @ %input @ "\c6 lumber out of the safe.");
			if(isObject(%group.client))
				MessageClient(%group.client,'',"\c3"@ %client.name @"\c6 has withdrawn \c3"@ %input @"\c6 lumber out of your safe.");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));

			CRPGData.data[%group.bl_id].Value["SafeLumber"] = setWord(CRPGData.data[%group.bl_id].Value["SafeLumber"],%brick.safeID,%Lumber-%input);
			CRPGData.data[%client.bl_id].Value["Lumber"] += %input;
			return;
		}
		if(%client.stage == 1.4)
		{
			if(%input < 1)
			{
				MessageClient(%client,'', "\c6Please enter a valid amount of Lumber to deposit.");
				return;
			}
			if(%input > CRPGData.data[%client.bl_id].Value["Lumber"])
				%input = CRPGData.data[%client.bl_id].Value["Lumber"];
			
			MessageClient(%client,'', "\c6You have deposited \c3" @ %input @ "\c6 lumber into the safe.");
			if(isObject(%group.client))
				MessageClient(%group.client,'',"\c3"@ %client.name @"\c6 has deposited \c3"@ %input @"\c6 lumber into your safe.");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			
			CRPGData.data[%group.bl_id].Value["SafeLumber"] = setWord(CRPGData.data[%group.bl_id].Value["SafeLumber"],%brick.safeID,%Lumber+%input);
			CRPGData.data[%client.bl_id].Value["Lumber"] -= %input;
			return;
		}
		if(%client.stage == 1.5)
		{
			if(%input < 1)
			{
				MessageClient(%client,'', "\c6Please enter a valid amount of ore to withdraw.");
				return;
			}
			if(%input > %Ore)
				%input = %Ore;

			MessageClient(%client,'', "\c6You have withdrawn \c3" @ %input @ "\c6 ore out of the safe.");
			if(isObject(%group.client))
				MessageClient(%group.client,'',"\c3"@ %client.name @"\c6 has withdrawn \c3"@ %input @"\c6 ore out of your safe.");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));

			CRPGData.data[%group.bl_id].Value["SafeOre"] = setWord(CRPGData.data[%group.bl_id].Value["SafeOre"],%brick.safeID,%Ore-%input);
			CRPGData.data[%client.bl_id].Value["Ore"] += %input;
			return;
		}
		if(%client.stage == 1.6)
		{
			if(%input < 1)
			{
				MessageClient(%client,'', "\c6Please enter a valid amount of ore to deposit.");
				return;
			}
			if(%input > CRPGData.data[%client.bl_id].Value["Ore"])
				%input = CRPGData.data[%client.bl_id].Value["Ore"];
			
			MessageClient(%client,'', "\c6You have deposited \c3" @ %input @ "\c6 ore into the safe.");
			if(isObject(%group.client))
				MessageClient(%group.client,'',"\c3"@ %client.name @"\c6 has deposited \c3"@ %input @"\c6 ore into your safe.");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			
			CRPGData.data[%group.bl_id].Value["SafeOre"] = setWord(CRPGData.data[%group.bl_id].Value["SafeOre"],%brick.safeID,%Ore+%input);
			CRPGData.data[%client.bl_id].Value["Ore"] -= %input;
			return;
		}
		if(%client.stage == 1.7)
		{
			%input -= 1;
			if(!isObject(%client.player.tool[%input]))
			{
				MessageClient(%client,'', "\c6Please enter a valid item to deposit.");
				return;
			}
			MessageClient(%client,'', "\c6You have deposited a \c3" @ trim(%client.player.tool[%input].UIname) @ "\c6 into the safe.");
			if(isObject(%group.client))
				MessageClient(%group.client,'',"\c3"@ %client.name @"\c6 has deposited a \c3"@ trim(%client.player.tool[%input].UIname) @"\c6 into your safe.");
			
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
			
			CRPGData.data[%group.bl_id].Value["SafeItems"] = setWord(CRPGData.data[%group.bl_id].Value["SafeItems"],%brick.safeID,%client.player.tool[%input].getName());
			%client.player.tool[%input] = 0;
			messageClient(%client, 'MsgItemPickup', "", %input, 0);
		}
	}
}
function SafeBrickData::onPlant(%data,%brick)
{
	if(%brick.isPlanted)
	{
		%group = %brick.getGroup();
		%brick.SafeID = "0"@ CRPGBrickData.safeCount[%group.bl_id];
		CRPGBrickData.safeCount[%group.bl_id]++;
		CRPGBrickData.safes[%group.bl_id] = trim(CRPGBrickData.safes[%group.bl_id] SPC %brick);
		if(getword(CRPGData.Data[%group.bl_id].Value["SafeMoney"],%brick.safeID) $= "")
		{
			CRPGData.Data[%group.bl_id].Value["SafeMoney"] = setWord(CRPGData.Data[%group.bl_id].Value["SafeMoney"],%brick.safeID,"0");
			CRPGData.Data[%group.bl_id].Value["SafeLumber"] = setWord(CRPGData.Data[%group.bl_id].Value["SafeLumber"],%brick.safeID,"0");
			CRPGData.Data[%group.bl_id].Value["SafeOre"] = setWord(CRPGData.Data[%group.bl_id].Value["SafeMOre"],%brick.safeID,"0");
			CRPGData.Data[%group.bl_id].Value["SafeItems"] = setWord(CRPGData.Data[%group.bl_id].Value["SafeItems"],%brick.safeID,"0");
		}
	}
}
function fxDtsBrick::HandleSafeDestroy(%this)
{
	%group = %this.getGroup();
	CRPGData.data[%group.bl_id].Value["SafeMoney"] = removeWord(CRPGData.data[%group.bl_id].Value["SafeMoney"],%this.SafeID);
	CRPGData.data[%group.bl_id].Value["SafeLumber"] = removeWord(CRPGData.data[%group.bl_id].Value["SafeLumber"],%this.SafeID);
	CRPGData.data[%group.bl_id].Value["SafeOre"] = removeWord(CRPGData.data[%group.bl_id].Value["SafeOre"],%this.SafeID);
	CRPGData.data[%group.bl_id].Value["SafeItems"] = removeWord(CRPGData.data[%group.bl_id].Value["SafeItems"],%this.SafeID);
	CRPGBrickData.safeCount[%group.bl_id]--;
	CRPGBrickData.safes[%group.bl_id] = removeWord(CRPGBrickData.safes[%group.bl_id],%this.safeID);
	%wordcount = getWordCount(CRPGBrickData.safes[%group.bl_id]);
	for(%a = %this.SafeID; %a < %wordcount; %a++)
		getWord(CRPGBrickData.safes[%group.bl_id],%a).safeID = %a;
}