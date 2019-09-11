using System.Collections.Generic;

namespace Assets.scripts
{
    class CommandProcessor
    {
      
        /// <summary>Processes the the user input and will return the output action in the form of string.</summary>
        /// <param name="pCmdStr">The p command string.</param>
        /// <returns name="strResult">The resultant string.</returns>
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

        
        /// <summary>Gets is used to process the user input and will return the output action in the form of string.</summary>
        /// <param name="pCmdStr">The p command string.</param>
        /// <returns name="listResult">The resultant list.</returns>
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
