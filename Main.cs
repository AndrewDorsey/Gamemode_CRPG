//+---------------------------------------------+
//| Title:	Main				|
//| Author:	Jasa				|
//+---------------------------------------------+
//| CRPG functions.			 	|
//+---------------------------------------------+
$CRPG::Pref::MaxTools = "6";
datablock PlayerData(PlayerFrozenArmor : PlayerStandardArmor)
{
	runForce = 0;
	maxForwardSpeed = 0;
	maxBackwardSpeed = 0;
	maxSideSpeed = 0;
	maxForwardCrouchSpeed = 0;
	maxBackwardCrouchSpeed = 0;
	maxSideCrouchSpeed = 0;

	jumpForce = 0;
	jumpEnergyDrain = 0;
	minJumpEnergy = 0;
	canJet = 0;
	jetEnergyDrain = 0;
	minJetEnergy = 0;

	runSurfaceAngle  = 0;
	jumpSurfaceAngle = 0;

	uiName = "";
};

function PlayerFrozenArmor::onTrigger(%this,%player,%slot,%state)
{
	return;
}

datablock PlayerData(PlayerCRPG : PlayerStandardArmor)
{
	canJet = 0;

	uiName = "CRPG Player";
	maxTools = $CRPG::Pref::MaxTools;
	maxWeapons = $CRPG::Pref::MaxTools;
	rechargeRate = 0;
};
datablock decalData(CRPGLogo)
{
	textureName = "Add-Ons/GameMode_CRPG/Welcome.png";
	preload = true;
};
function CRPGStartup()
{
	if(!$CRPG::HasStarted)
	{
		echo("   Starting Up CRPG");
		echo("   Loading CRPG Data");
		new scriptObject(CRPGBrickData){};
		new scriptObject(CRPGData)
		{
			class = "Jassy";
			AutomaticLoading = 1;
			filePath = "config/server/CRPG/Data/";
		};
		CRPGData.addField("Name", "Name");
		CRPGData.addField("Address", 0);
		CRPGData.addField("LastOnline", "Currently");
		CRPGData.addField("Mute", 0);

		CRPGData.addField("Money", 0);
		CRPGData.addField("Bank", 0);
		CRPGData.addField("Bricks", 0);

		CRPGData.addField("Demerits", 0);
		CRPGData.addField("JailData", 0);
		CRPGData.addField("Record", 0);
		CRPGData.addField("Bounty", 0);

		CRPGData.addField("JobID", "Job1");
		CRPGData.addField("Experience", 0);
		CRPGData.addField("Education", 0);
		CRPGData.addField("Student", 0);
	
		CRPGData.addField("MiningSkill", 0);
		CRPGData.addField("CuttingSkill", 0);
	
		CRPGData.addField("Lumber", 0);
		CRPGData.addField("Iron", 0);
		CRPGData.addField("Silver", 0);
		CRPGData.addField("Platinum", 0);
		CRPGData.addField("Oak", 0);
		CRPGData.addField("Maple", 0);
		CRPGData.addField("Morning", 0);

		CRPGData.addField("9mm", 0);
		CRPGData.addField("Rifle", 0);
		CRPGData.addField("Shell", 0);
		CRPGData.addField("Explosive", 0);

		CRPGData.addField("Coke", 0);
		CRPGData.addField("Weed", 0);

		CRPGData.addField("CellPhone", 0);
		CRPGData.addField("Light", 0);

		CRPGData.addField("Hunger", "10");
		CRPGData.addField("Tools", "");
		CRPGData.addField("Transform", 0);
		CRPGData.addField("Damage", 0);
		CRPGData.addField("Height", "1");

		CRPGData.addField("SafeMoney",0);
		CRPGData.addField("SafeLumber",0);
		CRPGData.addField("SafeOre",0);
		CRPGData.addField("SafeItems",0);

		for(%a = 0; %a < datablockGroup.getCount(); %a++)
		{
			%datablock = datablockGroup.getObject(%a);
				
			if(%datablock.BrickType $= "Lot")
			{
				if(!%datablock.adminOnly)
					CRPGBrickData.LotLine[CRPGBrickData.LotLineCount++] = "\c3" @ %datablock.uiName @ "<lmargin:215>\c6Size: \c3" @ %datablock.brickSizeX @ "x" @ %datablock.brickSizeY SPC "\c6Cost: \c3" @ %datablock.lotPrice SPC "\c6Tax: \c3" SPC %datablock.taxAmount @"<lmargin:0>";
			}
		}
		echo("   Creating CRPG Minigame");
		new ScriptObject(CRPGMini) 
		{
			class = miniGameSO;
				
			brickDamage = 1;
			brickRespawnTime = 15000;
			colorIdx = -1;
			
			enableBuilding = 1;
			enablePainting = 1;
			enableWand = 1;
			fallingDamage = 1;
			inviteOnly = 0;
			
			points_plantBrick = 0;
			points_breakBrick = 0;
			points_die = 0;
			points_killPlayer = 0;
			points_killSelf = 0;
			
			playerDatablock = playerCRPG;
			respawnTime = 5500;
			selfDamage = 1;
			
			playersUseOwnBricks = 0;
			useAllPlayersBricks = 1;
			useSpawnBricks = 0;
			VehicleDamage = 1;
			vehicleRespawnTime = 10000;
			weaponDamage = 1;
		
			numMembers = 1;

			vehicleRunOverDamage = 1;
		};
		echo("   Loading City Data");
		if(isfile("Config/Server/CRPG/CityData.cs"))
			exec("Config/Server/CRPG/CityData.cs");
		else
		{
			$CRPG::Data::Money = "100000";
			$CRPG::Data::Materials = "1000";
			$CRPG::Data::Hour = "23";
			$CRPG::Data::Minute = "59";
		}

		echo("   Initializing CRPG Events");
		%FundList = "Money 1 Lumber 2 Ore 3";
		registerInputEvent("fxDTSBrick", "onEnterLot", "Self fxDTSBrick" TAB "Client gameConnection" TAB "Minigame Minigame");
		registerInputEvent("fxDTSBrick", "onLeaveLot", "Self fxDTSBrick" TAB "Client gameConnection" TAB "Minigame Minigame");
		registerInputEvent("fxDTSBrick", "onTransferFunds", "Self fxDTSBrick" TAB "Client GameConnection" TAB "Minigame Minigame");
		registerInputEvent("fxDTSBrick", "onDay", "Self fxDTSBrick" TAB "Minigame Minigame");
		registerInputEvent("fxDTSBrick", "onNight", "Self fxDTSBrick" TAB "Minigame Minigame");

		registerOutputEvent("fxDTSBrick", "Funds", "string 80 200" TAB "int 1 5000 1");
		registerOutputEvent("fxDTSBrick", "GiveItem", "datablock ItemData");
		registerOutputEvent("fxDTSBrick", "GiveFood", "int 1 6 1");
		//registerOutputEvent("fxDTSBrick", "GiveFunds", "string 80 200" TAB "int 1 5000 1");
	
		echo("   Unregistering Abusive Events");
		UnRegisterOutputEvent("Client", "BottomPrint");
		
		UnRegisterOutputEvent("fxDTSBrick", "respawn");
		UnRegisterOutputEvent("fxDTSBrick", "spawnExplosion");
		UnRegisterOutputEvent("fxDTSBrick", "spawnProjectile");
		UnRegisterOutputEvent("fxDTSBrick", "spawnItem");
		UnRegisterOutputEvent("fxDTSBrick", "radiusImpulse");

		UnRegisterOutputEvent("GameConnection", "incScore");
		UnRegisterOutputEvent("Minigame", "Reset");
		UnRegisterOutputEvent("Minigame", "RespawnAll");
		UnRegisterOutputEvent("Minigame", "BottomPrint");
	
		UnRegisterOutputEvent("Player", "setVelocity");
		UnRegisterOutputEvent("Player", "AddVelocity");
		UnRegisterOutputEvent("Player", "setPlayerScale");
		UnRegisterOutputEvent("Player", "setHealth");
		UnRegisterOutputEvent("Player", "instantRespawn");
		UnRegisterOutputEvent("Player", "Dismount");
		UnRegisterOutputEvent("Player", "ClearTools");
		UnRegisterOutputEvent("Player", "ClearBurn");
		UnRegisterOutputEvent("Player", "BurnPlayer");
		UnRegisterOutputEvent("Player", "addHealth");
		UnRegisterOutputEvent("Player", "Kill");
		UnRegisterOutputEvent("Player", "spawnProjectile");
		UnRegisterOutputEvent("Player", "spawnExplosion");
		UnRegisterOutputEvent("Player", "changeDatablock");

		UnRegisterInputEvent("fxDTSBrick", "onBlownUp");
		UnRegisterInputEvent("fxDTSBrick", "onRespawn");
		UnRegisterInputEvent("fxDTSBrick", "onBotTouch");
		UnRegisterInputEvent("fxDTSBrick", "onProjectileHit");

		echo("   Loading CRPG Item Prices");

		if(!isFile("Config/Server/CRPG/Items.txt"))
		{
			%file = new fileObject();
			%file.openForWrite("Config/Server/CRPG/Items.txt");
	
			%file.WriteLine("200 Gun");
	
			%file.close();
			%file.delete();
		}
		%file = new fileObject();
		%file.openForRead("Config/Server/CRPG/Items.txt");
		while(!%file.isEOF())
		{
			%line = %file.readLine();
			%item = trim(RemoveWord(%line,0));
			$CRPG::Item[$CRPG::Itemcount++] = %item;
			$CRPG::Item::Price[%item] = getWord(%line,0);
		}
		%file.close();
		%file.delete();
		echo("   Loading CRPG Vehicle Prices");
		if(!isFile("Config/Server/CRPG/Vehicles.txt"))
		{
			%file = new fileObject();
			%file.openForWrite("Config/Server/CRPG/Vehicles.txt");
	
			%file.WriteLine("3000 Jeep");
	
			%file.close();
			%file.delete();
		}
		%file = new fileObject();
		%file.openForRead("Config/Server/CRPG/Vehicles.txt");
		while(!%file.isEOF())
		{
			%line = %file.readLine();
			%vehicle = trim(RemoveWord(%line,0));
			$CRPG::Vehicle[$CRPG::VehicleCount++] = %vehicle;
			$CRPG::Vehicle::Price[%vehicle] = getWord(%line,0);
		}
		%file.close();
		%file.delete();
		RebuildCRPGWater();
		echo("   Starting CRPG Clock");
		Clock();
		$CRPG::HasStarted = 1;
		echo("   CRPG Started");
	}
}
function ExportCityData()
{
	export("$CRPG::Data::*", "Config/Server/CRPG/CityData.cs");
}
function Job::onAdd(%job)
{
	%jobID = "Job"@ $CRPG::JobCount++;
	%job.Setname(%jobID);
	$CRPG::Job[%job.JobName] = %jobID;
	
	datablock fxDtsBrickData(JobSpawnBrickData : brickSpawnPointData)
	{
		category = "CRPG";
		subCategory = "Spawns";
		
		uiName = %job.jobname @" Spawn";
		
		BrickType = "Spawn";
		AdminOnly = 1;
		
		SpawnData = %jobID;
	};
	JobSpawnBrickData.setname(%jobID @"SpawnBrickData");
	$CRPG::JobLine[$CRPG::JobLineCount++] = "\c3"@ %job.JobName @"<lmargin:150>\c6Investment: \c3"@ %job.Investment @"\c6 Pay: \c3"@ %job.Income @"\c6 CleanRecord: \c3"@ YN(%job.CleanRecord) @"\c6 Items: \c3"@ YN(%job.SpawnItems) @"\c6 RequiredEXP: \c3"@ %job.RequiredExperience @"\c6 MaxEXP: \c3"@ %job.MaxExperience @"<lmargin:0>";
	$CRPG::JobLine[$CRPG::JobLineCount++] = "<lmargin:150>\c6PlaceBounties: \c3"@ YN(%job.PlaceBounties) @"\c6 ClaimBounties: \c3"@ YN(%job.ClaimBounties) @"\c6 Pardon: \c3"@ YN(%job.Pardon) @"\c6 ClearRecords: \c3"@ YN(%job.ClearRecords) @"\c6 PersonalSpawns: \c3"@ YN(%job.PersonalSpawns) @"<lmargin:0>";
	$CRPG::JobLine[$CRPG::JobLineCount++] = "<lmargin:150>\c3"@ %job.InfoLine @"<lmargin:0>";
}
function Clock()
{
	cancel($CRPGTick);
	$CRPG::Data::Minute++;
	if($CRPG::Data::Minute > 59)
	{
		$CRPG::Data::Minute = 0;
		$CRPG::Data::Hour++;
		if($CRPG::Data::Hour > 23)
		{
			$CRPG::Data::Hour = 0;
			$CRPG::Data::Day++;
			ChooseWeather();
		}
		Tick();
	}
//	ChangeSky($CRPG::Data::Weather @ $CRPG::Data::Hour @"_"@ $CRPG::Data::Minute);
	if($CRPG::Data::Minute == 31)
		saveCRPGdata();
	if($CRPG::Data::Hour == 7)
		DoDayEvents($CRPG::Data::Minute);
	if($CRPG::Data::Hour == 19)
		DoNightEvents($CRPG::Data::Minute);
	for(%a = 0; %a < $Server::PlayerCount; %a++)
	{
		%client = ClientGroup.getObject(%a);
		if(CRPGData.data[%client.bl_id].Value["Cellphone"])
			commandtoclient(%client,'CRPGSetClock',$CRPG::Data::Hour,$CRPG::Data::Minute);
		%client.setInfo();
	}
	$CRPGTick = schedule($CRPG::Pref::ClockSpeed*1000, 0, Clock);
}
function gameConnection::getWantedLevel(%client)
{
	if(CRPGData.data[%client.bl_id].Value["Demerits"] < 1200)
		return mfloor(CRPGData.data[%client.bl_id].Value["Demerits"]/200);
	return 6;
}
function gameConnection::getWantedStars(%client)
{
	if(CRPGData.data[%client.bl_id].Value["Demerits"] < 200)
		return 0;
	%star = "<bitmap:base/client/ui/ci/star>";
	if(CRPGData.data[%client.bl_id].Value["Demerits"] < 400)
		return %star;
	if(CRPGData.data[%client.bl_id].Value["Demerits"] < 600)
		return %star @ %star;
	if(CRPGData.data[%client.bl_id].Value["Demerits"] < 800)
		return %star @ %star @ %star;
	if(CRPGData.data[%client.bl_id].Value["Demerits"] < 1000)
		return %star @ %star @ %star @ %star;
	if(CRPGData.data[%client.bl_id].Value["Demerits"] < 1200)
		return %star @ %star @ %star @ %star @ %star;
	return %star @ %star @ %star @ %star @ %star @ %star;
}
function player::giveDefaultTools(%this)
{
	if(!CRPGData.Data[%this.client.bl_id].Value["JailData"])
	{
		if(!getWordCount(CRPGData.Data[%this.client.bl_id].Value["Tools"]))
			%tools = $CRPG::Pref::DefaultTools SPC CRPGData.Data[%this.client.bl_id].Value["JobID"].Tools;
		else
			%tools = CRPGData.Data[%this.client.bl_id].Value["Tools"];
		%toolcount = getWordCount(%tools);
		for(%a = 0; %a < %toolcount; %a++)
		{
			%tool = getWord(%tools, %a);
			if(isobject(%tool))
			{
				%this.tool[%a] = nametoID(%tool);
				messageClient(%this.client, 'MsgItemPickup', "", %a, %this.tool[%a]);
			}
		}
	}
	else
	{
		for(%a = 0; %a < $CRPG::Pref::MaxTools; %a++)
		{
			if(isObject($CRPG::Pref::Demerits::JailItem[%a]))
			{
				%this.tool[%a] = nametoID($CRPG::Pref::Demerits::JailItem[%a]);
				messageClient(%this.client, 'MsgItemPickup', "", %a, %this.tool[%a]);
			}
			
		}
	}
}
function Tick()
{
        for(%a=0;%a<$Server::PlayerCount;%a++)
        {
		%client = ClientGroup.getObject(%a);
		if(CRPGData.Data[%client.bl_id].Value["JailData"])
		{
			CRPGData.Data[%client.bl_id].Value["JailData"]--;
			if(CRPGData.Data[%client.bl_id].Value["JailData"])
				MessageClient(%client,'', "\c6You now have \c3"@ CRPGData.Data[%client.bl_id].Value["JailData"] @"\c6 ticks left in prison.");
			else
			{
				MessageClient(%client,'', "\c6You have been released from prison.");
				if(isObject(%client.player.tempBrick))
					%client.player.tempBrick.delete();
				CRPGData.data[%client.bl_id].Value["Tools"] = "";
				%client.spawnPlayer();
			}
			commandtoclient(%client, 'CRPGSetJob', 0, CRPGData.data[%client.bl_id].Value["Student"], CRPGData.Data[%client.bl_id].Value["JobID"].JobName);
		}
		else
		{
			if(isObject(%client.player))
			{
				if(%client.isIdle && !%client.isAdmin)
				{
					echo(%client.name@" was kicked for being idle.");
					%client.delete("You have been kicked for being idle.");
					continue;
				}
				CRPGData.Data[%client.bl_id].Value["Hunger"]-= $CRPG::Pref::HungerRate;
				if(CRPGData.Data[%client.bl_id].Value["Hunger"] <= 0)
				{
					%client.player.kill();
					MessageClient(%client,'', "\c6You died of starvation.");
					CRPGData.Data[%client.bl_id].Value["Hunger"] = 5;
					continue;
				}
				%client.isIdle = 1;
				%client.player.setCRPGScale();
				commandtoclient(%client, 'CRPGSetHunger', CRPGData.data[%client.bl_id].Value["Hunger"]);
				if(CRPGData.Data[%client.bl_id].Value["Demerits"])
				{
					if(CRPGData.Data[%client.bl_id].Value["Demerits"] >= $CRPG::Pref::Demerits::ReduceRate)
						CRPGData.Data[%client.bl_id].Value["Demerits"] -= $CRPG::Pref::Demerits::ReduceRate;
					else
						CRPGData.Data[%client.bl_id].Value["Demerits"] = 0;
					
					MessageClient(%client,'', "\c6Your demerits have been reduced to \c3"@ CRPGData.Data[%client.bl_id].Value["Demerits"] @"\c6 due to the <a:en.wikipedia.org/wiki/Statute_of_limitations>Statute of Limitations</a>\c6.");
					commandtoclient(%client, 'CRPGSetDems', CRPGData.data[%client.bl_id].Value["Demerits"]);
				}
				if(CRPGData.Data[%client.bl_id].Value["JobID"].Income)
				{
					if(CRPGBrickData.Taxes[%client.bl_id] <= 0)
						%sum = CRPGData.Data[%client.bl_id].Value["JobID"].Income;
					else
						%sum = CRPGData.Data[%client.bl_id].Value["JobID"].Income - CRPGBrickData.Taxes[%client.bl_id];
					if(%sum > 0)
					{
						CRPGData.Data[%client.bl_id].Value["Money"] += %sum;
						if(CRPGBrickData.Taxes[%client.bl_id] > 0)
							MessageClient(%client,'', "\c6You have recieved your paycheck of \c3$"@ %sum @"\c6 after paying \c3$"@ CRPGBrickData.Taxes[%client.bl_id] @"\c6 in taxes.");
						else
							MessageClient(%client,'', "\c6You have recieved your paycheck of \c3$"@ %sum @"\c6.");
						commandtoclient(%client, 'CRPGSetMoney', CRPGData.data[%client.bl_id].Value["Money"]);
					}
					else
						MessageClient(%client,'', "\c6You did not receive a paycheck because your taxes are more than your paycheck.");
				}
			//	if(!%client.hasClient)
			//	messageclient(%client,'',"\c6You need to <a:rtb-3823>Download the CRPG Client</a>\c6 to view your data.");
			}
		}
		continue;
	}
	echo("Tick "@ $CRPG::Data::Hour @" happened for "@ $Server::PlayerCount @" clients.");
	%client.setInfo();
}
function SaveCRPGData()
{
	exportCityData();
	for(%a = 0; %a < $Server::PlayerCount; %a++)
	{
		%client = ClientGroup.getObject(%a);
		%client.saveCRPGData();
		%client.setScore(CRPGData.data[%client.bl_id].Value["Bank"]);
	}
}
function gameConnection::saveCRPGData(%client)
{
	if(!CRPGData.Data[%client.bl_id].Value["JailData"] && isObject(%client.player))
	{
		CRPGData.data[%client.bl_id].Value["Transform"] = %client.player.getTransform();
		CRPGData.data[%client.bl_id].Value["Damage"] = %client.player.getDamageLevel();
		for(%a = 0; %a < $CRPG::Pref::MaxTools; %a++)
		{
			if(%client.player.tool[%a])
				%tools = trim(%tools SPC %client.player.tool[%a].getName());
		}
		CRPGData.data[%client.bl_id].Value["Tools"] = %tools;
	}
	CRPGData.saveData(%client.bl_id);
}
function WitnessTest(%offender, %offended)
{
	if($Server::PlayerCount < 5)
		return 1;
	%offenderpos = %offender.player.getPosition();
	%offendedpos = %offended.player.getPosition();
	for(%a = 0; %a < $Server::PlayerCount; %a++)
	{
		%client = ClientGroup.getObject(%a);
		if(%client != %offender && %client != %offended)
		{
			%clientpos = %client.player.getPosition();
			if(VectorDist(%clientpos,%offender.player.getPosition()) < 128 || VectorDist(%clientpos,%offended.player.getPosition()) < 128)
				return 1;
		}
	}
	return 0;
}
function gameConnection::checkCRPGClient(%client)
{
	if(!%client.hasClient)
	{
		if($CRPG::Pref::AutoKick)
		{
			echo(%client.name @" was kicked for not having the client.");
			%client.delete("You must have the CRPG client to play on this server. If you do not download it you will continue to be kicked.");
			return;
		}
	//	messageclient(%client,'',"\c6You need to <a:rtb-3823>Download the CRPG Client</a>\c6 to view your data.");
	}
	if(%client.isSuperAdmin)
		commandtoclient(%client,'setCRPGAdmin');
	%client.setInfo();
}
function fxDTSBrick::handleBrickPlant(%brick, %data)
{
	if(!%brick.trigger)
	{
		%data = %brick.getDatablock();
		
		%trigX = getWord(%data.triggerSize, 0);
		%trigY = getWord(%data.triggerSize, 1);
		%trigZ = getWord(%data.triggerSize, 2);
		
		if(mFloor(getWord(%brick.rotation, 3)) == 90)
			%scale = (%trigY / 2) SPC (%trigX / 2) SPC (%trigZ / 2);
		else
			%scale = (%trigX / 2) SPC (%trigY / 2) SPC (%trigZ / 2);
			
		
		%brick.trigger = new trigger()
		{
			datablock = %data.triggerDatablock;
			position = getWords(%brick.getWorldBoxCenter(), 0, 1) SPC getWord(%brick.getWorldBoxCenter(), 2) + ((getWord(%data.triggerSize, 2) / 4) - (%data.brickSizeZ * 0.1));
			rotation = "1 0 0 0";
			scale = %scale;
			polyhedron = "-0.5 -0.5 -0.5 1 0 0 0 1 0 0 0 1";
			parent = %brick;
		};
		
		%boxSize = getWord(%scale, 0) / 2.5 SPC getWord(%scale, 1) / 2.5 SPC getWord(%scale, 2) / 2.5;
		
		initContainerBoxSearch(%brick.trigger.getWorldBoxCenter(), %boxSize, $typeMasks::playerObjectType);
		
		while(isObject(%player = containerSearchNext()))
			%brick.trigger.getDatablock().onEnterTrigger(%brick.trigger, %player);
		
		if(%data.BrickType $= "Lot" && !%data.AdminOnly)
		{
			CRPGBrickData.LotsOwned[%brick.getGroup().bl_id]++;
			CRPGBrickData.Taxes[%brick.getGroup().bl_id] += %data.taxAmount;
		}
	}
}
function fxDTSBrick::handleBrickRemove(%brick, %data)
{
	if(%brick.trigger)
	{
		for(%a = 0; %a < $Server::PlayerCount; %a++)
		{
			%subClient = ClientGroup.getObject(%a);
			if(%subClient.player && %subClient.CRPGTrigger == %brick.trigger)
				%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, clientGroup.getObject(%a).player, 1);
		}
		
		%boxSize = getWord(%brick.trigger.scale, 0) / 2.5 SPC getWord(%brick.trigger.scale, 1) / 2.5 SPC getWord(%brick.trigger.scale, 2) / 2.5;
		
		initContainerBoxSearch(%brick.trigger.getWorldBoxCenter(), %boxSize, $typeMasks::playerObjectType);
		
		while(isObject(%player = containerSearchNext()))
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, %player);
		
		%brick.trigger.delete();
		%data = %brick.Getdatablock();
		if(%data.BrickType $= "Lot" && !%data.AdminOnly)
		{
			CRPGBrickData.LotsOwned[%brick.getGroup().bl_id]--;
			CRPGBrickData.Taxes[%brick.getGroup().bl_id] -= %data.taxAmount;
		}
	}	
}
function CRPGLotTriggerData::onEnterTrigger(%this, %trigger, %obj)
{
	parent::onEnterTrigger(%this, %trigger, %obj);
	
	if(!isObject(%obj.client))
	{
		if(isObject(%obj.getControllingClient()))
			%client = %obj.getControllingClient();
		else
			return;
	}
	else
		%client = %obj.client;
	
	%trigger.parent.onEnterLot(%obj);
	
	%client.CRPGTrigger = %trigger;
	%client.CRPGLotBrick = %trigger.parent;
	
	%client.SetInfo();
}

