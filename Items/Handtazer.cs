// ============================================================
// Project            :  City3
// File               :  .\Common.cs
// Created on         :  Tuesday, June 4, 2013 2:02 PM
// Description        :  Common operations of City3
// ============================================================

if(!isObject(HandtazerItem))
{
	//AddDamageType("Handtazer",   '<bitmap:Add-Ons/Gamemode_CityRPG/Shapes/CI/CI_Handtazer> %1',    '%2 <bitmap:Add-Ons/Gamemode_CityRPG/Shapes/CI/CI_Handtazer> %1', 0.5, 1);
	
	datablock AudioProfile(taserExplosionSound)
	{
		filename	 = "Add-Ons/Gamemode_CRPG/Sounds/Tazer.wav";
		description = AudioClosest3d;
		preload = true;
	};
	
	datablock ExplosionData(HandtazerExplosion : hammerExplosion)
	{
		soundProfile = taserExplosionSound;
	};
	datablock ProjectileData(HandtazerProjectile)
	{	
		directDamage		= 15;
		directDamageType	= $DamageType::Handtazer;
		radiusDamageType	= $DamageType::Handtazer;
		explosion		= HandtazerExplosion;
		
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
	datablock ItemData(HandtazerItem)
	{
		category		= "Weapon";
		className		= "Weapon";
		
		shapeFile		= "Add-Ons/Gamemode_CRPG/Shapes/Handtazer.dts";
		mass			= 1;
		density 		= 0.2;
		elasticity		= 0.2;
		friction		= 0.6;
		emap			= true;
	
		uiName			= "Handtazer";
		iconName		= "Add-Ons/Gamemode_CRPG/Shapes/ItemIcons/Icon_Handtazer";
		doColorShift		= false;
	
		image			= HandtazerImage;
		canDrop			= true;
	
	};
	datablock ShapeBaseImageData(HandtazerImage)
	{
		
		shapeFile		= "Add-Ons/Gamemode_CRPG/Shapes/Handtazer.dts";
		emap			= true;
		mountPoint		= 0;
		eyeOffset		= "0 0 0";
		offset			= "0 0 0";
		correctMuzzleVector	= false;
		className		= "WeaponImage";
		
		item			= HandtazerItem;
		ammo			= " ";
		projectile		= HandtazerProjectile;
		projectileType		= Projectile;
		
		melee			= true;
		doRetraction		= false;
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
function HandtazerImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armAttack);
}
function HandtazerImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

// misc shit functions

function handtazererProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	if((%col.getType() & $typeMasks::playerObjectType) && isObject(%col.client))
	{
		%col.setVelocity(VectorScale(getRandom(0, 0.250) SPC getRandom(0, 0.250) SPC "1", 10));
		tumble(%col.player);
		
		if(CRPG_illegalAttackTest(%obj.client, %col.client))
		{
			commandToClient(%obj.client, 'centerPrint', "\c6You have commited a crime. [\c3Tasing Innocents\c6]", 3);
			CityRPG_AddDemerits(%obj.client.bl_id, $CityRPG::demerits::tasingBros);
		}
	}
}