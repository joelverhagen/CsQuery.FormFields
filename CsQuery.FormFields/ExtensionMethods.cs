using System.Collections.Generic;

namespace CsQuery.FormFields
{
    /// <summary>
    /// Extension methods that provide addition extension methods on <see cref="IHTMLFormElement" />.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Returns sequence of form field data given a specific submitter element.
        /// </summary>
        /// <param name="form">The form element.</param>
        /// <param name="submitter">The submitter element.</param>
        /// <returns>The sequence of form field data.</returns>
        public static IEnumerable<NameValueType> GetNameValueTypes(this IHTMLFormElement form, IDomElement submitter)
        {
            return GetNameValueTypes(form, submitter, false);
        }

        /// <summary>
        /// Returns sequence of form field data using optional implicit submission.
        /// </summary>
        /// <param name="form">The form element.</param>
        /// <param name="implicitSubmission">Whether or not to use implicit submisssion.</param>
        /// <returns>The sequence of form field data.</returns>
        public static IEnumerable<NameValueType> GetNameValueTypes(this IHTMLFormElement form, bool implicitSubmission)
        {
            return GetNameValueTypes(form, null, implicitSubmission);
        }

        /// <summary>
        /// Returns sequence of form field data.
        /// </summary>
        /// <param name="form">The form element.</param>
        /// <param name="submitter">The submitter element.</param>
        /// <param name="implicitSubmission">Whether or not to use implicit submisssion.</param>
        /// <returns>The sequence of form field data.</returns>
        private static IEnumerable<NameValueType> GetNameValueTypes(IHTMLFormElement form, IDomElement submitter, bool implicitSubmission)
        {
            return FormFieldsParser.GetNameValueTypes(form, submitter, implicitSubmission);
        }
    }
}