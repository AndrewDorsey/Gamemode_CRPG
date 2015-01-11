//+---------------------------------------------+
//| Title:	Pickaxe				|
//| Author:	Jasa				|
//+---------------------------------------------+
//| Used to mine ore.			 	|
//+---------------------------------------------+
if(!isObject(PickaxeItem))
{
	AddDamageType("Pickaxe",   '<bitmap:Add-Ons/GameMode_CRPG/Shapes/CI/CI_Pickaxe> %1',    '%2 <bitmap:Add-Ons/GameMode_CRPG/Shapes/CI/CI_Pickaxe> %1', 0.5, 1);

	datablock ExplosionData(pickaxeExplosion : hammerExplosion)
	{
		soundProfile = hammerHitSound;
	};
	datablock ProjectileData(PickaxeProjectile)
	{	
		directDamage		= 10;
		directDamageType	= $DamageType::Pickaxe;
		radiusDamageType	= $DamageType::Pickaxe;
		explosion		= pickaxeExplosion;
		
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
	datablock ItemData(PickaxeItem)
	{
		category		= "Weapon";
		className		= "Weapon";
		
		shapeFile		= "Add-Ons/GameMode_CRPG/Shapes/Pickaxe.dts";
		mass			= 1;
		density 		= 0.2;
		elasticity		= 0.2;
		friction		= 0.6;
		emap			= true;
	
		uiName			= "Pickaxe";
		iconName		= "Add-Ons/GameMode_CRPG/Shapes/ItemIcons/Icon_Pickaxe";
		doColorShift		= false;
	
		image			= PickaxeImage;
		canDrop			= true;
	
	};
	datablock ShapeBaseImageData(PickaxeImage)
	{
		
		shapeFile		= "Add-Ons/GameMode_CRPG/Shapes/Pickaxe.dts";
		emap			= true;
		mountPoint		= 0;
		eyeOffset		= "0 0 0";
		offset			= "0 0 0";
		correctMuzzleVector	= false;
		className		= "WeaponImage";
		
		item			= PickaxeItem;
		ammo			= " ";
		projectile		= PickaxeProjectile;
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
function PickaxeImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armAttack);
}
function PickaxeImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}
//function PickAxeProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
//{
//	if(%col.getClassName() $= "fxDTSBrick")
//	{
//		if(%col.Ore)
//			%col.OnMine(%obj.client);
//		else if(!%col.Ore && %col.getDatablock().Ore)
//			centerprint(%obj.client,"\c6This rock contains no more ore.",4);
//	}
//	if(CRPGData.data[%col.client.bl_id].Value["JailData"])
//		return;
//	parent::onCollision(%this, %obj, %col, %fade, %pos, %normal);
//}