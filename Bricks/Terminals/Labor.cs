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
	
	triggerDatablock = CRPInputTriggerData;
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
		colorShift[1]= "1 0.5 0 1";
};
//function LaborBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
//{	
//	if(%triggerStatus !$= "")
//	{
//		if(%triggerStatus && %client.stage $= "")
//		{
//			if(!CRPGData.data[%client.bl_id].Value["Lumber"] && !CRPGData.data[%client.bl_id].Value["Ore"] && (CRPGData.data[%client.bl_id].Value["Money"] < $CRPG::Pref::LumberBuyPrice) && (CRPGData.data[%client.bl_id].Value["Money"] < $CRPG::Pref::OreBuyPrice))
//			{
//				MessageClient(%client,'',"\c6You don't have any money to buy resources or resources to sell. Gooby pls.");
//				%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
//				return;
//			}
//			MessageClient(%client,'', "\c6Please type the number corresponding to the options below.");
//
//			if(CRPGData.data[%client.bl_id].Value["Iron"] || CRPGData.data[%client.bl_id].Value["Silver"] || CRPGData.data[%client.bl_id].Value["Platinum"])
//				MessageClient(%client,'', "\c6 - You have \c3"@ CRPGData.data[%client.bl_id].Value["Iron"] @"\c6 Iron Ore, \c3"@ CRPGData.data[%client.bl_id].Value["Silver"] @"\c6 Silver Ore, and \c3"@ CRPGData.data[%client.bl_id].Value["Platinum"] @"\c6 Platinum Ore.");
//			
//			if(CRPGData.data[%client.bl_id].Value["Oak"] || CRPGData.data[%client.bl_id].Value["Maple"] || CRPGData.data[%client.bl_id].Value["Morning"])
//				MessageClient(%client,'', "\c6 - You also have \c3"@ CRPGData.data[%client.bl_id].Value["Oak"] @"\c6 Oak Wood, \c3"@ CRPGData.data[%client.bl_id].Value["Maple"] @"\c6 Maple Wood, and \c3"@ CRPGData.data[%client.bl_id].Value["Morning"] @"\c6 Morning Wood.");	
//
//			if(CRPGData.data[%client.bl_id].Value["Oak"] & CRPGData.data[%client.bl_id].Value["Maple"] & CRPGData.data[%client.bl_id].Value["Morning"])
//				MessageClient(%client,'', "\c31 \c6- Sell Lumber");
//
//			if(CRPGData.data[%client.bl_id].Value["Iron"] & CRPGData.data[%client.bl_id].Value["Silver"] & CRPGData.data[%client.bl_id].Value["Platinum"])
//				MessageClient(%client,'', "\c22 \c6- Sell Ore");
//			
//			if(CRPGData.data[%client.bl_id].Value["Money"] >= $CRPG::Pref::MatsBuyPrice && CRPGData.data[%client.bl_id].data.value["JobID"].buyMats)
//				MessageClient(%client,'', "\c33 \c6- Buy Materials");
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
//		if(%input == 907)
//		{
//			if(CRPGData.data[%client.bl_id].Value["Money"] >= $CRPG::Pref::LumberBuyPrice)
//			{
//				%client.stage = 1.1;
//				%lumbercanbuy = mfloor(CRPGData.data[%client.bl_id].Value["Money"]/$CRPG::Pref::LumberBuyPrice);
//				MessageClient(%client,'', "\c6Please enter the amount of lumber you wish to buy. You can buy \c3"@ %lumbercanbuy @"\c6 lumber for \c3$"@ mceil(%lumbercanbuy*$CRPG::Pref::LumberBuyPrice) @"\c6.");
//				return;
//			}
//		}
//		if(%input == 709)
//		{
//			if(CRPGData.data[%client.bl_id].Value["Money"] >= $CRPG::Pref::OreBuyPrice)
//			{
//				%client.stage = 1.2;
//				%orecanbuy = mfloor(CRPGData.data[%client.bl_id].Value["Money"]/$CRPG::Pref::OreBuyPrice);
//				MessageClient(%client,'', "\c6Please enter the amount of ore you wish to buy. You can buy \c3"@ %orecanbuy @"\c6 ore for \c3$"@ mceil(%orecanbuy*$CRPG::Pref::OreBuyPrice) @"\c6.");
//				return;
//			}
//			
//		}
//		if(%input == 1)
//		{
//			if(CRPGData.data[%client.bl_id].Value["Oak"] & CRPGData.data[%client.bl_id].Value["Maple"] & CRPGData.data[%client.bl_id].Value["Morning"])
//			{
//				%oakpls = CRPGData.data[%client.bl_id].Value["Oak"];
//				%maplepls = CRPGData.data[%client.bl_id].Value["Maple"];
//				%morningpls = CRPGData.data[%client.bl_id].Value["Morning"];
//				%client.stage = 1.3;
//				MessageClient(%client,'', "\c6Please enter the amount of wood you wish to sell. You can sell \c3"@ CRPGData.data[%client.bl_id].Value["Oak"] @"\c6 Oak Wood for \c3$"@ mfloor(CRPGData.data[%client.bl_id].Value["Oak"]*$CRPG::Pref::OakSellPrice) @"\c6.");
//				return;
//			}
//		}
//		if(%input == 2)
//		{
//			if(CRPGData.data[%client.bl_id].Value["Ore"] & CRPGData.data[%client.bl_id].Value["Silver"] & CRPGData.data[%client.bl_id].Value["Platinum"])
//			{
//				%client.stage = 1.4;
//				MessageClient(%client,'', "\c6Please enter the amount of ore you wish to sell. You can sell \c3"@ CRPGData.data[%client.bl_id].Value["Ore"] @"\c6 ore for \c3$"@ mfloor(CRPGData.data[%client.bl_id].Value["Ore"]*$CRPG::Pref::OreSellPrice) @"\c6.");
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
//			if(CRPGData.data[%client.bl_id].Value["Money"] - mceil(%input*$CRPG::Pref::LumberBuyPrice) < 1)
//				%input = mfloor(CRPGData.data[%client.bl_id].Value["Money"]/$CRPG::Pref::LumberBuyPrice);
//
//			%lumberbuyPrice = mceil(%input*$CRPG::Pref::LumberBuyPrice);
//			MessageClient(%client,'', "\c6You have bought \c3" @ %input @ "\c6 lumber for \c3$"@ %lumberbuyPrice @"\c6.");
//
//			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
//
//			CRPGData.data[%client.bl_id].Value["Money"] -= %lumberbuyPrice;
//			CRPGData.data[%client.bl_id].Value["Lumber"] += %input;
//			
//			commandtoclient(%client,'CRPGsetMoney',CRPGData.data[%client.bl_id].Value["Money"]);
//			return;
//		}
//		if(%client.stage == 1.2)
//		{
//			if(%input <= 0)
//			{
//				MessageClient(%client,'', "\c6Please enter a valid amount of ore to buy.");
//				return;
//			}
//			if(CRPGData.data[%client.bl_id].Value["Money"] - mceil(%input*$CRPG::Pref::OreBuyPrice) < 1)
//				%input = mfloor(CRPGData.data[%client.bl_id].Value["Money"]/$CRPG::Pref::OreBuyPrice);
//
//			%orebuyPrice = mceil(%input*$CRPG::Pref::OreBuyPrice);
//			MessageClient(%client,'', "\c6You have bought \c3" @ %input @ "\c6 ore for \c3$"@ %orebuyPrice @"\c6.");
//
//			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
//
//			CRPGData.data[%client.bl_id].Value["Money"] -= %orebuyPrice;
//			CRPGData.data[%client.bl_id].Value["Ore"] += %input;
//			
//			commandtoclient(%client,'CRPGsetMoney',CRPGData.data[%client.bl_id].Value["Money"]);
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
//			%LumberSellPrice = mfloor(%input*$CRPG::Pref::LumberSellPrice);
//
//			MessageClient(%client,'', "\c6You have sold \c3" @ %input @ "\c6 lumber for \c3$"@ %LumberSellPrice @"\c6.");
//
//			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
//
//			CRPGData.data[%client.bl_id].Value["Money"] += %LumberSellPrice;
//			CRPGData.data[%client.bl_id].Value["Lumber"] -= %input;
//			
//			commandtoclient(%client,'CRPGsetMoney',CRPGData.data[%client.bl_id].Value["Money"]);
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
//			%OreSellPrice = mfloor(%input*$CRPG::Pref::OreSellPrice);
//
//			MessageClient(%client,'', "\c6You have sold \c3" @ %input @ "\c6 ore for \c3$"@ %OreSellPrice @"\c6.");
//
//			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
//
//			CRPGData.data[%client.bl_id].Value["Money"] += %OreSellPrice;
//			CRPGData.data[%client.bl_id].Value["Ore"] -= %input;
//			
//			commandtoclient(%client,'CRPGsetMoney',CRPGData.data[%client.bl_id].Value["Money"]);
//		}
//	}
//}

