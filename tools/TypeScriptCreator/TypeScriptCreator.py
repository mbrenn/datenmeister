# Loading DatenMeister
import clr
clr.AddReferenceToFile("DatenMeister")
import DatenMeister
from BurnSystems.DatenMeister import TypeScriptFactory
from BurnSystems.DatenMeister import CSharpFactory

#
# Creates the information for the DatenMeister
print('Creating the DatenMeister')
tsUMLFilename = "..\\..\\src\\DatenMeisterWeb\\js\\datenmeister\\datenmeister.dm.objects.ts"
umlTypes = [
    DatenMeister.Entities.DM.ExtentInfo,     
    DatenMeister.Entities.DM.RecentProject,         
    DatenMeister.Entities.DM.Workbench]
TypeScriptFactory.createFiles(tsUMLFilename, umlTypes)

csFilename = "..\\..\\src\\DatenMeister\\Entities\\AsObject\\DM.Objects.cs"
CSharpFactory.createFiles(csFilename, umlTypes, "DatenMeister.Entities.AsObject.DM");
csFilename = "..\\..\\src\\DatenMeister\\Entities\\AsObject\\DM.Types.cs"
CSharpFactory.createTypeFile(csFilename, umlTypes, "DatenMeister.Entities.AsObject.DM", "Types", "datenmeister:///types/datenmeister");

#
# Creates the information for the fieldinfos, which is used by Datenmeister
print('Creating the DatenMeister Fieldinfo-File')
tsFilename = "..\\..\\src\\DatenMeisterWeb\\js\\datenmeister\\datenmeister.fieldinfo.objects.ts"
types = [
         DatenMeister.Entities.FieldInfos.Comment, 
         DatenMeister.Entities.FieldInfos.General, 
         DatenMeister.Entities.FieldInfos.Checkbox, 
         DatenMeister.Entities.FieldInfos.TextField,
         DatenMeister.Entities.FieldInfos.HyperLinkColumn,
         DatenMeister.Entities.FieldInfos.DatePicker,
         DatenMeister.Entities.FieldInfos.ActionButton,
         DatenMeister.Entities.FieldInfos.ReferenceBase,
         DatenMeister.Entities.FieldInfos.ReferenceByConstant,
         DatenMeister.Entities.FieldInfos.ReferenceByRef,
         DatenMeister.Entities.FieldInfos.ReferenceByValue,
         DatenMeister.Entities.FieldInfos.MultiReferenceField,
         DatenMeister.Entities.FieldInfos.SubElementList,
         DatenMeister.Entities.FieldInfos.View,
         DatenMeister.Entities.FieldInfos.FormView,
         DatenMeister.Entities.FieldInfos.TableView,
         DatenMeister.Entities.FieldInfos.TreeView]
TypeScriptFactory.createFiles(tsFilename, types)

csFilename = "..\\..\\src\\DatenMeister\\Entities\\AsObject\\FieldInfo.Objects.cs"
CSharpFactory.createFiles(csFilename, types, "DatenMeister.Entities.AsObject.FieldInfo");
csFilename = "..\\..\\src\\DatenMeister\\Entities\\AsObject\\FieldInfo.Types.cs"
CSharpFactory.createTypeFile(csFilename, types, "DatenMeister.Entities.AsObject.FieldInfo", "Types", "datenmeister:///types/fieldinfo");

print('Creating the UML-File')

#
# Creates the information for the UML Objects
tsUMLFilename = "..\\..\\src\\DatenMeisterWeb\\js\\datenmeister\\datenmeister.uml.objects.ts"
umlTypes = [
         DatenMeister.Entities.UML.NamedElement,       
         DatenMeister.Entities.UML.Type,       
         DatenMeister.Entities.UML.Property,    
         DatenMeister.Entities.UML.Class ]
TypeScriptFactory.createFiles(tsUMLFilename, umlTypes)

csFilename = "..\\..\\src\\DatenMeister\\Entities\\AsObject\\UML.Objects.cs"
CSharpFactory.createFiles(csFilename, umlTypes, "DatenMeister.Entities.AsObject.Uml");
csFilename = "..\\..\\src\\DatenMeister\\Entities\\AsObject\\UML.Types.cs"
CSharpFactory.createTypeFile(csFilename, umlTypes, "DatenMeister.Entities.AsObject.Uml", "Types", "datenmeister:///types/uml");

#
# Creates the entities for the ProjektMeister
print('Creating the ProjektMeister')
clr.AddReferenceToFile("ProjektMeister")
import ProjektMeister
projektMeisterTypes = [
    ProjektMeister.Data.Entities.Comment,     
    ProjektMeister.Data.Entities.Person,         
    ProjektMeister.Data.Entities.Task ]

csFilename = "..\\..\\..\\projektmeister\\src\\ProjektMeister\\Data\\Entities\\AsObject\\PM.Objects.cs"
CSharpFactory.createFiles(csFilename, projektMeisterTypes, "ProjektMeister.Data.Entities.AsObject");
csFilename = "..\\..\\..\\projektmeister\\src\\ProjektMeister\\Data\\Entities\\AsObject\\PM.Types.cs"
CSharpFactory.createTypeFile(csFilename, projektMeisterTypes, "ProjektMeister.Data.Entities.AsObject", "Types", "datenmeister:///projektmeister/types");

#print('Compiling the TypeScript-File')
#TypeScriptFactory.compile(tsFilename)
#TypeScriptFactory.compile(tsUMLFilename)
 
print('Files have been created and compiled')

#
# Creates the entities for the DatenMeister.AddOns
print('Creating for the DatenMeister.AddOns')
clr.AddReferenceToFile("DatenMeister.AddOns");
import DatenMeister.AddOns
datenMeisterAddonsTypes = [
        DatenMeister.AddOns.Data.FileSystem.File,
        DatenMeister.AddOns.Data.FileSystem.Directory
        ]

csFilename = "..\\..\\src\\DatenMeister.AddOns\\Data\\FileSystem\\AsObjects.cs"
CSharpFactory.createFiles(csFilename, datenMeisterAddonsTypes, "DatenMeister.AddOns.Data.FileSystem.AsObject");
csFilename = "..\\..\\src\\DatenMeister.AddOns\\Data\\FileSystem\\FileSystemTypes.cs"
CSharpFactory.createTypeFile(csFilename, datenMeisterAddonsTypes, "DatenMeister.AddOns.Data.FileSystem.AsObject", "Types", "datenmeister:///types/dmaddons/filesystem");