function CRPGLotTriggerData::onLeaveTrigger(%this, %trigger, %obj)
{
	if(!isObject(%obj.client))
	{
		if(isObject(%obj.getControllingClient()))
			%client = %obj.getControllingClient();
		else
			return;
	}
	else
		%client = %obj.client;
	
	%trigger.parent.onLeaveLot(%obj);
	
	%client.CRPGTrigger = "";
	%client.CRPGLotBrick = "";
	
	%client.SetInfo();
}
function CRPGInputTriggerData::onEnterTrigger(%this, %trigger, %obj)
{
	if(!%obj.client)
		return;
	%obj.client.CRPGTrigger = %trigger;
	%obj.client.isIdle = 0;
}
function CRPGInputTriggerData::onLeaveTrigger(%this, %trigger, %obj)
{
	if(!%obj.client)
		return;
	if(%obj.client.CRPGTrigger == %trigger)
		%obj.client.CRPGTrigger = 0;
	if(%obj.client.ActiveTrigger == %trigger)
	{
		%trigger.parent.getDatablock().parseData(%trigger.parent, %obj.client, 0, "");
		%obj.client.ActiveTrigger = 0;
	}
}
function gameConnection::arrest(%client, %jailer)
{
	serverCmddropTool(%client, %client.player.currTool);
	if(CRPGData.data[%client.bl_id].Value["Demerits"] >= "1200")
		%amount = "1200";
	if(CRPGData.data[%client.bl_id].Value["Demerits"] < "1200")
		%amount = CRPGData.data[%client.bl_id].Value["Demerits"];
	CRPGData.data[%client.bl_id].Value["Jaildata"] += %client.getwantedlevel();
	CRPGData.data[%client.bl_id].Value["Demerits"] = 0;
	
	if(CRPGData.data[%client.bl_id].Value["JailData"]>1 && !CRPGData.Data[%client.bl_id].Value["Record"])
	{
		messageclient(%client,'',"\c6You have lost your clean record.");
		CRPGData.data[%client.bl_id].Value["Record"] = 1;
	}
	if(CRPGData.data[%client.bl_id].Value["Jaildata"]==1 && $CRPG::Data::Minute>30)
		CRPGData.data[%client.bl_id].Value["Jaildata"] = 2;
	if(CRPGData.Data[%client.bl_id].Value["JobID"].CleanRecord && CRPGData.Data[%client.bl_id].Value["Record"])
	{
		CRPGData.data[%client.bl_id].Value["JobID"] = "Job1";
		MessageClient(%client,'', "\c6You have been demoted to \c3"@ CRPGData.Data[%client.bl_id].Value["JobID"].JobName @"\c6 for being jailed.");
	}
	if(CRPGData.Data[%jailer.bl_id].Value["JobID"].JailEXP)
	{
		MessageClient(%jailer,'',"\c6You have been paid \c3$"@ %amount @"\c6 for jailing \c3"@ %client.name @"\c6.");
		CRPGData.data[%jailer.bl_id].Value["Money"] += %amount;
		if(CRPGData.Data[%jailer.bl_id].Value["Experience"] < CRPGData.Data[%jailer.bl_id].Value["JobID"].MaxExperience)
		{
			if(getrandom(0,6) <= CRPGData.data[%client.bl_id].Value["Jaildata"])
			{
				CRPGData.data[%jailer.bl_id].Value["Experience"]++;
				MessageClient(%jailer,'',"\c6Your experience has increased to \c3"@ CRPGData.data[%jailer.bl_id].Value["Experience"] @"\c6.");
			}
		}
	}
	if(CRPGData.data[%client.bl_id].Value["Money"])
	{
		MessageClient(%jailer,'', "\c6You have taken \c3$"@ CRPGData.data[%client.bl_id].Value["Money"] @"\c6 from \c3"@ %client.name @"\c6 as evidence.");
		CRPGData.data[%jailer.bl_id].Value["Money"] += CRPGData.data[%client.bl_id].Value["Money"];
		CRPGData.data[%client.bl_id].Value["Money"] = 0;
	}
	commandtoclient(%jailer,'CRPGSetMoney',CRPGData.data[%jailer.bl_id].Value["Money"]);
	if(CRPGData.data[%client.bl_id].Value["Bounty"])
	{
		MessageClient(%jailer,'', "\c6Hit successful. \c3$"@ CRPGData.data[%client.bl_id].Value["Bounty"] @"\c6 has been wired to your bank account.");
		if(CRPGData.data[%jailer.bl_id].Value["Experience"] < CRPGData.data[%jailer.bl_id].Value["JobID"].maxExperience)
		{
			if(getRandom(0,1000) <= CRPGData.data[%client.bl_id].Value["Bounty"])
			{
				CRPGData.data[%jailer.bl_id].Value["Experience"]++;
				MessageClient(%jailer,'',"\c6Your experience has increased to \c3"@ CRPGData.data[%jailer.bl_id].Value["Experience"] @"\c6.");
			}
		}
		CRPGData.data[%jailer.bl_id].Value["Bank"] += CRPGData.data[%client.bl_id].Value["Bounty"];
		CRPGData.data[%client.bl_id].Value["Bounty"] = 0;
	}
	
	if(isObject(%client.player.tempBrick))
		%client.player.tempBrick.delete();
	CRPGData.data[%client.bl_id].Value["Tools"] = "";
	
	%client.spawnPlayer();

	if(CRPGData.data[%client.bl_id].Value["Jaildata"] != "1")
		%b = "s";
	echo(%jailer.name @" has jailed "@ %client.name @" for "@ CRPGData.data[%client.bl_id].value["JailData"] @" ticks.");
	MessageAll('',"\c3"@ %client.name @"\c6 was jailed by \c3"@ %jailer.name @"\c6 for \c3"@ CRPGData.data[%client.bl_id].Value["Jaildata"] @"\c6 tick"@ %b @".");
}
function Vehicle::onActivate(){}
function ServerCmdChangeMap(%client)
{
	MessageClient(%client,'', "\c6You cannot change maps.");
}
function ServerCmdLeaveMinigame(%client)
{
	MessageClient(%client,'', "\c6You cannot leave the minigame.");
}
function ServerCmdCreateMinigame(%client)
{
	MessageClient(%client,'', "\c6You cannot create a minigame.");
}
function miniGameCanDamage(%obj1, %obj2)
{
	return 1;
}
function miniGameCanUse(%obj1, %obj2)
{
	return 1;
}
function getMiniGameFromObject(%obj)
{
	return CRPGMini;
}
function Player::SetCRPGScale(%player)
{
	%scale = (0.79+CRPGData.data[%player.client.bl_id].Value["Hunger"]*0.03)*CRPGData.data[%player.client.bl_id].Value["Height"];
	%player.setPlayerScale(%scale SPC %scale SPC CRPGData.data[%player.client.bl_id].Value["Height"]);
}

function pickaxeProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	if(%col.dataBlock.CRPG_isOre)
	{
		if(CRPGData.data[%obj.client.bl_id].value["MiningSkill"] < %col.dataBlock.CRPG_requiredLevel)
		{
			centerPrint(%obj.client,"\c6You need a \c3Mining\c6 level of \c3" @ %col.dataBlock.CRPG_requiredLevel @ "\c6 to ground this ore.",3);
			return;
		}
		%bonus = (mCeil(%expl / 15)/10) + 0.9;
		%col.hits+=%bonus;
		if(%col.hits >= %col.dataBlock.CRPG_life)
		{
			%col.hits = 0;
			%col.disappear(%col.dataBlock.CRPG_life);
			%exp = %col.dataBlock.CRPG_exp;
			if(CRPGData.data[%obj.client.bl_id].value["OreEXP"])
			{
				%exp *= 0.05;
				CRPGData.data[%obj.client.bl_id].value["MiningSkill"] += %exp;
				if(getRandom(1,250) == 1)
					CRPGData.data[%obj.client.bl_id].value["Experience"] += 1;
			}
			centerPrint(%obj.client,"\c6You ground the \c3" @ %col.dataBlock.uiName @ "\c6 down.",3);
			if(%col.dataBlock.CRPG_gives == 1)
				CRPGData.data[%obj.client.bl_id].value["Iron"]++;
			else if(%col.dataBlock.CRPG_gives == 2)
				CRPGData.data[%obj.client.bl_id].value["Silver"]++;
			else if(%col.dataBlock.CRPG_gives == 3)
				CRPGData.data[%obj.client.bl_id].value["Platinum"]++;
		}
		else
		{
			centerPrint(%obj.client,"\c6The \c3" @ %col.dataBlock.uiName @ "\c6 is \c3" @ mFloor((%col.hits / mCeil(%col.dataBlock.CRPG_life)) * 100) @ "%\c6 grounded.",3);
		}
	}
}

function AxeProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	if(%col.dataBlock.CRPG_isTree)
	{
		if(CRPGData.data[%obj.client.bl_id].value["CuttingSkill"] < %col.dataBlock.CRPG_requiredLevel)
		{
			centerPrint(%obj.client,"\c6You need a \c3Cutting\c6 level of \c3" @ %col.dataBlock.CRPG_requiredLevel @ "\c6 to ground this ore.",3);
			return;
		}
		%bonus = (mCeil(%expl / 15)/10) + 0.9;
		%col.hits+=%bonus;
		if(%col.hits >= %col.dataBlock.CRPG_life)
		{
			%col.hits = 0;
			%col.disappear(%col.dataBlock.CRPG_life);
			%exp = %col.dataBlock.CRPG_exp;
			if(CRPGData.data[%obj.client.bl_id].value["LumberEXP"])
			{
				%exp *= 0.05;
				CRPGData.data[%obj.client.bl_id].value["CuttingSkill"] += %exp;
				if(getRandom(1,250) == 1)
					CRPGData.data[%obj.client.bl_id].value["Experience"] += 1;
			}
			centerPrint(%obj.client,"\c6You cut the \c3" @ %col.dataBlock.uiName @ "\c6 tree down.",3);
			if(%col.dataBlock.CRPG_gives == 4)
				CRPGData.data[%obj.client.bl_id].value["Oak"]++;
			else if(%col.dataBlock.CRPG_gives == 5)
				CRPGData.data[%obj.client.bl_id].value["Maple"]++;
			else if(%col.dataBlock.CRPG_gives == 6)
				CRPGData.data[%obj.client.bl_id].value["Morning"]++;
		}
		else
		{
			centerPrint(%obj.client,"\c6The \c3" @ %col.dataBlock.uiName @ "\c6 is \c3" @ mFloor((%col.hits / mCeil(%col.dataBlock.CRPG_life)) * 100) @ "%\c6 cut.",3);
		}
	}
}

