//+---------------------------------------------+
//| Title:	Jassy								|
//| Author:	Jasa								|
//+---------------------------------------------+
//| Saves and loads data.		 				|
//+---------------------------------------------+
function Jassy::onAdd(%this)
{
	%this.Name = %this.getName();
	if(%this.filePath $= "")
	{
		%this.schedule(0, "delete");
		echo(%this.name @" needs a filepath.");
		return 0;
	}
	%this.fieldCount = 0;
	%this.AutoLoadCount = 0;
	if(%this.AutomaticLoading && isfile(%this.filepath @"AutoLoad.txt"))
	{
		%file = new fileObject();
		%file.openForRead(%this.filepath @"AutoLoad.txt");
		while(!%file.isEOF())
		{
			%loadcount++;
			%line = %file.readLine();
			%this.LoadData(%line);
			%this.AutoLoad[%this.AutoLoadCount] = %line;
			%this.AutoLoadCount++;
		}
		%file.close();
		%file.delete();
		echo(%this.name @" has loaded "@ %loadcount @" keys.");
	}
		
}
function Jassy::addField(%this, %name, %default)
{
	if(%name $= "" || %this.FieldExist[%name])
		echo("Field "@ %name @" already exists.");
	%this.FieldName[%this.FieldCount] = %name;
	%this.FieldValue[%this.FieldCount] = %default;
	%this.FieldExist[%name] = 1;
	%this.FieldCount++;
}
function Jassy::loadData(%this,%key)
{
	if(isObject(%this.Data[%key]))
	{
		echo("Key "@ %key @" has already been loaded.");
		return 0;
	}
	if(!isFile(%this.filepath @ %key @".dat"))
		return %this.addData(%key);
	%this.Data[%key] = new ScriptObject(){class = "JassyData";};
	%file = new fileObject();
	%file.openForRead(%this.filepath @ %key @".dat");
	while(!%file.isEOF())
	{
		%line = %file.readLine();
		%equalpos = striPos(%line,"=");
		%this.Data[%key].Value[getSubStr(%line,0,%equalpos)] = getSubStr(%line,%equalpos+1,500);
	}
	%file.close();
	%file.delete();
}
function Jassy::AddData(%this,%key)
{
	if(isObject(%this.Data[%key]) || isFile(%this.filepath @ %key @".dat"))
	{
		echo("Key "@ %key @" already exists.");
		return 0;
	}
	%this.Data[%key] = new ScriptObject(){class = "JassyData";};
	for(%a = 0; %a < %this.FieldCount;%a++)
		%this.Data[%key].Value[%this.fieldname[%a]] = %this.fieldValue[%a];
	if(%this.AutomaticLoading)
	{
		%this.AutoLoad[%this.AutoLoadCount] = %key;
		%this.AutoLoadCount++;
		%file = new fileObject();
		%file.openForWrite(%this.FilePath @"AutoLoad.txt");
		for(%a = 0;%a<%this.AutoLoadCount;%a++)
			%file.writeline(%this.AutoLoad[%a]);
		%file.close();
		%file.delete();
	}
	echo("Key "@ %key @" has been added to "@ %this.name @".");
}
function Jassy::ResetData(%this,%key)
{
	if(!isObject(%this.Data[%key]))
	{
		echo("Key "@ %key @" does not exist.");
		return 0;
	}
	for(%a = 0; %a < %this.FieldCount; %a++)
		%this.Data[%key].Value[%this.fieldname[%a]] = %this.fieldValue[%a];
	echo("Key "@ %key @" has been reset.");
}
function Jassy::SaveData(%this,%key)
{
	if(!isobject(%this.Data[%key]))
	{
		echo("Key "@ %key @" does not exist.");
		return 0;
	}
	%file = new fileObject();
	%file.openForWrite(%this.FilePath @ %key @".dat");
	for(%a = 0; %a < %this.FieldCount; %a++)
		%file.writeLine(%this.FieldName[%a] @"="@ %this.Data[%key].Value[%this.FieldName[%a]]);
	%file.close();
	%file.delete();
}