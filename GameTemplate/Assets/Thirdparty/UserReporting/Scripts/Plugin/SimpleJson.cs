//-----------------------------------------------------------------------
// <copyright file="SimpleJson.cs" company="The Outercurve Foundation">
//    Copyright (c) 2011, The Outercurve Foundation.
//
//    Licensed under the MIT License (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.opensource.org/licenses/mit-license.php
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <website>https://github.com/facebook-csharp-sdk/simple-json</website>
//-----------------------------------------------------------------------

// VERSION:

// NOTE: uncomment the following line to make SimpleJson class internal.
//#define SIMPLE_JSON_INTERNAL

// NOTE: uncomment the following line to make JsonArray and JsonObject class internal.
//#define SIMPLE_JSON_OBJARRAYINTERNAL

// NOTE: uncomment the following line to enable dynamic support.
//#define SIMPLE_JSON_DYNAMIC

// NOTE: uncomment the following line to enable DataContract support.
//#define SIMPLE_JSON_DATACONTRACT

// NOTE: uncomment the following line to enable IReadOnlyCollection<T> and IReadOnlyList<T> support.
//#define SIMPLE_JSON_READONLY_COLLECTIONS

// NOTE: uncomment the following line to disable linq expressions/compiled lambda (better performance) instead of method.invoke().
// define if you are using .net framework <= 3.0 or < WP7.5

#define SIMPLE_JSON_NO_LINQ_EXPRESSION

// NOTE: uncomment the following line if you are compiling under Window Metro style application/library.
// usually already defined in properties
//#define NETFX_CORE;

// If you are targetting WinStore, WP8 and NET4.5+ PCL make sure to #define SIMPLE_JSON_TYPEINFO;

// original json parsing code from http://techblog.procurios.nl/k/618/news/view/14605/14863/How-do-I-write-my-own-parser-for-JSON.html

#if NETFX_CORE
#define SIMPLE_JSON_TYPEINFO
#endif

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Unity.Cloud.UserReporting.Plugin.SimpleJson.Reflection;

#if !SIMPLE_JSON_NO_LINQ_EXPRESSION
using System.Linq.Expressions;

#endif
#if SIMPLE_JSON_DYNAMIC
using System.Dynamic;
#endif

// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable RedundantExplicitArrayCreation
// ReSharper disable SuggestUseVarKeywordEvident
namespace Unity.Cloud.UserReporting.Plugin.SimpleJson
{
    [GeneratedCode("simple-json", "1.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
#if SIMPLE_JSON_OBJARRAYINTERNAL
    internal
#else
    public
#endif
        class JsonArray : List<object>
    {
        public JsonArray()
        {
        }

        public JsonArray(int capacity) : base(capacity)
        {
        }

        public override string ToString()
        {
            return SimpleJson.SerializeObject(this) ?? string.Empty;
        }
    }

    [GeneratedCode("simple-json", "1.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
#if SIMPLE_JSON_OBJARRAYINTERNAL
    internal
#else
    public
#endif
        class JsonObject :
#if SIMPLE_JSON_DYNAMIC
 DynamicObject,
#endif
            IDictionary<string, object>
    {
        private readonly Dictionary<string, object> _members;

        public JsonObject()
        {
            _members = new Dictionary<string, object>();
        }

        public JsonObject(IEqualityComparer<string> comparer)
        {
            _members = new Dictionary<string, object>(comparer);
        }

        public object this[int index]
        {
            get { return JsonObject.GetAtIndex(_members, index); }
        }

        internal static object GetAtIndex(IDictionary<string, object> obj, int index)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            if (index >= obj.Count)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            int i = 0;
            foreach (KeyValuePair<string, object> o in obj)
            {
                if (i++ == index)
                {
                    return o.Value;
                }
            }
            return null;
        }

        public void Add(string key, object value)
        {
            _members.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _members.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return _members.Keys; }
        }

        public bool Remove(string key)
        {
            return _members.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _members.TryGetValue(key, out value);
        }

        public ICollection<object> Values
        {
            get { return _members.Values; }
        }

        public object this[string key]
        {
            get { return _members[key]; }
            set { _members[key] = value; }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            _members.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _members.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _members.ContainsKey(item.Key) && this._members[item.Key] == item.Value;
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            int num = Count;
            foreach (KeyValuePair<string, object> kvp in this)
            {
                array[arrayIndex++] = kvp;
                if (--num <= 0)
                {
                    return;
                }
            }
        }

        public int Count
        {
            get { return _members.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return _members.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _members.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _members.GetEnumerator();
        }

        public override string ToString()
        {
            return SimpleJson.SerializeObject(this);
        }

#if SIMPLE_JSON_DYNAMIC
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            // <pex>
            if (binder == null)
                throw new ArgumentNullException("binder");
            // </pex>
            Type targetType = binder.Type;

            if ((targetType == typeof(IEnumerable)) ||
                (targetType == typeof(IEnumerable<KeyValuePair<string, object>>)) ||
                (targetType == typeof(IDictionary<string, object>)) ||
                (targetType == typeof(IDictionary)))
            {
                result = this;
                return true;
            }

            return base.TryConvert(binder, out result);
        }       
        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            // <pex>
            if (binder == null)
                throw new ArgumentNullException("binder");
            // </pex>
            return _members.Remove(binder.Name);
        }         
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes == null) throw new ArgumentNullException("indexes");
            if (indexes.Length == 1)
            {
                result = ((IDictionary<string, object>)this)[(string)indexes[0]];
                return true;
            }
            result = null;
            return true;
        }        
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            object value;
            if (_members.TryGetValue(binder.Name, out value))
            {
                result = value;
                return true;
            }
            result = null;
            return true;
        }         
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (indexes == null) throw new ArgumentNullException("indexes");
            if (indexes.Length == 1)
            {
                ((IDictionary<string, object>)this)[(string)indexes[0]] = value;
                return true;
            }
            return base.TrySetIndex(binder, indexes, value);
        }        
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // <pex>
            if (binder == null)
                throw new ArgumentNullException("binder");
            // </pex>
            _members[binder.Name] = value;
            return true;
        }      
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            foreach (var key in Keys)
                yield return key;
        }
#endif
    }
}

namespace Unity.Cloud.UserReporting.Plugin.SimpleJson
{
    [GeneratedCode("simple-json", "1.0.0")]
#if SIMPLE_JSON_INTERNAL
    internal
#else
    public
#endif
        static class SimpleJson
    {
        private const int TOKEN_NONE = 0;

        private const int TOKEN_CURLY_OPEN = 1;

        private const int TOKEN_CURLY_CLOSE = 2;

        private const int TOKEN_SQUARED_OPEN = 3;

        private const int TOKEN_SQUARED_CLOSE = 4;

        private const int TOKEN_COLON = 5;

        private const int TOKEN_COMMA = 6;

        private const int TOKEN_STRING = 7;

        private const int TOKEN_NUMBER = 8;

        private const int TOKEN_TRUE = 9;

        private const int TOKEN_FALSE = 10;

        private const int TOKEN_NULL = 11;

        private const int BUILDER_CAPACITY = 2000;

        private static readonly char[] EscapeTable;

        private static readonly char[] EscapeCharacters = new char[] {'"', '\\', '\b', '\f', '\n', '\r', '\t'};

        static SimpleJson()
        {
            SimpleJson.EscapeTable = new char[93];
            SimpleJson.EscapeTable['"'] = '"';
            SimpleJson.EscapeTable['\\'] = '\\';
            SimpleJson.EscapeTable['\b'] = 'b';
            SimpleJson.EscapeTable['\f'] = 'f';
            SimpleJson.EscapeTable['\n'] = 'n';
            SimpleJson.EscapeTable['\r'] = 'r';
            SimpleJson.EscapeTable['\t'] = 't';
        }

