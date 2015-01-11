//+---------------------------------------------+
//| Title:	Drug				|
//| Author:	Jasa				|
//+---------------------------------------------+
//| Drugs.					|
//+---------------------------------------------+
datablock fxDtsBrickData(DrugBrickData)
{
	category = "CRPG";
	subCategory = "Terminals";
	uiName = "Drug";
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
		colorShift[1]= "1 1 0 1";
};
function DrugsBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{	
	if(%triggerStatus !$= "")
	{
		if(%triggerStatus && %client.stage $= "")
		{
			if(!CRPGData.data[%client.bl_id].Value["Coke"] && !CRPGData.data[%client.bl_id].Value["Weed"] && (CRPGData.data[%client.bl_id].Value["Money"] < $CRPG::Pref::Drugs::CokeBuyPrice) && (CRPGData.data[%client.bl_id].Value["Money"] < $CRPG::Pref::Drugs::WeedBuyPrice))
			{
				CommandToClient(%client,'',"\c6You don't have any money to buy drugs or drugs to sell.");
				%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
				return;
			}
			MessageClient(%client,'', "\c6Please type the number corresponding to the options below.");

			if(CRPGData.data[%client.bl_id].Value["Coke"] || CRPGData.data[%client.bl_id].Value["Weed"])
				MessageClient(%client,'', "\c6 - You have \c3"@ CRPGData.data[%client.bl_id].Value["Coke"] @"\c6 coke and \c3"@ CRPGData.data[%client.bl_id].Value["Weed"] @"\c6 weed.");

			if(CRPGData.data[%client.bl_id].Value["Money"] >= $CRPG::Pref::Drugs::CokeBuyPrice)
				MessageClient(%client,'', "\c31 \c6- Buy Coke [\c3$"@ getsubstr($CRPG::Pref::Drugs::CokeBuyPrice,0,4) @"\c6 Each]");

			if(CRPGData.data[%client.bl_id].Value["Money"] >= $CRPG::Pref::Drugs::WeedBuyPrice)
				MessageClient(%client,'', "\c32 \c6- Buy Weed [\c3$"@ getsubstr($CRPG::Pref::Drugs::WeedBuyPrice,0,4) @"\c6 Each]");

			if(CRPGData.data[%client.bl_id].Value["Coke"])
				MessageClient(%client,'', "\c33 \c6- Sell Coke [\c3$"@ getsubstr($CRPG::Pref::Drugs::CokeSellPrice,0,4) @"\c6 Each]");

			if(CRPGData.data[%client.bl_id].Value["Weed"])
				MessageClient(%client,'', "\c34 \c6- Sell Weed [\c3$"@ getsubstr($CRPG::Pref::Drugs::WeedSellPrice,0,4) @"\c6 Each]");
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
			if(CRPGData.data[%client.bl_id].Value["Money"] >= $CRPG::Pref::Drugs::CokeBuyPrice)
			{
				%client.stage = 1.1;
				%cokecanbuy = mfloor(CRPGData.data[%client.bl_id].Value["Money"]/$CRPG::Pref::Drugs::CokeBuyPrice);
				MessageClient(%client,'', "\c6Please enter the amount of coke you wish to buy. You can buy \c3"@ %cokecanbuy @"\c6 coke for \c3$"@ %cokecanbuy*$CRPG::Pref::Drugs::CokeBuyPrice @"\c6.");
				return;
			}
		}
		if(%input == 2)
		{
			if(CRPGData.data[%client.bl_id].Value["Money"] >= $CRPG::Pref::Drugs::WeedBuyPrice)
			{
				%client.stage = 1.2;
				%weedcanbuy = mfloor(CRPGData.data[%client.bl_id].Value["Money"]/$CRPG::Pref::Drugs::WeedBuyPrice);
				MessageClient(%client,'', "\c6Please enter the amount of weed you wish to buy. You can buy \c3"@ %weedcanbuy @"\c6 weed for \c3$"@ %weedcanbuy*$CRPG::Pref::Drugs::WeedBuyPrice @"\c6.");
				return;
			}
		}
		if(%input == 3)
		{
			if(CRPGData.data[%client.bl_id].Value["Coke"])
			{
				%client.stage = 1.3;
				MessageClient(%client,'', "\c6Please enter the amount of coke you wish to sell. You can sell \c3"@ CRPGData.data[%client.bl_id].Value["Coke"] @"\c6 coke for \c3$"@ CRPGData.data[%client.bl_id].Value["Coke"]*$CRPG::Pref::Drugs::CokeSellPrice @"\c6.");
				return;
			}
		}
		if(%input == 4)
		{
			if(CRPGData.data[%client.bl_id].Value["Weed"])
			{
				%client.stage = 1.4;
				MessageClient(%client,'', "\c6Please enter the amount of weed you wish to sell. You can sell \c3"@ CRPGData.data[%client.bl_id].Value["Weed"] @"\c6 weed for \c3$"@ CRPGData.data[%client.bl_id].Value["Weed"]*$CRPG::Pref::Drugs::WeedSellPrice @"\c6.");
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
			if(%input <= 0)
			{
				MessageClient(%client,'', "\c6Please enter a valid amount of coke to buy.");
				return;
			}
			if(CRPGData.data[%client.bl_id].Value["Money"] - %input*$CRPG::Pref::Drugs::CokeBuyPrice < 1)
				%input = mfloor(CRPGData.data[%client.bl_id].Value["Money"]/$CRPG::Pref::Drugs::CokeBuyPrice);

			%cokebuyPrice = %input*$CRPG::Pref::Drugs::CokeBuyPrice;
			MessageClient(%client,'', "\c6You have bought \c3" @ %input @ "\c6 coke for \c3$"@ %cokebuyPrice @"\c6.");

			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));

			CRPGData.data[%client.bl_id].Value["Money"] -= %cokebuyPrice;
			CRPGData.data[%client.bl_id].Value["Coke"] += %input;
			
			commandtoclient(%client,'CRPGsetMoney',CRPGData.data[%client.bl_id].Value["Money"]);
			return;
		}
		if(%client.stage == 1.2)
		{
			if(%input <= 0)
			{
				MessageClient(%client,'', "\c6Please enter a valid amount of weed to buy.");
				return;
			}
			if(CRPGData.data[%client.bl_id].Value["Money"] - %input*$CRPG::Pref::Drugs::WeedBuyPrice < 1)
				%input = mfloor(CRPGData.data[%client.bl_id].Value["Money"]/$CRPG::Pref::Drugs::WeedBuyPrice);

			%weedbuyPrice = %input*$CRPG::Pref::Drugs::WeedBuyPrice;
			MessageClient(%client,'', "\c6You have bought \c3" @ %input @ "\c6 weed for \c3$"@ %weedbuyPrice @"\c6.");

			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));

			CRPGData.data[%client.bl_id].Value["Money"] -= %weedbuyPrice;
			CRPGData.data[%client.bl_id].Value["Weed"] += %input;
			
			commandtoclient(%client,'CRPGsetMoney',CRPGData.data[%client.bl_id].Value["Money"]);
			return;
		}
		if(%client.stage == 1.3)
		{
			if(%input <= 0)
			{
				MessageClient(%client,'', "\c6Please enter a valid amount of coke to sell.");
				return;
			}
			if(CRPGData.data[%client.bl_id].Value["Coke"] - %input < 1)
				%input = CRPGData.data[%client.bl_id].Value["Coke"];

			%cokeSellPrice = %input*$CRPG::Pref::Drugs::CokeSellPrice;

			MessageClient(%client,'', "\c6You have sold \c3" @ %input @ "\c6 coke for \c3$"@ %cokeSellPrice @"\c6.");

			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));

			CRPGData.data[%client.bl_id].Value["Money"] += %cokeSellPrice;
			CRPGData.data[%client.bl_id].Value["Coke"] -= %input;
			
			commandtoclient(%client,'CRPGsetMoney',CRPGData.data[%client.bl_id].Value["Money"]);
			return;
		}
		if(%client.stage == 1.4)
		{
			if(%input <= 0)
			{
				MessageClient(%client,'', "\c6Please enter a valid amount of weed to sell.");
				return;
			}
			if(CRPGData.data[%client.bl_id].Value["Weed"] - %input < 1)
				%input = CRPGData.data[%client.bl_id].Value["Weed"];

			%weedSellPrice = %input*$CRPG::Pref::Drugs::WeedSellPrice;

			MessageClient(%client,'', "\c6You have sold \c3" @ %input @ "\c6 weed for \c3$"@ %weedSellPrice @"\c6.");

			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));

			CRPGData.data[%client.bl_id].Value["Money"] += %weedSellPrice;
			CRPGData.data[%client.bl_id].Value["Weed"] -= %input;
			
			commandtoclient(%client,'CRPGsetMoney',CRPGData.data[%client.bl_id].Value["Money"]);
		}
	}
}