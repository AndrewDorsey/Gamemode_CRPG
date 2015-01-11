//+---------------------------------------------+
//| Title:	Axe			|
//| Author:	Jasa				|
//+---------------------------------------------+
//| Used to chop trees.				|
//+---------------------------------------------+
if(!isObject(AxeItem))
{
	AddDamageType("Axe",   '<bitmap:Add-Ons/GameMode_CRPG/Shapes/CI/CI_Axe> %1',    '%2 <bitmap:Add-Ons/GameMode_CRPG/Shapes/CI/CI_Axe> %1', 0.5, 1);

	datablock ProjectileData(AxeProjectile)
	{	
		directDamage		= 10;
		directDamageType	= $DamageType::Axe;
		radiusDamageType	= $DamageType::Axe;
		explosion		= SwordExplosion;
		
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
	datablock ItemData(AxeItem)
	{
		category		= "Weapon";
		className		= "Weapon";
		
		shapeFile		= "Add-Ons/GameMode_CRPG/Shapes/Axe.dts";
		mass			= 1;
		density 		= 0.2;
		elasticity		= 0.2;
		friction		= 0.6;
		emap			= true;
	
		uiName			= "Lumberjack Axe";
		iconName		= "Add-Ons/GameMode_CRPG/Shapes/ItemIcons/Icon_Axe";
		doColorShift		= false;
	
		image			= AxeImage;
		canDrop			= true;
	
	};
	datablock ShapeBaseImageData(AxeImage)
	{
		
		shapeFile		= "Add-Ons/GameMode_CRPG/Shapes/Axe.dts";
		emap			= true;
		mountPoint		= 0;
		eyeOffset		= "0 0 0";
		offset			= "0 0 0";
		correctMuzzleVector	= false;
		className		= "WeaponImage";
		
		item			= AxeItem;
		ammo			= " ";
		projectile		= AxeProjectile;
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
function AxeImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armAttack);
}
function AxeImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}
//function AxeProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
//{
//	if(%col.getClassName() $= "fxDTSBrick")
//		%col.OnChop(%obj.client);
//	parent::onCollision(%this, %obj, %col, %fade, %pos, %normal);
//}