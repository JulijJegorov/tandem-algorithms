using System.Diagnostics;
using System.IO;
using Tandem.Elements;
using Tandem.Excel.Utilities;
using MongoDB.Bson;
using YamlDotNet.RepresentationModel;
using System;

namespace Tandem.Excel.Functions
{
    public class PythonFunctions
    {
        private readonly ElementUtilities _elementUtilities;
        private readonly string _projectPath;
        private readonly JsonFunctions _jsonFunctions;

        public PythonFunctions(
                                ElementUtilities elementUtilities,
                                JsonFunctions jsonFunctions,
                                string projectPath
                                )
        {
            _elementUtilities = elementUtilities;
            _jsonFunctions = jsonFunctions;
            _projectPath = projectPath;
        }


        public PythonFunctions()
            : this(
                    new ElementUtilities(),
                    new JsonFunctions(),
                    Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
                    ) { }



        /// <summary>
        /// Executes Python script via Windows inter-process communication.</summary>
        /// <param name="fullPath">Full path of the .py file.></param>
        /// <param name="args">Input arguments to be saved in a text file and passed to the Python script.</param> 
        /// <param name="saveInputFile">Optional bollean to save input arguments file (can be used to debug your Python ccode).false if omitted.)</param> 
        /// <returns><see cref="ITdmElement"/></returns>
        public ITdmElement tdmPyScript(string fullPath, object[,] args, bool saveInputFile = false)
        {
            BsonDocument argsBson = _elementUtilities.RangeToElement(new object[,] { { "args" } }, args).Group[0].ToBsonDocument();
            string argsFile = string.Format("{0}\\{1}.txt", Path.GetDirectoryName(fullPath), Guid.NewGuid());

            string outputString = null;

            ProcessStartInfo prcStartInfo = new ProcessStartInfo
            {
                FileName = _getPythonExecutablePath(),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false
            };
       
            try
            {
                using (StreamWriter sw = new StreamWriter(argsFile))
                {
                    sw.WriteLine(argsBson);
                    prcStartInfo.Arguments = string.Format("{0} {1}", string.Format(@"""{0}""", fullPath), string.Format(@"""{0}""", argsFile));
                }

                using (Process process = Process.Start(prcStartInfo))
                {
                    using (StreamReader myStreamReader = process.StandardOutput)
                    {
                        outputString = myStreamReader.ReadLine();
                        process.WaitForExit();
                    }
                }
            }
            finally
            {
                if (!saveInputFile)
                {
                    File.Delete(argsFile);
                }
            }

            ITdmElement outputElement = new TdmElementGroup();
            outputElement.FromBsonDocument(new BsonDocument(BsonDocument.Parse(outputString)));

            return outputElement;
        }


        private string _getPythonExecutablePath()
        {
            var configPythonScript = (YamlMappingNode)ConfigFile.Instance.PythonScript;
            string configPath = configPythonScript.Children[new YamlScalarNode("ExecutionPath")].ToString();
            return string.Format(@"""{0}""", configPath);
        }

    }
}