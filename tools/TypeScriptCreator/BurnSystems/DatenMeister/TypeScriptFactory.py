import clr
import subprocess
clr.AddReferenceToFile("DatenMeister")
from DatenMeister.Logic.SourceFactory import *

def createFiles(file, types):
    dotNetTypes = [clr.GetClrType(x) for x in types]
    provider = DotNetTypeProvider(dotNetTypes)
    sourceFactory = TypeScriptSourceFactory(provider)
    sourceFactory.CreateFile(file)


def compile(file):
    subprocess.call(["tsc", file, "--module", "amd", "--target", "es5", "--sourcemap"])
    pass
