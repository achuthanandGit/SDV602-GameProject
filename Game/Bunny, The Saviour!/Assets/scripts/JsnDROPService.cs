using Newtonsoft.Json;
using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// using Parse;
using UnityEngine.Networking;

namespace Assets.scripts
{
    /// <summary>
    /// To recieve fail case response message
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pStrRecObj">The p string record object.</param>
    public delegate void ReceiveRecordDelegate<T>(T pStrRecObj);

    /// <summary>
    /// To recieve success case response message
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pStrRecObjList">The p string record object list.</param>
    public delegate void ReceiveRecordDelegateList<T>(List<T> pStrRecObjList);

    /// <summary>
    /// To parse messages from server
    /// </summary>
    [Serializable]
    public class JsnReceiver
    {
        public String JsnMsg;
        public String Msg;
    }

    /// <summary>
    /// creating gateway to the remote server
    /// </summary>
    public class JsnDROPService
    {
        // to define the url
        private string _URL;

        // to define the token command
        const string tok = "?tok=";

        // to get and set the result
        public string Result { get; set; }

        // to get and set the token
        public string Token { get; set; }

        // Getter and setter for _URL
        public string URL
        {
            get
            {
                return _URL;
            }
            set
            {
                _URL = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsnDROPService"/> class.
        /// </summary>
        public JsnDROPService()
        {
            URL = "https://newsimland.com/~todd/JSON/";
        }

        #region Create Table

        /// <summary>
        /// Enum to define Jsn parameter state
        /// </summary>
        enum JsnPState
        {
            JSONSkip = 0,
            JSONStartCurly = 1,
            JSONEndCurly = 2,
            JSONStartArray = 3,
            JSONEndArray = 4,
            JSONStartName = 5,
            JSONEndName = 6
        }

        /// <summary>
        /// Replaces the name of the pk.
        /// </summary>
        /// <param name="pJsnString">The p JSN string.</param>
        /// <param name="pPKName">Name of the p pk.</param>
        /// <param name="pIsAUTO">if set to <c>true</c> [p is automatic].</param>
        /// <returns></returns>
        private string ReplacePKName(string pJsnString, string pPKName, bool pIsAUTO = false)
        {
            JsnPState currentState = JsnPState.JSONSkip;
            string currentName = "";
            string currentJsn = "";

            foreach (char currentChar in pJsnString)
            {

                switch (currentState)
                {

                    case JsnPState.JSONSkip:
                        if (currentChar == '{')
                        {
                            currentState = JsnPState.JSONStartCurly;
                        }
                        currentJsn += currentChar;
                        break;
                    case JsnPState.JSONStartCurly:
                        if (currentChar == '"')
                        {
                            currentName += '"';
                            currentState = JsnPState.JSONStartName;
                        }
                        else
                            currentJsn += currentChar;
                        break;
                    case JsnPState.JSONStartName:
                        if (currentChar == '"')
                        {
                            if (currentName == "\"" + pPKName)
                            {
                                currentJsn += currentName + " PK" + currentChar;
                            }
                            else
                                currentJsn += currentName + currentChar;

                            currentState = JsnPState.JSONSkip;
                        }// if currentChar
                        else
                            currentName += currentChar;
                        break;

                }
            }
            return currentJsn;
        }

        //public void Create<T, S>(T pExample, ReceiveRecordDelegate<S> pReceiveGoesHere)
        //{
        //    try
        //    {
        //        //string result = "";
        //        bool isAuto = false;
        //        String tblName = ParseTableName<T>();
        //        var tblType = typeof(T);
        //        var DB = new DataService("gameDB.db");
        //        TableMapping map = DB.Connection.GetMapping(tblType);
        //        var PrimaryKeys = map.Columns.Where<TableMapping.Column>(
        //            x =>
        //            {
        //                if (x.IsAutoInc) isAuto = true; // a bit shonky
        //            return x.IsPK;
        //            }
        //            ).ToList<TableMapping.Column>();
        //        string pkName = PrimaryKeys.First<TableMapping.Column>().Name;

        //        string jsnString = JsonConvert.SerializeObject(pExample);
        //        jsnString = ReplacePKName(jsnString, pkName, isAuto);
        //        jsnString = "{\"CREATE\":\"" + tblName + "\",\"EXAMPLE\":" + jsnString + "}";

        //        Debug.Log(jsnString);
        //        Post<S>(jsnString, pReceiveGoesHere);


        //        // Return is via pReceiveGoesHere
        //    }
        //    catch (Exception ex)
        //    {

        //        throw (ex);

        //    }
        //}

        /// <summary>
        /// Parses the name of the table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private string ParseTableName<T>()
        {
            string typeName = typeof(T).ToString();
            if (typeName.Contains('.'))
            {
                string[] nameList = typeName.Split('.');
                return nameList[(nameList.Count() - 1)];
            }
            else
                return typeName;

        }
        #endregion

        #region Store records        
        /// <summary>
        /// Stores the specified p records.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="pRecords">The p records.</param>
        /// <param name="pReceiveGoesHere">The p receive goes here.</param>
        public void Store<T, S>(List<T> pRecords, ReceiveRecordDelegate<S> pReceiveGoesHere)
        {
            try
            {
                string tblName = ParseTableName<T>();
                string jsnList = JsonConvert.SerializeObject(pRecords);//ListToJson<T>(pRecords);
                string jsnString = "{\"STORE\":\"" + tblName + "\",\"VALUE\":" + jsnList + "}";
                Post<S>(jsnString, pReceiveGoesHere);
            }
            catch (Exception ex)
            {

                throw (ex);

            }
        }//Store
        #endregion
        #region retrieve ALL records        
        /// <summary>
        /// Alls the specified p receive success goes here.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="pReceiveSuccessGoesHere">The p receive success goes here.</param>
        /// <param name="pReceiveFailGoesHere">The p receive fail goes here.</param>
        public void All<T, S>(ReceiveRecordDelegateList<T> pReceiveSuccessGoesHere, ReceiveRecordDelegate<S> pReceiveFailGoesHere)
        {
            try
            {
                string tblName = ParseTableName<T>();
                string jsnString = "{\"ALL\":\"" + tblName + "\"}";
                Post<T, S>(jsnString, pReceiveSuccessGoesHere, pReceiveFailGoesHere);
            }
            catch (Exception ex)
            {

                throw (ex);

            }
        }//Store
        #endregion

        #region SELECT records WHERE        
        /// <summary>
        /// Selects the specified p string where.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="pStrWhere">The p string where.</param>
        /// <param name="pReceiveSuccessGoesHere">The p receive success goes here.</param>
        /// <param name="pReceiveFailGoesHere">The p receive fail goes here.</param>
        public void Select<T, S>(string pStrWhere, ReceiveRecordDelegateList<T> pReceiveSuccessGoesHere, ReceiveRecordDelegate<S> pReceiveFailGoesHere)
        {
            try
            {
                string tblName = ParseTableName<T>();
                string jsnString = "{\"SELECT\":\"" + tblName + "\",\"WHERE\":\"" + pStrWhere + "\"}";
                Post<T, S>(jsnString, pReceiveSuccessGoesHere, pReceiveFailGoesHere);
            }
            catch (Exception ex)
            {

                throw (ex);

            }
        }//SELECT WHERE
        #endregion
        #region DELETE records WHERE        
        /// <summary>
        /// Deletes the specified p string where.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="pStrWhere">The p string where.</param>
        /// <param name="pReceiveGoesHere">The p receive goes here.</param>
        public void Delete<T, S>(string pStrWhere, ReceiveRecordDelegate<S> pReceiveGoesHere)
        {
            try
            {
                string tblName = ParseTableName<T>();
                string jsnString = "{\"DELETE\":\"" + tblName + "\",\"WHERE\":\"" + pStrWhere + "\"}";
                Post<S>(jsnString, pReceiveGoesHere);
            }
            catch (Exception ex)
            {

                throw (ex);

            }
        }//DELETE WHERE
        #endregion
        #region DROP table        
        /// <summary>
        /// Drops the specified p receive goes here.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="pReceiveGoesHere">The p receive goes here.</param>
        public void Drop<T, S>(ReceiveRecordDelegate<S> pReceiveGoesHere)
        {
            try
            {
                string tblName = ParseTableName<T>();
                string jsnString = "{\"DROP\":\"" + tblName + "\"}";
                Post<S>(jsnString, pReceiveGoesHere);
            }
            catch (Exception ex)
            {

                throw (ex);

            }
        }//DROP table

        #endregion
        #region Http with JSONDrop - JSON to/from <T> and List<T>        
        /// <summary>
        /// Lists to json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pRecordList">The p record list.</param>
        /// <returns></returns>
        private string ListToJson<T>(List<T> pRecordList)
        {
            string lcResult = "[";
            int lcCount = 0;

            foreach (var record in pRecordList)
            {
                if ((lcCount != pRecordList.Count) && (lcCount != 0))
                    lcResult += ",";
                lcResult += JsonConvert.SerializeObject(record);

                lcCount++;
            }
            lcResult += "]";
            return lcResult;
        }

        /// <summary>
        /// Gets the array from MSG.
        /// </summary>
        /// <param name="pJsn">The p JSN.</param>
        /// <returns></returns>
        private string GetArrayFromMsg(string pJsn)
        // this only works for a Jsn that has one array of records that do not 
        // contain fields of type array
        {
            // state = 0 looking for '[',
            // state = 1 retrieving adding text between '[' and ']'
            // state = 2 when the enclosing ']' is found
            int state = 0;
            string lcResult = "";
            foreach (char lcChar in pJsn)
            {
                switch (state)
                {
                    case 0:
                        if (lcChar == '[')
                        {
                            state = 1;
                        }
                        break;
                    case 1:
                        if (lcChar == ']')
                        {
                            state = 2;

                        }
                        else
                            lcResult += lcChar;
                        break;
                    case 2:

                        break;
                }

            }
            return lcResult;

        }

        /// <summary>
        /// Froms the json array to list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pStrJsonAry">The p string json ary.</param>
        /// <returns></returns>
        private List<T> FromJsonArrayToList<T>(string pStrJsonAry)
        {
            if (string.IsNullOrWhiteSpace(pStrJsonAry))
                return new List<T>();

            string[] sep = { "},{" };
            string[] aryOfJSON = (pStrJsonAry.Substring(1, pStrJsonAry.Length - 2)).Split(sep, StringSplitOptions.RemoveEmptyEntries);
            List<string> lstOfJSONClean = new List<string>(); ;
            foreach (string aString in aryOfJSON)
            {
                string bString = aString;
                if (aString.Substring(0, 1) != "{" && aString.Substring(aString.Length - 1, 1) != "}")
                {
                    bString = "{" + aString + "}";
                }
                else
                if (aString.Substring(aString.Length - 1, 1) != "}")
                {
                    bString = aString + "}";
                }
                else
                if (aString.Substring(0, 1) != "{")
                {
                    bString = "{" + aString;
                }
                lstOfJSONClean.Add(bString);

            }
            aryOfJSON = lstOfJSONClean.ToArray();

            List<T> lcResult = new List<T>();
            foreach (string aJSONStr in aryOfJSON)
            {

                lcResult.Add(JsonConvert.DeserializeObject<T>(aJSONStr));
            }

            return lcResult;
        }

        /// <summary>
        /// Posts the JSON to JSONDrop
        /// </summary>
        /// <param name="pJsn">The p JSN.</param>
        /// <returns>control asynchonously to the delegate</returns>
        private UnityWebRequest jsnWebRequest(string pJsn)
        {
            string URIJsn = URL + tok + "{\"tok\":\"" + Token + "\",\"cmd\":" + pJsn + "}";
            System.Uri lcURI = new System.Uri(URIJsn);

            UnityWebRequest lcWebReq = new UnityWebRequest(lcURI, "GET")
            {
                downloadHandler = new DownloadHandlerBuffer(),

            };
            return lcWebReq;
        }

        /// <summary>
        /// Posts the specified p JSN.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="pJsn">The p JSN.</param>
        /// <param name="pReceiveSuccessGoesHere">The p receive success goes here.</param>
        /// <param name="pReceiveFailGoesHere">The p receive fail goes here.</param>
        private void Post<T, S>(string pJsn, ReceiveRecordDelegateList<T> pReceiveSuccessGoesHere, ReceiveRecordDelegate<S> pReceiveFailGoesHere)
        {
            UnityWebRequest lcWebReq = jsnWebRequest(pJsn);

            // VERY WEIRD, because Send returns an object that
            // you then add a "completed" event handler to the
            // completed listeners
            var lcAsyncOp = lcWebReq.SendWebRequest();
            lcAsyncOp.completed += (x =>
            {
                Debug.Log("RETURNED FROM JsnDROP:" + lcWebReq.downloadHandler.text);
                string lcStrJsnReceived = lcWebReq.downloadHandler.text;
                string lcJsnMsgArray = "";

                // THIS IS LOOKING MESSY!! 
                S jsnAsTypeS = JsonUtility.FromJson<S>(lcStrJsnReceived);
                JsnReceiver jsnReceived = jsnAsTypeS as JsnReceiver;
                List<T> receivedList;
                switch (jsnReceived.JsnMsg)
                {
                    case "SUCCESS.ALL":
                    case "SUCCESS.SELECT":
                        lcJsnMsgArray = GetArrayFromMsg(lcStrJsnReceived);

                        receivedList = FromJsonArrayToList<T>(lcJsnMsgArray);
                        Debug.Log("Post<T,S> LIST length is " + receivedList.Count().ToString());
                        pReceiveSuccessGoesHere(receivedList);
                        break;
                    default:
                        pReceiveFailGoesHere(jsnAsTypeS);
                        break;
                }

            });
        }

        /// <summary>
        /// Posts the specified p JSN.
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="pJsn">The p JSN.</param>
        /// <param name="pReceiveGoesHere">The p receive goes here.</param>
        private void Post<S>(string pJsn, ReceiveRecordDelegate<S> pReceiveGoesHere)
        {


            UnityWebRequest lcWebReq = jsnWebRequest(pJsn);

            // VERY WEIRD, because Send returns an object that
            // you then add a "completed" event handler to the
            // completed listeners
            var lcAsyncOp = lcWebReq.SendWebRequest();
            lcAsyncOp.completed += (x =>
            {
                Debug.Log("RETURNED FROM JsnDROP:" + lcWebReq.downloadHandler.text);

                pReceiveGoesHere(JsonUtility.FromJson<S>(lcWebReq.downloadHandler.text));
            });
        }
        #endregion

    }
}