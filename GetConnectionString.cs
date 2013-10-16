/*
Create a class file like this, call it Utilities.cs and add it to your project.

Go to Project, Add reference, Assemblies, and choose System.Configuration.
Go to Project, Properties, Settings and add a ConnectionString, giving a name for it, and the connection string needed to acces your database. 
If you have added a datasource to your project and choose to save Connection string you don´t need to do the previous step, visual studio creates
the connection string var for you.

To acces this in any part of the code use: string aString=Name_of_the_project.Utilities.GetConnectionString();

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DataAppUsingADONet
{
    class Utilities
    {
        //Retrieve the conn string.
        internal static string GetConnString()
        {
            string value = null;

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["DataAppUsingADONet.Properties.Settings.TrainITConnectionString"];
            if (settings != null)                                             //Change DataAppUsingADONet for the name of your project.
                value = settings.ConnectionString;                            //Change TrainITConnectionString for the name given for the connection String
            return value;
        }
    }
}