function gameConnection::buyResources(%client)
{
	%pricetag = CRPGData.Data[%client.bl_id].value["Oak"] * $CRPG::Prefs::OakSellPrice + CRPGData.Data[%client.bl_id].value["Maple"] * $CRPG::Prefs::MapleSellPrice + CRPGData.Data[%client.bl_id].value["Morning"] * $CRPG::Prefs::MorningSellPrice + CRPGData.Data[%client.bl_id].value["Iron"] * $CRPG::Prefs::IronSellPrice + CRPGData.Data[%client.bl_id].value["Silver"] * $CRPG::Prefs::SilverSellPrice + CRPGData.Data[%client.bl_id].value["Platinum"] * $CRPG::Prefs::PlatinumSellPrice;
	talk("asdf");	
	if(%pricetag > 0)
	{	
        talk("asdf2");
		if(!getWord(CRPGData.Data[%client.bl_id].value["JailData"]))
		{
			CRPGData.Data[%client.bl_id].value["Money"] += %pricetag;
			messageClient(%client, '', "\c6The state has bought all of your resources for \c3$" @ %pricetag @ "\c6.");
		}
		else
		{
			CRPGData.Data[%client.bl_id].value["Bank"] += %pricetag;
			messageClient(%client, '', '\c6The state has set aside \c3$%1\c6 for when you get out of Prison.', %pricetag);
		}
		
		%errythang = CRPGData.Data[%client.bl_id].value["Oak"] + CRPGData.Data[%client.bl_id].value["Maple"] + CRPGData.Data[%client.bl_id].value["Morning"] + CRPGData.Data[%client.bl_id].value["Iron"] + CRPGData.Data[%client.bl_id].value["Silver"] + CRPGData.Data[%client.bl_id].value["Platinum"];
		
	    $CRPG::Data::Materials += %errythang;
		$CRPG::Data::Money -= %pricetag;
		CRPGData.Data[%client.bl_id].value["Oak"] = "0";
		CRPGData.Data[%client.bl_id].value["Maple"] = "0";
		CRPGData.Data[%client.bl_id].value["Morning"] = "0";
		CRPGData.Data[%client.bl_id].value["Iron"] = "0";
		CRPGData.Data[%client.bl_id].value["Silver"] = "0";
		CRPGData.Data[%client.bl_id].value["Platinum"] = "0";
		
		
		%client.SetInfo();	
	}
}


// ============================================================
// Section 2 : Trigger Data
// ============================================================
function CityRPGLaborBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	%client.buyResources();
}