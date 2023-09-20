using Emgu.CV;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VisionLibrary
{
    public class SaveImageDef
    {
        private bool Save = false;
        public void SetSave(bool ESave)
        {
            Save = ESave;
        }

        #region SaveImage
        private string GetSaveImgDir(EImgDirPath eFileDir, string eNotes = null)
        {
            if (eNotes == null)
                return "C:\\Image\\" + eFileDir.ToString();
            else
                return "C:\\Image\\" + eFileDir.ToString() + "_" + eNotes;
        }
        private string GetSaveImgName(string eNotes = null)
        {
            if (eNotes == null)
                return DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".bmp";
            else
                return DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + eNotes + ".bmp";
        }
        public void SaveImg(EImgDirPath eFileDir, IInputOutputArray Img, int ImgMaxNum = 200, string eImgNameNotes = null, string eImgDirNotes = null)
        {
            if (!Save)
                return;

            string _imgDir = GetSaveImgDir(eFileDir, eImgDirNotes);
            string _imgFileName = GetSaveImgName(eImgNameNotes);

            if (!Directory.Exists(_imgDir))
                Directory.CreateDirectory(_imgDir);

            if (GetHardDiskFreeSpace("C") < 1)
                DeleteImg(_imgDir);

            try
            {
                CvInvoke.Imwrite(_imgDir + "\\" + _imgFileName, Img);
            }
            catch (Exception) { }

            DeleteImg(_imgDir, ImgMaxNum);
        }
        public void SaveImg(EImgDirPath eFileDir, Mat Img, int ImgMaxNum = 200, string eImgNameNotes = null, string eImgDirNotes = null)
        {
            if (!Save)
                return;

            string _imgDir = GetSaveImgDir(eFileDir, eImgDirNotes);
            string _imgFileName = GetSaveImgName(eImgNameNotes);

            if (!Directory.Exists(_imgDir))
                Directory.CreateDirectory(_imgDir);

            if (GetHardDiskFreeSpace("C") < 1)
                DeleteImg(_imgDir);

            try
            {
                CvInvoke.Imwrite(_imgDir + "\\" + _imgFileName, Img);
            }
            catch (Exception) { }

            DeleteImg(_imgDir, ImgMaxNum);
        }
        private void DeleteImg(string ImgDir, int ImgMaxNum)
        {
            try
            {
                string[] BMPFiles = Directory.GetFiles(ImgDir, "*.bmp");

                if (BMPFiles.Length - ImgMaxNum > 0)
                {
                    Array.Sort(BMPFiles);

                    for (int i = 0; i < BMPFiles.Length - ImgMaxNum; i++)
                        File.Delete(BMPFiles[i]);
                }
            }
            catch (Exception) { }
        }
        private void DeleteImg(string ImgDir)
        {
            try
            {
                string[] BMPFiles = Directory.GetFiles(ImgDir, "*.bmp");

                Array.Sort(BMPFiles);
                File.Delete(BMPFiles[0]);
            }
            catch (Exception) { }
        }
        #endregion

        #region 獲取指定驅動器的剩餘空間總大小(單位為G)
        /// <summary>獲取指定驅動器的剩餘空間總大小(單位為G), 只需輸入代表驅動器的字母即可</summary>
        /// <param name="DiskName"></param>
        /// <returns></returns>
        private static double GetHardDiskFreeSpace(string DiskName)
        {
            double freeSpace = new double();
            DiskName = DiskName.ToUpper() + ":\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == DiskName)
                {
                    freeSpace = drive.TotalFreeSpace / (double)(1024 * 1024 * 1024);
                }
            }
            return freeSpace;
        }
        #endregion
    }
}