//+---------------------------------------------+
//| Title:	Events				|
//| Author:	Jasa				|
//+---------------------------------------------+
//| Events.				 	|
//+---------------------------------------------+
function fxDTSBrick::Funds(%brick, %service, %cost, %client)
{
	if(!%brick.lot)
	{
		%data = %brick.getDataBlock();
		if(mFloor(getWord(%brick.rotation, 3)) == 90)
			%boxSize = getWord(%data.brickSizeY, 1) / 2.5 SPC getWord(%data.brickSizeX, 0) / 2.5 SPC getWord(%data.brickSizeZ, 2) / 2.5;
		else
			%boxSize = getWord(%data.brickSizeX, 1) / 2.5 SPC getWord(%data.brickSizeY, 0) / 2.5 SPC getWord(%data.brickSizeZ, 2) / 2.5;
		initContainerBoxSearch(%brick.getWorldBoxCenter(), %boxSize, $typeMasks::triggerObjectType);
		
		while(isObject(%trigger = containerSearchNext()))
		{
			if(%trigger.getDatablock() == CRPGLotTriggerData.getID())
			{
				
				%brick.lot = %trigger.parent;
				break;
			}
		}
	}
	%lotdata = %brick.lot.getDataBlock();
	if(%lotdata.lotType !$= "Commercial" && %lotdata.lotType !$= "Industrial")
	{
		MessageClient(%client,'',"\c6This brick is not inside a commercial lot.");
		return;
	}
	commandtoclient(%client,'messageBoxYesNo',"CRPG Service", "Pay $"@ %cost @" for "@ %service @"?", 'acceptfunds');
	%client.player.activeTransferBrick = %brick;
	%client.player.activeTransferName = %service;
	%client.player.activeTransferCost = %cost;
}
function fxDTSBrick::GiveFood(%brick, %portions, %client)
{
	if(!%brick.lot)
	{
		%data = %brick.getDataBlock();
		if(mFloor(getWord(%brick.rotation, 3)) == 90)
			%boxSize = getWord(%data.brickSizeY, 1) / 2.5 SPC getWord(%data.brickSizeX, 0) / 2.5 SPC getWord(%data.brickSizeZ, 2) / 2.5;
		else
			%boxSize = getWord(%data.brickSizeX, 1) / 2.5 SPC getWord(%data.brickSizeY, 0) / 2.5 SPC getWord(%data.brickSizeZ, 2) / 2.5;
		initContainerBoxSearch(%brick.getWorldBoxCenter(), %boxSize, $typeMasks::triggerObjectType);
		
		while(isObject(%trigger = containerSearchNext()))
		{
			if(%trigger.getDatablock() == CRPGLotTriggerData.getID())
			{
				
				%brick.lot = %trigger.parent;
				break;
			}
		}
	}
	if(%brick.lot.getDatablock().lotType !$= "Commercial")
	{
		MessageClient(%client,'',"\c6This brick is not inside a commercial lot.");
		return;
	}
	%ownerID = %brick.getGroup().bl_id;
	if(!CRPGData.data[%ownerid].Value["JobID"].Food)
	{
		MessageClient(%client,'',"\c6The owner of this brick cannot sell food.");
		return;
	}
	if(CRPGData.data[%client.bl_id].Value["Hunger"]+%portions > 14)
	{
		MessageClient(%client,'',"\c6You are too full to eat any more.");
		return;
	}
	%cost = %portions*$CRPG::Pref::FoodPrice;
	if(%cost > CRPGData.data[%ownerid].Value["Bank"])
	{
		MessageClient(%client,'',"\c6The owner of this brick cannot afford any food.");
		return;
	}
	MessageClient(%client,'',"\c6You eat \c3"@ %portions @"\c6 portions of food.");

	CRPGData.data[%client.bl_id].Value["Hunger"] += %portions;
	CRPGData.data[%ownerid].Value["Bank"] -= %cost;

	commandtoclient(%client,'setCRPGHunger',CRPGData.data[%client.bl_id].Value["Hunger"]);

	MessageClient(%brick.getGroup().client,'',"\c6You have paid \c3$"@ %cost @"\c6 to give \c3"@ %portions @"\c6 portions of food to \c3"@ %client.name @"\c6.");
	%client.player.setCRPGScale();
}
function serverCmdAcceptFunds(%client)
{
	if(isObject(%client.player.activeTransferBrick))
	{
		if(isObject(%client.player))
		{
			if(mFloor(VectorDist(%client.player.activeTransferBrick.getPosition(), %client.player.getPosition())) < 16)
			{
				if(CRPGData.data[%client.bl_id].Value["Money"] >= %client.player.activeTransferCost)
				{
					%owner = %client.player.activeTransferBrick.getGroup().client;
					%ownerID = %client.player.activeTransferBrick.getGroup().bl_id;

					CRPGData.data[%client.bl_id].Value["Money"] -= %client.player.activeTransferCost;
					CRPGData.data[%ownerid].Value["Bank"] += %client.player.activeTransferCost;
					commandtoclient(%client,'setCRPGMoney',CRPGData.data[%client.bl_id].Value["Money"]);

					MessageClient(%client,'',"\c6You have payed \c3$"@ %client.player.activeTransferCost @"\c6 for \c3"@ %client.player.activeTransferName @"\c6.");
					MessageClient(%owner,'',"\c3"@ %client.name @"\c6 has payed you \c3$"@ %client.player.activeTransferCost @"\c6 for \c3"@ %client.player.activeTransferName @"\c6.");
					%client.player.activeTransferBrick.onTransferFunds(%client);
					if(%client != %owner)
					{
						if(CRPGData.data[%ownerid].Value["JobID"].SalesEXP)
						{
							if(CRPGData.data[%ownerid].Value["Experience"] < CRPGData.data[%ownerid].Value["JobID"].MaxExperience)
							{
								%a = getrandom(1,5000);
								if(%a <= %client.player.activeTransferCost)
								{
									CRPGData.data[%ownerid].Value["Experience"]++;
									messageclient(%owner,'',"\c6Your experience has increased to \c3"@ CRPGData.data[%ownerid].Value["Experience"] @"\c6.");
								}
							}
						}
					}
					%client.player.activeTransferBrick = "";
					%client.player.activeTransferName = "";
					%client.player.activeTransferCost = "";
				}
				else
					MessageClient(%client,'', "\c6You cannot afford this service.");
			}
			else
				MessageClient(%client,'', "\c6You are too far away from this service to buy it.");
		}
		else
			MessageClient(%client,'', "\c6You must spawn before buying this service.");
	}
	else
		MessageClient(%client,'', "\c6You have no active services you can buy.");
}
function fxDTSBrick::onEnterLot(%obj, %client)
{
	$InputTarget_["Self"] = %obj;
	$InputTarget_["Client"] = %client;
	$InputTarget_["MiniGame"] = CRPGMini;

	%obj.processInputEvent("OnEnterLot", %client);
}
function fxDTSBrick::onLeaveLot(%obj, %client)
{
	$InputTarget_["Self"] = %obj;
	$InputTarget_["Client"] = %client;
	$InputTarget_["MiniGame"] = CRPGMini;

	%obj.processInputEvent("OnLeaveLot", %client);
}
function fxDTSBrick::onTransferFunds(%obj, %client)
{
	$InputTarget_["Self"] = %obj;
	$InputTarget_["Client"] = %client;
	$InputTarget_["MiniGame"] = CRPGMini;

	%obj.processInputEvent("OnTransferFunds", %client);
}
function fxDTSBrick::GiveItem(%brick, %item, %client)
{
	if(!%item)
		return;
	if(!%brick.lot)
	{
		%data = %brick.getDataBlock();
		if(mFloor(getWord(%brick.rotation, 3)) == 90)
			%boxSize = getWord(%data.brickSizeY, 1) / 2.5 SPC getWord(%data.brickSizeX, 0) / 2.5 SPC getWord(%data.brickSizeZ, 2) / 2.5;
		else
			%boxSize = getWord(%data.brickSizeX, 1) / 2.5 SPC getWord(%data.brickSizeY, 0) / 2.5 SPC getWord(%data.brickSizeZ, 2) / 2.5;
		initContainerBoxSearch(%brick.getWorldBoxCenter(), %boxSize, $typeMasks::triggerObjectType);
		
		while(isObject(%trigger = containerSearchNext()))
		{
			if(%trigger.getDatablock() == CRPGLotTriggerData.getID())
			{
				
				%brick.lot = %trigger.parent;
				break;
			}
		}
	}
	if(%brick.lot.getDatablock().lotType !$= "Commercial")
	{
		MessageClient(%client,'',"\c6This brick is not inside a commercial lot.");
		return;
	}
	%ownerID = %brick.getGroup().bl_id;
	if(CRPGData.data[%ownerid].Value["JobID"].spawnItems)
	{
		%itemname = trim(%item.UIname);
		if($CRPG::Item::Price[%itemname])
		{
			%discount = CRPGData.data[%ownerid].Value["JobID"].itemDiscount;
			if(CRPGData.data[%ownerid].Value["Bank"] >= $CRPG::Item::Price[%itemname]*%discount)
			{
				%player = %client.player;
				for(%i = 0; %i < $CRPG::Pref::MaxTools; %i++)
				{
					if(!%player.tool[%i])
					{
						%player.tool[%i] = %item;
						messageClient(%client,'MsgItemPickup','',%i,%item);
						CRPGData.data[%ownerid].Value["Bank"] -= $CRPG::Item::Price[%itemname]*%discount;
						MessageClient(%brick.getGroup().client,'',"\c6You have paid \c3$"@ $CRPG::Item::Price[%itemname]*%discount @"\c6 to give a \c3"@ %itemname @"\c6 to \c3"@ %client.name @"\c6.");
						return;
					}
				}
				MessageClient(%client,'',"\c6You did not have an open slot for a \c3"@ %itemname @"\c6.");
			}
			else
				MessageClient(%client,'',"\c6The owner of this brick cannot afford a \c3"@ %itemname @"\c6.");
		}
		else
			MessageClient(%client,'',"\c6The owner of this brick cannot spawn a \c3"@ %itemname @"\c6.");
	}
	else
		MessageClient(%client,'',"\c6The owner of this brick cannot spawn items.");
}