using System.Diagnostics;
using System.IO;
using System.Linq;
using Tandem.Elements;
using Tandem.Excel.Utilities;
using Tandem.Elements.Utilities;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using YamlDotNet.RepresentationModel;

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


        public ITdmElement tdmPyScript(string fullPath, object[,] kwargs)
        {
            var tdmKwargs = _elementUtilities.RangeToElement(new object [,] { { "args" } }, kwargs);
            var dictKwargs = tdmKwargs.Group.ToDictionary( x => (object)x.Key, x => x.Value );

            BsonDocument kwargs_json = tdmKwargs.Group[0].ToBsonDocument();

            var configPythonScript = (YamlMappingNode)ConfigFile.Instance.PythonScript;
            string executablePath = _getPythonExecutablePath();

            string ptyhonScript = string.Format( @"""{0}""", fullPath );

            string mykwargs = Regex.Replace(kwargs_json.ToString(), @"(\\*)" + "\"", @"$1\$0");
            mykwargs = Regex.Replace(mykwargs, @"^(.*\s.*?)(\\*)$", "\"$1$2$2\"", RegexOptions.Singleline);

            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(executablePath);
            
            // make sure we can read the output from stdout 
            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardOutput = true;
            myProcessStartInfo.CreateNoWindow = true;
            myProcessStartInfo.WindowStyle = ProcessWindowStyle.Normal;
     
            myProcessStartInfo.Arguments = ptyhonScript + " " + mykwargs;

            Process myProcess = new Process();
            myProcess.StartInfo = myProcessStartInfo;

            myProcess.Start();

            StreamReader myStreamReader = myProcess.StandardOutput;
            string myString = myStreamReader.ReadLine();

            myProcess.WaitForExit();

            ITdmElement element = new TdmElementGroup();

            var out_bson = new BsonDocument();
            out_bson.Add(BsonDocument.Parse(myString));

            element.FromBsonDocument(out_bson);

            return element;
        }

        private string _getPythonExecutablePath()
        {
            var configPythonScript = (YamlMappingNode)ConfigFile.Instance.PythonScript;
            string configPath = configPythonScript.Children[new YamlScalarNode("ExecutionPath")].ToString();
            return string.Format(@"""{0}""", configPath);
        }

    }
}
