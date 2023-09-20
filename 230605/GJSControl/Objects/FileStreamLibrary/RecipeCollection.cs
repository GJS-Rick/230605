using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FileStreamLibrary
{
    public class RecipeCollection : IDisposable
    {
        private RecipeDef _recipe;
        private String _systemDirPath;

        public RecipeCollection(String sSystemDirPath)
        {
            _systemDirPath = sSystemDirPath + "\\Recipe";

            if (!Directory.Exists(_systemDirPath))
                Directory.CreateDirectory(_systemDirPath);

            _recipe = new RecipeDef(_systemDirPath, GetDefaultRecipeName());
        }

        public String GetDefaultRecipeName()
        {
            if (!Directory.Exists(_systemDirPath))
                return "";

            DirectoryInfo cDirInfo = new DirectoryInfo(_systemDirPath);
            FileInfo[] cFileInfos = cDirInfo.GetFiles();

            for (int i = 0; i < cFileInfos.Count(); i++)
            {
                if (cFileInfos[i].Name.IndexOf(".rcp") > 0)
                {
                    return cFileInfos[i].Name.Substring(0, cFileInfos[i].Name.Length - 4);
                }
            }

            return "Default";
        }

        public String GetCurrentRecipeName()
        {
            return _recipe.GetName();
        }

        public String GetCurrentRecipePath()
        {
            return _systemDirPath + "\\" + _recipe.GetName();
        }

        public void GetRecipeContent(String Name, ERecipeDouble eIndex, ref String Caption, ref double Value)
        {
            RecipeDef Recipe;
            Recipe = new RecipeDef(_systemDirPath, Name);
            Recipe.GetFactor(eIndex, ref Caption, ref Value);
            Recipe.Dispose();
        }

        public void GetRecipeContent(string Name, ERecipeInt eIndex, ref String Caption, ref int Value)
        {

            RecipeDef Recipe;
            Recipe = new RecipeDef(_systemDirPath, Name);
            Recipe.GetFactor(eIndex, ref Caption, ref Value);
            Recipe.Dispose();

        }

        public double GetRecipeValue(string Name, ERecipeDouble eIndex)
        {
            RecipeDef Recipe;
            Recipe = new RecipeDef(_systemDirPath, Name);
            return Recipe.GetValue(eIndex);
        }

        public int GetRecipeValue(string Name, ERecipeInt eIndex)
        {
            RecipeDef Recipe;
            Recipe = new RecipeDef(_systemDirPath, Name);
            return Recipe.GetValue(eIndex);
        }

        public void GetRecipeInfo(string Name, ref string Data, ref string User)
        {
            RecipeDef Recipe;
            Recipe = new RecipeDef(_systemDirPath, Name);
            Recipe.GetInfo(ref Data, ref User);
        }

        public void GetRecipeContent(ERecipeDouble eIndex, ref String Caption, ref double Value)
        {
            _recipe.GetFactor(eIndex, ref Caption, ref Value);
        }

        public void GetRecipeContent(ERecipeInt eIndex, ref String Caption, ref int Value)
        {
            _recipe.GetFactor(eIndex, ref Caption, ref Value);
        }

        public double GetRecipeValue(ERecipeDouble eIndex)
        {
            return _recipe.GetValue(eIndex);
        }

        public int GetRecipeValue(ERecipeInt eIndex)
        {
            return _recipe.GetValue(eIndex);
        }

        public int GetRecipeContentNum()
        {
            return _recipe.GetFactorNum();
        }

        public void SetRecipeContent(ERecipeDouble eIndex, double Value)
        {
            _recipe.SetValue(eIndex, Value);
        }

        public void SetRecipeContent(ERecipeInt eIndex, int Value)
        {
            _recipe.SetValue(eIndex, Value);
        }

        public void SetRecipeContent(string Name, ERecipeDouble eIndex, double Value, string User)
        {
            RecipeDef Recipe;
            Recipe = new RecipeDef(_systemDirPath, Name);
            Recipe.SetValue(eIndex, Value);
            Recipe.Save(Name, User);
            Recipe.Dispose();
        }


        public void SetRecipeContent(string Name, ERecipeInt eIndex, int Value, string User)
        {
            RecipeDef Recipe;
            Recipe = new RecipeDef(_systemDirPath, Name);
            Recipe.SetValue(eIndex, Value);
            Recipe.Save(Name, User);
            Recipe.Dispose();
        }

        public void GetRecipeInfo(ref String sName, ref String sNotice)
        {
            sName = _recipe.GetName();
            sNotice = _recipe.GetNotice();
        }

        public void SetRecipeInfo(String sName, String sNotice)
        {
            _recipe.SetName(sName);
            _recipe.SetNotice(sNotice);

            try
            {
                DirectoryInfo cDirInfo = new DirectoryInfo(_systemDirPath);
                FileInfo[] cFileInfos = cDirInfo.GetFiles();

                for (int i = 0; i < cFileInfos.Count(); i++)
                {
                    if (cFileInfos[i].Name.IndexOf(".rcp") > 0)
                    {
                        File.Delete(cFileInfos[i].FullName);
                    }
                }

                FileStream cFileStream = File.Create(_systemDirPath + "\\" + sName + ".rcp");
                cFileStream.Close();

            }
            catch
            {

            }
        }

        public int GetRecipeNum()
        {
            if (!Directory.Exists(_systemDirPath))
                return 0;

            DirectoryInfo cDirInfo = new DirectoryInfo(_systemDirPath);
            return cDirInfo.GetDirectories().Count();
        }

        public String[] GetRecipeNames()
        {
            if (!Directory.Exists(_systemDirPath))
                return null;

            DirectoryInfo cDirInfo = new DirectoryInfo(_systemDirPath);
            DirectoryInfo[] cDirInfoArray = cDirInfo.GetDirectories();
            if (cDirInfoArray.Count() < 1)
                return null;

            String[] strArray = new string[cDirInfoArray.Count()];
            for (int i = 0; i < strArray.Count(); i++)
            {
                strArray[i] = cDirInfoArray[i].Name;
            }

            return strArray;
        }


        public bool Load(String sName, ref String sErrorCode)
        {
            if (!Directory.Exists(_systemDirPath + "\\" + sName))
                return false;

            if (!_recipe.Load(sName))
            {
                sErrorCode = sName + " Recipe Load Fail";
                return false;
            }

            try
            {
                DirectoryInfo cDirInfo = new DirectoryInfo(_systemDirPath);
                FileInfo[] cFileInfos = cDirInfo.GetFiles();

                for (int i = 0; i < cFileInfos.Count(); i++)
                {
                    if (cFileInfos[i].Name.IndexOf(".rcp") > 0)
                    {
                        File.Delete(cFileInfos[i].FullName);
                    }
                }

                FileStream cFileStream = File.Create(_systemDirPath + "\\" + sName + ".rcp");
                cFileStream.Close();

            }
            catch (Exception e)
            {
                sErrorCode = sName + " Recipe Info Create Fail," + e.Message;
                return false;
            }


            return true;
        }

        public bool Save(String sName, string User)
        {
            _recipe.Save(sName, User);
            return true;
        }

        public bool SaveCurrentRecipe(string User)
        {
            _recipe.Save(_recipe.GetName(), User);
            return true;
        }


        public bool Add(string sName, string User)
        {
            Directory.CreateDirectory(_systemDirPath + "\\" + sName);
            if (!Save(sName, User))
                return false;

            return true;
        }


        public bool Delete(string sName, ref String sErrorCode)
        {
            if (sName.Trim() == GetCurrentRecipeName().Trim())
            {
                sErrorCode = sName + " 料號使用中";
                return false;
            }

            try
            {
                DirectoryInfo cDirInfo = new DirectoryInfo(_systemDirPath + "\\" + sName);
                if (cDirInfo != null)
                    cDirInfo.Delete(true);
            }
            catch (Exception e)
            {
                sErrorCode = sName + " 料號刪除失敗," + e.Message;
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            _recipe.Dispose();
        }
    }
}