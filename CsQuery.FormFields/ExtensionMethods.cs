using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace CsQuery.FormFields
{
    public static class ExtensionMethods
    {
        public static IEnumerable<NameValueType> GetNameValueTypes(this IHTMLFormElement form, IDomElement submitterNode)
        {
            return GetNameValueTypes(form, submitterNode, false);
        }

        public static IEnumerable<NameValueType> GetNameValueTypes(this IHTMLFormElement form, bool implicitSubmission)
        {
            return GetNameValueTypes(form, null, implicitSubmission);
        }

        private static IEnumerable<NameValueType> GetNameValueTypes(IHTMLFormElement form, IDomElement submitterNode, bool implicitSubmission)
        {
            return new FormFieldsParser().GetNameValueTypes(form, submitterNode, implicitSubmission);
        }

        public static HttpContent GetHttpContent(this IHTMLFormElement form, IDomElement submitterNode)
        {
            return GetHttpContent(form, submitterNode, false);
        }

        public static HttpContent GetHttpContent(this IHTMLFormElement form, bool implicitSubmission)
        {
            return GetHttpContent(form, null, implicitSubmission);
        }

        private static HttpContent GetHttpContent(IHTMLFormElement form, IDomElement submitterNode, bool implicitSubmission)
        {
            return new FormUrlEncodedContent(GetNameValueTypes(form, submitterNode, implicitSubmission).Select(t => new KeyValuePair<string, string>(t.Name, t.Value)));
        }
    }
}