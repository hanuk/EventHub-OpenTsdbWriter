/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See LICENSE in the root of the repository for license information 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Utility.Core
{
    public class Message:IMessage
    {
        private string _idFieldName; 
        private Dictionary<string, string> _fieldDict;
        public Message()
        {
            _fieldDict = new Dictionary<string, string>();
        }
        public string GetId()
        {
            if (_idFieldName == null)
                return null;

            if (!_fieldDict.ContainsKey(_idFieldName))
                return null;
            
            return _fieldDict[_idFieldName];
        }

        public string GetValue(string fieldName)
        {
            if (_fieldDict.ContainsKey(fieldName))
            {
                return _fieldDict[fieldName];
            }
            return null; 
        }

        public void Add(string fieldName, string fieldValue)
        {
            AddOrReplace(fieldName, fieldValue);
        }
        public void Add(string fieldName, string fieldValue,bool isId)
        {
            _idFieldName = fieldName;
            AddOrReplace(fieldName, fieldValue);
        }
        private void AddOrReplace(string fieldName, string fieldValue)
        {
            if (_fieldDict.ContainsKey(fieldName))
            {
                _fieldDict[fieldName] = fieldValue;
            }
            else
            {
                _fieldDict.Add(fieldName, fieldValue);
            }
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static string Serialize(Message msg)
        {
            return JsonConvert.SerializeObject(msg);
        }
        public static Message Deserialize(string serializedObject)
        {
            return JsonConvert.DeserializeObject<Message>(serializedObject);
        }
        public string KeyName
        {
            get { return _idFieldName; }
            set {_idFieldName = value; }
        }
        public Dictionary<string, string> FieldList
        {
            get {return _fieldDict; }
            set {_fieldDict = value; }
        }
    }
}
