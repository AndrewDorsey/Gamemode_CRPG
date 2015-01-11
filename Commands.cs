//+---------------------------------------------+
//| Title:	Commands			|
//| Author:	Jasa				|
//+---------------------------------------------+
//| Commands for CRPG.			 	|
//+---------------------------------------------+
function servercmdCRPGPing(%client)
{
	%client.hasClient = 1;
}

//function serverCmdSetInfo(%client)
//{
//	echo("Sending " @ %client.name @ " CRPG Information.");
//	commandtoclient(%client,'CRPGsetMoney',CRPGData.Data[%client.bl_id].Value["Money"]);
//	commandtoclient(%target,'CRPGsetWanted',CRPGData.data[%target.bl_id].Value["Demerits"]);
//	commandtoclient(%client,'CRPGsetJob', 0, CRPGData.data[%client.bl_id].Value["Student"], CRPGData.Data[%client.bl_id].Value["JobID"].JobName);
//	commandtoclient(%target,'CRPGsetHunger',CRPGData.data[%target.bl_id].Value["Hunger"]);
//	commandtoclient(%client,'CRPGsetStats',%data.ValueBank,%data.ValueRecord,%data.ValueBounty,%data.ValueExperience,%data.ValueEducation,%data.ValueLumber,%data.ValueOre,%data.Value9mm,%data.ValueRifle,%data.ValueShell,%data.ValueExplosive,%data.ValueCoke,%data.ValueWeed);
////	commandToClient(%client,'updateText',%data.value["JobID"].jobName,%data.valueMoney,%data.valueHunger,%data.valueWanted,%client.dataJailData);
//}