function GameConnection::ChooseSpawn(%client)
{
	if(CRPGData.data[%client.bl_id].Value["Jaildata"])
	{
		%randombrick1 = getword(CRPGBrickData.Spawns[JailSpawn],getrandom(0, CRPGBrickData.SpawnCount[JailSpawn]-1));
		if(isObject(%randombrick1))
			return vectorSub(%randombrick1.getWorldBoxCenter(), "0 0" SPC (%randombrick1.getDatablock().brickSizeZ - 3) * 0.1) SPC getWords(%randombrick1.getTransform(), 3, 6);
	}
	if(CRPGData.data[%client.bl_id].Value["JobID"].PersonalSpawns)
	{
		for(%a = 0; %a < CRPGBrickData.SpawnCount[PersonalSpawn]; %a++)
		{
			%brick2 = getword(CRPGBrickData.Spawns[PersonalSpawn], %a);
			if(%brick2.getGroup().bl_id == %client.bl_id)
				%possiblespawns2 = trim(%possiblespawns2 SPC %brick2);
		}
		%randombrick2 = getword(%possiblespawns2,getrandom(0, getwordcount(%possiblespawns2)-1));
		if(isObject(%randombrick2))
			return vectorSub(%randombrick2.getWorldBoxCenter(), "0 0" SPC (%randombrick2.getDatablock().brickSizeZ - 3) * 0.1) SPC getWords(%randombrick2.getTransform(), 3, 6);
	}
	%randombrick3 = getword(CRPGBrickData.Spawns[CRPGData.data[%client.bl_id].Value["JobID"]],getrandom(0, CRPGBrickData.SpawnCount[CRPGData.data[%client.bl_id].Value["JobID"]]-1));
	if(isObject(%randombrick3))
		return vectorSub(%randombrick3.getWorldBoxCenter(), "0 0" SPC (%randombrick3.getDatablock().brickSizeZ - 3) * 0.1) SPC getWords(%randombrick3.getTransform(), 3, 6);
	return 0;
}
function DoDayEvents(%minute)
{
	$InputTarget_["MiniGame"] = "CRPGMini";
	for(%a = 0; %a < CRPGBrickData.TimeBrickCount[%minute]; %a++)
	{
		$InputTarget_["Self"] = getword(CRPGBrickData.TimeBricks[%minute], %a);
		$InputTarget_["Self"].processInputEvent("OnDay", CRPGMini);
	}
}
function DoNightEvents(%minute)
{
	$InputTarget_["MiniGame"] = "CRPGMini";
	for(%a = 0; %a < CRPGBrickData.TimeBrickCount[%minute]; %a++)
	{
		$InputTarget_["Self"] = getword(CRPGBrickData.TimeBricks[%minute], %a);
		$InputTarget_["Self"].processInputEvent("OnNight", CRPGMini);
	}
}

