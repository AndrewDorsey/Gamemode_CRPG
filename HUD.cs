// ============================================================
// Project            :  CRPG
// File               :  .\HUD.cs
// Copyright          :  
// Author             :  Andrew
// Created on         :  Wednesday, July 31, 2013 2:07 AM
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  shit that runs the hud
//                    :  
//                    :  
// ============================================================
function serverCmdRegisterArmor(%client)
{
	%client.hasArmorBar = true;
	%dmgPercent = %player.getDamageLevel() - (%player.pseudoArmor + %player.tf2Overheal);
	%dmgPercent = %dmgPercent / %player.getDatablock().maxDamage;
	commandToClient(%client, 'setDamageLevel', %dmgPercent);
}

package Armor_Server
{
	// onDamage also handles negative integers.
	function Armor::onDamage(%this, %obj, %damage)
	{
		parent::onDamage(%this, %obj, %damage);

		// Send their Armor.
		%dmgPercent = %obj.getDamageLevel() - (%obj.pseudoArmor + %obj.tf2Overheal);
		%dmgPercent = mFloatLength(%dmgPercent / %obj.getDatablock().maxDamage, 4);
		
		commandToClient(%client, 'setDamageLevel', %dmgPercent);
	}
	
	function GameConnection::onClientEnterGame(%client)
	{
		parent::onClientEnterGame(%client);
		commandToClient(%client, 'detectArmor');
	}
	
	function GameConnection::spawnPlayer(%client)
	{
		%result = parent::spawnPlayer(%client);
		
		if(%client.hasArmorBar)
		{
			%player = %client.player;
			
			if(isObject(%player))
			{
				%dmgPercent = %player.getDamageLevel() - (%player.pseudoArmor + %player.tf2Overheal);
				%dmgPercent = mFloatLength(%dmgPercent / %player.getDatablock().maxDamage, 4);
				commandToClient(%client, 'setDamageLevel', %dmgPercent);
			}
		}
		
		return %result;
	}
};
activatePackage(Armor_Server);

function serverCmdRegisterHealth(%client)
{
	%client.hasHealthBar = true;
	%dmgPercent = %player.getDamageLevel() - (%player.pseudoHealth + %player.tf2Overheal);
	%dmgPercent = %dmgPercent / %player.getDatablock().maxDamage;
	commandToClient(%client, 'setDamageLevel', %dmgPercent);
}

package Health_Server
{
	// onDamage also handles negative integers.
	function Armor::onDamage(%this, %obj, %damage)
	{
		parent::onDamage(%this, %obj, %damage);

		// Send their health.
		%dmgPercent = %obj.getDamageLevel() - (%obj.pseudoHealth + %obj.tf2Overheal);
		%dmgPercent = mFloatLength(%dmgPercent / %obj.getDatablock().maxDamage, 4);
		
		commandToClient(%client, 'setDamageLevel', %dmgPercent);
	}
	
	function GameConnection::onClientEnterGame(%client)
	{
		parent::onClientEnterGame(%client);
		commandToClient(%client, 'detectHealth');
	}
	
	function GameConnection::spawnPlayer(%client)
	{
		%result = parent::spawnPlayer(%client);
		
		if(%client.hasHealthBar)
		{
			%player = %client.player;
			
			if(isObject(%player))
			{
				%dmgPercent = %player.getDamageLevel() - (%player.pseudoHealth + %player.tf2Overheal);
				%dmgPercent = mFloatLength(%dmgPercent / %player.getDatablock().maxDamage, 4);
				commandToClient(%client, 'setDamageLevel', %dmgPercent);
			}
		}
		
		return %result;
	}
};
activatePackage(Health_Server);