function serverCmdGetPackages(%client,%vehicle)
{
	%data = CRPGData.data[%client.bl_id];
	%vehicle = %client.player.vehicle;
	if(%vehicle.getDatablock.isTruck = 1)
	{	
		if(%data.value["Money"] > $CRPG::Prefs::Trucking::PackPrice)
		{
			if(%vehicle.Packs < $CRPG::Prefs::Trucking::MaxPacks)
			{
				%data.value["Money"] -= 50;
				%vehicle.Packs++;
				messageClient(%client,'',"You have purchased a package and it has been placed in your truck.");
			}
			else
				messageClient(%client,'',"You have too many packages, sorry.");
		}
		else
			messageClient(%client,'',"You don't have the cash.");
	}
	else
		messageClient(%client,'',"You have purchased a package and it has been placed in your truck.");
}
			