//======================================================
//Old School No-GUI Stats Display <3 system by Dionysus
//======================================================

function gameConnection::getCashString(%client)
{
	%money = CRPGData.Data[%client.bl_id].valueMoney;
	if(CRPGData.Data[%client.bl_id].valueMoney >= 0)
	{
	    if(%money >= 100000000) %temp = "$" @ %money;
		else if(%money >= 10000000) %temp = "$0" @ %money;
		else if(%money >= 1000000) %temp = "$00" @ %money;
		else if(%money >= 100000) %temp = "$000" @ %money;
		else if(%money >= 10000) %temp = "$0000" @ %money;
		else if(%money >= 1000) %temp = "$00000" @ %money;
		else if(%money >= 100) %temp = "$000000" @ %money;
		else if(%money >= 10) %temp = "$0000000" @ %money;
		else if(%money >= 1) %temp = "$00000000" @ %money;
		else if(%money >= 0) %temp = "$00000000" @ %money;
		else %temp = "$000000000";
	}
	else
		%temp = "\c0Error.";
	return %temp;
}

function gameConnection::getBrickString(%client)
{
	%Bricks = CRPGData.data[%client.bl_id].valueBricks;
	if(CRPGData.Data[%client.bl_id].valueBricks >= 0)
	{
		if(%Bricks >= 2) %temp = %Bricks @ " Bricks.";
		else if(%Bricks == 1) %temp = %Bricks @ " Brick.";
		else %temp = "\c0No bricks.";
	}
	else
		%temp = "\c0Error.";
	return %temp;
}

