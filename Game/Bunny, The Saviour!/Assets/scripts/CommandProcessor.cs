using System.Collections.Generic;

namespace Assets.scripts
{
    class CommandProcessor
    {
        /**
         * ProcessAction is used to process the user input and will return the output action in the form of string
         * 
         * input {pCmdStr (type - string) - user action}
         * output {strResult (type - string) - resultant action}
         */
        public string ProcessAction(string pCmdStr)
        {
            string strResult = "Sorry, I didn't get you.";

            // converting to lower case inorder to avoid the case issues
            pCmdStr = pCmdStr.ToLower();

            if (!string.IsNullOrWhiteSpace(pCmdStr))
            {
                CommandMap aCommandMap = new CommandMap();
                if (aCommandMap.RunCommand(pCmdStr)) { 
}
                    return aCommandMap.Result;
            }
            return string.Concat(GameManager.GameManagerInstance.GameModelInstance.CurrentScene.Story,
                   "\n", strResult);
        }

        /**
         * GetNext is used to process the user input and will return the output action in the form of string
         * 
         * input {pCmdStr (type - string) - user action}
         * output {listResult (type - List<string>) - resultant action}
         */
        public List<string> GetNext(string pCmdStr)
        {
            List<string> listResult = new List<string>();

            // converting to lower case inorder to avoid the case issues
            pCmdStr = pCmdStr.ToLower();

            if (!string.IsNullOrWhiteSpace(pCmdStr))
            {
                CommandMap aCommandMap = new CommandMap();
                if(aCommandMap.GetNextQuestion(pCmdStr))
                {
                    listResult.Add(aCommandMap.Question);
                    listResult.Add(aCommandMap.Answer);
                }
            }
            return listResult;
        }
    }
}
