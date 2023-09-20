using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileStreamLibrary
{
    public class FileManagerDef
    {
        public MachineDataDef MachineData;
        public RecipeCollection RecipeCollection;
       
        public FileManagerDef(string sSystemDirPath)
        {
            MachineData = new MachineDataDef(sSystemDirPath);
            RecipeCollection = new RecipeCollection(sSystemDirPath);
        }

        public void Dispose()
        {
            RecipeCollection.Dispose();
            RecipeCollection = null;

            MachineData.Dispose();
            MachineData = null;
        }
    }
}
