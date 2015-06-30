// ============================================================
// Project            :  City3
// File               :  .\Common.cs
// Created on         :  Tuesday, June 4, 2013 2:02 PM
// Description        :  Common operations of City3
// ============================================================

if(!isObject(HandtaserItem))
{
	//AddDamageType("Handtaser",   '<bitmap:Add-Ons/Gamemode_CityRPG/Shapes/CI/CI_Handtaser> %1',    '%2 <bitmap:Add-Ons/Gamemode_CityRPG/Shapes/CI/CI_Handtaser> %1', 0.5, 1);
	
	datablock AudioProfile(taserExplosionSound)
	{
		filename	 = "Add-Ons/Gamemode_CRPG/Sounds/Tazer.wav";
		description = AudioClosest3d;
		preload = true;
	};
	
	datablock ExplosionData(HandtaserExplosion : hammerExplosion)
	{
		soundProfile = taserExplosionSound;
	};
	datablock ProjectileData(HandtaserProjectile)
	{	
		directDamage		= 15;
		directDamageType	= $DamageType::Handtaser;
		radiusDamageType	= $DamageType::Handtaser;
		explosion		= HandtaserExplosion;
		
		muzzleVelocity		= 50;
		velInheritFactor	= 1;
	
		armingDelay		= 0;
		lifetime		= 100;
		fadeDelay		= 70;
		explodeOnDeath		= false;
		bounceElasticity	= 0;
		bounceFriction		= 0;
		isBallistic		= false;
		gravityMod 		= 0.0;
	
		hasLight		= false;
		lightRadius		= 1.0;
		lightColor		= "0 0.25 0.5";
	};
	datablock ItemData(HandtaserItem)
	{
		category		= "Weapon";
		className		= "Weapon";
		
		shapeFile		= "Add-Ons/Gamemode_CRPG/Shapes/Handtaser.dts";
		mass			= 1;
		density 		= 0.2;
		elasticity		= 0.2;
		friction		= 0.6;
		emap			= true;
	
		uiName			= "Handtaser";
		iconName		= "Add-Ons/Gamemode_CRPG/Shapes/ItemIcons/Icon_Handtaser";
		doColorShift		= false;
	
		image			= HandtaserImage;
		canDrop			= true;
	
	};
	datablock ShapeBaseImageData(HandtaserImage)
	{
		
		shapeFile		= "Add-Ons/Gamemode_CRPG/Shapes/Handtaser.dts";
		emap			= true;
		mountPoint		= 0;
		eyeOffset		= "0 0 0";
		offset			= "0 0 0";
		correctMuzzleVector	= false;
		className		= "WeaponImage";
		
		item			= HandtaserItem;
		ammo			= " ";
		projectile		= HandtaserProjectile;
		projectileType		= Projectile;
		
		melee			= true;
		doRetraction		= true;
		armReady		= true;
		
		doColorShift		= false;
		colorShiftColor		= "0 3 5 1";

		stateName[0]			= "Activate";
		stateTimeoutValue[0]		= 0.1;
		stateTransitionOnTimeout[0]	= "Ready";

		stateName[1]			= "Ready";
		stateTransitionOnTriggerDown[1]	= "PreFire";
		stateAllowImageChange[1]	= true;
	
		stateName[2]			= "PreFire";
		stateScript[2]			= "onPreFire";
		stateAllowImageChange[2]	= false;
		stateTimeoutValue[2]		= 0.085;
		stateTransitionOnTimeout[2]	= "Fire";
	
		stateName[3]			= "Fire";
		stateTimeoutValue[3]		= 0.1;
		stateTransitionOnTimeout[3]	= "PreCheckFire";
		stateFire[3]			= true;
		stateAllowImageChange[3]	= false;
		stateSequence[3]		= "Fire";
		stateScript[3]			= "onFire";
		stateWaitForTimeout[3]		= true;

		stateName[4]			= "PreCheckFire";
		stateTimeoutValue[4]		= 0.15;
		stateTransitionOnTimeout[4]	= "CheckFire";
		stateScript[4]			= "onStopFire";

		stateName[5]			= "CheckFire";
		stateTransitionOnTriggerUp[5]	= "StopFire";
		stateTransitionOnTriggerDown[5]	= "PreFire";
	
		stateName[6]			= "StopFire";
		stateTransitionOnTimeout[6]	= "Ready";
		stateTimeoutValue[6]		= 0.1;
		stateAllowImageChange[6]	= false;
		stateWaitForTimeout[6]		= true;
		stateSequence[6]		= "StopFire";
		stateScript[6]			= "onStopFire";
	};
}

// ============================================================
// Section 2 : Functions
// ============================================================


function HandtaserProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	%class = %col.getClassName();
	if(%class $= "Player")
	{
		%col.setVelocity(VectorScale(getRandom(0, 0.250) SPC getRandom(0, 0.250) SPC "1", 10));
		freezeLoop(%col);
		tumble(%col);
		if(CRPGData.Data[%col.client.bl_id].Value["Demerits"] < 200)
		{
			CRPGData.data[%obj.client.bl_id].Value["Demerits"] += $CRPG::Pref::Demerits::TasingBros;
			MessageClient(%obj.client,'',"\c6You have commited a crime. [\c3Tasing Innocents\c6]");
		}
	}
}

function freezeLoop(%this)
{
	cancel(%this.freezeLoop);
	if(%this.loops >= 20)
	{
		%this.setDataBlock(PlayerCRPG);
		%this.loops = "";
		return;
	}

	if(!isObject(%this))
	{
		return;
	}

	if(%this.getDataBlock() !$= nameToId(playerFrozenArmor))
	{
		%this.setDataBlock(playerFrozenArmor);
	}

	%pos = %this.getTransform();
	%rnd = getRandom(-10, 10) * 0.1;
	%newrot = getWord(%pos, 5) + %rnd;
	%this.setTransform(getWords(%pos, 0, 4) SPC %newrot);

	%this.loops += 1;
	%this.freezeLoop = schedule(100, 0, "freezeLoop", %this);
}

package CRPG_HandtaserPackage
{
	function Armor::damage(%this, %obj, %src, %unk, %dmg, %type)
	{
		// Handtaser Abuse Preventitive Measures
		if(!(isObject(%src) && %src.getDatablock().getName() $= "deathVehicle"))
		{
			parent::damage(%this, %obj, %src, %unk, %dmg, %type);
		}
	}	
};
activatePackage(CRPG_HandtaserPackage);