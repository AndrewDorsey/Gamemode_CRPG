function Build()
{
	if(!isFile("Config/Server/CRPG/Jobs.txt"))
	{
		%file = new fileObject();
		%file.openForWrite("Config/Server/CRPG/Jobs.txt");

		%file.WriteLine("Civilian");

		%file.close();
		%file.delete();

		%file = new fileObject();
		%file.openForWrite("Config/Server/CRPG/Jobs/Civilian.txt");

		%file.WriteLine("color <color:mmmmmm>");
		%file.WriteLine("JobName Civilian");
		%file.WriteLine("Path Civilian");
		%file.WriteLine("Income 25");
		%file.WriteLine("Investment 0");
		%file.WriteLine("CleanRecord 0");
		%file.WriteLine("RequiredEducation 0");
		%file.WriteLine("RequiredExperience 0");
		%file.WriteLine("MaxExperience 10");
		%file.WriteLine("PlaceBounties 0");
		%file.WriteLine("ClaimBounties 0");
		%file.WriteLine("SpawnItems 0");
		%file.WriteLine("ClearRecords 0");
		%file.WriteLine("RequiredJob");
		%file.WriteLine("InfoLine Civilians are the basic option and have no perks.");
		%file.WriteLine("Pardon 0");
		%file.WriteLine("Lots 0");
		%file.WriteLine("PersonalSpawns 0");
		%file.WriteLine("Tools 0");
		%file.WriteLine("Food 0");
		%file.WriteLine("PayDems 0");
		%file.WriteLine("Heal 0");
		%file.WriteLine("PickPocket 0");
		%file.WriteLine("ItemDiscount 1");
		%file.WriteLine("LumberEXP 0");
		%file.WriteLine("OreEXP 0");
		%file.WriteLine("JailEXP 0");
		%file.WriteLine("BountyEXP 0");
		%file.WriteLine("JailMoney 0");
		%file.WriteLine("BuyMats 0");
		%file.WriteLine("MatEXP 0");

		%file.close();
		%file.delete();
	}
	%file = new fileObject();
	%file.openForRead("Config/Server/CRPG/Jobs.txt");
	while(!%file.isEOF())
	{
		%line = %file.readLine();
		loadJobFromFile("Config/Server/CRPG/Jobs/"@%line@".txt");
	}
	%file.close();
	%file.delete();

}
function LoadJobFromFile(%path)
{
	if(isFile(%path))
	{
		%file = new fileObject();
		%file.openForRead(%path);
		while(!%file.isEOF())
		{
			%line = %file.readLine();
			%data[getWord(%line,0)] = RemoveWord(%line,0);
		}
		%file.close();
		%file.delete();
		%job = new ScriptObject()
		{
			class = "Job";
			color = %data[Color];
			JobName = %data[JobName];
			Path = %data[Path];
			Income = %data[Income];
			Investment = %data[Investment];
			CleanRecord = %data[CleanRecord];
			RequiredEducation = %data[RequiredEducation];
			RequiredExperience = %data[RequiredExperience];
			MaxExperience = %data[MaxExperience];
			PlaceBounties = %data[PlaceBounties];
			ClaimBounties = %data[ClaimBounties];
			SpawnItems = %data[SpawnItems];
			ClearRecords = %data[ClearRecords];
			RequiredJob = %data[RequiredJob];
			InfoLine = %data[InfoLine];

			Pardon = %data[Pardon];
			Lots = %data[Lots];
			PersonalSpawns = %data[PersonalSpawns];
			Tools = %data[Tools];
			Food = %data[Food];
			PayDems = %data[PayDems];
			Heal = %data[Heal];
			PickPocket = %data[PickPocket];
			ItemDiscount = %data[ItemDiscount];
		
			LumberEXP = %data[LumberExp];
			OreEXP = %data[OreExp];
			JailEXP = %data[JailExp];
			BountyEXP = %data[BountyExp];
			SalesEXP = %data[SalesExp];
			MatEXP = %data[MatExp];
		
			JailMoney = %data[JailMoney];
			BuyMats = %data[BuyMats];
		};
		echo("Job "@ %job.JobName @" has been added from "@ %path);
	}
}
function Job::Export(%this)
{
	%file = new fileObject();
	%file.openForWrite("Config/Server/CRPG/Jobs/"@ %this.jobName @".txt");

	%file.WriteLine("color "@ %this.Color);
	%file.WriteLine("JobName "@ %this.JobName);
	%file.WriteLine("Path "@ %this.Path);
	%file.WriteLine("Income "@ %this.Income);
	%file.WriteLine("Investment "@ %this.Investment);
	%file.WriteLine("CleanRecord "@ %this.CleanRecord);
	%file.WriteLine("RequiredEducation "@ %this.RequiredEducation);
	%file.WriteLine("RequiredExperience "@ %this.RequiredExperience);
	%file.WriteLine("MaxExperience "@ %this.MaxExperience);
	%file.WriteLine("PlaceBounties "@ %this.PlaceBounties);
	%file.WriteLine("ClaimBounties "@ %this.ClaimBounties);
	%file.WriteLine("SpawnItems "@ %this.SpawnItems);
	%file.WriteLine("ClearRecords "@ %this.ClearRecords);
	%file.WriteLine("RequiredJob "@ %this.RequiredJob);
	%file.WriteLine("InfoLine "@ %this.InfoLine);
	%file.WriteLine("Pardon "@ %this.Pardon);
	%file.WriteLine("Lots "@ %this.Lots);
	%file.WriteLine("PersonalSpawns "@ %this.PersonalSpawns);
	%file.WriteLine("Tools "@ %this.Tools);
	%file.WriteLine("Food "@ %this.Food);
	%file.WriteLine("PayDems "@ %this.PayDems);
	%file.WriteLine("Heal "@ %this.Heal);
	%file.WriteLine("PickPocket "@ %this.PickPocket);
	%file.WriteLine("ItemDiscount "@ %this.ItemDiscount);
	%file.WriteLine("LumberEXP "@ %this.LumberEXP);
	%file.WriteLine("OreEXP "@ %this.OreExp);
	%file.WriteLine("JailEXP "@ %this.JailEXP);
	%file.WriteLine("BountyEXP "@ %this.BountyExp);
	%file.WriteLine("JailMoney "@ %this.JailMoney);
	%file.WriteLine("BuyMats "@ %this.BuyMats);
	%file.WriteLine("MatEXP "@ %this.OreEXP);

	%file.close();
	%file.delete();
}