        public static object DeserializeObject(string json)
        {
            object obj;
            if (SimpleJson.TryDeserializeObject(json, out obj))
            {
                return obj;
            }
            throw new SerializationException("Invalid JSON string");
        }

        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Need to support .NET 2")]
        public static bool TryDeserializeObject(string json, out object obj)
        {
            bool success = true;
            if (json != null)
            {
                char[] charArray = json.ToCharArray();
                int index = 0;
                obj = SimpleJson.ParseValue(charArray, ref index, ref success);
            }
            else
            {
                obj = null;
            }
            return success;
        }

        public static object DeserializeObject(string json, Type type, IJsonSerializerStrategy jsonSerializerStrategy)
        {
            object jsonObject = SimpleJson.DeserializeObject(json);
            return type == null || jsonObject != null && ReflectionUtils.IsAssignableFrom(jsonObject.GetType(), type) ? jsonObject : (jsonSerializerStrategy ?? SimpleJson.CurrentJsonSerializerStrategy).DeserializeObject(jsonObject, type);
        }

        public static object DeserializeObject(string json, Type type)
        {
            return SimpleJson.DeserializeObject(json, type, null);
        }

        public static T DeserializeObject<T>(string json, IJsonSerializerStrategy jsonSerializerStrategy)
        {
            return (T) SimpleJson.DeserializeObject(json, typeof(T), jsonSerializerStrategy);
        }

        public static T DeserializeObject<T>(string json)
        {
            return (T) SimpleJson.DeserializeObject(json, typeof(T), null);
        }

        public static string SerializeObject(object json, IJsonSerializerStrategy jsonSerializerStrategy)
        {
            StringBuilder builder = new StringBuilder(SimpleJson.BUILDER_CAPACITY);
            bool success = SimpleJson.SerializeValue(jsonSerializerStrategy, json, builder);
            return success ? builder.ToString() : null;
        }

        public static string SerializeObject(object json)
        {
            return SimpleJson.SerializeObject(json, SimpleJson.CurrentJsonSerializerStrategy);
        }

