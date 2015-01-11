//datablock fxDtsBrickData(MatPBrickData)
//{
//	category = "CRPG";
//	subCategory = "Terminals";
//	uiName = "Materials Pickup";
//	bricktype = "Terminal";
//	
//	triggerDatablock = CRPGInputTriggerData;
//	triggerSize = "7 10 4";
//	trigger = 0;
//	
//	adminOnly = true;
//
//	brickSizeX = 1;
//	brickSizeY = 4;
//	brickSizeZ = 8;
//};
//function MatPBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
//{	
//	if(%triggerStatus !$= "")
//	{
//		if(%triggerStatus && %client.stage $= "")
//		{
//			if(!CRPGData.data[%client.bl_id].value["Money"])
//			{
//				MessageClient(%client,'',"\c6You're broke. Get out of here and get some money. Gooby pls.");
//				%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));
//				return;
//			}
//			if(!CRPGData.data[%client.bl_id].value["JobID"].buyMats)
//			{
//				MessageClient(%client,'', "\c6You cant buy materials with your current job.");
//				%client.stage = "";
//				return;
//			}
//			MessageClient(%client,'', "\c6What do you want to do?");
//
//			if(CRPGData.data[%client.bl_id].Value["Money"] >= $CRPG::Pref::Matpacks && CRPGData.data[%client.bl_id].value["JobID"].buyMats)
//				MessageClient(%client,'', "\c61 - Buy a \c0Materials \c6package. Deliver this to a materials dropoff point, usually located in factories.");
//			return;
//		}
//		MessageClient(%client,'', "\c6See ya.");
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
//			if(CRPGData.data[%client.bl_id].Value["Money"] >= $CRPG::Pref::MatpackBuyPrice && !CRPGData.data[%client.bl_id].value["Matpacks"])
//			{
//				%client.stage = 1.1;
//				CRPGData.data[%client.bl_id].Value["Money"] -= %MatpackBuyPrice;
//				CRPGData.data[%client.bl_id].Value["Matpacks"] += 1;
//				MessageClient(%client,'', "\c6You just bought a materials package.");
//				return;
//			}
//			MessageClient(%client,'', "\c6You can't do that right now. Get $250 on hand and 0 Materials Packages, then come back. fagt.");
//		}
//		
//		MessageClient(%client,'', "\c3" @ %text @ " \c6is not a valid option fagt.");
//		return;
//	}
//}