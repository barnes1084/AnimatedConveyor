using System;
using System.IO;

namespace CommonTools
{
    public static class Drive
    {


        public static bool isDriveSpaceAvaialable(string driveName = @"C:\")
        {
            try
            {
                Log.ToFile("Checking drive space...");
                DriveInfo drive = new DriveInfo(driveName);
                if (drive.IsReady)
                {
                    int driveSpace = (int)(drive.TotalFreeSpace / 1000000000);
                    if (driveSpace < 10)
                    {
                        Log.ToFile("Drive space low, data being replaced, not appended.");
                        return false;
                    }
                    else { Log.ToFile($"Drive space ok. {driveSpace}Gb remaining."); }
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.ToFile(e.Message);
                return true;
            }
        }



    }
}
