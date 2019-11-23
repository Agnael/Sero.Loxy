using Sero.Loxy.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Loxy.EfCore
{
    public class CleanStateFormatter : IStateFormatter
    {
        public IEnumerable<string> Format<TState>(TState state)
        {
            IEnumerable<KeyValuePair<String, Object>> stateList = state as IEnumerable<KeyValuePair<String, Object>>;
            KeyValuePair<String, Object> nullKvp = default(KeyValuePair<String, Object>);

            string key_MessageTemplate = "{OriginalFormat}";
            string key_NewLine = "newLine";
            string key_CommandText = "commandText";

            if (!stateList.Any(x => x.Key.Equals(key_MessageTemplate)))
                throw new Exception();

            var logTemplateKvp = stateList.FirstOrDefault(x => x.Key == key_MessageTemplate);
            string logTemplate = null;

            if (!logTemplateKvp.Equals(nullKvp))
            {
                logTemplate = logTemplateKvp.Value.ToString();
                stateList = stateList.Where(x => x.Key != key_MessageTemplate);

                foreach (KeyValuePair<String, Object> kvp in stateList)
                {
                    // If commandText key exists, it duplicates it with an unescaped value without newLine symbols
                    if (kvp.Key == key_CommandText)
                    {
                        // Replaces escaped quotes with unescaped ones
                        string unescapedCommand = kvp.Value.ToString().Replace("\"", "'");
                        //string unescapedCommand = kvp.Value.ToString().Replace("\\", "");

                        var newLineKvp = stateList.FirstOrDefault(x => x.Key == key_NewLine);
                        if (!newLineKvp.Equals(nullKvp))
                        {
                            unescapedCommand = unescapedCommand.Replace(newLineKvp.Value.ToString(), " ");
                        }

                        // Appends the clean commandText to the previous information
                        //logTemplate = string.Format("{0} --- UNESCAPED COMMAND: < {1} >", logTemplate, unescapedCommand);
                        logTemplate = logTemplate.Replace("{" + kvp.Key + "}", unescapedCommand);
                    }
                    else
                    {
                        logTemplate = logTemplate.Replace("{" + kvp.Key + "}", kvp.Value.ToString());
                    }
                }
            }

            foreach (KeyValuePair<System.String, System.Object> kvp in (state as IEnumerable))
            {

            }

            string[] objToLog = new string[] { logTemplate };

            if (!string.IsNullOrEmpty(logTemplate))
            {
                logTemplate = ClearExtraWhiteSpaces(logTemplate);

                string[] logArray;

                var newLineKvp = stateList.FirstOrDefault(x => x.Key == key_NewLine);
                if (newLineKvp.Equals(nullKvp))
                {
                    objToLog = new string[] { logTemplate };
                }
                else
                {
                    logArray = logTemplate.Split(new string[] { newLineKvp.Value.ToString() }, StringSplitOptions.RemoveEmptyEntries);
                    objToLog = logArray;
                }
            }

            return objToLog;
        }

        private string ClearExtraWhiteSpaces(string commandText)
        {
            StringBuilder tmpbuilder = new StringBuilder(commandText.Length);

            string scopy = commandText;
            bool inspaces = false;
            tmpbuilder.Length = 0;

            for (int k = 0; k < commandText.Length; ++k)
            {
                char c = scopy[k];

                if (inspaces)
                {
                    if (c != ' ')
                    {
                        inspaces = false;
                        tmpbuilder.Append(c);
                    }
                }
                else if (c == ' ')
                {
                    inspaces = true;
                    tmpbuilder.Append(' ');
                }
                else
                    tmpbuilder.Append(c);
            }

            string result = tmpbuilder.ToString();
            return result;
        }
    }
}
