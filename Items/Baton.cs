//+---------------------------------------------+
//| Title:	Baton				|
//| Author:	Jasa				|
//+---------------------------------------------+
//| Jails criminals and breaks down doors. 	|
//+---------------------------------------------+
if(!isObject(BatonItem))
{
	AddDamageType("Baton",   '<bitmap:Add-Ons/GameMode_CRPG/Shapes/CI/CI_Baton> %1',    '%2 <bitmap:Add-Ons/GameMode_CRPG/Shapes/CI/CI_Baton> %1', 0.5, 1);

	datablock ExplosionData(batonExplosion : hammerExplosion)
	{
		soundProfile = hammerHitSound;
	};
	datablock ProjectileData(BatonProjectile)
	{	
		directDamage		= 15;
		directDamageType	= $DamageType::Baton;
		radiusDamageType	= $DamageType::Baton;
		explosion		= BatonExplosion;
		
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
	datablock ItemData(BatonItem)
	{
		category		= "Weapon";
		className		= "Weapon";
		
		shapeFile		= "Add-Ons/GameMode_CRPG/Shapes/Baton.dts";
		mass			= 1;
		density 		= 0.2;
		elasticity		= 0.2;
		friction		= 0.6;
		emap			= true;
	
		uiName			= "Baton";
		iconName		= "Add-Ons/GameMode_CRPG/Shapes/ItemIcons/Icon_Baton";
		doColorShift		= false;
	
		image			= BatonImage;
		canDrop			= true;
	
	};
	datablock ShapeBaseImageData(BatonImage)
	{
		
		shapeFile		= "Add-Ons/GameMode_CRPG/Shapes/Baton.dts";
		emap			= true;
		mountPoint		= 0;
		eyeOffset		= "0 0 0";
		offset			= "0 0 0";
		correctMuzzleVector	= false;
		className		= "WeaponImage";
		
		item			= BatonItem;
		ammo			= " ";
		projectile		= BatonProjectile;
		projectileType		= Projectile;
		
		melee			= true;
		doRetraction		= false;
		armReady		= true;
		
		doColorShift		= false;
		colorShiftColor		= "0 0 0 1";

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
function BatonImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armAttack);
}
function BatonImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}
function BatonProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	%class = %col.getClassName();
	if(%class $= "fxDTSBrick")
	{
		if(%col.shape)
		{
			%col.fakeKillBrick("0 0 0", 10, %obj.client);
			if(CRPGData.data[%col.getGroup().bl_id].Value["Demerits"] < 200)
			{
				MessageClient(%obj.client,'',"\c6You have commited a crime. [\c3Breaking and Entering\c6]");
				CRPGData.data[%obj.client.bl_id].Value["Demerits"] += $CRPG::Pref::Demerits::BaE;
				commandtoclient(%obj.client,'setCRPGwanted', CRPGData.data[%obj.client.bl_id].Value["Demerits"]);
			}
		}
	}
	if(%class $= "Player")
	{
		if(CRPGData.Data[%col.client.bl_id].Value["Demerits"] < 200 && CRPGData.Data[%player.client.bl_id].Value["Demerits"] < 200)
			return;
		if(CRPGData.Data[%col.client.bl_id].Value["Demerits"] >= 200 && CRPGData.Data[%player.client.bl_id].Value["Demerits"] < 200)
		{
			if(%col.getDatablock().maxDamage - (%col.getDamageLevel() + %this.DirectDamage) < %this.DirectDamage)
			{
				%col.setDamageLevel(%this.DirectDamage + 1);
				%col.client.arrest(%obj.client);
			}
		}
	}
	parent::onCollision(%this, %obj, %col, %fade, %pos, %normal);
}