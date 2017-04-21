using System;
using System.IO;
using System.Reflection;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace Tandem.Excel
{
    public sealed class ConfigFile
    {
        private YamlMappingNode _configFile;

        private ConfigFile()
        {
            string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string file = path + "\\CONFIG.txt";
            YamlStream stream = _textToYamlStream(file);
            _configFile = (YamlMappingNode)stream.Documents[0].RootNode;
        }

        /// <summary>
        /// Instance of <see cref="ConfigFile"/> class</summary>
        public static ConfigFile Instance
        {
            get { return Nested.instance; }
        }

        private class Nested
        {
            static Nested()
            {
            }
            internal static readonly ConfigFile instance = new ConfigFile();
        }


        /// <summary>
        /// PythonScript property</summary>
        /// <value>
        /// 'PythonScript' configuration of type <see cref="YamlSequenceNode"/> </value>
        public YamlMappingNode PythonScript
        {
            get { return (YamlMappingNode)_configFile.Children[new YamlScalarNode("PythonScript")]; }
        }

        /// <summary>
        /// SpaceConfigs property</summary>
        /// <value>
        /// 'Space' configuration of type <see cref="YamlSequenceNode"/> </value>
        public YamlMappingNode SpaceConfigs
        {
            get { return (YamlMappingNode)_configFile.Children[new YamlScalarNode("Space")]; }
        }

        public YamlSequenceNode SpaceConnections
        {
            get { return (YamlSequenceNode)SpaceConfigs.Children[new YamlScalarNode("Connections")]; }
        }

        public YamlMappingNode SpaceDefaultConnection
        {
            get { return (YamlMappingNode)SpaceConfigs.Children[new YamlScalarNode("DefaultConnection")]; }
        }


        /// <summary>
        /// Convert text file to YamlStream
        /// </summary>
        /// <param name="filePath">Full path of the .txt file</param>
        /// <returns>YamlStream</returns>
        private YamlStream _textToYamlStream(string filePath)
        {
            YamlStream yamlStream = new YamlStream();
            var sb = new StringBuilder();
            using (var sr = new StreamReader(filePath))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }
            yamlStream.Load(new StringReader(sb.ToString()));
            return yamlStream;
        }
    }
}
