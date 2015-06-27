//+---------------------------------------------+
//| Title:	Labor				|
//| Author:	Jasa				|
//+---------------------------------------------+
//| Buy and sell resources.			|
//+---------------------------------------------+
datablock fxDtsBrickData(LaborBrickData)
{
	category = "CRPG";
	subCategory = "Terminals";
	uiName = "Labor";
	bricktype = "Terminal";
	
	brickFile   = "Add-ons/Gamemode_CRPG/Shapes/Bricks/TerminalLightBlue.blb";
	
	triggerDatablock = CRPInputTriggerData;
	triggerSize = "7 10 4";
	trigger = 0;
	
	adminOnly = true;

	brickSizeX = 1;
	brickSizeY = 4;
	brickSizeZ = 8;
};
function serverCmdSellResources(%client)
{
	%ore = CRPGData.data[%client.bl_id].Value["Iron"] * $CRPG::Pref::IronSellPrice + CRPGData.data[%client.bl_id].Value["Silver"]*$CRPG::Pref::SilverSellPrice+CRPGData.data[%client.bl_id].Value["Platinum"]*$CRPG::Pref::PlatinumSellPrice;
	%wood = CRPGData.data[%client.bl_id].Value["Oak"] * $CRPG::Pref::OakSellPrice + CRPGData.data[%client.bl_id].Value["Maple"]*$CRPG::Pref::MapleSellPrice+CRPGData.data[%client.bl_id].Value["Morning"]*$CRPG::Pref::MorningSellPrice;
	if(!CRPGData.data[%client.bl_id].Value["Lumber"] && !CRPGData.data[%client.bl_id].Value["Ore"] && (CRPGData.data[%client.bl_id].Value["Money"] < $CRP::Pref::LumberBuyPrice) && (CRPGData.data[%client.bl_id].Value["Money"] < $CRP::Pref::OreBuyPrice))
	{
		MessageClient(%client,'',"\c6You don't have any resources to sell.");
	}
	if(CRPGData.data[%client.bl_id].Value["Oak"] || CRPGData.data[%client.bl_id].Value["Maple"] || CRPGData.data[%client.bl_id].Value["Morning"] || CRPGData.data[%client.bl_id].Value["Iron"] || CRPGData.data[%client.bl_id].Value["Silver"] || CRPGData.data[%client.bl_id].Value["Platinum"])
		MessageClient(%client,'', "\c6You have \c3"@ CRPGData.data[%client.bl_id].Value["Oak"] @"\c6 oak, \c3"@ CRPGData.data[%client.bl_id].Value["Maple"] @"\c6 maple, and \c3"@ CRPGData.data[%client.bl_id].Value["Morning"] @"\c6 morning wood. You have \c3"@ CRPGData.data[%client.bl_id].Value["Iron"] @"\c6 iron, \c3"@ CRPGData.data[%client.bl_id].Value["Silver"] @"\c6 silver, and \c3"@ CRPGData.data[%client.bl_id].Value["Platinum"] @"\c6 platinum ore.");
	
	if(CRPGData.data[%client.bl_id].Value["Oak"] || CRPGData.data[%client.bl_id].Value["Maple"] || CRPGData.data[%client.bl_id].Value["Morning"])
		MessageClient(%client,'', "\c6- Your wood is worth $"@ %wood @".");
	
	if(CRPGData.data[%client.bl_id].Value["Iron"] || CRPGData.data[%client.bl_id].Value["Silver"] || CRPGData.data[%client.bl_id].Value["Platinum"])
		MessageClient(%client,'', "\c6- Your ore is worth $"@ %ore @".");
	
	MessageClient(%client,'', "\c3If you want to sell your resources, do /sellresourcesyes");
}