        public static string EscapeToJavascriptString(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
            {
                return jsonString;
            }
            StringBuilder sb = new StringBuilder();
            char c;
            for (int i = 0; i < jsonString.Length;)
            {
                c = jsonString[i++];
                if (c == '\\')
                {
                    int remainingLength = jsonString.Length - i;
                    if (remainingLength >= 2)
                    {
                        char lookahead = jsonString[i];
                        if (lookahead == '\\')
                        {
                            sb.Append('\\');
                            ++i;
                        }
                        else if (lookahead == '"')
                        {
                            sb.Append("\"");
                            ++i;
                        }
                        else if (lookahead == 't')
                        {
                            sb.Append('\t');
                            ++i;
                        }
                        else if (lookahead == 'b')
                        {
                            sb.Append('\b');
                            ++i;
                        }
                        else if (lookahead == 'n')
                        {
                            sb.Append('\n');
                            ++i;
                        }
                        else if (lookahead == 'r')
                        {
                            sb.Append('\r');
                            ++i;
                        }
                    }
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private static IDictionary<string, object> ParseObject(char[] json, ref int index, ref bool success)
        {
            IDictionary<string, object> table = new JsonObject();
            int token;

            // {
            SimpleJson.NextToken(json, ref index);
            bool done = false;
            while (!done)
            {
                token = SimpleJson.LookAhead(json, index);
                if (token == SimpleJson.TOKEN_NONE)
                {
                    success = false;
                    return null;
                }
                else if (token == SimpleJson.TOKEN_COMMA)
                {
                    SimpleJson.NextToken(json, ref index);
                }
                else if (token == SimpleJson.TOKEN_CURLY_CLOSE)
                {
                    SimpleJson.NextToken(json, ref index);
                    return table;
                }
                else
                {
                    // name
                    string name = SimpleJson.ParseString(json, ref index, ref success);
                    if (!success)
                    {
                        success = false;
                        return null;
                    }

                    // :
                    token = SimpleJson.NextToken(json, ref index);
                    if (token != SimpleJson.TOKEN_COLON)
                    {
                        success = false;
                        return null;
                    }

                    // value
                    object value = SimpleJson.ParseValue(json, ref index, ref success);
                    if (!success)
                    {
                        success = false;
                        return null;
                    }
                    table[name] = value;
                }
            }
            return table;
        }

        private static JsonArray ParseArray(char[] json, ref int index, ref bool success)
        {
            JsonArray array = new JsonArray();

            // [
            SimpleJson.NextToken(json, ref index);
            bool done = false;
            while (!done)
            {
                int token = SimpleJson.LookAhead(json, index);
                if (token == SimpleJson.TOKEN_NONE)
                {
                    success = false;
                    return null;
                }
                else if (token == SimpleJson.TOKEN_COMMA)
                {
                    SimpleJson.NextToken(json, ref index);
                }
                else if (token == SimpleJson.TOKEN_SQUARED_CLOSE)
                {
                    SimpleJson.NextToken(json, ref index);
                    break;
                }
                else
                {
                    object value = SimpleJson.ParseValue(json, ref index, ref success);
                    if (!success)
                    {
                        return null;
                    }
                    array.Add(value);
                }
            }
            return array;
        }

        private static object ParseValue(char[] json, ref int index, ref bool success)
        {
            switch (SimpleJson.LookAhead(json, index))
            {
                case SimpleJson.TOKEN_STRING: return SimpleJson.ParseString(json, ref index, ref success);
                case SimpleJson.TOKEN_NUMBER: return SimpleJson.ParseNumber(json, ref index, ref success);
                case SimpleJson.TOKEN_CURLY_OPEN: return SimpleJson.ParseObject(json, ref index, ref success);
                case SimpleJson.TOKEN_SQUARED_OPEN: return SimpleJson.ParseArray(json, ref index, ref success);
                case SimpleJson.TOKEN_TRUE:
                    SimpleJson.NextToken(json, ref index);
                    return true;
                case SimpleJson.TOKEN_FALSE:
                    SimpleJson.NextToken(json, ref index);
                    return false;
                case SimpleJson.TOKEN_NULL:
                    SimpleJson.NextToken(json, ref index);
                    return null;
                case SimpleJson.TOKEN_NONE: break;
            }
            success = false;
            return null;
        }

        private static string ParseString(char[] json, ref int index, ref bool success)
        {
            StringBuilder s = new StringBuilder(SimpleJson.BUILDER_CAPACITY);
            char c;
            SimpleJson.EatWhitespace(json, ref index);

            // "
            c = json[index++];
            bool complete = false;
            while (!complete)
            {
                if (index == json.Length)
                {
                    break;
                }
                c = json[index++];
                if (c == '"')
                {
                    complete = true;
                    break;
                }
                else if (c == '\\')
                {
                    if (index == json.Length)
                    {
                        break;
                    }
                    c = json[index++];
                    if (c == '"')
                    {
                        s.Append('"');
                    }
                    else if (c == '\\')
                    {
                        s.Append('\\');
                    }
                    else if (c == '/')
                    {
                        s.Append('/');
                    }
                    else if (c == 'b')
                    {
                        s.Append('\b');
                    }
                    else if (c == 'f')
                    {
                        s.Append('\f');
                    }
                    else if (c == 'n')
                    {
                        s.Append('\n');
                    }
                    else if (c == 'r')
                    {
                        s.Append('\r');
                    }
                    else if (c == 't')
                    {
                        s.Append('\t');
                    }
                    else if (c == 'u')
                    {
                        int remainingLength = json.Length - index;
                        if (remainingLength >= 4)
                        {
                            // parse the 32 bit hex into an integer codepoint
                            uint codePoint;
                            if (!(success = UInt32.TryParse(new string(json, index, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out codePoint)))
                            {
                                return "";
                            }

                            // convert the integer codepoint to a unicode char and add to string
                            if (0xD800 <= codePoint && codePoint <= 0xDBFF) // if high surrogate
                            {
                                index += 4; // skip 4 chars
                                remainingLength = json.Length - index;
                                if (remainingLength >= 6)
                                {
                                    uint lowCodePoint;
                                    if (new string(json, index, 2) == "\\u" && UInt32.TryParse(new string(json, index + 2, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out lowCodePoint))
                                    {
                                        if (0xDC00 <= lowCodePoint && lowCodePoint <= 0xDFFF) // if low surrogate
                                        {
                                            s.Append((char) codePoint);
                                            s.Append((char) lowCodePoint);
                                            index += 6; // skip 6 chars
                                            continue;
                                        }
                                    }
                                }
                                success = false; // invalid surrogate pair
                                return "";
                            }
                            s.Append(SimpleJson.ConvertFromUtf32((int) codePoint));

                            // skip 4 chars
                            index += 4;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    s.Append(c);
                }
            }
            if (!complete)
            {
                success = false;
                return null;
            }
            return s.ToString();
        }

        private static string ConvertFromUtf32(int utf32)
        {
            // http://www.java2s.com/Open-Source/CSharp/2.6.4-mono-.net-core/System/System/Char.cs.htm
            if (utf32 < 0 || utf32 > 0x10FFFF)
            {
                throw new ArgumentOutOfRangeException("utf32", "The argument must be from 0 to 0x10FFFF.");
            }
            if (0xD800 <= utf32 && utf32 <= 0xDFFF)
            {
                throw new ArgumentOutOfRangeException("utf32", "The argument must not be in surrogate pair range.");
            }
            if (utf32 < 0x10000)
            {
                return new string((char) utf32, 1);
            }
            utf32 -= 0x10000;
            return new string(new char[] {(char) ((utf32 >> 10) + 0xD800), (char) (utf32 % 0x0400 + 0xDC00)});
        }

        private static object ParseNumber(char[] json, ref int index, ref bool success)
        {
            SimpleJson.EatWhitespace(json, ref index);
            int lastIndex = SimpleJson.GetLastIndexOfNumber(json, index);
            int charLength = lastIndex - index + 1;
            object returnNumber;
            string str = new string(json, index, charLength);
            if (str.IndexOf(".", StringComparison.OrdinalIgnoreCase) != -1 || str.IndexOf("e", StringComparison.OrdinalIgnoreCase) != -1)
            {
                double number;
                success = double.TryParse(new string(json, index, charLength), NumberStyles.Any, CultureInfo.InvariantCulture, out number);
                returnNumber = number;
            }
            else
            {
                long number;
                success = long.TryParse(new string(json, index, charLength), NumberStyles.Any, CultureInfo.InvariantCulture, out number);
                returnNumber = number;
            }
            index = lastIndex + 1;
            return returnNumber;
        }

        private static int GetLastIndexOfNumber(char[] json, int index)
        {
            int lastIndex;
            for (lastIndex = index; lastIndex < json.Length; lastIndex++)
            {
                if ("0123456789+-.eE".IndexOf(json[lastIndex]) == -1)
                {
                    break;
                }
            }
            return lastIndex - 1;
        }

        private static void EatWhitespace(char[] json, ref int index)
        {
            for (; index < json.Length; index++)
            {
                if (" \t\n\r\b\f".IndexOf(json[index]) == -1)
                {
                    break;
                }
            }
        }

        private static int LookAhead(char[] json, int index)
        {
            int saveIndex = index;
            return SimpleJson.NextToken(json, ref saveIndex);
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private static int NextToken(char[] json, ref int index)
        {
            SimpleJson.EatWhitespace(json, ref index);
            if (index == json.Length)
            {
                return SimpleJson.TOKEN_NONE;
            }
            char c = json[index];
            index++;
            switch (c)
            {
                case '{': return SimpleJson.TOKEN_CURLY_OPEN;
                case '}': return SimpleJson.TOKEN_CURLY_CLOSE;
                case '[': return SimpleJson.TOKEN_SQUARED_OPEN;
                case ']': return SimpleJson.TOKEN_SQUARED_CLOSE;
                case ',': return SimpleJson.TOKEN_COMMA;
                case '"': return SimpleJson.TOKEN_STRING;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '-': return SimpleJson.TOKEN_NUMBER;
                case ':': return SimpleJson.TOKEN_COLON;
            }
            index--;
            int remainingLength = json.Length - index;

            // false
            if (remainingLength >= 5)
            {
                if (json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
                {
                    index += 5;
                    return SimpleJson.TOKEN_FALSE;
                }
            }

            // true
            if (remainingLength >= 4)
            {
                if (json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
                {
                    index += 4;
                    return SimpleJson.TOKEN_TRUE;
                }
            }

            // null
            if (remainingLength >= 4)
            {
                if (json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
                {
                    index += 4;
                    return SimpleJson.TOKEN_NULL;
                }
            }
            return SimpleJson.TOKEN_NONE;
        }

        private static bool SerializeValue(IJsonSerializerStrategy jsonSerializerStrategy, object value, StringBuilder builder)
        {
            bool success = true;
            string stringValue = value as string;
            if (stringValue != null)
            {
                success = SimpleJson.SerializeString(stringValue, builder);
            }
            else
            {
                IDictionary<string, object> dict = value as IDictionary<string, object>;
                if (dict != null)
                {
                    success = SimpleJson.SerializeObject(jsonSerializerStrategy, dict.Keys, dict.Values, builder);
                }
                else
                {
                    IDictionary<string, string> stringDictionary = value as IDictionary<string, string>;
                    if (stringDictionary != null)
                    {
                        success = SimpleJson.SerializeObject(jsonSerializerStrategy, stringDictionary.Keys, stringDictionary.Values, builder);
                    }
                    else
                    {
                        IEnumerable enumerableValue = value as IEnumerable;
                        if (enumerableValue != null)
                        {
                            success = SimpleJson.SerializeArray(jsonSerializerStrategy, enumerableValue, builder);
                        }
                        else if (SimpleJson.IsNumeric(value))
                        {
                            success = SimpleJson.SerializeNumber(value, builder);
                        }
                        else if (value is bool)
                        {
                            builder.Append((bool) value ? "true" : "false");
                        }
                        else if (value == null)
                        {
                            builder.Append("null");
                        }
                        else
                        {
                            object serializedObject;
                            success = jsonSerializerStrategy.TrySerializeNonPrimitiveObject(value, out serializedObject);
                            if (success)
                            {
                                SimpleJson.SerializeValue(jsonSerializerStrategy, serializedObject, builder);
                            }
                        }
                    }
                }
            }
            return success;
        }

        private static bool SerializeObject(IJsonSerializerStrategy jsonSerializerStrategy, IEnumerable keys, IEnumerable values, StringBuilder builder)
        {
            builder.Append("{");
            IEnumerator ke = keys.GetEnumerator();
            IEnumerator ve = values.GetEnumerator();
            bool first = true;
            while (ke.MoveNext() && ve.MoveNext())
            {
                object key = ke.Current;
                object value = ve.Current;
                if (!first)
                {
                    builder.Append(",");
                }
                string stringKey = key as string;
                if (stringKey != null)
                {
                    SimpleJson.SerializeString(stringKey, builder);
                }
                else if (!SimpleJson.SerializeValue(jsonSerializerStrategy, value, builder))
                {
                    return false;
                }
                builder.Append(":");
                if (!SimpleJson.SerializeValue(jsonSerializerStrategy, value, builder))
                {
                    return false;
                }
                first = false;
            }
            builder.Append("}");
            return true;
        }

        private static bool SerializeArray(IJsonSerializerStrategy jsonSerializerStrategy, IEnumerable anArray, StringBuilder builder)
        {
            builder.Append("[");
            bool first = true;
            foreach (object value in anArray)
            {
                if (!first)
                {
                    builder.Append(",");
                }
                if (!SimpleJson.SerializeValue(jsonSerializerStrategy, value, builder))
                {
                    return false;
                }
                first = false;
            }
            builder.Append("]");
            return true;
        }

        private static bool SerializeString(string aString, StringBuilder builder)
        {
            // Happy path if there's nothing to be escaped. IndexOfAny is highly optimized (and unmanaged)
            if (aString.IndexOfAny(SimpleJson.EscapeCharacters) == -1)
            {
                builder.Append('"');
                builder.Append(aString);
                builder.Append('"');
                return true;
            }
            builder.Append('"');
            int safeCharacterCount = 0;
            char[] charArray = aString.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                char c = charArray[i];

                // Non ascii characters are fine, buffer them up and send them to the builder
                // in larger chunks if possible. The escape table is a 1:1 translation table
                // with \0 [default(char)] denoting a safe character.
                if (c >= SimpleJson.EscapeTable.Length || SimpleJson.EscapeTable[c] == default(char))
                {
                    safeCharacterCount++;
                }
                else
                {
                    if (safeCharacterCount > 0)
                    {
                        builder.Append(charArray, i - safeCharacterCount, safeCharacterCount);
                        safeCharacterCount = 0;
                    }
                    builder.Append('\\');
                    builder.Append(SimpleJson.EscapeTable[c]);
                }
            }
            if (safeCharacterCount > 0)
            {
                builder.Append(charArray, charArray.Length - safeCharacterCount, safeCharacterCount);
            }
            builder.Append('"');
            return true;
        }

        private static bool SerializeNumber(object number, StringBuilder builder)
        {
            if (number is long)
            {
                builder.Append(((long) number).ToString(CultureInfo.InvariantCulture));
            }
            else if (number is ulong)
            {
                builder.Append(((ulong) number).ToString(CultureInfo.InvariantCulture));
            }
            else if (number is int)
            {
                builder.Append(((int) number).ToString(CultureInfo.InvariantCulture));
            }
            else if (number is uint)
            {
                builder.Append(((uint) number).ToString(CultureInfo.InvariantCulture));
            }
            else if (number is decimal)
            {
                builder.Append(((decimal) number).ToString(CultureInfo.InvariantCulture));
            }
            else if (number is float)
            {
                builder.Append(((float) number).ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                builder.Append(Convert.ToDouble(number, CultureInfo.InvariantCulture)
                    .ToString("r", CultureInfo.InvariantCulture));
            }
            return true;
        }

        private static bool IsNumeric(object value)
        {
            if (value is sbyte)
            {
                return true;
            }
            if (value is byte)
            {
                return true;
            }
            if (value is short)
            {
                return true;
            }
            if (value is ushort)
            {
                return true;
            }
            if (value is int)
            {
                return true;
            }
            if (value is uint)
            {
                return true;
            }
            if (value is long)
            {
                return true;
            }
            if (value is ulong)
            {
                return true;
            }
            if (value is float)
            {
                return true;
            }
            if (value is double)
            {
                return true;
            }
            if (value is decimal)
            {
                return true;
            }
            return false;
        }

        private static IJsonSerializerStrategy _currentJsonSerializerStrategy;

        public static IJsonSerializerStrategy CurrentJsonSerializerStrategy
        {
            get
            {
                return SimpleJson._currentJsonSerializerStrategy ?? (SimpleJson._currentJsonSerializerStrategy =
#if SIMPLE_JSON_DATACONTRACT
 DataContractJsonSerializerStrategy
#else
                               SimpleJson.PocoJsonSerializerStrategy
#endif
                       );
            }
            set { SimpleJson._currentJsonSerializerStrategy = value; }
        }

        private static PocoJsonSerializerStrategy _pocoJsonSerializerStrategy;

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static PocoJsonSerializerStrategy PocoJsonSerializerStrategy
        {
            get { return SimpleJson._pocoJsonSerializerStrategy ?? (SimpleJson._pocoJsonSerializerStrategy = new PocoJsonSerializerStrategy()); }
        }

#if SIMPLE_JSON_DATACONTRACT
        private static DataContractJsonSerializerStrategy _dataContractJsonSerializerStrategy;
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Advanced)]
        public static DataContractJsonSerializerStrategy DataContractJsonSerializerStrategy
        {
            get
            {
                return _dataContractJsonSerializerStrategy ?? (_dataContractJsonSerializerStrategy = new DataContractJsonSerializerStrategy());
            }
        }

#endif
    }

    [GeneratedCode("simple-json", "1.0.0")]
#if SIMPLE_JSON_INTERNAL
    internal
#else
    public
#endif
        interface IJsonSerializerStrategy
    {
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Need to support .NET 2")]
        bool TrySerializeNonPrimitiveObject(object input, out object output);

        object DeserializeObject(object value, Type type);
    }

    [GeneratedCode("simple-json", "1.0.0")]
#if SIMPLE_JSON_INTERNAL
    internal
#else
    public
#endif
        class PocoJsonSerializerStrategy : IJsonSerializerStrategy
    {
        internal IDictionary<Type, ReflectionUtils.ConstructorDelegate> ConstructorCache;

        internal IDictionary<Type, IDictionary<string, ReflectionUtils.GetDelegate>> GetCache;

        internal IDictionary<Type, IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>> SetCache;

        internal static readonly Type[] EmptyTypes = new Type[0];

        internal static readonly Type[] ArrayConstructorParameterTypes = new Type[] {typeof(int)};

        private static readonly string[] Iso8601Format = new string[] {@"yyyy-MM-dd\THH:mm:ss.FFFFFFF\Z", @"yyyy-MM-dd\THH:mm:ss\Z", @"yyyy-MM-dd\THH:mm:ssK"};

        public PocoJsonSerializerStrategy()
        {
            ConstructorCache = new ReflectionUtils.ThreadSafeDictionary<Type, ReflectionUtils.ConstructorDelegate>(ContructorDelegateFactory);
            GetCache = new ReflectionUtils.ThreadSafeDictionary<Type, IDictionary<string, ReflectionUtils.GetDelegate>>(GetterValueFactory);
            SetCache = new ReflectionUtils.ThreadSafeDictionary<Type, IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>>(SetterValueFactory);
        }

        protected virtual string MapClrMemberNameToJsonFieldName(string clrPropertyName)
        {
            return clrPropertyName;
        }

        internal virtual ReflectionUtils.ConstructorDelegate ContructorDelegateFactory(Type key)
        {
            return ReflectionUtils.GetContructor(key, key.IsArray ? PocoJsonSerializerStrategy.ArrayConstructorParameterTypes : PocoJsonSerializerStrategy.EmptyTypes);
        }

        internal virtual IDictionary<string, ReflectionUtils.GetDelegate> GetterValueFactory(Type type)
        {
            IDictionary<string, ReflectionUtils.GetDelegate> result = new Dictionary<string, ReflectionUtils.GetDelegate>();
            foreach (PropertyInfo propertyInfo in ReflectionUtils.GetProperties(type))
            {
                if (propertyInfo.CanRead)
                {
                    MethodInfo getMethod = ReflectionUtils.GetGetterMethodInfo(propertyInfo);
                    if (getMethod.IsStatic || !getMethod.IsPublic)
                    {
                        continue;
                    }
                    result[MapClrMemberNameToJsonFieldName(propertyInfo.Name)] = ReflectionUtils.GetGetMethod(propertyInfo);
                }
            }
            foreach (FieldInfo fieldInfo in ReflectionUtils.GetFields(type))
            {
                if (fieldInfo.IsStatic || !fieldInfo.IsPublic)
                {
                    continue;
                }
                result[MapClrMemberNameToJsonFieldName(fieldInfo.Name)] = ReflectionUtils.GetGetMethod(fieldInfo);
            }
            return result;
        }

        internal virtual IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> SetterValueFactory(Type type)
        {
            IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> result = new Dictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>();
            foreach (PropertyInfo propertyInfo in ReflectionUtils.GetProperties(type))
            {
                if (propertyInfo.CanWrite)
                {
                    MethodInfo setMethod = ReflectionUtils.GetSetterMethodInfo(propertyInfo);
                    if (setMethod.IsStatic || !setMethod.IsPublic)
                    {
                        continue;
                    }
                    result[MapClrMemberNameToJsonFieldName(propertyInfo.Name)] = new KeyValuePair<Type, ReflectionUtils.SetDelegate>(propertyInfo.PropertyType, ReflectionUtils.GetSetMethod(propertyInfo));
                }
            }
            foreach (FieldInfo fieldInfo in ReflectionUtils.GetFields(type))
            {
                if (fieldInfo.IsInitOnly || fieldInfo.IsStatic || !fieldInfo.IsPublic)
                {
                    continue;
                }
                result[MapClrMemberNameToJsonFieldName(fieldInfo.Name)] = new KeyValuePair<Type, ReflectionUtils.SetDelegate>(fieldInfo.FieldType, ReflectionUtils.GetSetMethod(fieldInfo));
            }
            return result;
        }

        public virtual bool TrySerializeNonPrimitiveObject(object input, out object output)
        {
            return TrySerializeKnownTypes(input, out output) || TrySerializeUnknownTypes(input, out output);
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public virtual object DeserializeObject(object value, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            string str = value as string;
            if (type == typeof(Guid) && string.IsNullOrEmpty(str))
            {
                return default(Guid);
            }
            if (value == null)
            {
                return null;
            }
            object obj = null;
            if (str != null)
            {
                if (str.Length != 0) // We know it can't be null now.
                {
                    if (type == typeof(DateTime) || ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(DateTime))
                    {
                        return DateTime.ParseExact(str, PocoJsonSerializerStrategy.Iso8601Format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                    }
                    if (type == typeof(DateTimeOffset) || ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(DateTimeOffset))
                    {
                        return DateTimeOffset.ParseExact(str, PocoJsonSerializerStrategy.Iso8601Format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                    }
                    if (type == typeof(Guid) || ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(Guid))
                    {
                        return new Guid(str);
                    }
                    if (type == typeof(Uri))
                    {
                        bool isValid = Uri.IsWellFormedUriString(str, UriKind.RelativeOrAbsolute);
                        Uri result;
                        if (isValid && Uri.TryCreate(str, UriKind.RelativeOrAbsolute, out result))
                        {
                            return result;
                        }
                        return null;
                    }
                    if (type == typeof(string))
                    {
                        return str;
                    }
                    if (type == typeof(TimeSpan) || ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(TimeSpan))
                    {
                        return TimeSpan.Parse(str);
                    }
                    return Convert.ChangeType(str, type, CultureInfo.InvariantCulture);
                }
                else
                {
                    if (type == typeof(Guid))
                    {
                        obj = default(Guid);
                    }
                    else if (ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(Guid))
                    {
                        obj = null;
                    }
                    else
                    {
                        obj = str;
                    }
                }

                // Empty string case
                if (!ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(Guid))
                {
                    return str;
                }
            }
            if (type.IsEnum)
            {
                return Enum.Parse(type, value.ToString());
            }
            else if (value is bool)
            {
                return value;
            }
            bool valueIsLong = value is long;
            bool valueIsDouble = value is double;
            if (valueIsLong && type == typeof(long) || valueIsDouble && type == typeof(double))
            {
                return value;
            }
            if (valueIsDouble && type != typeof(double) || valueIsLong && type != typeof(long))
            {
                obj = type == typeof(int) || type == typeof(long) || type == typeof(double) || type == typeof(float) || type == typeof(bool) || type == typeof(decimal) || type == typeof(byte) || type == typeof(short) ? Convert.ChangeType(value, type, CultureInfo.InvariantCulture) : value;
            }
            else
            {
                IDictionary<string, object> objects = value as IDictionary<string, object>;
                if (objects != null)
                {
                    IDictionary<string, object> jsonObject = objects;
                    if (ReflectionUtils.IsTypeDictionary(type))
                    {
                        // if dictionary then
                        Type[] types = ReflectionUtils.GetGenericTypeArguments(type);
                        Type keyType = types[0];
                        Type valueType = types[1];
                        Type genericType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
                        IDictionary dict = (IDictionary) ConstructorCache[genericType]();
                        foreach (KeyValuePair<string, object> kvp in jsonObject)
                        {
                            dict.Add(kvp.Key, this.DeserializeObject(kvp.Value, valueType));
                        }
                        obj = dict;
                    }
                    else
                    {
                        if (type == typeof(object))
                        {
                            obj = value;
                        }
                        else
                        {
                            obj = Activator.CreateInstance(type);
                            foreach (KeyValuePair<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> setter in SetCache[type])
                            {
                                object jsonValue;
                                if (jsonObject.TryGetValue(setter.Key, out jsonValue))
                                {
                                    jsonValue = DeserializeObject(jsonValue, setter.Value.Key);
                                    setter.Value.Value(obj, jsonValue);
                                }
                                else
                                {
                                    string camelKey = setter.Key;
                                    camelKey = char.ToLower(camelKey[0]) + camelKey.Substring(1);
                                    if (jsonObject.TryGetValue(camelKey, out jsonValue))
                                    {
                                        jsonValue = DeserializeObject(jsonValue, setter.Value.Key);
                                        setter.Value.Value(obj, jsonValue);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    IList<object> valueAsList = value as IList<object>;
                    if (valueAsList != null)
                    {
                        IList<object> jsonObject = valueAsList;
                        IList list = null;
                        if (type.IsArray)
                        {
                            list = (IList) ConstructorCache[type](jsonObject.Count);
                            int i = 0;
                            foreach (object o in jsonObject)
                            {
                                list[i++] = this.DeserializeObject(o, type.GetElementType());
                            }
                        }
                        else if (ReflectionUtils.IsTypeGenericeCollectionInterface(type) || ReflectionUtils.IsAssignableFrom(typeof(IList), type))
                        {
                            Type innerType = ReflectionUtils.GetGenericListElementType(type);
                            list = (IList) (ConstructorCache[type] ?? ConstructorCache[typeof(List<>).MakeGenericType(innerType)])();
                            foreach (object o in jsonObject)
                            {
                                list.Add(this.DeserializeObject(o, innerType));
                            }
                        }
                        obj = list;
                    }
                }
                return obj;
            }
            if (ReflectionUtils.IsNullableType(type))
            {
                return ReflectionUtils.ToNullableType(obj, type);
            }
            return obj;
        }

        protected virtual object SerializeEnum(Enum p)
        {
            return Convert.ToDouble(p, CultureInfo.InvariantCulture);
        }

        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Need to support .NET 2")]
        protected virtual bool TrySerializeKnownTypes(object input, out object output)
        {
            bool returnValue = true;
            if (input is DateTime)
            {
                output = ((DateTime) input).ToUniversalTime()
                    .ToString(PocoJsonSerializerStrategy.Iso8601Format[0], CultureInfo.InvariantCulture);
            }
            else if (input is DateTimeOffset)
            {
                output = ((DateTimeOffset) input).ToUniversalTime()
                    .ToString(PocoJsonSerializerStrategy.Iso8601Format[0], CultureInfo.InvariantCulture);
            }
            else if (input is TimeSpan)
            {
                output = ((TimeSpan) input).ToString();
            }
            else if (input is Guid)
            {
                output = ((Guid) input).ToString("D");
            }
            else if (input is Uri)
            {
                output = input.ToString();
            }
            else
            {
                Enum inputEnum = input as Enum;
                if (inputEnum != null)
                {
                    output = this.SerializeEnum(inputEnum);
                }
                else
                {
                    returnValue = false;
                    output = null;
                }
            }
            return returnValue;
        }

        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Need to support .NET 2")]
        protected virtual bool TrySerializeUnknownTypes(object input, out object output)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            output = null;
            Type type = input.GetType();
            if (type.FullName == null)
            {
                return false;
            }
            IDictionary<string, object> obj = new JsonObject();
            IDictionary<string, ReflectionUtils.GetDelegate> getters = GetCache[type];
            foreach (KeyValuePair<string, ReflectionUtils.GetDelegate> getter in getters)
            {
                if (getter.Value != null)
                {
                    obj.Add(this.MapClrMemberNameToJsonFieldName(getter.Key), getter.Value(input));
                }
            }
            output = obj;
            return true;
        }
    }

#if SIMPLE_JSON_DATACONTRACT
    [GeneratedCode("simple-json", "1.0.0")]
#if SIMPLE_JSON_INTERNAL
    internal
#else
    public
#endif
 class DataContractJsonSerializerStrategy : PocoJsonSerializerStrategy
    {
        public DataContractJsonSerializerStrategy()
        {
            GetCache = new ReflectionUtils.ThreadSafeDictionary<Type, IDictionary<string, ReflectionUtils.GetDelegate>>(GetterValueFactory);
            SetCache = new ReflectionUtils.ThreadSafeDictionary<Type, IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>>(SetterValueFactory);
        }

        internal override IDictionary<string, ReflectionUtils.GetDelegate> GetterValueFactory(Type type)
        {
            bool hasDataContract = ReflectionUtils.GetAttribute(type, typeof(DataContractAttribute)) != null;
            if (!hasDataContract)
                return base.GetterValueFactory(type);
            string jsonKey;
            IDictionary<string, ReflectionUtils.GetDelegate> result = new Dictionary<string, ReflectionUtils.GetDelegate>();
            foreach (PropertyInfo propertyInfo in ReflectionUtils.GetProperties(type))
            {
                if (propertyInfo.CanRead)
                {
                    MethodInfo getMethod = ReflectionUtils.GetGetterMethodInfo(propertyInfo);
                    if (!getMethod.IsStatic && CanAdd(propertyInfo, out jsonKey))
                        result[jsonKey] = ReflectionUtils.GetGetMethod(propertyInfo);
                }
            }
            foreach (FieldInfo fieldInfo in ReflectionUtils.GetFields(type))
            {
                if (!fieldInfo.IsStatic && CanAdd(fieldInfo, out jsonKey))
                    result[jsonKey] = ReflectionUtils.GetGetMethod(fieldInfo);
            }
            return result;
        }

        internal override IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> SetterValueFactory(Type type)
        {
            bool hasDataContract = ReflectionUtils.GetAttribute(type, typeof(DataContractAttribute)) != null;
            if (!hasDataContract)
                return base.SetterValueFactory(type);
            string jsonKey;
            IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> result = new Dictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>();
            foreach (PropertyInfo propertyInfo in ReflectionUtils.GetProperties(type))
            {
                if (propertyInfo.CanWrite)
                {
                    MethodInfo setMethod = ReflectionUtils.GetSetterMethodInfo(propertyInfo);
                    if (!setMethod.IsStatic && CanAdd(propertyInfo, out jsonKey))
                        result[jsonKey] = new KeyValuePair<Type, ReflectionUtils.SetDelegate>(propertyInfo.PropertyType, ReflectionUtils.GetSetMethod(propertyInfo));
                }
            }
            foreach (FieldInfo fieldInfo in ReflectionUtils.GetFields(type))
            {
                if (!fieldInfo.IsInitOnly && !fieldInfo.IsStatic && CanAdd(fieldInfo, out jsonKey))
                    result[jsonKey] = new KeyValuePair<Type, ReflectionUtils.SetDelegate>(fieldInfo.FieldType, ReflectionUtils.GetSetMethod(fieldInfo));
            }
            // todo implement sorting for DATACONTRACT.
            return result;
        }

        private static bool CanAdd(MemberInfo info, out string jsonKey)
        {
            jsonKey = null;
            if (ReflectionUtils.GetAttribute(info, typeof(IgnoreDataMemberAttribute)) != null)
                return false;
            DataMemberAttribute dataMemberAttribute = (DataMemberAttribute)ReflectionUtils.GetAttribute(info, typeof(DataMemberAttribute));
            if (dataMemberAttribute == null)
                return false;
            jsonKey = string.IsNullOrEmpty(dataMemberAttribute.Name) ? info.Name : dataMemberAttribute.Name;
            return true;
        }
    }

#endif

    namespace Reflection
    {
        // This class is meant to be copied into other libraries. So we want to exclude it from Code Analysis rules
        // that might be in place in the target project.
        [GeneratedCode("reflection-utils", "1.0.0")]
#if SIMPLE_JSON_REFLECTION_UTILS_PUBLIC
        public
#else
        internal
#endif
            class ReflectionUtils
        {
            private static readonly object[] EmptyObjects = new object[] { };

            public delegate object GetDelegate(object source);

            public delegate void SetDelegate(object source, object value);

            public delegate object ConstructorDelegate(params object[] args);

            public delegate TValue ThreadSafeDictionaryValueFactory<TKey, TValue>(TKey key);

#if SIMPLE_JSON_TYPEINFO
            public static TypeInfo GetTypeInfo(Type type)
            {
                return type.GetTypeInfo();
            }
#else
            public static Type GetTypeInfo(Type type)
            {
                return type;
            }
#endif

            public static Attribute GetAttribute(MemberInfo info, Type type)
            {
#if SIMPLE_JSON_TYPEINFO
                if (info == null || type == null || !info.IsDefined(type))
                    return null;
                return info.GetCustomAttribute(type);
#else
                if (info == null || type == null || !Attribute.IsDefined(info, type))
                {
                    return null;
                }
                return Attribute.GetCustomAttribute(info, type);
#endif
            }

            public static Type GetGenericListElementType(Type type)
            {
                IEnumerable<Type> interfaces;
#if SIMPLE_JSON_TYPEINFO
                interfaces = type.GetTypeInfo().ImplementedInterfaces;
#else
                interfaces = type.GetInterfaces();
#endif
                foreach (Type implementedInterface in interfaces)
                {
                    if (ReflectionUtils.IsTypeGeneric(implementedInterface) && implementedInterface.GetGenericTypeDefinition() == typeof(IList<>))
                    {
                        return ReflectionUtils.GetGenericTypeArguments(implementedInterface)[0];
                    }
                }
                return ReflectionUtils.GetGenericTypeArguments(type)[0];
            }

            public static Attribute GetAttribute(Type objectType, Type attributeType)
            {
#if SIMPLE_JSON_TYPEINFO
                if (objectType == null || attributeType == null || !objectType.GetTypeInfo().IsDefined(attributeType))
                    return null;
                return objectType.GetTypeInfo().GetCustomAttribute(attributeType);
#else
                if (objectType == null || attributeType == null || !Attribute.IsDefined(objectType, attributeType))
                {
                    return null;
                }
                return Attribute.GetCustomAttribute(objectType, attributeType);
#endif
            }

            public static Type[] GetGenericTypeArguments(Type type)
            {
#if SIMPLE_JSON_TYPEINFO
                return type.GetTypeInfo().GenericTypeArguments;
#else
                return type.GetGenericArguments();
#endif
            }

            public static bool IsTypeGeneric(Type type)
            {
                return ReflectionUtils.GetTypeInfo(type)
                    .IsGenericType;
            }

            public static bool IsTypeGenericeCollectionInterface(Type type)
            {
                if (!ReflectionUtils.IsTypeGeneric(type))
                {
                    return false;
                }
                Type genericDefinition = type.GetGenericTypeDefinition();
                return genericDefinition == typeof(IList<>) || genericDefinition == typeof(ICollection<>) || genericDefinition == typeof(IEnumerable<>);
            }

            public static bool IsAssignableFrom(Type type1, Type type2)
            {
                return ReflectionUtils.GetTypeInfo(type1)
                    .IsAssignableFrom(ReflectionUtils.GetTypeInfo(type2));
            }

            public static bool IsTypeDictionary(Type type)
            {
#if SIMPLE_JSON_TYPEINFO
                if (typeof(IDictionary<,>).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
                    return true;
#else
                if (typeof(System.Collections.IDictionary).IsAssignableFrom(type))
                {
                    return true;
                }
#endif
                if (!ReflectionUtils.GetTypeInfo(type)
                    .IsGenericType)
                {
                    return false;
                }
                Type genericDefinition = type.GetGenericTypeDefinition();
                return genericDefinition == typeof(IDictionary<,>);
            }

            public static bool IsNullableType(Type type)
            {
                return ReflectionUtils.GetTypeInfo(type)
                           .IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
            }

            public static object ToNullableType(object obj, Type nullableType)
            {
                return obj == null ? null : Convert.ChangeType(obj, Nullable.GetUnderlyingType(nullableType), CultureInfo.InvariantCulture);
            }

            public static bool IsValueType(Type type)
            {
                return ReflectionUtils.GetTypeInfo(type)
                    .IsValueType;
            }

            public static IEnumerable<ConstructorInfo> GetConstructors(Type type)
            {
#if SIMPLE_JSON_TYPEINFO
                return type.GetTypeInfo().DeclaredConstructors;
#else
                return type.GetConstructors();
#endif
            }

            public static ConstructorInfo GetConstructorInfo(Type type, params Type[] argsType)
            {
                IEnumerable<ConstructorInfo> constructorInfos = ReflectionUtils.GetConstructors(type);
                int i;
                bool matches;
                foreach (ConstructorInfo constructorInfo in constructorInfos)
                {
                    ParameterInfo[] parameters = constructorInfo.GetParameters();
                    if (argsType.Length != parameters.Length)
                    {
                        continue;
                    }
                    i = 0;
                    matches = true;
                    foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters())
                    {
                        if (parameterInfo.ParameterType != argsType[i])
                        {
                            matches = false;
                            break;
                        }
                    }
                    if (matches)
                    {
                        return constructorInfo;
                    }
                }
                return null;
            }

            public static IEnumerable<PropertyInfo> GetProperties(Type type)
            {
#if SIMPLE_JSON_TYPEINFO
                return type.GetRuntimeProperties();
#else
                return type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
#endif
            }

            public static IEnumerable<FieldInfo> GetFields(Type type)
            {
#if SIMPLE_JSON_TYPEINFO
                return type.GetRuntimeFields();
#else
                return type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
#endif
            }

            public static MethodInfo GetGetterMethodInfo(PropertyInfo propertyInfo)
            {
#if SIMPLE_JSON_TYPEINFO
                return propertyInfo.GetMethod;
#else
                return propertyInfo.GetGetMethod(true);
#endif
            }

            public static MethodInfo GetSetterMethodInfo(PropertyInfo propertyInfo)
            {
#if SIMPLE_JSON_TYPEINFO
                return propertyInfo.SetMethod;
#else
                return propertyInfo.GetSetMethod(true);
#endif
            }

            public static ConstructorDelegate GetContructor(ConstructorInfo constructorInfo)
            {
#if SIMPLE_JSON_NO_LINQ_EXPRESSION
                return ReflectionUtils.GetConstructorByReflection(constructorInfo);
#else
                return ReflectionUtils.GetConstructorByExpression(constructorInfo);
#endif
            }

            public static ConstructorDelegate GetContructor(Type type, params Type[] argsType)
            {
#if SIMPLE_JSON_NO_LINQ_EXPRESSION
                return ReflectionUtils.GetConstructorByReflection(type, argsType);
#else
                return ReflectionUtils.GetConstructorByExpression(type, argsType);
#endif
            }

            public static ConstructorDelegate GetConstructorByReflection(ConstructorInfo constructorInfo)
            {
                return delegate(object[] args) { return constructorInfo.Invoke(args); };
            }

            public static ConstructorDelegate GetConstructorByReflection(Type type, params Type[] argsType)
            {
                ConstructorInfo constructorInfo = ReflectionUtils.GetConstructorInfo(type, argsType);
                return constructorInfo == null ? null : ReflectionUtils.GetConstructorByReflection(constructorInfo);
            }

#if !SIMPLE_JSON_NO_LINQ_EXPRESSION
            public static ConstructorDelegate GetConstructorByExpression(ConstructorInfo constructorInfo)
            {
                ParameterInfo[] paramsInfo = constructorInfo.GetParameters();
                ParameterExpression param = Expression.Parameter(typeof(object[]), "args");
                Expression[] argsExp = new Expression[paramsInfo.Length];
                for (int i = 0; i < paramsInfo.Length; i++)
                {
                    Expression index = Expression.Constant(i);
                    Type paramType = paramsInfo[i]
                        .ParameterType;
                    Expression paramAccessorExp = Expression.ArrayIndex(param, index);
                    Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);
                    argsExp[i] = paramCastExp;
                }
                NewExpression newExp = Expression.New(constructorInfo, argsExp);
                Expression<Func<object[], object>> lambda = Expression.Lambda<Func<object[], object>>(newExp, param);
                Func<object[], object> compiledLambda = lambda.Compile();
                return delegate(object[] args) { return compiledLambda(args); };
            }

            public static ConstructorDelegate GetConstructorByExpression(Type type, params Type[] argsType)
            {
                ConstructorInfo constructorInfo = ReflectionUtils.GetConstructorInfo(type, argsType);
                return constructorInfo == null ? null : ReflectionUtils.GetConstructorByExpression(constructorInfo);
            }

#endif

            public static GetDelegate GetGetMethod(PropertyInfo propertyInfo)
            {
#if SIMPLE_JSON_NO_LINQ_EXPRESSION
                return ReflectionUtils.GetGetMethodByReflection(propertyInfo);
#else
                return ReflectionUtils.GetGetMethodByExpression(propertyInfo);
#endif
            }

            public static GetDelegate GetGetMethod(FieldInfo fieldInfo)
            {
#if SIMPLE_JSON_NO_LINQ_EXPRESSION
                return ReflectionUtils.GetGetMethodByReflection(fieldInfo);
#else
                return ReflectionUtils.GetGetMethodByExpression(fieldInfo);
#endif
            }

            public static GetDelegate GetGetMethodByReflection(PropertyInfo propertyInfo)
            {
                MethodInfo methodInfo = ReflectionUtils.GetGetterMethodInfo(propertyInfo);
                return delegate(object source) { return methodInfo.Invoke(source, ReflectionUtils.EmptyObjects); };
            }

            public static GetDelegate GetGetMethodByReflection(FieldInfo fieldInfo)
            {
                return delegate(object source) { return fieldInfo.GetValue(source); };
            }

#if !SIMPLE_JSON_NO_LINQ_EXPRESSION
            public static GetDelegate GetGetMethodByExpression(PropertyInfo propertyInfo)
            {
                MethodInfo getMethodInfo = ReflectionUtils.GetGetterMethodInfo(propertyInfo);
                ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
                UnaryExpression instanceCast = !ReflectionUtils.IsValueType(propertyInfo.DeclaringType) ? Expression.TypeAs(instance, propertyInfo.DeclaringType) : Expression.Convert(instance, propertyInfo.DeclaringType);
                Func<object, object> compiled = Expression.Lambda<Func<object, object>>(Expression.TypeAs(Expression.Call(instanceCast, getMethodInfo), typeof(object)), instance)
                    .Compile();
                return delegate(object source) { return compiled(source); };
            }

            public static GetDelegate GetGetMethodByExpression(FieldInfo fieldInfo)
            {
                ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
                MemberExpression member = Expression.Field(Expression.Convert(instance, fieldInfo.DeclaringType), fieldInfo);
                GetDelegate compiled = Expression.Lambda<GetDelegate>(Expression.Convert(member, typeof(object)), instance)
                    .Compile();
                return delegate(object source) { return compiled(source); };
            }

#endif

            public static SetDelegate GetSetMethod(PropertyInfo propertyInfo)
            {
#if SIMPLE_JSON_NO_LINQ_EXPRESSION
                return ReflectionUtils.GetSetMethodByReflection(propertyInfo);
#else
                return ReflectionUtils.GetSetMethodByExpression(propertyInfo);
#endif
            }

            public static SetDelegate GetSetMethod(FieldInfo fieldInfo)
            {
#if SIMPLE_JSON_NO_LINQ_EXPRESSION
                return ReflectionUtils.GetSetMethodByReflection(fieldInfo);
#else
                return ReflectionUtils.GetSetMethodByExpression(fieldInfo);
#endif
            }

            public static SetDelegate GetSetMethodByReflection(PropertyInfo propertyInfo)
            {
                MethodInfo methodInfo = ReflectionUtils.GetSetterMethodInfo(propertyInfo);
                return delegate(object source, object value) { methodInfo.Invoke(source, new object[] {value}); };
            }

            public static SetDelegate GetSetMethodByReflection(FieldInfo fieldInfo)
            {
                return delegate(object source, object value) { fieldInfo.SetValue(source, value); };
            }

#if !SIMPLE_JSON_NO_LINQ_EXPRESSION
            public static SetDelegate GetSetMethodByExpression(PropertyInfo propertyInfo)
            {
                MethodInfo setMethodInfo = ReflectionUtils.GetSetterMethodInfo(propertyInfo);
                ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
                ParameterExpression value = Expression.Parameter(typeof(object), "value");
                UnaryExpression instanceCast = !ReflectionUtils.IsValueType(propertyInfo.DeclaringType) ? Expression.TypeAs(instance, propertyInfo.DeclaringType) : Expression.Convert(instance, propertyInfo.DeclaringType);
                UnaryExpression valueCast = !ReflectionUtils.IsValueType(propertyInfo.PropertyType) ? Expression.TypeAs(value, propertyInfo.PropertyType) : Expression.Convert(value, propertyInfo.PropertyType);
                Action<object, object> compiled = Expression.Lambda<Action<object, object>>(Expression.Call(instanceCast, setMethodInfo, valueCast), new ParameterExpression[] {instance, value})
                    .Compile();
                return delegate(object source, object val) { compiled(source, val); };
            }

            public static SetDelegate GetSetMethodByExpression(FieldInfo fieldInfo)
            {
                ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
                ParameterExpression value = Expression.Parameter(typeof(object), "value");
                Action<object, object> compiled = Expression.Lambda<Action<object, object>>(ReflectionUtils.Assign(Expression.Field(Expression.Convert(instance, fieldInfo.DeclaringType), fieldInfo), Expression.Convert(value, fieldInfo.FieldType)), instance, value)
                    .Compile();
                return delegate(object source, object val) { compiled(source, val); };
            }

            public static BinaryExpression Assign(Expression left, Expression right)
            {
#if SIMPLE_JSON_TYPEINFO
                return Expression.Assign(left, right);
#else
                MethodInfo assign = typeof(Assigner<>).MakeGenericType(left.Type)
                    .GetMethod("Assign");
                BinaryExpression assignExpr = Expression.Add(left, right, assign);
                return assignExpr;
#endif
            }

            private static class Assigner<T>
            {
            #region Static Methods

                public static T Assign(ref T left, T right)
                {
                    return left = right;
                }

            #endregion
            }

#endif

            public sealed class ThreadSafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>
            {
                #region Constructors

                public ThreadSafeDictionary(ThreadSafeDictionaryValueFactory<TKey, TValue> valueFactory)
                {
                    _valueFactory = valueFactory;
                }

                #endregion

                #region Fields

                private Dictionary<TKey, TValue> _dictionary;

                private readonly object _lock = new object();

                private readonly ThreadSafeDictionaryValueFactory<TKey, TValue> _valueFactory;

                #endregion

                #region Properties

                public int Count
                {
                    get { return _dictionary.Count; }
                }

                public bool IsReadOnly
                {
                    get { throw new NotImplementedException(); }
                }

                public TValue this[TKey key]
                {
                    get { return Get(key); }
                    set { throw new NotImplementedException(); }
                }

                public ICollection<TKey> Keys
                {
                    get { return _dictionary.Keys; }
                }

                public ICollection<TValue> Values
                {
                    get { return _dictionary.Values; }
                }

                #endregion

                #region Methods

                public void Add(TKey key, TValue value)
                {
                    throw new NotImplementedException();
                }

                public void Add(KeyValuePair<TKey, TValue> item)
                {
                    throw new NotImplementedException();
                }

                private TValue AddValue(TKey key)
                {
                    TValue value = _valueFactory(key);
                    lock (_lock)
                    {
                        if (_dictionary == null)
                        {
                            _dictionary = new Dictionary<TKey, TValue>();
                            _dictionary[key] = value;
                        }
                        else
                        {
                            TValue val;
                            if (_dictionary.TryGetValue(key, out val))
                            {
                                return val;
                            }
                            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>(_dictionary);
                            dict[key] = value;
                            _dictionary = dict;
                        }
                    }
                    return value;
                }

                public void Clear()
                {
                    throw new NotImplementedException();
                }

                public bool Contains(KeyValuePair<TKey, TValue> item)
                {
                    throw new NotImplementedException();
                }

                public bool ContainsKey(TKey key)
                {
                    return _dictionary.ContainsKey(key);
                }

                public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
                {
                    throw new NotImplementedException();
                }

                private TValue Get(TKey key)
                {
                    if (_dictionary == null)
                    {
                        return this.AddValue(key);
                    }
                    TValue value;
                    if (!_dictionary.TryGetValue(key, out value))
                    {
                        return this.AddValue(key);
                    }
                    return value;
                }

                public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
                {
                    return _dictionary.GetEnumerator();
                }

                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
                {
                    return _dictionary.GetEnumerator();
                }

                public bool Remove(TKey key)
                {
                    throw new NotImplementedException();
                }

                public bool Remove(KeyValuePair<TKey, TValue> item)
                {
                    throw new NotImplementedException();
                }

                public bool TryGetValue(TKey key, out TValue value)
                {
                    value = this[key];
                    return true;
                }

                #endregion
            }
        }
    }
}

// ReSharper restore LoopCanBeConvertedToQuery
// ReSharper restore RedundantExplicitArrayCreation
// ReSharper restore SuggestUseVarKeywordEvident