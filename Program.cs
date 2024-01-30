using System;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using static ConsoleHelper.Helper;

namespace Crypter
{
    // HAVE CODE ONLY FOR WINDOWS.
    // Check method TraverseTree()
    class Program
    {
        // Encrypt = true, Decrypt = false.
        // There is not so much point of encrypt/decrypt
        // 'cause encrypt also can decrypt and vice versa.
        static bool CoderMode = false;

        static string? CoderPath = string.Empty;
        static string? CoderModeString;

        public static void Main()
        {
            // To cut off the logo
            // remove this line
            Logo();

            WriteLine("\n\nHello World! Lets crypt system34!!!", ConsoleColor.Gray);

            // Get path to files
            WriteLine("Type path to crypt files: ", ConsoleColor.Magenta);
            CoderPath = Console.ReadLine();

            // Check path
            while(!Directory.Exists(CoderPath))
            {
                WriteError("Wrong path. ", "Try again: ");
                CoderPath = Console.ReadLine();
            }
            WriteLine("All files and directories in path will be crypt: " + CoderPath, ConsoleColor.Gray);

            // Get coder mode
            WriteLine("Coder mode? ([c]rypt, [d]ecrypt): ", ConsoleColor.Magenta);
            CoderModeString = Console.ReadLine();

            // Check coder mode
            while(CoderModeString != "c" && CoderModeString != "d")
            {
                WriteError("Wrong mode. ", "Try again: ");
                CoderModeString = Console.ReadLine();
            }

            switch(CoderModeString)
            {
                case "c":
                    CoderMode = true;
                    break;
                
                case "d":
                    CoderMode = false;
                    break;
            }

            WriteLine("Start crypt files...", ConsoleColor.DarkGreen);

            TraverseTree(CoderPath, CoderMode); // false = crypt, true = decrypt
            DelTempFiles(CoderPath);

            WriteLine("Done! Press any key to exit...", ConsoleColor.Green);
            Console.ReadKey();
        }
        public static void Copy(string inputFilePath, string outputFilePath, bool coderMode)
        {
            // Set the buffer size to 64KB
            int bufferSize = 1024 * 64;

            // Open the input file for reading
            using (FileStream inStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
            {
                // Open or create the output file for writing
                using (FileStream fileStream = new FileStream(outputFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    // Initialize variables for decoding
                    int bytesRead = -1;
                    byte[] bytes = new byte[bufferSize];
                    string decodeString = "454567679999999999";

                    // Convert decodeString to double and calculate logarithm
                    double decodeInt = Convert.ToDouble(decodeString);
                    int ln = (int)(Math.Log(Math.Log(decodeInt, 2)));

                    int byteIndex = 0;

                    // Read from input file, decode/encode bytes, and write to output file
                    while ((bytesRead = inStream.Read(bytes, 0, bufferSize)) > 0)
                    {
                        int[] bytesAsInts = bytes.Select(x => (int)x).ToArray();

                        // Decode or encode the bytes based on coderMode
                        for (int i = 0; i < bytesAsInts.Length; i++)
                        {
                            checked
                            {
                                if (coderMode == false)
                                {
                                    // Decode the bytes
                                    bytesAsInts[i] = (bytesAsInts[i] - (ln * 2) - 128 - byteIndex) * -1;
                                }
                                else
                                {
                                    // Encode the bytes
                                    bytesAsInts[i] = (bytesAsInts[i] * -1 + ln * 2 + 128 + byteIndex);
                                }
                            }
                            byteIndex++;
                        }

                        byte[] bytes1 = bytesAsInts.Select(x => (byte)x).ToArray();
                        fileStream.Write(bytes1, 0, bytesRead);
                        fileStream.Flush();
                    }
                }
            }
        }
            
        public static void TraverseTree(string root, bool coderMode)
        {
            try
            {
                string[] subDirectories = Directory.GetDirectories(root);

                // Recursively traverse subdirectories
                foreach (string subDir in subDirectories)
                {
                    TraverseTree(subDir, coderMode);
                }

                string[] files = Directory.GetFiles(root);

                // If no files found in the directory, print a message and return
                if (files.Length == 0)
                {
                    WriteLine($"No files found in directory: {root}", ConsoleColor.Blue);
                    return;
                }

                // Process each file in the directory
                foreach (string file in files)
                {
                    try
                    {
                        FileInfo fileInfo = new FileInfo(file);

                        // Set input and output file paths
                        string inputFile = fileInfo.FullName;
                        string outputFile = Path.Combine(fileInfo.Directory!.FullName, fileInfo.Name + ".sh");

                        // Perform encryption or decryption
                        Copy(inputFile, outputFile, coderMode); // false = crypt, true = decrypt

                        try
                        {
                            // Set full control access for the current user (Windows only)
                            #pragma warning disable CA1416 // Validate platform compatibility
                            FileSecurity inputFS = new FileSecurity(inputFile, AccessControlSections.Access);
                            inputFS.AddAccessRule(new FileSystemAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, FileSystemRights.FullControl, AccessControlType.Allow));
                            fileInfo.SetAccessControl(inputFS);
                            #pragma warning restore CA1416 // Validate platform compatibility
                            File.Replace(outputFile, inputFile, null, true);
                            WriteLine($"{fileInfo.FullName}: {fileInfo.Length}, {fileInfo.CreationTime}");
                        }
                        catch (Exception e)
                        {
                            WriteError("Error while replacing file: ", e.Message);
                        }
                    }
                    catch (Exception e)
                    {
                        WriteError("Error while accessing file: ", e.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError($"Error occurred accessing subdirectories: ", ex.Message);
            }
        }

        public static void DelTempFiles(string path)
        {
            string _del =   @"*.tmp";
            string[] _files = Directory.GetFiles(path, _del);

            foreach(string fl in _files)
            {
                try
                {
                    File.Delete(fl);
                    WriteLine(fl + " deleted.", ConsoleColor.Blue);
                }
                catch(Exception e)
                {
                    WriteError("File " + fl + " not deleted. ", e.Message);
                }
            }
        }
    }
}