function serverCmdSellResourcesYes(%client)
{
	if(CRPGData.data[%client.bl_id].Value["Oak"] || CRPGData.data[%client.bl_id].Value["Maple"] || CRPGData.data[%client.bl_id].Value["Morning"] || CRPGData.data[%client.bl_id].Value["Iron"] || CRPGData.data[%client.bl_id].Value["Silver"] || CRPGData.data[%client.bl_id].Value["Platinum"])
	{
	%sell = (CRPGData.data[%client.bl_id].Value["Iron"] * $CRPG::Pref::IronSellPrice)+(CRPGData.data[%client.bl_id].Value["Silver"]*$CRPG::Pref::SilverSellPrice)+(CRPGData.data[%client.bl_id].Value["Platinum"]*$CRPG::Pref::PlatinumSellPrice)+(CRPGData.data[%client.bl_id].Value["Oak"]*$CRPG::Pref::OakSellPrice)+(CRPGData.data[%client.bl_id].Value["Maple"]*$CRPG::Pref::MapleSellPrice)+(CRPGData.data[%client.bl_id].Value["Morning"]*$CRPG::Pref::MorningSellPrice);
	CRPGData.data[%client.bl_id].value["Money"] += %sell;
	MessageClient(%client,'', "\c3Any resources on your character have been sold.");
		CRPGData.data[%client.bl_id].value["Iron"] = 0;
		CRPGData.data[%client.bl_id].value["Silver"] = 0;
		CRPGData.data[%client.bl_id].value["Morning"] = 0;
		CRPGData.data[%client.bl_id].value["Oak"] = 0;
		CRPGData.data[%client.bl_id].value["Maple"] = 0;
		CRPGData.data[%client.bl_id].value["Platinum"] = 0;
	}
}
//function LaborBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
//{	
//	if(%triggerStatus !$= "")
//	{
//		if(%triggerStatus && %client.stage $= "")
//		{
//			%ore = (CRPGData.data[%client.bl_id].Value["Iron"]*$CRP::Pref::IronSellPrice)+(CRPGData.data[%client.bl_id].Value["Silver"]*$CRP::Pref::SilverSellPrice)+(CRPGData.data[%client.bl_id].Value["Platinum"]*$CRP::Pref::PlatinumSellPrice);
//			%wood = (CRPGData.data[%client.bl_id].Value["Oak"]*$CRP::Pref::OakSellPrice)+(CRPGData.data[%client.bl_id].Value["Maple"]*$CRP::Pref::MapleSellPrice)+(CRPGData.data[%client.bl_id].Value["Morning"]*$CRP::Pref::MorningSellPrice);
//			if(!CRPGData.data[%client.bl_id].Value["Lumber"] && !CRPGData.data[%client.bl_id].Value["Ore"] && (CRPGData.data[%client.bl_id].Value["Money"] < $CRP::Pref::LumberBuyPrice) && (CRPGData.data[%client.bl_id].Value["Money"] < $CRP::Pref::OreBuyPrice))
//			{
//				MessageClient(%client,'',"\c6You don't have any resources to sell.");
//				return;
//			}
//			if(CRPGData.data[%client.bl_id].Value["Oak"] || CRPGData.data[%client.bl_id].Value["Maple"] || CRPGData.data[%client.bl_id].Value["Morning"] || CRPGData.data[%client.bl_id].Value["Iron"] || CRPGData.data[%client.bl_id].Value["Silver"] || CRPGData.data[%client.bl_id].Value["Platinum"])
//				MessageClient(%client,'', "\c6 - You have \c3"@ CRPGData.data[%client.bl_id].Value["Oak"] @"\c6 iron,\c3"@ CRPGData.data[%client.bl_id].Value["Maple"] @"\c6 maple, \c3and"@ CRPGData.data[%client.bl_id].Value["Morning"] @"\c6 morning wood. You have"@ CRPGData.data[%client.bl_id].Value["Iron"] @"\c6 iron,\c3"@ CRPGData.data[%client.bl_id].Value["Silver"] @"\c6 silver, \c3and"@ CRPGData.data[%client.bl_id].Value["Platinum"] @"\c6 platinum ore.");
//
//			if(CRPGData.data[%client.bl_id].Value["Oak"] || CRPGData.data[%client.bl_id].Value["Maple"] || CRPGData.data[%client.bl_id].Value["Morning"])
//				MessageClient(%client,'', "\c31 \c6- Your wood is worth $"@ %wood @".");
//
//			if(CRPGData.data[%client.bl_id].Value["Iron"] || CRPGData.data[%client.bl_id].Value["Silver"] || CRPGData.data[%client.bl_id].Value["Platinum"])
//				MessageClient(%client,'', "\c31 \c6- Your ore is worth $"@ %ore @".");
//
//			MessageClient(%client,'', "If you want to sell your shit, do /sellit
//			return;
//		}
//		MessageClient(%client,'', "\c6Thanks, come again.");
//		%client.stage = "";
//		return;
//	}
//
//	%input = mfloor(%text);
//	%stage = mfloor(%client.stage);
//	if(!%stage)
//	{
//		if(%input == 1)
//		{
//			if(CRPGData.data[%client.bl_id].Value["Money"] >= $CRP::Pref::LumberBuyPrice)
//			{
//				%client.stage = 1.1;
//				%lumbercanbuy = mfloor(CRPGData.data[%client.bl_id].Value["Money"]/$CRP::Pref::LumberBuyPrice);
//				MessageClient(%client,'', "\c6Please enter the amount of lumber you wish to buy. You can buy \c3"@ %lumbercanbuy @"\c6 lumber for \c3$"@ mceil(%lumbercanbuy*$CRP::Pref::LumberBuyPrice) @"\c6.");
//				return;
//			}
//		}
//		if(%input == 2)
//		{
//			if(CRPGData.data[%client.bl_id].Value["Money"] >= $CRP::Pref::OreBuyPrice)
//			{
//				%client.stage = 1.2;
//				%orecanbuy = mfloor(CRPGData.data[%client.bl_id].Value["Money"]/$CRP::Pref::OreBuyPrice);
//				MessageClient(%client,'', "\c6Please enter the amount of ore you wish to buy. You can buy \c3"@ %orecanbuy @"\c6 ore for \c3$"@ mceil(%orecanbuy*$CRP::Pref::OreBuyPrice) @"\c6.");
//				return;
//			}
//			
//		}
//		if(%input == 3)
//		{
//			if(CRPGData.data[%client.bl_id].Value["Lumber"])
//			{
//				%client.stage = 1.3;
//				MessageClient(%client,'', "\c6Please enter the amount of lumber you wish to sell. You can sell \c3"@ CRPGData.data[%client.bl_id].Value["Lumber"] @"\c6 lumber for \c3$"@ mfloor(CRPGData.data[%client.bl_id].Value["Lumber"]*$CRP::Pref::LumberSellPrice) @"\c6.");
//				return;
//			}
//		}
//		if(%input == 4)
//		{
//			if(CRPGData.data[%client.bl_id].Value["Ore"])
//			{
//				%client.stage = 1.4;
//				MessageClient(%client,'', "\c6Please enter the amount of ore you wish to sell. You can sell \c3"@ CRPGData.data[%client.bl_id].Value["Ore"] @"\c6 ore for \c3$"@ mfloor(CRPGData.data[%client.bl_id].Value["Ore"]*$CRP::Pref::OreSellPrice) @"\c6.");
//				return;
//			}
//		}
//		MessageClient(%client,'', "\c3" @ %text @ " \c6is not a valid option.");
//		return;
//	}
//	if(%stage)
//	{
//		if(%client.stage == 1.1)
//		{
//			if(%input <= 0)
//			{
//				MessageClient(%client,'', "\c6Please enter a valid amount of lumber to buy.");
//				return;
//			}
//			if(CRPGData.data[%client.bl_id].Value["Money"] - mceil(%input*$CRP::Pref::LumberBuyPrice) < 1)
//				%input = mfloor(CRPGData.data[%client.bl_id].Value["Money"]/$CRP::Pref::LumberBuyPrice);
//
//			%lumberbuyPrice = mceil(%input*$CRP::Pref::LumberBuyPrice);
//			MessageClient(%client,'', "\c6You have bought \c3" @ %input @ "\c6 lumber for \c3$"@ %lumberbuyPrice @"\c6.");
//
//			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
//
//			CRPGData.data[%client.bl_id].Value["Money"] -= %lumberbuyPrice;
//			CRPGData.data[%client.bl_id].Value["Lumber"] += %input;
//			
//			commandtoclient(%client,'setMoney',CRPGData.data[%client.bl_id].Value["Money"]);
//			return;
//		}
//		if(%client.stage == 1.2)
//		{
//			if(%input <= 0)
//			{
//				MessageClient(%client,'', "\c6Please enter a valid amount of ore to buy.");
//				return;
//			}
//			if(CRPGData.data[%client.bl_id].Value["Money"] - mceil(%input*$CRP::Pref::OreBuyPrice) < 1)
//				%input = mfloor(CRPGData.data[%client.bl_id].Value["Money"]/$CRP::Pref::OreBuyPrice);
//
//			%orebuyPrice = mceil(%input*$CRP::Pref::OreBuyPrice);
//			MessageClient(%client,'', "\c6You have bought \c3" @ %input @ "\c6 ore for \c3$"@ %orebuyPrice @"\c6.");
//
//			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
//
//			CRPGData.data[%client.bl_id].Value["Money"] -= %orebuyPrice;
//			CRPGData.data[%client.bl_id].Value["Ore"] += %input;
//			
//			commandtoclient(%client,'setMoney',CRPGData.data[%client.bl_id].Value["Money"]);
//			return;
//		}
//		if(%client.stage == 1.3)
//		{
//			if(%input <= 0)
//			{
//				MessageClient(%client,'', "\c6Please enter a valid amount of lumber to sell.");
//				return;
//			}
//			if(CRPGData.data[%client.bl_id].Value["Lumber"] - %input < 1)
//				%input = CRPGData.data[%client.bl_id].Value["Lumber"];
//
//			%LumberSellPrice = mfloor(%input*$CRP::Pref::LumberSellPrice);
//
//			MessageClient(%client,'', "\c6You have sold \c3" @ %input @ "\c6 lumber for \c3$"@ %LumberSellPrice @"\c6.");
//
//			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
//
//			CRPGData.data[%client.bl_id].Value["Money"] += %LumberSellPrice;
//			CRPGData.data[%client.bl_id].Value["Lumber"] -= %input;
//			
//			commandtoclient(%client,'setMoney',CRPGData.data[%client.bl_id].Value["Money"]);
//			return;
//		}
//		if(%client.stage == 1.4)
//		{
//			if(%input <= 0)
//			{
//				MessageClient(%client,'', "\c6Please enter a valid amount of ore to sell.");
//				return;
//			}
//			if(CRPGData.data[%client.bl_id].Value["Ore"] - %input < 1)
//				%input = CRPGData.data[%client.bl_id].Value["Ore"];
//
//			%OreSellPrice = mfloor(%input*$CRP::Pref::OreSellPrice);
//
//			MessageClient(%client,'', "\c6You have sold \c3" @ %input @ "\c6 ore for \c3$"@ %OreSellPrice @"\c6.");
//
//			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
//
//			
//			CRPGData.data[%client.bl_id].Value["Money"] += %OreSellPrice;
//			CRPGData.data[%client.bl_id].Value["Ore"] -= %input;
//			
//			commandtoclient(%client,'setMoney',CRPGData.data[%client.bl_id].Value["Money"]);
//		}
//	}
//}