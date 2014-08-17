import clr
import subprocess
clr.AddReferenceToFile("DatenMeister")
from DatenMeister.Logic.SourceFactory import *

def createFiles(file, types, nameSpace):
    dotNetTypes = [clr.GetClrType(x) for x in types]
    provider = DotNetTypeProvider(dotNetTypes)
    sourceFactory = CSharpSourceFactory(provider, nameSpace)
    sourceFactory.CreateFile(file)

def createTypeFile(file, types, nameSpace, className, typeExtentUri):
    dotNetTypes = [clr.GetClrType(x) for x in types]
    provider = DotNetTypeProvider(dotNetTypes)
    sourceFactory = CSharpTypeDefinitionFactory(provider, nameSpace, className, typeExtentUri)
    sourceFactory.CreateFile(file)

