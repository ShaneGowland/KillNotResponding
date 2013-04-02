using System;
using System.IO;

namespace KillNotResponding
{
    class Program
    {
        static void Main(string[] args)
        {

            //Variables for settings
            bool write_log = false, show_ui = false;

            //Get the commandline arguments
            foreach (string arg in args)
            {

                //Trim the arguments of trailing slashes
                string trimmed_arg = arg.Replace("/", "");

                //Assign the correct settings as determined by the arguments
                if (trimmed_arg == "UI")
                {
                    show_ui = true;
                }
                else if (trimmed_arg == "LOG")
                {
                    write_log = true;
                }
            }

            //Enumerate the running processes
            System.Diagnostics.Process[] pList = System.Diagnostics.Process.GetProcesses();
            bool program_was_ended = false;

            //Iterate through list of processes
            foreach (System.Diagnostics.Process proc in pList)
            {

                try
                {
                    //Check if process is responding
                    if (proc.Responding == false)
                    {

                        //Kill the selected process
                        proc.Kill();
                        Console.WriteLine("Ended "  + " " + proc.ProcessName + ".exe (" + proc.MainWindowTitle + ") with ID:" + proc.Id + " at " + DateTime.Now.TimeOfDay);
                        program_was_ended = true;


                        //Log the kill if requested
                        if (write_log == true)
                        {
                            StreamWriter SW = new StreamWriter(Environment.CurrentDirectory + "\\kill-log.txt");
                            SW.WriteLine("Ended " + " " + proc.ProcessName + ".exe (" + proc.MainWindowTitle + ") with ID:" + proc.Id + " at " + DateTime.Now.TimeOfDay);
                            SW.Close();
                        }

                    }

                    //Catch any exceptions
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            //Keep the console window active
            if (show_ui == true)
            {
                if (program_was_ended == false)
                {
                    Console.WriteLine("No non-responsive programs were ended.");
                }
                Console.ReadKey();
            }

        }
    }
}
