//IsoViewer - Allows you to easily view the structure of your application's Isolated Storage
//Created by keyboardP
//Twitter: @keyboardP
//email: phone7@live.co.uk
//website: phone7.wordpress.com

using System;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Collections.Generic;

using Engine;

namespace Engine.Core
{
#if WINDOWS_PHONE
    public static class IsoViewer
    {

        //*******************************************************************************************
        //Enable this if you'd like to be able access the list from outside the class. All list 
        //methods are commented out below and replaced with Debug.WriteLine.
        //Default implementation is to simply output the directories to the console (Debug.WriteLine)
        //so if it looks like nothing's happening, check the Output window :) - keyboardP
        
        // public static List<string> listDir = new List<string>();
       
        //********************************************************************************************

		//Use "*" as the pattern string in order to retrieve all files and directories
        public static void GetIsolatedStorageView(string pattern)
        {
            
            string root = System.IO.Path.GetDirectoryName(pattern);

            if (root != "")
            {
                root += "/";
            }

            string[] directories = FileMananger.Instance.GetFile.GetDirectoryNames(pattern);
			//if the root directory has not FOLDERS, then the GetFiles() method won't be called.
			//the line below calls the GetFiles() method in this event so files are displayed
			//even if there are no folders
            if (directories.Length == 0) GetFiles(root, "*", FileMananger.Instance.GetFile); 


            for (int i = 0; i < directories.Length; i++)
            {
                string dir = directories[i] + "/";

                //Add this directory into the list
               // listDir.Add(root + directories[i]);

                //Write to output window
                Debug.WriteLine(root + directories[i]);


                //Get all the files from this directory
                GetFiles(root + directories[i], pattern, FileMananger.Instance.GetFile);

                //Continue to get the next directory
                GetIsolatedStorageView(root + dir + "*");


            }

            
        }

        private static void GetFiles(string dir, string pattern, IsolatedStorageFile storeFile)
        {

            string fileString = System.IO.Path.GetFileName(pattern);

            string[] files = storeFile.GetFileNames(pattern);



            try
            {
                for (int i = 0; i < storeFile.GetFileNames(dir + "/" + fileString).Length; i++)
                {

                    //Files are prefixed with "--"

                    //Add to the list
                    //listDir.Add("--" + dir + "/" + storeFile.GetFileNames(dir + "/" + fileString)[i]);

                    Debug.WriteLine("--" + dir + "/" + storeFile.GetFileNames(dir + "/" + fileString)[i]);
                }
            }
            catch (IsolatedStorageException ise)
            {
                Debug.WriteLine("An IsolatedStorageException exception has occurred: " + ise.InnerException);
            }
            catch (Exception e)
            {
                Debug.WriteLine("An exception has occurred: " + e.InnerException);
            }
        } 
    }
#endif
}