function serverCmdRequestCRPGJobs(%client)
{
	if(!%client.hasJobs)
	{
		for(%a=1;%a<=$CRPG::JobCount;%a++)
		{
			%job = "Job"@ %a;
			commandtoclient(%client,'addCRPGJob',%job.JobName,%job.Path,%job.Income,%job.RequiredJob,%job.CleanRecord,%job.RequiredExperience,%job.MaxExperience,%job.RequiredEducation,%job.Investment,%job.Admin,%job.ClaimBounties SPC %job.PlaceBounties,%job.SpawnItems,%job.ClearRecords,%job.Pardon,%job.Lots SPC %job.PersonalSpawns,%job.PayDems,%job.Food);
		}
	}
	%client.hasJobs = 1;
}
function serverCmdRequestCRPGStats(%client)
{
	%data = CRPGData.data[%client.bl_id];
	commandtoclient(%client,'CRPGSetStats',%data.ValueDemerits,%data.valueHunger,%data.Value["JobID"].Income,%data.Value["JobID"].JobName,%data.valueMoney,%data.ValueBank,%data.ValueRecord,%data.ValueBounty,%data.ValueExperience,%data.ValueEducation,%data.ValueLumber,%data.ValueOre);
}
function ServerCmdHelp(%client, %section)
{
	if(%client.lastCommand $= "" || $Sim::Time - %client.lastCommand > 2)
	{
		%client.lastCommand = $Sim::Time;
		if(%section $= "")
		{
			MessageClient(%client,'', "\c6Say \c3/Help\c6 followed by one of the sections below for information on that section.<lmargin:0>");
			MessageClient(%client,'', "\c3Jobs<lmargin:100>\c6Displays a list of jobs and their specifications.<lmargin:0>");
			MessageClient(%client,'', "\c3Items<lmargin:100>\c6Displays a list of items and their prices.<lmargin:0>");
			MessageClient(%client,'', "\c3Vehicles<lmargin:100>\c6Displays a list of vehicles and their prices.<lmargin:0>");
			MessageClient(%client,'', "\c3Lots<lmargin:100>\c6Displays a list of lots and their prices.<lmargin:0>");
			MessageClient(%client,'', "\c3Commands<lmargin:100>\c6Displays a list of commands.<lmargin:0>");
			MessageClient(%client,'', "\c3Credits<lmargin:100>\c6Displays a list of credits for CRPG.<lmargin:0>");

			if(%client.isAdmin)
				MessageClient(%client,'', "\c3Admin<lmargin:100>\c6Displays a list of admin only commands.<lmargin:0>");
		}
		else if(%section $= "Jobs")
		{
			MessageClient(%client,'', "\c6Here is a list of jobs.");
			for(%a = 1; %a <= $CRPG::JobLineCount; %a++)
				messageclient(%client,'',$CRPG::JobLine[%a]);
		
			MessageClient(%client,'', "\c6Say \c3/Job\c6 followed by one of the jobs above to become that job.");
		}
		else if(%section $= "Items")
		{
			MessageClient(%client,'', "\c6Here is a list of items and their prices.");
			for(%a = 1; %a <= $CRPG::Itemcount; %a++)
				MessageClient(%client,'', "\c3"@ $CRPG::Item[%a] @"<lmargin:150>\c6Price: \c3$"@ $CRPG::Item::Price[$CRPG::Item[%a]] * CRPGData.data[%client.bl_id].Value["JobID"].itemDiscount @"<lmargin:0>");
		}
		else if(%section $= "Vehicles")
		{
			MessageClient(%client,'', "\c6Here is a list of vehicles and their prices.");

			for(%a = 1; %a <= $CRPG::Vehiclecount; %a++)
				MessageClient(%client,'', "\c3"@ $CRPG::Vehicle[%a] @"<lmargin:150>\c6Price: \c3$"@ $CRPG::Vehicle::Price[$CRPG::Vehicle[%a]] @"<lmargin:0>");
		}
		else if(%section $= "Lots")
		{
			MessageClient(%client,'', "\c6Here is a list of lots you can plant.");
					
			for(%a = 1; %a <= CRPGBrickData.LotLineCount; %a++)
				messageclient(%client,'',CRPGBrickData.LotLine[%a]);
		}
		else if(%section $= "Commands")
		{
			MessageClient(%client,'', "\c6Here is a list of commands.");
			MessageClient(%client,'', "\c6/Give<lmargin:100>\c6[\c3Data\c6] [\c3Amount\c6] [\c3Target\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/Drop<lmargin:100>\c6[\c3Data\c6] [\c3Amount\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/Buy<lmargin:100>\c6[\c3Data\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/ClearRecord<lmargin:100>\c6[\c3Target\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/PayDems<lmargin:100>\c6[\c3Amount\c6] [\c3Target\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/Pardon<lmargin:100>\c6[\c3Target\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/Reset<lmargin:0>");
			MessageClient(%client,'', "\c6/Text<lmargin:100>\c6[\c3Target\c6] [\c3Text\c6]<lmargin:0>");
		}
		else if(%section $= "Credits")
		{
			MessageClient(%client,'', "\c6Here is a list of credits for CRPG.");
			MessageClient(%client,'', "\c3Jasa<lmargin:100>\c6Scripting<lmargin:0>");
			MessageClient(%client,'', "\c3Dionysus<lmargin:100>\c6Scripting<lmargin:0>");
		}
		else if(%section $= "Admin" && %client.isAdmin)
		{
			MessageClient(%client,'', "\c6Here is a list of admin only commands.");
			MessageClient(%client,'', "\c6/Set<lmargin:100>\c6[\c3Data\c6] [\c3Amount\c6] [\c3Target\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/Add<lmargin:100>\c6[\c3Data\c6] [\c3Amount\c6] [\c3Target\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/Deduct<lmargin:100>\c6[\c3Data\c6] [\c3Amount\c6] [\c3Target\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/SetHour<lmargin:100>\c6[\c3Hour\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/DayTime");
			MessageClient(%client,'', "\c6/NextHour");
			MessageClient(%client,'', "\c6/Mute<lmargin:100>\c6[\c3Target\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/GetMuted");
			MessageClient(%client,'', "\c6/UnMute<lmargin:100>\c6[\c3Target\c6]<lmargin:0>");
		}
		else
			MessageClient(%client,'', "\c6Help section \c3"@ %section @"\c6 not found.");
	}
	else
		MessageClient(%client,'',"\c6You must wait before using this command again.");
}
function YN(%value)
{
	if(%value)
		return "Yes";
	return "No";
}
function serverCmdClearDroppables(%client)
{
	if(!%client.isSuperAdmin)
		return MessageClient(%client,'',"\c6You must be a \c3Super Admin\c6 to use this command.");
	%droppablecount = 0;
	while(isObject("Droppable"))
	{
		Droppable.delete();
		%droppablecount++;
	}
	MessageAll('',"\c3" @ %client.name @ "\c0 cleared droppables. ("@ %droppablecount @")");
}
function serverCmdJob(%client, %job, %job2)
{
	if(%client.lastCommand $= "" || $Sim::Time - %client.lastCommand > 2)
	{
		%client.lastCommand = $Sim::Time;
		if(CRPGData.Data[%client.bl_id].Value["JailData"])
			return MessageClient(%client,'',"\c6You can't switch jobs while in jail.");
		if(CRPGData.data[%client.bl_id].Value["Demerits"] >= 200)
			return MessageClient(%client,'',"\c6You can't switch jobs while wanted.");
		if(!isobject(%client.player))
			return MessageClient(%client,'',"\c6Spawn first before using this command.");
		%job = trim(%job SPC %job2);	
		if(%job $= "")
			return MessageClient(%client,'',"\c6You must enter a job.");
		if($CRPG::Job[%job] $= CRPGData.Data[%client.bl_id].Value["JobID"])
			return MessageClient(%client,'',"\c6You are already a \c3"@ $CRPG::Job[%job].JobName @"\c6.");
		if(!isobject($CRPG::Job[%job]))
			return MessageClient(%client,'',"\c6Job \c3"@ %job @"\c6 not found. Say \c3/Help Jobs\c6 for a list of jobs.");
		if($CRPG::Job[%job].CleanRecord && CRPGData.Data[%client.bl_id].Value["CriminalRecord"])
			return MessageClient(%client,'',"\c6You need a clean record to become a \c3"@ $CRPG::Job[%job].JobName @"\c6.");
		if(getWordCount($CRPG::Job[%job].RequiredJob) && $CRPG::Job[%job].RequiredJob !$= CRPGData.Data[%client.bl_id].Value["JobID"].JobName)
			return MessageClient(%client,'',"\c6You need to be a \c3"@ $CRPG::Job[%job].RequiredJob @"\c6 before you can be a \c3"@ $CRPG::Job[%job].JobName @"\c6.");
		if($CRPG::Job[%job].RequiredExperience > CRPGData.Data[%client.bl_id].Value["Experience"])
			return MessageClient(%client,'',"\c6You need at least \c3"@ $CRPG::Job[%job].RequiredExperience @"\c6 experience to become a \c3"@ $CRPG::Job[%job].JobName @"\c6.");
		if($CRPG::Job[%job].Investment && $CRPG::Job[%job].Investment > CRPGData.Data[%client.bl_id].Value["Money"])
			return MessageClient(%client,'',"\c6You need at least \c3$"@ $CRPG::Job[%job].Investment @"\c6 to become a \c3"@ $CRPG::Job[%job].JobName @"\c6.");
		MessageClient(%client,'',"\c6You have paid \c3$"@ $CRPG::Job[%job].Investment @"\c6 to become a \c3"@ $CRPG::Job[%job].JobName @"\c6.");
	
		CRPGData.Data[%client.bl_id].Value["Money"] -= $CRPG::Job[%job].Investment;
		CRPGData.Data[%client.bl_id].Value["JobID"] = $CRPG::Job[%job];
	
		if(isObject(%client.player.tempBrick))
			%client.player.tempBrick.delete();
		CRPGData.Data[%client.bl_id].Value["Tools"] = "";
		%client.spawnplayer();
	}
	else
		MessageClient(%client,'',"\c6You must wait before using this command again.");
}
function serverCmdDrop(%client, %data, %amount)
{
	if(CRPGData.Data[%client.bl_id].Value["JailData"])
		return MessageClient(%client,'',"\c6You can't drop while in jail.");
	if(!isobject(%client.player))
		return MessageClient(%client,'',"\c6Spawn first before using this command.");
	if(%amount $= "")
		return MessageClient(%client,'',"\c6You must enter an amount to drop.");
	if(%data $= "Money" || %data $= "Cash")
	{
		if(!CRPGData.Data[%client.bl_id].Value["Money"])
			return MessageClient(%client,'',"\c6You need \c3Money\c6 to drop.");
		%amount = mfloor(%amount);
		if(%amount <= 0)
			return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Money\c6 to drop.");
		if(%amount > CRPGData.Data[%client.bl_id].Value["Money"])
			%amount = CRPGData.Data[%client.bl_id].Value["Money"];					

		MessageClient(%client,'',"\c6You have dropped \c3$"@ %amount @"\c6.");

		CRPGData.Data[%client.bl_id].Value["Money"] -= %amount;
		%eyevector = %client.player.getEyeVector();
		%cash = new Item("Droppable")
		{
			datablock = cashItem;
			canPickup = 1;
			value = %amount;
			position = vectorAdd(%client.player.getEyePoint(),vectorScale(%eyevector,1));
		};
		%cash.setVelocity(VectorScale(%eyevector,7));
		%cash.setShapeName("$" @ %cash.value);

		commandtoclient(%client,'CRPGsetMoney',CRPGData.Data[%client.bl_id].Value["Money"]);
	}
	else if(%data $= "Lumber")
	{
		if(!CRPGData.Data[%client.bl_id].Value["Lumber"])
			return MessageClient(%client,'',"\c6You need \c3Lumber\c6 to drop.");

		%amount = mfloor(%amount);
		if(%amount <= 0)
			return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Lumber\c6 to drop.");
		if(%amount > CRPGData.Data[%client.bl_id].Value["Lumber"])
			%amount = CRPGData.Data[%client.bl_id].Value["Lumber"];		
		MessageClient(%client,'',"\c6You have dropped \c3"@ %amount @"\c6 lumber.");

		CRPGData.Data[%client.bl_id].Value["Lumber"] -= %amount;
		%eyevector = %client.player.getEyeVector();
		%Lumber = new Item("Droppable")
		{
			datablock = LumberItem;
			canPickup = 1;
			value = %amount;
			position = vectorAdd(%client.player.getEyePoint(),vectorScale(%eyevector,1));
		};
		%Lumber.setVelocity(VectorScale(%eyevector,7));
		%lumber.setShapeName(%lumber.value @ " Lumber");
	}
	else if(%data $= "Ore")
	{
		if(!CRPGData.Data[%client.bl_id].Value["Ore"])
			return MessageClient(%client,'',"\c6You need \c3Ore\c6 to drop.");
		%amount = mfloor(%amount);
		if(%amount <= 0)
			return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Ore\c6 to drop.");
		if(%amount > CRPGData.Data[%client.bl_id].Value["Ore"])
			%amount = CRPGData.Data[%client.bl_id].Value["Ore"];

		MessageClient(%client,'',"\c6You have dropped \c3"@ %amount @"\c6 ore.");

		CRPGData.Data[%client.bl_id].Value["Ore"] -= %amount;
		%eyevector = %client.player.getEyeVector();
		%Ore = new Item("Droppable")
		{
			datablock = OreItem;
			canPickup = 1;
			value = %amount;
			position = vectorAdd(%client.player.getEyePoint(),vectorScale(%eyevector,1));
		};
		%Ore.setVelocity(VectorScale(%eyevector,7));
		%Ore.setShapeName(%Ore.value @ " Ore");
	}
	else
	{
		messageclient(%client,'',"\c6Data \c3"@ %data @"\c6 not found.");
	}
	%client.SetInfo();
}
function serverCmdDoDrugs(%client, %drug)
{
	if(CRPGData.Data[%client.bl_id].Value["JailData"])
		return MessageClient(%client,'',"\c6You can't do drugs while in jail.");
	if(!isobject(%client.player))
		return MessageClient(%client,'',"\c6Spawn first before using this command.");
	if(%drug $= "")
		return MessageClient(%client,'',"\c6You must enter a drug to do.");
	if(%drug $= "Coke" || %drug $= "Cocaine")
	{
		if(!CRPGData.Data[%client.bl_id].Value["Coke"])
			return MessageClient(%client,'',"\c6You need \c3"@ %drug @"\c6 to do.");
		%time = getrandom(300, 600);
		MessageClient(%client,'',"\c6You have done \c3"@ %drug @"\c6. The effects will ware off in about \c3"@ mfloor(%time/60) @"\c6 minutes.");
		CRPGData.Data[%client.bl_id].Value["Coke"]--;

		commandtoclient(%client,'doCoke',%time);
	}
	else if(%drug $= "Weed" || %drug $= "Marijuana")
	{
		if(!CRPGData.Data[%client.bl_id].Value["Weed"])
			return MessageClient(%client,'',"\c6You need \c3"@ %drug @"\c6 to do.");
		%time = getrandom(180, 360);
		MessageClient(%client,'',"\c6You have done \c3"@ %drug @"\c6. The effects will ware off in about \c3"@ mfloor(%time/60) @"\c6 minutes.");
		CRPGData.Data[%client.bl_id].Value["Weed"]--;

		commandtoclient(%client,'doWeed',%time);
	}
	else
		MessageClient(%client,'',"\c6No such drug as \c3"@ %drug @"\c6.");
}
function ServerCmdNextHour(%client)
{
	if(!%client.isSuperAdmin)
		return MessageClient(%client,'',"\c6You must be a \c3Super Admin\c6 to use this command.");
	if($CRPG::Data::Minute < "30")
		return MessageClient(%client,'',"\c6There was recently a tick. Wait until the minute is at least \c330\c6.");
	MessageClient(%client,'',"\c6You just forced the next hour.");
	$CRPG::Data::Minute = "59";
	Clock();
}
function ServerCmdSetHour(%client, %hour)
{
	if(!%client.isSuperAdmin)
		return MessageClient(%client,'',"\c6You must be a \c3Super Admin\c6 to use this command.");
	if(%hour $= "")
		return MessageClient(%client,'',"\c6You must enter an hour to set.");
	if($CRPG::Data::Minute < "30")
		return MessageClient(%client,'',"\c6There was recently a tick. Wait until the minute is at least \c330\c6.");
	%hour = mfloor(%hour);
	if(%hour == $CRPG::Data::Hour)
		return MessageClient(%client,'',"\c6The hour is already \c3"@ $CRPG::Data::Hour @"\c6.");
	if(%hour > 23)
		%hour = "23";
	if(%hour < 0)
		%hour = 0;
	MessageClient(%client,'',"\c6You set the hour to \c3"@ %hour @"\c6.");
	$CRPG::Data::Minute = "59";
	$CRPG::Data::Hour = %hour-1;
	%client.SetInfo();
	Clock();
}
function ServerCmdDayTime(%client)
{
	if(!%client.isSuperAdmin)
		return MessageClient(%client,'',"\c6You must be a \c3Super Admin\c6 to use this command.");
	MessageClient(%client,'',"\c6It is now day.");
	$CRPG::Data::Hour = "9";
	$CRPG::Data::Minute = "0";
	%client.SetInfo();
	Clock();
}
function ServerCmdSet(%client, %data, %amount, %target)
{
	if(%client.lastCommand $= "" || $Sim::Time - %client.lastCommand > 2)
	{
		%client.lastCommand = $Sim::Time;
		if(!%client.isSuperAdmin)
			return MessageClient(%client,'',"\c6You must be a \c3Super Admin\c6 to use this command.");
		if(%target $= "")
			return MessageClient(%client,'',"\c6You must enter a target.");
		%target = findclientbyname(%target);
		if(!isobject(%target))
			return MessageClient(%client,'',"\c6Target not found.");
		if(%data $= "Money" || %data $= "Cash")
		{
			%amount = mfloor(%amount);
			if(%amount < 0)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Money\c6 to set.");
			MessageClient(%client,'',"\c6You set the \c3Money\c6 of \c3"@ %target.name @"\c6 to \c3$"@ %amount @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 set your \c3Money\c6 to \c3$"@ %amount @"\c6.");
			
			CRPGData.data[%target.bl_id].Value["Money"] = %amount;
			commandtoclient(%target,'CRPGsetMoney',%amount);
		}
		else if(%data $= "Bank")
		{
			%amount = mfloor(%amount);
			if(%amount < 0)
				return MessageClient(%client,'',"\c6Please enter a valid \c3Bank\c6 to set.");
			MessageClient(%client,'',"\c6You set the \c3Bank\c6 of \c3"@ %target.name @"\c6 to \c3$"@ %amount @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 set your \c3Bank\c6 to \c3$"@ %amount @"\c6.");
			
			CRPGData.data[%target.bl_id].Value["Bank"] = %amount;
		}
		else if(%data $= "Demerits" || %data $= "Dems")
		{
			%amount = mfloor(%amount);
			if(%amount < 0)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Demerits\c6 to set.");
			MessageClient(%client,'',"\c6You set the \c3Demerits\c6 of \c3"@ %target.name @"\c6 to \c3"@ %amount @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 set your \c3Demerits\c6 to \c3"@ %amount @"\c6.");
			
			CRPGData.data[%target.bl_id].Value["Demerits"] = %amount;
			commandtoclient(%target,'CRPGSetDems',%amount);
		}
		else if(%data $= "Bounty")
		{
			%amount = mfloor(%amount);
			if(%amount < 0)
				return MessageClient(%client,'',"\c6Please enter a valid \c3Bounty\c6 to set.");
			MessageClient(%client,'',"\c6You set the \c3Bounty\c6 of \c3"@ %target.name @"\c6 to \c3"@ %amount @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 set your \c3Bounty\c6 to \c3"@ %amount @"\c6.");
			
			CRPGData.data[%target.bl_id].Value["Bounty"] = %amount;
		}
		else if(%data $= "Experience" || %data $= "EXP")
		{
			%amount = mfloor(%amount);
			if(%amount < 0)
				return MessageClient(%client,'',"\c6Please enter a valid \c3Experience\c6 to set.");
			MessageClient(%client,'',"\c6You set the \c3Experience\c6 of \c3"@ %target.name @"\c6 to \c3"@ %amount @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 set your \c3Experience\c6 to \c3"@ %amount @"\c6.");
			
			CRPGData.data[%target.bl_id].Value["Experience"] = %amount;
		}
		else if(%data $= "Education")
		{
			%amount = mfloor(%amount);
			if(%amount < 0)
				return MessageClient(%client,'',"\c6Please enter a valid \c3Education\c6 to set.");
			MessageClient(%client,'',"\c6You set the \c3Education\c6 of \c3"@ %target.name @"\c6 to \c3"@ %amount @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 set your \c3Education\c6 to \c3"@ %amount @"\c6.");
			
			CRPGData.data[%target.bl_id].Value["Education"] = %amount;
		}
		else if(%data $= "Lumber")
		{
			%amount = mfloor(%amount);
			if(%amount < 0)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Lumber\c6 to set.");
			MessageClient(%client,'',"\c6You set the \c3Lumber\c6 of \c3"@ %target.name @"\c6 to \c3"@ %amount @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 set your \c3Lumber\c6 to \c3"@ %amount @"\c6.");
			
			CRPGData.data[%target.bl_id].Value["Lumber"] = %amount;
		}
		else if(%data $= "Ore")
		{
			%amount = mfloor(%amount);
			if(%amount < 0)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Ore\c6 to set.");
			MessageClient(%client,'',"\c6You set the \c3Ore\c6 of \c3"@ %target.name @"\c6 to \c3"@ %amount @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 set your \c3Ore\c6 to \c3"@ %amount @"\c6.");
			
			CRPGData.data[%target.bl_id].Value["Ore"] = %amount;
		}
		else if(%data $= "9mm")
		{
			%amount = mfloor(%amount);
			if(%amount < 0)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c39mm Ammo\c6 to set.");
			MessageClient(%client,'',"\c6You set the \c39mm Ammo\c6 of \c3"@ %target.name @"\c6 to \c3"@ %amount @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 set your \c39mm Ammo\c6 to \c3"@ %amount @"\c6.");
			
			CRPGData.data[%target.bl_id].Value["9mm"] = %amount;
		}
		else if(%data $= "Rifle")
		{
			%amount = mfloor(%amount);
			if(%amount < 0)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Rifle Ammo\c6 to set.");
			MessageClient(%client,'',"\c6You set the \c3Rifle Ammo\c6 of \c3"@ %target.name @"\c6 to \c3"@ %amount @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 set your \c3Rifle Ammo\c6 to \c3"@ %amount @"\c6.");
			
			CRPGData.data[%target.bl_id].Value["Rifle"] = %amount;
		}
		else if(%data $= "Shell")
		{
			%amount = mfloor(%amount);
			if(%amount < 0)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Shell Ammo\c6 to set.");
			MessageClient(%client,'',"\c6You set the \c3Shell Ammo\c6 of \c3"@ %target.name @"\c6 to \c3"@ %amount @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 set your \c3Shell Ammo\c6 to \c3"@ %amount @"\c6.");
			
			CRPGData.data[%target.bl_id].Value["Shell"] = %amount;
		}
		else if(%data $= "Explosive")
		{
			%amount = mfloor(%amount);
			if(%amount < 0)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Explosive Ammo\c6 to set.");
			MessageClient(%client,'',"\c6You set the \c3Explosive Ammo\c6 of \c3"@ %target.name @"\c6 to \c3"@ %amount @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 set your \c3Explosive Ammo\c6 to \c3"@ %amount @"\c6.");
			
			CRPGData.data[%target.bl_id].Value["Explosive"] = %amount;
		}
		else if(%data $= "Hunger")
		{
			if(%amount < 0 || %amount > 14)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Hunger\c6 to set under \c314\c6.");
			MessageClient(%client,'',"\c6You set the \c3Hunger\c6 of \c3"@ %target.name @"\c6 to \c3"@ %amount @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 set your \c3Hunger\c6 to \c3"@ %amount @"\c6.");
			
			CRPGData.data[%target.bl_id].Value["Hunger"] = %amount;
			commandtoclient(%target,'CRPGsetHunger',CRPGData.data[%target.bl_id].Value["Hunger"]);
			%target.player.setCRPGScale();
		}
		else if(%data $= "Height")
		{
			if(%amount < 0.5 || %amount > 1.5)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Height\c6 to set under \c31.5\c6.");
			MessageClient(%client,'',"\c6You set the \c3Height\c6 of \c3"@ %target.name @"\c6 to \c3"@ %amount @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 set your \c3Height\c6 to \c3"@ %amount @"\c6.");
			
			CRPGData.data[%target.bl_id].Value["Height"] = %amount;
			
			if(isobject(%target.player))
				%target.player.setCRPGScale();
		}
		else
			messageclient(%client,'',"\c6Data \c3"@ %data @"\c6 not found.");
	}
	else
	{
		MessageClient(%client,'',"\c6You must wait before using this command again.");
	}
	%client.SetInfo();
}
function ServerCmdAdd(%client, %data,  %amount, %target)
{
	if(%client.lastCommand $= "" || $Sim::Time - %client.lastCommand > 2)
	{
		%client.lastCommand = $Sim::Time;
		if(!%client.isSuperAdmin)
			return MessageClient(%client,'',"\c6You must be a \c3Super Admin\c6 to use this command.");
		if(%target $= "")
			return MessageClient(%client,'',"\c6You must enter a target.");
		%target = findclientbyname(%target);
		if(!isobject(%target))
			return MessageClient(%client,'',"\c6Target not found.");
		if(%data $= "Money" || %data $= "Cash")
		{
			%amount = mfloor(%amount);
			if(%amount <= 0)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Money\c6 to add.");
			MessageClient(%client,'',"\c6You added \c3$"@ %amount @"\c6 to \c3"@ %target.name @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 added you \c3$"@ %amount @"\c6.");
			
			CRPGData.data[%target.bl_id].Value["Money"] += %amount;
			commandtoclient(%target,'CRPGsetMoney',CRPGData.data[%target.bl_id].Value["Money"]);
		}
		else if(%data $= "Bank")
		{
			%amount = mfloor(%amount);
			if(%amount <= 0)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Bank\c6 to add.");
			MessageClient(%client,'',"\c6You added \c3$"@ %amount @"\c6 to the \c3Bank\c6 of \c3"@ %target.name @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 added \c3$"@ %amount @"\c6 to your \c3Bank\c6.");
			
			CRPGData.data[%target.bl_id].Value["Bank"] += %amount;
		}
		else if(%data $= "Demerits" || %data $= "Dems")
		{
			%amount = mfloor(%amount);
			if(%amount <= 0)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Demerits\c6 to add.");
			MessageClient(%client,'',"\c6You added \c3"@ %amount @" Demerits\c6 to \c3"@ %target.name @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 added you \c3"@ %amount @" Demerits\c6.");
			
			CRPGData.data[%target.bl_id].Value["Demerits"] += %amount;
			commandtoclient(%target,'CRPGSetDems',CRPGData.data[%target.bl_id].Value["Demerits"]);
		}
		else if(%data $= "Bounty")
		{
			%amount = mfloor(%amount);
			if(%amount <= 0)
				return MessageClient(%client,'',"\c6Please enter a valid \c3Bounty\c6 to add.");
			MessageClient(%client,'',"\c6You added \c3"@ %amount @" Bounty\c6 to \c3"@ %target.name @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 added you \c3"@ %amount @" Bounty\c6.");
			
			CRPGData.data[%target.bl_id].Value["Bounty"] += %amount;
		}
		else if(%data $= "Experience" || %data $= "EXP")
		{
			%amount = mfloor(%amount);
			if(%amount <= 0)
				return MessageClient(%client,'',"\c6Please enter a valid \c3Experience\c6 to add.");
			MessageClient(%client,'',"\c6You added \c3"@ %amount @" Experience\c6 to \c3"@ %target.name @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 added you \c3"@ %amount @" Experience\c6.");
			
			CRPGData.data[%target.bl_id].Value["Experience"] += %amount;
		}
		else if(%data $= "Education")
		{
			%amount = mfloor(%amount);
			if(%amount <= 0)
				return MessageClient(%client,'',"\c6Please enter a valid \c3Education\c6 to add.");
			MessageClient(%client,'',"\c6You added \c3"@ %amount @" Education\c6 to \c3"@ %target.name @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 added you \c3"@ %amount @" Education\c6.");
			
			CRPGData.data[%target.bl_id].Value["Education"] += %amount;
		}
		else if(%data $= "Lumber")
		{
			%amount = mfloor(%amount);
			if(%amount <= 0)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Lumber\c6 to add.");
			MessageClient(%client,'',"\c6You added \c3"@ %amount @" Lumber\c6 to \c3"@ %target.name @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 added you \c3"@ %amount @" Lumber\c6.");
			
			CRPGData.data[%target.bl_id].Value["Lumber"] += %amount;
		}
		else if(%data $= "Ore")
		{
			%amount = mfloor(%amount);
			if(%amount <= 0)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Ore\c6 to add.");
			MessageClient(%client,'',"\c6You added \c3"@ %amount @" Ore\c6 to \c3"@ %target.name @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 added you \c3"@ %amount @" Ore\c6.");
			
			CRPGData.data[%target.bl_id].Value["Ore"] += %amount;
		}
		else if(%data $= "9mm")
		{
			%amount = mfloor(%amount);
			if(%amount <= 0)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c39mm Ammo\c6 to add.");
			MessageClient(%client,'',"\c6You added \c3"@ %amount @" 9mm Ammo\c6 to \c3"@ %target.name @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 added you \c3"@ %amount @" 9mm Ammo\c6.");
			
			CRPGData.data[%target.bl_id].Value["9mm"] += %amount;
		}
		else if(%data $= "Rifle")
		{
			%amount = mfloor(%amount);
			if(%amount <= 0)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Rifle Ammo\c6 to add.");
			MessageClient(%client,'',"\c6You added \c3"@ %amount @" Rifle Ammo\c6 to \c3"@ %target.name @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 added you \c3"@ %amount @" Rifle Ammo\c6.");
			
			CRPGData.data[%target.bl_id].Value["Rifle"] += %amount;
		}
		else if(%data $= "Shell")
		{
			%amount = mfloor(%amount);
			if(%amount <= 0)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Shell Ammo\c6 to add.");
			MessageClient(%client,'',"\c6You added \c3"@ %amount @" Shell Ammo\c6 to \c3"@ %target.name @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 added you \c3"@ %amount @" Shell Ammo\c6.");
			
			CRPGData.data[%target.bl_id].Value["Shell"] += %amount;
		}
		else if(%data $= "Explosive")
		{
			%amount = mfloor(%amount);
			if(%amount <= 0)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Explosive Ammo\c6 to add.");
			MessageClient(%client,'',"\c6You added \c3"@ %amount @" Explosive Ammo\c6 to \c3"@ %target.name @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 added you \c3"@ %amount @" Explosive Ammo\c6.");
			
			CRPGData.data[%target.bl_id].Value["Explosive"] += %amount;
		}
		else if(%data $= "Bricks")
		{
			%amount = mfloor(%amount);
			if(%amount <= 0)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Bricks\c6 to add.");
			MessageClient(%client,'',"\c6You added \c3"@ %amount @" Bricks\c6 to \c3"@ %target.name @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 added you \c3"@ %amount @" Bricks\c6.");
			
			CRPGData.data[%target.bl_id].Value["Bricks"] += %amount;
		}
		else if(%data $= "Hunger")
		{
			if(CRPGData.data[%target.bl_id].Value["Hunger"] + %amount > 14)
				return MessageClient(%client,'',"\c6Please enter a lower hunger to add.");
			if(%amount <= 0 || %amount > 14)
				return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Hunger\c6 to add under \c314\c6.");
			MessageClient(%client,'',"\c6You added \c3"@ %amount @" Hunger\c6 to \c3"@ %target.name @"\c6.");
			MessageClient(%target,'',"\c3"@ %client.name @"\c6 added you \c3"@ %amount @" Hunger\c6.");
			
			CRPGData.data[%target.bl_id].Value["Hunger"] += %amount;
			commandtoclient(%target,'CRPGsetHunger',CRPGData.data[%target.bl_id].Value["Hunger"]);

			%target.player.setCRPGScale();
		}
		else
				messageclient(%client,'',"\c6Data \c3"@ %data @"\c6 not found.");
	}
	else
	{
		MessageClient(%client,'',"\c6You must wait before using this command again.");
	}
	%client.SetInfo();
}

function servercmdTeamMessageSent(%client,%text)
{
	if(%client.lastChat $= "" || $Sim::Time - %client.lastChat > 2)
	{
		if(CRPGData.data[%client.bl_id].Value["Mute"])
			return MessageClient(%client,'',"\c6You cannot chat while muted.");
		%text = StripMLControlChars(%text);
		%text = trim(%text);
		%text = getsubstr(%text,0,"64");
		if(strlen(%text) <= 1)
			return MessageClient(%client,'',"\c6Your message was too short.");
		if(CRPGData.data[%client.bl_id].Value["JailData"])
		{
			for(%a = 0; %a < $Server::PlayerCount;%a++)
			{
				%subClient = ClientGroup.getObject(%a);
				if(CRPGData.data[%subclient.bl_id].Value["JailData"] || %subClient.isAdmin)
					MessageClient(%subClient,'',"\c6[<color:uummgg>Inmate\c6]\c3"@ %client.name @ "\c6: \c7"@ %text);
			}
		}
		else
		{
			for(%a = 0; %a < $Server::PlayerCount;%a++)
			{
				%subClient = ClientGroup.getObject(%a);
				if(CRPGData.data[%subclient.bl_id].Value["JobID"].path $= CRPGData.data[%client.bl_id].Value["JobID"].path || %subClient.isAdmin)
					MessageClient(%subClient,'',"\c6["@ CRPGData.data[%client.bl_id].Value["JobID"].color @ CRPGData.data[%client.bl_id].Value["JobID"].jobname @"\c6]\c3"@ %client.name @ "\c6: \c7"@ %text);
			}
		}
		%client.isIdle = 0;
		%client.lastChat = $Sim::Time;
	}
	else
		MessageClient(%client,'',"\c6You must wait before using the chat again.");
}
function serverCmdPardon(%client,%target)
{
	if(!CRPGData.data[%client.bl_id].Value["JobID"].Pardon)
		return MessageClient(%client,'',"\c6You cannot pardon with your current job.");
	if(%target $= "")
		return MessageClient(%client,'',"\c6You must enter someone to pardon.");
	%target = findclientbyname(%target);
	if(%target == %client)
		return MessageClient(%client,'',"\c6You cannot pardon yourself.");
	if(!isobject(%target))
		return MessageClient(%client,'',"\c6Target not found.");
	if(!CRPGData.data[%target.bl_id].Value["JailData"])
		return MessageClient(%client,'',"\c3"@ %target.name @"\c6 is not in jail.");
	%price = CRPGData.data[%target.bl_id].Value["JailData"]*$CRPG::Pref::Price::Pardon;
	if(%price > CRPGData.data[%client.bl_id].Value["Money"])
		return MessageClient(%client,'',"\c6You do not have \c3$"@ %price @"\c6 to pardon \c3"@ %target.name @"\c6.");
	MessageClient(%client,'',"\c6You have payed \c3$"@ %price @"\c6 and issued \c3"@ %target.name @"\c6 a pardon.");
	MessageClient(%target,'',"\c3"@ %client.name @"\c6 has issued you a pardon.");
	if(CRPGData.Data[%client.bl_id].Value["Experience"] < CRPGData.Data[%client.bl_id].Value["JobID"].MaxExperience)
	{
		if(getrandom(0,6) <= CRPGData.data[%target.bl_id].Value["Jaildata"])
		{
			CRPGData.data[%client.bl_id].Value["Experience"]++;
			MessageClient(%client,'',"\c6Your experience has increased to \c3"@ CRPGData.data[%client.bl_id].Value["Experience"] @"\c6.");
		}
	}

	CRPGData.data[%client.bl_id].Value["Money"] -= %price;
	commandtoclient(%client,'CRPGsetMoney',CRPGData.data[%client.bl_id].Value["Money"]);
	CRPGData.data[%target.bl_id].Value["JailData"] = 0;
	commandtoclient(%client, 'CRPGsetJob', 0, CRPGData.data[%client.bl_id].Value["Student"], CRPGData.Data[%client.bl_id].Value["JobID"].JobName);

	if(isObject(%client.player.tempBrick))
		%client.player.tempBrick.delete();
	%target.spawnPlayer();
}
function serverCmdClearRecord(%client,%target)
{
	if(!CRPGData.data[%client.bl_id].Value["JobID"].ClearRecords)
		return MessageClient(%client,'',"\c6You cannot clear records with your current job.");
	if(%target $= "")
		return MessageClient(%client,'',"\c6You must enter someone to clear.");
	%target = findclientbyname(%target);
	if(%target == %client)
		return MessageClient(%client,'',"\c6You cannot clear your own record.");
	if(!isobject(%target))
		return MessageClient(%client,'',"\c6Target not found.");
	if(!CRPGData.data[%target.bl_id].Value["Record"])
		return MessageClient(%client,'',"\c3"@ %target.name @"\c6 already has a clean record.");
	if($CRPG::Pref::Price::ClearRecord > CRPGData.data[%client.bl_id].Value["Money"])
		return MessageClient(%client,'',"\c6You do not have \c3$"@ $CRPG::Pref::Price::ClearRecord @"\c6 to clear the record of \c3"@ %target.name @"\c6.");
	MessageClient(%client,'',"\c6You have payed \c3$"@ $CRPG::Pref::Price::ClearRecord @"\c6 and cleared the record of \c3"@ %target.name @"\c6.");
	MessageClient(%target,'',"\c3"@ %client.name @"\c6 has cleared your record.");
	if(CRPGData.Data[%client.bl_id].Value["Experience"] < CRPGData.Data[%client.bl_id].Value["JobID"].MaxExperience)
	{
		if(getrandom(1,2) == 1)
		{
			CRPGData.data[%client.bl_id].Value["Experience"]++;
			MessageClient(%client,'',"\c6Your experience has increased to \c3"@ CRPGData.data[%client.bl_id].Value["Experience"] @"\c6.");
		}
	}
	CRPGData.data[%client.bl_id].Value["Money"] -= $CRPG::Pref::Price::ClearRecord;
	commandtoclient(%client,'CRPGsetMoney',CRPGData.data[%client.bl_id].Value["Money"]);
	CRPGData.data[%target.bl_id].Value["Record"] = 0;
	CRPGData.data[%target.bl_id].Value["FirstOffense"] = 0;
	%client.SetInfo();
}
function serverCmdMute(%client, %target)
{
	if(!%client.isAdmin)
		return MessageClient(%client,'',"\c6You must be an admin to use this command.");
	if(%target $= "")
		return MessageClient(%client,'',"\c6You must enter someone to mute.");
	%target = findclientbyname(%target);
	if(%target == %client)
		return MessageClient(%client,'',"\c6You cannot mute yourself.");
	if(!isobject(%target))
		return MessageClient(%client,'',"\c6Target not found.");
	if(%target.isAdmin)
		return MessageClient(%client,'',"\c6You cannot mute an admin.");
	if(CRPGData.data[%target.bl_id].Value["Mute"])
		return MessageClient(%client,'',"\c3"@ %target.name @"\c6 is already muted.");
	MessageClient(%client,'',"\c6You have muted \c3"@ %target.name @"\c6.");
	MessageClient(%target,'',"\c3"@ %client.name @"\c6 has muted you.");
	CRPGData.data[%target.bl_id].Value["Mute"] = 1;
	%client.SetInfo();
}
function serverCmdUnMute(%client, %target)
{
	if(!%client.isAdmin)
		return MessageClient(%client,'',"\c6You must be an admin to use this command.");
	if(%target $= "")
		return MessageClient(%client,'',"\c6You must enter someone to unmute.");
	%target = findclientbyname(%target);
	if(%target == %client)
		return MessageClient(%client,'',"\c6You cannot unmute yourself.");
	if(!isobject(%target))
		return MessageClient(%client,'',"\c6Target not found.");
	if(!CRPGData.data[%target.bl_id].Value["Mute"])
		return MessageClient(%client,'',"\c3"@ %target.name @"\c6 is already unmuted.");
	MessageClient(%client,'',"\c6You have unmuted \c3"@ %target.name @"\c6.");
	MessageClient(%target,'',"\c3"@ %client.name @"\c6 has unmuted you.");
	CRPGData.data[%target.bl_id].Value["Mute"] = 0;
}
function ServerCmdGetMuted(%client)
{
	if(%client.lastCommand $= "" || $Sim::Time - %client.lastCommand > 2)
	{
		%client.lastCommand = $Sim::Time;
		if(!%client.isAdmin)
			return MessageClient(%client,'',"\c6You must be an admin to use this command.");
		messageclient(%client,'',"\c6The following players are currently muted.");
		for(%a = 0; %a < $Server::PlayerCount;%a++)
		{
			%subClient = ClientGroup.getObject(%a);
			if(CRPGData.data[%subclient.bl_id].Value["Mute"])
				MessageClient(%Client,'',"\c3"@ %SubClient.name);
		}
	}
	else
		MessageClient(%client,'',"\c6You must wait before using this command again.");
}
function ServerCmdPayDems(%client, %amount, %target)
{
	if(!CRPGData.data[%client.bl_id].Value["JobID"].PayDems)
		return MessageClient(%client,'',"\c6You cannot pay demerits with your current job.");
	if(%target $= "")
		return MessageClient(%client,'',"\c6You must enter someone to pay demerits.");
	%target = findclientbyname(%target);
	if(%target == %client)
		return MessageClient(%client,'',"\c6You cannot pay your own demerits.");
	if(!isobject(%target))
		return MessageClient(%client,'',"\c6Target not found.");
	if(!CRPGData.data[%target.bl_id].Value["Demerits"])
		return MessageClient(%client,'',"\c3"@ %target.name @"\c6 has no demerits to pay off.");
	%amount = mfloor(%amount);
	if(%amount <= 0)
		return MessageClient(%client,'',"\c6Please enter a valid amount of demerits to pay off.");
	if(%amount > CRPGData.data[%target.bl_id].Value["Demerits"])
		%amount = CRPGData.data[%target.bl_id].Value["Demerits"];
	if(%amount > CRPGData.data[%client.bl_id].Value["Money"])
		return MessageClient(%client,'',"\c6You cannot afford to pay off \c3$"@ %amount @"\c6 of demerits.");
	MessageClient(%client,'',"\c6You have payed off \c3"@ %amount @"\c6 demerits of \c3"@ %target.name @"\c6.");
	MessageClient(%target,'',"\c3"@ %client.name @"\c6 has payed off \c3"@ %amount @"\c6 of your demerits.");
	if(CRPGData.Data[%client.bl_id].Value["Experience"] < CRPGData.Data[%client.bl_id].Value["JobID"].MaxExperience)
	{
		if(getrandom(0,1200) <= %amount)
		{
			CRPGData.data[%client.bl_id].Value["Experience"]++;
			MessageClient(%client,'',"\c6Your experience has increased to \c3"@ CRPGData.data[%client.bl_id].Value["Experience"] @"\c6.");
		}
	}
	CRPGData.data[%client.bl_id].Value["Money"]-= %amount;
	CRPGData.data[%target.bl_id].Value["Demerits"]-= %amount;
	commandtoclient(%client,'CRPGsetMoney',CRPGData.data[%client.bl_id].Value["Money"]);
	commandtoclient(%target,'CRPGSetDems',CRPGData.data[%target.bl_id].Value["Demerits"]);
}
function servercmdGive(%client, %data, %amount, %target)
{
	if(!isobject(%client.player))
		return MessageClient(%client,'',"\c6Spawn first before using this command.");
	if(CRPGData.Data[%client.bl_id].Value["JailData"])
		return MessageClient(%client,'',"\c6You cannot give while in jail.");
	if(getWordCount(%target))
		%target = findclientbyname(%target);
	else
		%target = containerRayCast(%client.player.getEyePoint(),vectorAdd(vectorScale(vectorNormalize(%client.player.getEyeVector()),5),%client.player.getEyePoint()),$typeMasks::playerObjectType,%client.player).client;
	if(%target == %client)
		return MessageClient(%client,'',"\c6You cannot give to yourself.");
	if(!isObject(%target.player))
		return MessageClient(%client,'',"\c6Target not found.");
	if(VectorDist(%client.player.getPosition(),%target.player.getPosition()) > 8)
		return MessageClient(%client,'',"\c6Your target is too far away.");
	if(%data $= "Money" || %data $= "Cash")
	{
		if(!CRPGData.Data[%client.bl_id].Value["Money"])
			return MessageClient(%client,'',"\c6You need \c3Money\c6 to give.");
		%amount = mfloor(%amount);
		if(%amount <= 0)
			return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Money\c6 to give.");
		if(%amount > CRPGData.Data[%client.bl_id].Value["Money"])
			%amount = CRPGData.Data[%client.bl_id].Value["Money"];					

		MessageClient(%client,'',"\c6You gave \c3$"@ %amount @"\c6 to \c3"@ %target.name @"\c6.");
		MessageClient(%target,'',"\c3"@ %client.name @"\c6 gave you \c3$"@ %amount @"\c6.");

		CRPGData.Data[%client.bl_id].Value["Money"] -= %amount;
		CRPGData.Data[%target.bl_id].Value["Money"] += %amount;
			
		commandtoclient(%client,'CRPGsetMoney',CRPGData.Data[%client.bl_id].Value["Money"]);
		commandtoclient(%target,'CRPGSetMoney',CRPGData.Data[%target.bl_id].Value["Money"]);
	}
	else if(%data $= "Lumber")
	{
		if(!CRPGData.Data[%client.bl_id].Value["Lumber"])
		{
			return MessageClient(%client,'',"\c6You need \c3Lumber\c6 to give.");
			return;
		}
		%amount = mfloor(%amount);
		if(%amount <= 0)
		{
			return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Lumber\c6 to give.");
			return;
		}
		if(%amount > CRPGData.Data[%client.bl_id].Value["Lumber"])
			%amount = CRPGData.Data[%client.bl_id].Value["Lumber"];					

		MessageClient(%client,'',"\c6You gave \c3"@ %amount @"\c6 lumber to \c3"@ %target.name @"\c6.");
		MessageClient(%target,'',"\c3"@ %client.name @"\c6 gave you \c3"@ %amount @"\c6 lumber.");

		CRPGData.Data[%client.bl_id].Value["Lumber"] -= %amount;
		CRPGData.Data[%target.bl_id].Value["Lumber"] += %amount;
	}
	else if(%data $= "Ore")
	{
		if(!CRPGData.Data[%client.bl_id].Value["Ore"])
			return MessageClient(%client,'',"\c6You need \c3Ore\c6 to give.");
		%amount = mfloor(%amount);
		if(%amount <= 0)
			return MessageClient(%client,'',"\c6Please enter a valid amount of \c3Ore\c6 to give.");
		if(%amount > CRPGData.Data[%client.bl_id].Value["Ore"])
			%amount = CRPGData.Data[%client.bl_id].Value["Ore"];					

		MessageClient(%client,'',"\c6You gave \c3"@ %amount @"\c6 ore to \c3"@ %target.name @"\c6.");
		MessageClient(%target,'',"\c3"@ %client.name @"\c6 gave you \c3"@ %amount @"\c6 ore.");

		CRPGData.Data[%client.bl_id].Value["Ore"] -= %amount;
		CRPGData.Data[%target.bl_id].Value["Ore"] += %amount;
	}
	else
		messageclient(%client,'',"\c6Data \c3"@ %data @"\c6 not found.");
}
function servercmdBuy(%client, %data)
{
	if(%data $= "")
	{
		MessageClient(%client,'',"\c6Here is a list of things you can buy.");
		MessageClient(%client,'', "\c6Celphone<lmargin:100>\c6Price: \c3$"@ $CRPG::Pref::Price::Cellphone @"<lmargin:0>");
		MessageClient(%client,'', "\c6Light<lmargin:100>\c6Price: \c3$"@ $CRPG::Pref::Price::Light @"<lmargin:0>");
	}
	else if(%data $= "CellPhone")
	{
		if(CRPGData.data[%client.bl_id].Value["CellPhone"])
			return MessageClient(%client,'',"\c6You already have a cellphone.");
		if(CRPGData.data[%client.bl_id].Value["Money"] < $CRPG::Pref::Price::Cellphone)
			return MessageClient(%client,'',"\c6You cannot afford a cellphone for \c3$"@ $CRPG::Pref::Price::Cellphone @"\c6.");
		CRPGData.data[%client.bl_id].Value["Money"] -= $CRPG::Pref::Price::Cellphone;
		CRPGData.data[%client.bl_id].Value["Cellphone"] = 1;
		commandtoclient(%client,'CRPGSetMoney',CRPGData.data[%client.bl_id].Value["Money"]);
		MessageClient(%client,'',"\c6You have bought a cellphone for \c3$"@ $CRPG::Pref::Price::Cellphone @"\c6.");
		commandtoclient(%client,'CRPGSetClock',$CRPG::Data::Hour, $CRPG::Data::Minute);
	}
	else if(%data $= "Light")
	{
		if(CRPGData.data[%client.bl_id].Value["Light"])
			return MessageClient(%client,'',"\c6You already have a light.");
		if(CRPGData.data[%client.bl_id].Value["Money"] < $CRPG::Pref::Price::Light)
			return MessageClient(%client,'',"\c6You cannot afford a light for \c3$"@ $CRPG::Pref::Price::Light @"\c6.");
		CRPGData.data[%client.bl_id].Value["Money"] -= $CRPG::Pref::Price::Light;
		CRPGData.data[%client.bl_id].Value["Light"] = 1;
		MessageClient(%client,'',"\c6You have bought a light for \c3$"@ $CRPG::Pref::Price::Light @"\c6.");
		commandtoclient(%client,'CRPGSetMoney',CRPGData.data[%client.bl_id].Value["Money"]);
		%client.SetInfo();
	}
	%client.SetInfo();
}
function serverCmdText(%client, %target, %a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k,%l,%m,%n,%o,%p,%q,%r,%s,%t,%u,%v,%w,%x,%y,%z)
{
	if(%client.lastChat $= "" || $Sim::Time - %client.lastChat > 2)
	{
		%client.lastChat = $Sim::Time;
		if(CRPGData.data[%client.bl_id].Value["Mute"])
			return MessageClient(%client,'',"\c6You cannot text while muted.");
		if(CRPGData.Data[%client.bl_id].Value["JailData"])
			return MessageClient(%client,'',"\c6You cannot text while in jail.");
		if(!CRPGData.data[%client.bl_id].Value["Cellphone"])
			return messageClient(%client,'',"\c6You need a cellphone to text. To get a cellphone say \c3/Buy Cellphone\c6.");
		%target = findclientbyname(%target);
		if(%client == %target)
			return messageclient(%client,'',"\c6You cannot text yourself.");
		if(!isobject(%target))
			return messageclient(%client,'',"\c6Target not found.");
		if(!CRPGData.data[%target.bl_id].Value["Cellphone"])
			return messageclient(%client,'',"\c3"@ %target.name @"\c6 does not have a cellphone.");
		if(CRPGData.data[%target.bl_id].Value["JailData"])
			return messageclient(%client,'',"\c3"@ %target.name @"\c6 is in jail.");
		%text = trim(%a SPC %b SPC %c SPC %d SPC %e SPC %f SPC %g SPC %h SPC %i SPC %j SPC %k SPC %l SPC %m SPC %n SPC %o SPC %p SPC %q SPC %r SPC %s SPC %t SPC %u SPC %v SPC %w SPC %x SPC %y SPC %z);
		%text = StripMLControlChars(%text);
		%text = getsubstr(%text,0,"64");
		if(strlen(%text) <= 1)
			return MessageClient(%client,'',"\c6Your message was too short.");
		messageclient(%client,'',"\c6[\c7Text to "@ %target.name @"\c6]\c3"@ %client.name @"\c6: "@ %text);
		messageclient(%target,'',"\c6[\c7Text to You\c6]\c3"@ %client.name @"\c6: "@ %text);
	}
	else
		MessageClient(%client,'',"\c6You must wait before texting again.");
}
function serverCmdReset(%client)
{
	if(CRPGData.data[%client.bl_id].Value["Money"] < $CRPG::Pref::Price::Reset)
		return MessageClient(%client,'',"\c6You need \c3$"@ $CRPG::Pref::Price::Reset @"\c6 to reset.");
	MessageClient(%client,'',"\c6Your account has been reset.");

	CRPGData.resetData(%client.bl_id);
	CRPGData.data[%client.bl_id].Value["Height"] = getrandom(90,110)*0.01;
	CRPGData.data[%client.bl_id].Value["Name"] = %client.name;
	%client.setScore(0);
	while(isObject("Droppable"))
		Droppable.delete();

	%client.spawnPlayer();
}
function serverCmdNope(%client)
{
	if(isObject(%client.player) && !%client.player.nope)
	{
		%client.player.playThread(0,"headUp");
		%client.player.schedule(200,"playThread",1,"headUp");
		%client.player.schedule(400,"playThread",2,"headUp");
		%client.player.schedule(600,"playThread",3,"headUp");
		%client.player.schedule(800,"setHeadUp",1);
		%client.player.nope = 1;
	}
}

function serverCmdRefresh(%client)
{
	%client.setInfo();
}

function serverCmdStats(%client)
{
	%data = CRPGData.data[%client.bl_id];
	%hlevel = CRPGData.data[%client.bl_id].value["Hunger"];
	messageClient(%client,'',"\c6[Stats for " @ %client.name @ "][Details][Cash: " @ %client.getCashString() @ "\c6][Money in Bank: " @ %data.value["Bank"] @ "][Hunger: " @ $CRPG::Hunger[mfloor(%hlevel)] @ "\c6]");
	messageClient(%client,'',"\c6[Stats for " @ %client.name @ "][Details][Arrest Record: " @ YN(%data.Record) @ "][Job: " @ %data.value["JobID"].jobName @ "]");
	messageClient(%client,'',"\c6[Stats for " @ %client.name @ "][Levels][Experience: " @ %data.value["Experience"] @ " ][Mining: " @ %data.value["MiningSkill"] @ "][Woodcutting: " @ %data.value["CuttingSkill"] @ "]");
	messageClient(%client,'',"\c6[Stats for " @ %client.name @ "][Inventory][Iron Ore: " @ %data.value["Iron"] @ "][Silver Ore: " @ %data.value["Silver"] @ "][Platinum Ore: " @ %data.value["Platinum"] @ "]");
	messageClient(%client,'',"\c6[Stats for " @ %client.name @ "][Inventory][Oak Wood: " @ %data.value["Oak"] @ "][Maple Wood: " @ %data.value["Maple"] @ "][Morning Wood: " @ %data.value["Morning"] @ "]");
	
}

function serverCmdCityStats(%client)
{
	messageClient(%client,'',"\c6The city currently has the following resources:");
	messageClient(%client,'',"\c6" @ $CRPG::Data::Materials @ " materials in the treasury, and " @ $CRPG::Data::Money @ " dollars in the treasury.");
}