using System;

namespace CsQuery.FormFields
{
    public class NameValueType : IEquatable<NameValueType>
    {
        public NameValueType()
        {
        }

        public NameValueType(string name, string value, string type)
        {
            Name = name;
            Value = value;
            Type = type;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public string Type { get; set; }

        public bool Equals(NameValueType other)
        {
            return Name == other.Name && Value == other.Value && Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((NameValueType) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Value != null ? Value.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Type != null ? Type.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}