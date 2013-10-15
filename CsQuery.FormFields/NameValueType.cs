using System;

namespace CsQuery.FormFields
{
    public class NameValueType : Tuple<string, string, string>
    {
        public NameValueType(string name, string value, string type) : base(name, value, type)
        {
        }

        public string Name
        {
            get { return Item1; }
        }

        public string Value
        {
            get { return Item2; }
        }

        public string Type
        {
            get { return Item3; }
        }
    }
}