function gameConnection::getJobString(%client)
{
	%job = CRPGData.Data[%client.bl_id].valueJobID.JobName;
	%porfavor = %job @ ".";
	return %porfavor;
}

function gameConnection::getWantedLevel(%client)
{
	if(CRPGData.Data[%client.bl_id].value["Demerits"] >= $CRPG::Pref::Demerits::Wanted)
	{
		%div = CRPGData.Data[%client.bl_id].value["Demerits"] / $CRPG::Pref::Demerits::Wanted;
		
		if(%div <= 3)
			return 1;
		else if(%div <= 8)
			return 2;
		else if(%div <= 14)
			return 3;
		else if(%div <= 21)
			return 4;
		else if(%div <= 29)
			return 5;
		else
			return 6;
	}
	else
		return 0;
}

function gameConnection::setGameBottomPrint(%client)
{
	%mainFont = "<font:impact:20>";
	%client.CRPGPrint = %mainFont;
	%client.CRPGPrint = %client.CRPGPrint NL "\c6| " @ %client.getCashString();
	
	%client.CRPGPrint = %client.CRPGPrint SPC "\c6| " @ %client.getJobString();
	
	%bricks = CRPGData.data[%client.bl_id].value["Bricks"];
	%client.CRPGPrint = %client.CRPGPrint SPC "\c6| " @ %client.getBrickString();
	
	%hlevel = CRPGData.data[%client.bl_id].value["Hunger"];
	%client.CRPGPrint = %client.CRPGPrint SPC "\c6| " @ $CRPG::Hunger[mfloor(%hlevel)] @ ".";
	
	%client.CRPGPrint = %client.CRPGPrint SPC "\c6| ";
	
	if(CRPGData.Data[%client.bl_id].value["Demerits"] >= $CRPG::Pref::Demerits::Wanted)
	{
		%client.CRPGPrint = %client.CRPGPrint NL "<color:ffffff>Wanted:";
		%stars = %client.getWantedLevel();
		%client.CRPGPrint = %client.CRPGPrint SPC "<font:impact:13><color:ffff00>";
		
		for(%a = 0; %a < %stars; %a++)
			%client.CRPGPrint = %client.CRPGPrint @ "*";
		
		%client.CRPGPrint = %client.CRPGPrint @ "<color:888888>";
		for(%a = %a; %a < 6; %a++)
			%client.CRPGPrint = %client.CRPGPrint @ "*";
		
		%client.CRPGPrint = %client.CRPGPrint @ %mainFont;
	}
	
	if(isObject(%client.CRPGLotBrick)) 
	{
		%type = %client.CRPLotBrick.getDatablock().lottype;
		%own = getBrickGroupFromObject(%client.CRPGLotBrick).name;
		if(%type $= "Residential")
			%client.CRPGPrint = %client.CRPGPrint SPC "<just:right>\c3" @ %own;
			else if(%type $= "Commercial")
			%client.CRPGPrint = %client.CRPGPrint SPC "<just:right>\c1" @ %own;
			else if(%type $= "Industrial")
			%client.CRPGPrint = %client.CRPGPrint SPC "<just:right>\c0" @ %own;
			else
			%client.CRPGPrint = %client.CRPGPrint SPC "<just:right>\c6" @ %own;
	}
		
	commandToClient(%client, 'bottomPrint', %client.CRPGPrint, 0, true);
		
	return %client.CRPGPrint;
}

function gameConnection::setInfo(%client)
{
	CRPGData.Data[%client.bl_id].value["Money"] = mFloor(CRPGData.Data[%client.bl_id].value["Money"]);
	CRPGData.Data[%client.bl_id].value["Name"] = %client.name;
	
	if(isObject(%client.player))
	{	
		if(!%client.hasClient)
		{
			%client.player.setShapeName("foobar");
			%client.player.setShapeNameColor("foobar");
			%client.player.setShapeNameDistance(24);
			
			%client.setGameBottomPrint();
		}
		else
		{
			commandtoclient(%client, 'CRPGSetMoney',%client.getCashString());
			commandtoclient(%client, 'CRPGSetDems',CRPGData.Data[%client.bl_id].value["Demerits"]);
			commandtoclient(%client, 'CRPGSetBricks',%client.getBrickString);
			commandtoclient(%client, 'CRPGSetHunger',CRPGData.Data[%client.bl_id].value["Hunger"]);
		}
	}
}

