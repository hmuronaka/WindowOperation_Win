using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowOperation
{
    public class ResponseMaker
    {
        public String commandName
        {
            private set;
            get;
        }

        public String response
        {
            private set;
            get;
        }

        public int code
        {
            private set;
            get;
        }

        public ResponseMaker(String commandName, String response, int code)
        {
            this.commandName = commandName;
            this.response = response;
            this.code = code;
        }

        public String make()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(commandName);
            builder.Append(";");
            builder.Append("#");
            builder.Append(code.ToString());
            builder.Append(";");
            String editedResponse = replaceEscapeSeq(response);
            builder.Append(editedResponse);
            builder.Append(";");

            return builder.ToString();
        }

        private String replaceEscapeSeq(String response)
        {
            String[] olds = {
                                "\\",
                                "@",
                                ";",
                                "#"
                            };
            String result = response;
            for (int i = 0; i < olds.Length; i++)
            {
                result = result.Replace(olds[i], "\\" + olds[i]);
            }

            return result;
        }

        


    }
}
