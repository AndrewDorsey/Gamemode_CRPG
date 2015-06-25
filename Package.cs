//+---------------------------------------------+
//| Title:	Package				|
//| Author:	Jasa				|
//+---------------------------------------------+
//| Changes some functions of Blockland. 	|
//+---------------------------------------------+

package CRPG
{
	function ServerCmdClearAllbricks(%client)
	{
		if(%client.isSuperAdmin)
			Parent::ServerCmdClearAllbricks(%client, %target);
		else
			MessageClient(%client,'',"\c6You must be \c3Super Admin\c6 to clear all bricks.");
	}
	function gameConnection::AutoAdminCheck(%client)
	{
		if(!%client.CRPGjoin)
		{
			if(!$CRPG::FirstConnect)
			{
				ClearSkyObjects();
				$CRPG::FirstConnect = 1;
				ChangeSky($CRPG::Data::Weather @ $CRPG::Data::Hour @"_1");
			}
			commandtoclient(%client, 'CRPGPing');
			CRPGData.addData(%client.bl_id);
			if(!isFile("Config/Server/CRPG/Data/"@ %client.bl_id @".dat"))
				CRPGData.data[%client.bl_id].Value["Height"] = getrandom(90,110)*0.01;

			CRPGData.data[%client.bl_id].Value["Name"] = %client.name;
			CRPGData.data[%client.bl_id].Value["LastOnline"] = "Currently";
			for(%i=0;%i<$Server::PlayerCount;%i++)
			{
				%subClient = clientGroup.getObject(%i);
				if(%subClient.bl_id == %client.bl_id && %subClient != %client)
				{
					echo(%client.name @" was kicked for multi-clienting.");
					%subClient.delete("You have been kicked for multiclienting.");
					break;
				}
			}
			if(CRPGData.data[%client.bl_id].Value["Bank"] > $CRPG::Data::RichestBank)
			{
				$CRPG::Data::RichestBank = CRPGData.data[%client.bl_id].Value["Bank"];
				$CRPG::Data::RichestUser = %client.name;
			}
			%client.schedule(5000,"CheckCRPGClient");
			%client.CRPGjoin = 1;
		}
		return parent::AutoAdminCheck(%client);
	}
	function gameConnection::onClientEnterGame(%client)
	{
		parent::onClientEnterGame(%client);

		CRPGMini.addMember(%client);
		%client.setScore(CRPGData.data[%client.bl_id].Value["Bank"]);

		if(CRPGData.data[%client.bl_id].Value["Transform"] && !CRPGData.data[%client.bl_id].Value["JailData"])
			%client.player.setTransform(CRPGData.data[%client.bl_id].Value["Transform"]);
		%client.player.setdamagelevel(CRPGData.data[%client.bl_id].Value["Damage"]);

		centerPrint(%client, "<bitmap:Add-Ons/Gamemode_CRPG/Welcome.png>", 10);

	//	if(!%client.hasClient)
	//		messageclient(%client,'',"\c6You need to <a:rtb-3823>Download the CRPG Client</a>\c6 to view your data.");
		commandtoclient(%client, 'CRPGOpenClient');
	}
	function gameConnection::onClientLeaveGame(%client)
	{
		%client.saveCRPGdata();
		CRPGData.data[%client.bl_id].Value["LastOnline"] = getDateTime();
		CRPGData.data[%client.bl_id].Value["Address"] = %client.getRawIP();
		commandToClient(%client, 'CRPGCloseClient');
		parent::onClientLeaveGame(%client);
	}
	function gameConnection::spawnPlayer(%client)
	{
		parent::SpawnPlayer(%client);
		%client.schedule(600,'setInfo');
		schedule(600,"playThread",3,"headUp");
		%client.player.giveDefaultTools();
		%client.player.setCRPGScale();
		%client.player.setShapeNameDistance("32");
		commandToClient(%client, 'CRPGOpenClient');
		commandtoclient(%client, 'CRPGSetMoney',CRPGData.data[%client.bl_id].Value["Money"]);
		commandtoclient(%client, 'CRPGSetHunger',CRPGData.data[%client.bl_id].Value["Hunger"]);
		commandtoclient(%client, 'CRPGSetDems', CRPGData.data[%client.bl_id].Value["Demerits"]);
		commandtoclient(%client, 'CRPGSetIncome', CRPGData.Data[%client.bl_id].Value["JobID"].Income);
		commandtoclient(%client, 'CRPGSetJob', CRPGData.data[%client.bl_id].Value["JailData"], CRPGData.data[%client.bl_id].Value["Student"], CRPGData.Data[%client.bl_id].Value["JobID"].JobName);
	}
	function player::damage(%player, %obj, %pos, %damage, %damageType)
	{
		if(isObject(%obj.client) && isObject(%player.client))
		{
			if(%player.client != %obj.client)
			{
				if(%player.getDamageLevel() < %player.getDatablock().maxDamage)
				{
					if(CRPGData.data[%player.client.bl_id].Value["Demerits"] < 200 && %player.client.LastAttack[%obj.client] < 20)
					{
						MessageClient(%obj.client,'',"\c6You have commited a crime. [\c3Assault\c6]");
						CRPGData.data[%obj.client.bl_id].Value["Demerits"] += mfloor(%damage*2);
						commandtoclient(%obj.client,'CRPGSetDems', CRPGData.data[%obj.client.bl_id].Value["Demerits"]);
						%obj.client.LastAttack[%player.client] += %damage;
					}
				}
			}
		}
		parent::damage(%player, %obj, %pos, %damage, %damageType);
		%player.setEnergyLevel(%player.getDatablock().maxDamage-%player.getDamageLevel());
	}
	function gameConnection::onDeath(%client, %killerplayer, %killer, %damageType, %damageLoc)
	{
		serverCmddropTool(%client, %client.player.currTool);
		if(%client.CRPGTrigger)
			%client.CRPGTrigger.getDatablock().onLeaveTrigger(%client.CRPGTrigger, %client.player);
		if(%killer != %client)
		{

			if(CRPGData.data[%client.bl_id].Value["Bounty"])
			{
				MessageClient(%killer,'', "\c6Hit successful. \c3$"@ CRPGData.data[%client.bl_id].Value["Bounty"] @"\c6 has been wired to your bank account.");
				if(CRPGData.data[%jailer.bl_id].Value["Experience"] < CRPGData.data[%jailer.bl_id].Value["JobID"].maxExperience)
				{
					if(getRandom(0,1000) <= CRPGData.data[%client.bl_id].Value["Bounty"])
					{
						CRPGData.data[%jailer.bl_id].Value["Experience"]++;
						MessageClient(%jailer,'',"\c6Your experience has increased to \c3"@ CRPGData.data[%jailer.bl_id].Value["Experience"] @"\c6.");
					}
				}
				CRPGData.data[%killer.bl_id].Value["Bank"] += CRPGData.data[%client.bl_id].Value["Bounty"];
				CRPGData.data[%client.bl_id].Value["Bounty"] = 0;
			}
			if(CRPGData.data[%client.bl_id].Value["Demerits"] < 200 && %client.LastAttack[%killer.player] < 20)
			{
				if(WitnessTest(%client,%killer))
				{
					MessageClient(%killer,'', "\c6You have commited a crime. [\c3Murder\c6]");
					CRPGData.data[%killer.bl_id].Value["Demerits"] += $CRPG::Pref::Demerits::Murder;
					if(CRPGData.data[%killer.bl_id].Value["JailData"])
						CRPGData.data[%killer.bl_id].Value["Demerits"] = 1200;
					commandtoclient(%killer,'CRPGSetDems',CRPGData.data[%killer.bl_id].Value["Demerits"]);
				}
				%killer.LastAttack[%client.player] = 50;
			}
		}
		%position = %client.player.getPosition();
		if(CRPGData.data[%client.bl_id].Value["Money"])
		{
			%cash = new Item("Droppable")
			{
				datablock = cashItem;
				canPickup = true;
				value = CRPGData.data[%client.bl_id].Value["Money"];
				position = %position;
			};
			%cash.setVelocity(getrandom(-5,5) SPC getrandom(-5,5) SPC "5");
			%cash.setShapeName("$" @ %cash.value);
			
			CRPGData.data[%client.bl_id].Value["Money"] = 0;
			commandtoclient(%client,'CRPGSetMoney',0);
			%client.setGameBottomPrint();
		}
		if(CRPGData.data[%client.bl_id].Value["Lumber"])
		{
			%lumber = new Item("Droppable")
			{
				datablock = lumberItem;
				canPickup = true;
				value = CRPGData.data[%client.bl_id].Value["Lumber"];
				position = %position;
			};
			%lumber.setVelocity(getrandom(-5,5) SPC getrandom(-5,5) SPC "5");
			%lumber.setShapeName(%lumber.value @ " Lumber");
			
			CRPGData.data[%client.bl_id].Value["Lumber"] = 0;
		}
		if(CRPGData.data[%client.bl_id].Value["Ore"])
		{
			%ore = new Item("Droppable")
			{
				datablock = oreItem;
				canPickup = true;
				value = CRPGData.data[%client.bl_id].Value["Ore"];
				position = %position;
			};
			%ore.setVelocity(getrandom(-5,5) SPC getrandom(-5,5) SPC "5");
			%ore.setShapeName(%ore.value @ " Ore");
			
			CRPGData.data[%client.bl_id].Value["Ore"] = 0;
		}
		%client.lastAttack[%killer.player] = 0;
		CRPGData.data[%client.bl_id].Value["Tools"] = "";
		CRPGData.data[%client.bl_id].Value["Transform"] = 0;
		CRPGData.data[%client.bl_id].Value["Damage"] = 0;
		commandtoclient(%client,'CRPGsetlotinfo',"","");
		commandtoclient(%client,'doWeed',0);
		%client.schedule(5000,"SpawnPlayer");
		parent::onDeath(%client, %killerplayer, %killer, %damageType, %damageLocation);
	}
	function fxDTSBrick::onActivate(%brick, %obj, %client, %pos, %dir)
	{
		parent::onActivate(%brick, %obj, %client, %pos, %dir);
		%data = %brick.getDatablock();
		
		if(%data.bricktype $= "Terminal")
		{
			if(%client.CRPGTrigger == %brick.trigger && %brick.Trigger != %client.ActiveTrigger)
			{
			//	if(!%client.hasClient)
			//		messageclient(%client,'',"\c6You need to <a:rtb-3823>Download the CRPG Client</a>\c6 to view your data.");
				%client.ActiveTrigger = %brick.Trigger;
				%data.parseData(%brick, %client, true, "");
			}
		}
		else if(%data.lumber && CRPGData.data[%client.bl_id].Value["JobID"].LumberEXP)
			messageclient(%client,'',"\c6This tree has \c3"@ %brick.lumber @"\c6 lumber left.");
		else if(%data.ore && CRPGData.data[%client.bl_id].Value["JobID"].OreEXP)
			messageclient(%client,'',"\c6This rock has \c3"@ %brick.ore @"\c6 ore left.");
	}
	function fxDTSBrick::onPlant(%brick)
	{
		Parent::onPlant(%brick);
		if(%brick.isPlanted)
		{
			%data = %brick.getdatablock();
			%group = %brick.getgroup();
		
			if(isObject(%group.client))
			{
				if(mFloor(getWord(%brick.rotation, 3)) == 90)
					%boxSize = getWord(%data.brickSizeY, 1) / 2.5 SPC getWord(%data.brickSizeX, 0) / 2.5 SPC getWord(%data.brickSizeZ, 2) / 2.5;
				else
					%boxSize = getWord(%data.brickSizeX, 1) / 2.5 SPC getWord(%data.brickSizeY, 0) / 2.5 SPC getWord(%data.brickSizeZ, 2) / 2.5;
				
				initContainerBoxSearch(%brick.getWorldBoxCenter(), %boxSize, $typeMasks::triggerObjectType);
				
				while(isObject(%trigger = containerSearchNext()))
				{
					if(%trigger.getDatablock() == CRPGLotTriggerData.getID())
					{
						%lotTrigger = %trigger;
						break;
					}
				}
				if(%lotTrigger && %data.brickType !$= "Lot")
				{
					if(%data.brickType $= "Terminal")
						%brick.handleBrickPlant();
					if(%data.brickType $= "Resource")
					{
						%brick.Lumber = %data.Lumber;
						%brick.Ore = %data.Ore;
					}
					if(%data.brickType $= "Spawn")
					{
						CRPGBrickData.Spawns[%data.spawndata] = trim(CRPGBrickData.Spawns[%data.spawndata] SPC %brick);
						CRPGBrickData.SpawnCount[%data.spawndata] = getWordCount(CRPGBrickData.Spawns[%data.spawndata]);
					}
					if(CRPGData.data[%group.bl_id].Value["Bricks"] && %data.category !$= "CRPG" && %data.subCategory !$= "Interactive" && $CRPG::Pref::BrickLumber)
					{
						CRPGData.data[%group.bl_id].Value["Bricks"]--;
					}
					if(%data.price)
					{
						MessageClient(%group.client,'', "\c6You have paid \c3$"@ %data.price @"\c6 to plant a \c3"@ %data.UIname @"\c6.");
						CRPGData.data[%group.bl_id].Value["Money"] -= %data.price;
						commandtoclient(%group.client,'CRPGSetMoney',CRPGData.data[%group.bl_id].Value["Money"]);
					}
				}
				else if(!%lotTrigger && %data.brickType $= "Lot")
				{
					if(CRPGData.data[%group.bl_id].Value["Money"] >= %data.lotPrice)
					{
						if(CRPGBrickData.lotsOwned[%group.BL_ID] < $CRPG::Pref::Realestate::MaxLots)
						{
							MessageClient(%group.client,'', "\c6You have paid \c3$"@ %data.lotPrice @"\c6 to plant a \c3"@ %data.UIname @"\c6.");
							CRPGData.data[%group.bl_id].Value["Money"] -= %data.lotPrice;
							commandtoclient(%group.client,'CRPGSetMoney',CRPGData.data[%group.bl_id].Value["Money"]);
							%client.SetInfo();
							%brick.handleBrickPlant();
						}
					}
				}
				else if(!%lotTrigger)
				{
					if(%data.brickType $= "Terminal")
						%brick.handleBrickPlant();
					if(%data.brickType $= "Resource")
					{
						%brick.Lumber = %data.Lumber;
						%brick.Ore = %data.Ore;
					}
					if(%data.brickType $= "Spawn")
					{
						CRPGBrickData.Spawns[%data.spawndata] = trim(CRPGBrickData.Spawns[%data.spawndata] SPC %brick);
						CRPGBrickData.SpawnCount[%data.spawndata] = getWordCount(CRPGBrickData.Spawns[%data.spawndata]);
					}
				}
			}
			else
			{
				if(%data.brickType $= "Terminal")
					%brick.handleBrickPlant();
				if(%data.brickType $= "Resource")
				{
					%brick.Lumber = %data.Lumber;
					%brick.Ore = %data.Ore;
				}
				if(%data.brickType $= "Spawn")
				{
					CRPGBrickData.Spawns[%data.spawndata] = trim(CRPGBrickData.Spawns[%data.spawndata] SPC %brick);
					CRPGBrickData.SpawnCount[%data.spawndata] = getWordCount(CRPGBrickData.Spawns[%data.spawndata]);
				}
			}
		}
	}
	function fxDTSBrick::onLoadPlant(%brick)
	{
		Parent::onLoadPlant(%brick);
		if(%brick.isPlanted)
		{
			%data = %brick.getdatablock();
			if(%data.bricktype $= "Lot")
				%brick.schedule(1,"handleBrickPlant");
			if(%data.bricktype $= "Terminal")
				%brick.handleBrickPlant();
			if(%data.brickType $= "Resource")
			{
				%brick.Lumber = %data.Lumber;
				%brick.Ore = %data.Ore;
			}
			if(%data.bricktype $= "Spawn")
			{
				CRPGBrickData.Spawns[%data.spawnData] = trim(CRPGBrickData.Spawns[%data.spawnData] SPC %brick);
				CRPGBrickData.SpawnCount[%data.spawndata] = getWordCount(CRPGBrickData.Spawns[%data.spawndata]);
			}
		}
	}
	function fxDTSBrick::onRemove(%brick)
	{
		if(%brick.isPlanted)
		{
			%data = %brick.getdatablock();
			if(%data.brickType $= "Lot")
				%brick.handleBrickRemove();
			if(%data.brickType $= "Terminal")
				%brick.handleBrickRemove();
			if(%data.brickType $= "Spawn")
			{
				CRPGBrickData.Spawns[%data.spawndata] = trim(strReplace(" "@ CRPGBrickData.Spawns[%data.spawndata] @" ", " "@ %brick @" ", " "));
				CRPGBrickData.SpawnCount[%data.spawndata] = getWordCount(CRPGBrickData.Spawns[%data.spawndata]);
			}
			if(%brick.ClockTime)
			{
				CRPGBrickData.TimeBricks[%brick.ClockTime] = trim(strReplace(" "@ CRPGBrickData.TimeBricks[%brick.ClockTime] @" ", " "@ %brick @" ", " "));
				CRPGBrickData.TimeBrickCount[%brick.ClockTime]--;
			}
		}
		parent::onRemove(%brick);
	}
	function fxDTSBrick::KillBrick(%brick)
	{
		%group = %brick.getGroup();
		%data = %brick.getDataBlock();
		if(%brick.item)
		{
			%itemname = trim(%brick.item.getDataBlock().UIname);
			%itemRefund = $CRPG::Item::Price[%itemname]*0.5;
			CRPGData.data[%group.BL_id].Value["Money"] += %itemRefund;
			if(isObject(%group.client))
			{
				messageclient(%group.client,'',"\c6You have recieved a refund of \c3$"@ %itemRefund @"\c6 for a \c3"@ %itemname @"\c6.");
				Commandtoclient(%group.client,'CRPGSetMoney',CRPGData.data[%group.BL_id].Value["Money"]);
			}
		}
		if(%brick.vehicle)
		{
			%vehiclename = trim(%brick.vehicle.getDataBlock().UIname);
			%vehicleRefund = $CRPG::Vehicle::Price[%vehiclename]*0.5;
			CRPGData.data[%group.BL_id].Value["Money"] += %vehicleRefund;
			if(isObject(%group.client))
			{
				messageclient(%group.client,'',"\c6You have recieved a refund of \c3$"@ %vehicleRefund @"\c6 for a \c3"@ %vehiclename @"\c6.");
				Commandtoclient(%group.client,'CRPGSetMoney',CRPGData.data[%group.BL_id].Value["Money"]);
			}
		}
		if(%data.Price)
		{
			%brickRefund = %data.price*0.5;
			CRPGData.data[%group.BL_id].Value["Money"] += %brickRefund;
			if(isObject(%group.client) && !%group.client.isAdmin)
			{
				messageclient(%group.client,'',"\c6You have recieved a refund of \c3$"@ %brickRefund @"\c6 for a \c3"@ %data.UIName @"\c6.");
				Commandtoclient(%group.client,'CRPGSetMoney',CRPGData.data[%group.BL_id].Value["Money"]);
			}
		}
		if(%data.LotPrice)
		{
			%lotRefund = %data.Lotprice*0.5;
			CRPGData.data[%group.BL_id].Value["Money"] += %lotRefund;
			if(isObject(%group.client))
			{
				messageclient(%group.client,'',"\c6You have recieved a refund of \c3$"@ %lotRefund @"\c6 for a \c3"@ %data.UIName @"\c6.");
				Commandtoclient(%group.client,'CRPGSetMoney',CRPGData.data[%group.BL_id].Value["Money"]);
				%client.SetInfo();
			}
		}
		if(%data == SafeBrickData.getID())
			%brick.handleSafeDestroy();
	CRPGData.data(%group.client.bl_id).value["Bricks"] += 1;
		parent::KillBrick(%brick);
	}
	function serverCmdmessageSent(%client, %text)
	{
		if(%text $= "")
			return;
		if(isObject(%client.player) && isObject(%client.ActiveTrigger) && isObject(%client.ActiveTrigger.parent) && %client.ActiveTrigger.parent.getDatablock().Bricktype $= "Terminal")
		{
			%client.ActiveTrigger.parent.getDatablock().parseData(%client.ActiveTrigger.parent, %client, "", %text);
			%client.isIdle = 0;
		}
		else
		{
			if(CRPGData.data[%client.bl_id].Value["Mute"])
				return MessageClient(%client,'',"\c6You cannot chat while muted.");
			if(CRPGData.data[%client.bl_id].Value["JailData"] && !%client.isAdmin && !$CRPG::Pref::JailChat)
				return serverCmdTeamMessageSent(%client, %text);
			parent::serverCmdmessageSent(%client, %text);
			%client.isIdle = 0;
		}
	}
	function HammerImage::onHitObject(%this, %obj, %slot, %col, %pos, %normal)
	{
		if(%col.getClassName() $= "Player" && CRPGData.data[%col.client.bl_id].Value["Demerits"]<200 || %col.getClassName() $= "WheeledVehicle")
			return;
		parent::onHitObject(%this, %obj, %slot, %col, %pos, %normal);
	}
	function WandImage::onHitObject(%this, %obj, %slot, %col, %pos, %normal)
	{
		if(%col.getClassName() $= "Player")
			return;
		parent::onHitObject(%this, %obj, %slot, %col, %pos, %normal);
	}
	function fxDTSBrick::setItem(%brick, %item, %client)
	{
		if(!%item)
			return parent::setItem(%brick, %item, %client);
		if(%brick == $LastLoadedBrick)
			return parent::setItem(%brick, %item, %client);
		if(%brick.item && %brick.item.getDataBlock() == %item)
			return;
		%itemname = trim(%item.UIname);
		%owner = %brick.getGroup().client;
		%ownerID = %brick.getGroup().bl_id;
		if(CRPGData.data[%ownerid].Value["JobID"].spawnItems)
		{
			if($CRPG::Item::Price[%itemname])
			{
				%discount = CRPGData.data[%ownerid].Value["JobID"].itemDiscount;
				if(CRPGData.data[%ownerid].Value["Money"] >= $CRPG::Item::Price[%itemname]*%discount)
				{
					CRPGData.data[%ownerid].Value["Money"] -= $CRPG::Item::Price[%itemname]*%discount;
					MessageClient(%owner,'',"\c6You have paid \c3$"@ $CRPG::Item::Price[%itemname]*%discount @"\c6 to spawn a \c3"@ %itemname @"\c6.");
					commandtoclient(%owner,'CRPGSetMoney',CRPGData.data[%ownerid].Value["Money"]);
					parent::setItem(%brick, %item, %client);
				}
				else
					MessageClient(%owner,'',"\c6You do not have \c3$"@ $CRPG::Item::Price[%itemname]*%discount @"\c6 to spawn a \c3"@ %itemname @"\c6.");
			}
			else
				MessageClient(%owner,'',"\c6You cannot spawn a \c3"@ %itemname @"\c6.");
		}
		else
			MessageClient(%owner,'',"\c6You cannot spawn items with your current job.");
	}
	function fxDTSBrick::setVehicle(%brick, %vehicle)
	{
		if(!%vehicle)
			return parent::setVehicle(%brick, %vehicle);
		if(%brick == $LastLoadedBrick)
			return parent::setVehicle(%brick, %vehicle);
		if(%brick.vehicle && %brick.vehicle.getDataBlock() == %vehicle)
			return;
		%vehiclename = trim(%vehicle.uiName);
		%owner = %brick.getGroup().client;
		%ownerID = %brick.getGroup().bl_id;
		if($CRPG::Vehicle::Price[%vehiclename])
		{
			if(CRPGData.data[%ownerid].Value["Money"] >= $CRPG::Vehicle::Price[%vehiclename])
			{
				CRPGData.data[%ownerid].Value["Money"] -= $CRPG::Vehicle::Price[%vehiclename];
				MessageClient(%owner,'',"\c6You have paid \c3$"@ $CRPG::Vehicle::Price[%vehiclename] @"\c6 to spawn a \c3"@ %vehiclename @"\c6.");
				commandtoclient(%owner,'CRPGSetMoney',CRPGData.data[%ownerid].Value["Money"]);
				%client.setInfo();
				parent::setVehicle(%brick, %vehicle);
			}
			else
				MessageClient(%owner,'',"\c6You do not have \c3$"@ $CRPG::Vehicle::Price[%vehiclename] @"\c6 to spawn a \c3"@ %vehiclename @"\c6.");
		}
		else
			MessageClient(%owner,'',"\c6You cannot spawn a \c3"@ %vehiclename @"\c6.");
	}
	function MinigameSO::pickSpawnPoint(%mini, %client)
	{
		%spawn = %client.ChooseSpawn();
		if(%spawn)
			return %spawn;
		return parent::pickSpawnPoint(%mini, %client);
	}
	function servercmdaddevent(%client, %enabled, %inputEventIDx, %delay, %targetIDx, %NamedTargetNameIdx, %outputEventIdx, %param1, %param2, %param3)
	{
		if(%inputEventIDx == InputEvent_GetInputEventIDx("OnDay") || %inputEventIDx == InputEvent_GetInputEventIDx("OnNight"))
		{
			if(!%client.WrenchBrick.ClockTime)
			{
				%client.WrenchBrick.ClockTime = getrandom(0,59);
				CRPGBrickData.TimeBricks[%client.WrenchBrick.ClockTime] = trim(CRPGBrickData.TimeBricks[%client.WrenchBrick.ClockTime] SPC %client.WrenchBrick);
				CRPGBrickData.TimeBrickCount[%client.WrenchBrick.ClockTime] = getWordCount(CRPGBrickData.TimeBricks[%client.WrenchBrick.ClockTime]);
			}
		}
		parent::serverCmdAddEvent(%client, %enabled, %inputEventIDx, %delay, %targetIDx, %NamedTargetNameIdx, %outputEventIdx, %param1, %param2, %param3);
	}
	function Player::AddHealth(%player, %amount)
	{
		Parent::AddHealth(%player, %amount);
		%player.setEnergyLevel(%player.getDatablock().maxDamage-%player.getDamageLevel());
	}
	function Player::setDamageLevel(%player, %int)
	{
		Parent::setDamageLevel(%player, %int);
		%player.setEnergyLevel(%player.getDatablock().maxDamage-%player.getDamageLevel());
	}
	function itemData::onPickup(%this, %item, %obj)
	{
		parent::onPickup(%this, %item, %obj);
		if(isObject(%item.spawnBrick))
			%item.spawnBrick.setItem(0);
	}
	function ServerCmdSuicide(%client)
	{
		if(CRPGData.data[%client.bl_id].Value["JailData"])
			return MessageClient(%client,'',"\c6You cannot suicide while in jail.");
		if(CRPGData.data[%client.bl_id].Value["Demerits"] >= 200)
			return MessageClient(%client,'',"\c6You cannot suicide while wanted.");
		if(CRPGData.data[%client.bl_id].Value["Bounty"])
			return MessageClient(%client,'',"\c6You cannot suicide with a bounty.");
		Parent::ServerCmdSuicide(%client);
	}
	function Player::activateStuff(%player)
	{
		parent::ActivateStuff(%player);

		%target = containerRayCast(%player.getEyePoint(),vectorAdd(vectorScale(vectorNormalize(%player.getEyeVector()),3),%player.getEyePoint()),$typeMasks::playerObjectType,%player).client;
		if(CRPGData.data[%player.Client.bl_id].Value["JailData"] || !%target)
			return;
		if(CRPGData.data[%player.Client.bl_id].Value["JobID"].Pickpocket)
		{
			if(isobject(%player.client.CRPGlotBrick))
				%admin = %player.client.CRPGlotBrick.getDatablock().AdminOnly;
			if(%player.crouch && !%admin && CRPGData.data[%target.bl_id].Value["Money"] && !CRPGData.data[%target.bl_id].Value["JobID"].Pickpocket)
			{
				%startmoney = CRPGData.data[%target.bl_id].Value["Money"];
				if(CRPGData.data[%target.bl_id].Value["Money"] >= CRPGData.data[%player.Client.bl_id].Value["JobID"].Pickpocket)
					CRPGData.data[%target.bl_id].Value["Money"]-= CRPGData.data[%player.Client.bl_id].Value["JobID"].Pickpocket;
				else
					CRPGData.data[%target.bl_id].Value["Money"] = 0;

				%difference = %startmoney-CRPGData.data[%target.bl_id].Value["Money"];
				commandtoclient(%target,'CRPGSetMoney',CRPGData.data[%target.bl_id].Value["Money"]);
				centerprint(%target,"\c3"@ %player.Client.name @"\c6 has stolen \c3$"@ %difference @"\c6 from you.",4);

				centerprint(%player.Client,"\c6You have stolen \c3$"@ %difference @"\c6 from \c3"@ %target.name @"\c6.",4);
				messageclient(%player.Client,'',"\c6You have commited a crime. [\c3Pickpocketing\c6]");
				CRPGData.data[%player.Client.bl_id].Value["Money"] += %difference;
				CRPGData.data[%player.Client.bl_id].Value["Demerits"] += 15; //WINK ADDED THIS
				commandtoclient(%player.Client,'CRPGSetMoney',CRPGData.data[%player.Client.bl_id].Value["Money"]);
				if(CRPGData.data[%player.Client.bl_id].Value["Demerits"]<150)
					CRPGData.data[%player.Client.bl_id].Value["Demerits"] = 150;
				commandtoclient(%player.Client,'CRPGSetDems',CRPGData.data[%player.Client.bl_id].Value["Demerits"]);

				if(CRPGData.data[%player.Client.bl_id].Value["Experience"] < CRPGData.data[%player.Client.bl_id].Value["JobID"].MaxExperience)
				{
					if(getRandom(0,250) <= %difference)
					{
						CRPGData.data[%player.Client.bl_id].Value["Experience"]++;
						messageclient(%player.Client,'',"\c6Your experience has increased to \c3"@ CRPGData.data[%player.Client.bl_id].Value["Experience"] @"\c6.");
					}
				}
			}
		}
		else if(CRPGData.data[%player.Client.bl_id].Value["JobID"].PayDems)
		{
			if(CRPGData.data[%target.bl_id].Value["Demerits"])
				messageclient(%player.Client,'',"\c3"@ %target.name @"\c6 has \c3"@ CRPGData.data[%target.bl_id].Value["Demerits"] @"\c6 demerits.");
			else
				messageclient(%player.Client,'',"\c3"@ %target.name @"\c6 currently has no demerits.");
		}
		else if(CRPGData.data[%player.Client.bl_id].Value["JobID"].BountyEXP)
		{
			if(CRPGData.data[%target.bl_id].Value["Bounty"])
				messageclient(%player.Client,'',"\c3"@ %target.name @"\c6 has a \c3$"@ CRPGData.data[%target.bl_id].Value["Bounty"] @"\c6 bounty.");
			else
				messageclient(%player.Client,'',"\c3"@ %target.name @"\c6 currently has no bounty.");
		}
		else if(CRPGData.data[%player.Client.bl_id].Value["JobID"].JailEXP)
		{
			if(CRPGData.data[%target.bl_id].Value["Demerits"] >= 200)
				messageclient(%player.Client,'',"\c3"@ %target.name @"\c6 is wanted and has a "@ %target.getWantedStars() @"\c6 wanted level.");
			else
				messageclient(%player.Client,'',"\c3"@ %target.name @"\c6 currently is not wanted.");
		}
		else if(CRPGData.data[%player.Client.bl_id].Value["JobID"].SalesEXP)
		{
			if(CRPGData.data[%target.bl_id].Value["Money"])
				messageclient(%player.Client,'',"\c3"@ %target.name @"\c6 has \c3$"@ CRPGData.data[%target.bl_id].Value["Money"] @"\c6.");
			else
				messageclient(%player.Client,'',"\c3"@ %target.name @"\c6 currently has no money.");
		}
		else if(CRPGData.data[%player.Client.bl_id].Value["JobID"].BountyEXP)
		{
			if(CRPGData.data[%target.bl_id].Value["Bounty"])
				messageclient(%player.Client,'',"\c3"@ %target.name @"\c6 has a \c3$"@ CRPGData.data[%target.bl_id].Value["Bounty"] @"\c6 bounty.");
			else
				messageclient(%player.Client,'',"\c3"@ %target.name @"\c6 currently has no bounty.");
		}
	}
	function serverCmdLight(%client)
	{
		if(CRPGData.data[%client.bl_id].Value["Light"])
			parent::ServerCmdLight(%client);
		else
			messageclient(%client,'',"\c6You do not have a light. Buy a light with \c3/Buy Light\c6.");
	}
	function servercmdDropTool(%client, %slot)
	{
		if(!CRPGData.data[%client.bl_id].Value["JailData"])
			parent::ServerCmdDropTool(%client,%slot);
		else
			messageclient(%client,'',"\c6You cannot drop tools while in jail.");
	}
	function serverCmdPlantBrick(%client)
	{
		if(isObject(%client.player.tempBrick))
		{
			%data = %client.player.tempBrick.getDataBlock();

			if(%data.adminOnly && !%client.isAdmin)
			{
				MessageClient(%client,'', "\c6You must be an \c3Admin\c6 to plant a \c3"@ %data.UIname @"\c6.");
				return %client.player.tempBrick.delete();
			}
			if(mFloor(getWord(%client.player.tempBrick.rotation, 3)) == 90)
				%boxSize = getWord(%data.brickSizeY, 1) / 2.5 SPC getWord(%data.brickSizeX, 0) / 2.5 SPC getWord(%data.brickSizeZ, 2) / 2.5;
			else
				%boxSize = getWord(%data.brickSizeX, 1) / 2.5 SPC getWord(%data.brickSizeY, 0) / 2.5 SPC getWord(%data.brickSizeZ, 2) / 2.5;
			
			initContainerBoxSearch(%client.player.tempBrick.getWorldBoxCenter(), %boxSize, $typeMasks::triggerObjectType);
			
			while(isObject(%trigger = containerSearchNext()))
			{
				if(%trigger.getDatablock() == CRPGLotTriggerData.getID())
				{
					%lotTrigger = %trigger;
					break;
				}
			}
			if(%lotTrigger && %data.brickType !$= "Lot")
			{
				if(%data.category !$= "CRPG" && %data.subCategory !$= "Interactive" && $CRPG::Pref::BrickLumber)
				{
					if(!CRPGData.data[%client.bl_id].Value["Bricks"])
					{
						MessageClient(%client,'', "\c6You need bricks to build.");
						return %client.player.tempBrick.delete();
					}
				}
				if(%data.price && CRPGData.data[%client.bl_id].Value["Money"] < %data.price)
				{
					MessageClient(%client,'', "\c6You need \c3$"@ %data.price @"\c6 to plant a \c3"@ %data.UIname @"\c6.");
					return %client.player.tempBrick.delete();
				}
			}
			else if(%lotTrigger && %data.brickType $= "Lot")
			{
				MessageClient(%client,'', "\c6You cannot put a lot inside another lot.");
				return %client.player.tempBrick.delete();
			}
			else if(!%lotTrigger && %data.brickType $= "Lot")
			{
				if(CRPGData.data[%client.bl_id].Value["Money"] >= %data.lotPrice)
				{
					if(CRPGBrickData.lotsOwned[%group.BL_ID] >= $CRPG::Pref::Realestate::MaxLots)
					{
						MessageClient(%client,'', "\c6You already own the maximum number of lots.");
						return %client.player.tempBrick.delete();
					}
				}
				else
				{
					MessageClient(%client,'', "\c6You need at least \c3$"@ %data.lotPrice @"\c6 to plant a \c3"@ %data.UIname @"\c6.");
					return %client.player.tempBrick.delete();
				}
			}
			else if(!%lotTrigger && !%client.isAdmin)
			{
				MessageClient(%client,'', "\c6You must be an \c3Admin\c6 to build outside of a lot.");
				return %client.player.tempBrick.delete();
			}
			parent::serverCmdPlantBrick(%client);
		}
	}
};
activatePackage(CRPG);
if(isPackage(PlayerPersistencePackage))
	deActivatePackage(PlayerPersistencePackage);