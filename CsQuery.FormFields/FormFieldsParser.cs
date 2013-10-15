using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using CsQuery.ExtensionMethods.Internal;

namespace CsQuery.FormFields
{
    /// <summary>
    /// The internal implementation of HTML5 form data algorithm
    /// </summary>
    /// <url>
    /// http://www.w3.org/html/wg/drafts/html/master/forms.html#constructing-form-data-set
    /// </url>
    internal static class FormFieldsParser
    {
        /// <summary>
        /// Returns the form data of the provided form.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="submitter">The validated submitter element.</param>
        /// <param name="implicitSubmission">
        /// Whether or not to use implicit submission. Implicit submission takes precedence over
        /// the provided submitter.
        /// </param>
        /// <returns>A lazy list of form field data.</returns>
        public static IEnumerable<NameValueType> GetNameValueTypes(IHTMLFormElement form, IDomElement submitter, bool implicitSubmission)
        {
            // ensure the submitter node is valid
            if (submitter != null && !IsValidSubmitter(form, submitter))
            {
                throw new ArgumentException("The provided submitter node is not a button or does not belong to the provided form.");
            }

            // find the first valid submitter if we are doing implicit submission
            if (implicitSubmission)
            {
                submitter = form
                    .Document
                    .GetDescendentElements()
                    .FirstOrDefault(e => IsValidSubmitter(form, e));
            }

            return GetNameValueTypes(form, submitter).ToArray();
        }

        /// <summary>
        /// Returns the form data of the provided form.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="submitter">The validated submitter element.</param>
        /// <returns>A lazy list of form field data.</returns>
        private static IEnumerable<NameValueType> GetNameValueTypes(IHTMLFormElement form, IDomElement submitter)
        {
            // walk the form fields
            foreach (IDomElement e in form.Document.GetDescendentElements().Where(e => !IsExcludedFromDataSet(form, e, submitter)))
            {
                // TODO: handle dirname
                // TODO: handle file uploads
                // TODO: handle plugin objects
                // TODO: handle keygen

                if (e is IHTMLInputElement && e.Type == "image")
                {
                    string name = e.GetAttribute("name", string.Empty);
                    if (name != string.Empty)
                    {
                        name += ".";
                    }

                    yield return new NameValueType(name + "x", "0", e.Type);
                    yield return new NameValueType(name + "y", "0", e.Type);
                }
                else if (e is IHTMLSelectElement)
                {
                    foreach (NameValueType pair in GetOptionKeyValueTuples(e as IHTMLSelectElement))
                    {
                        yield return pair;
                    }
                }
                else if (e is IHTMLInputElement && (e.Type == "checkbox" || e.Type == "radio"))
                {
                    yield return new NameValueType(e.Name, e.GetAttribute("value", "on"), e.Type);
                }
                else if (e is IHTMLTextAreaElement)
                {
                    // TODO: handle textarea properly
                    yield return new NameValueType(e.Name, WebUtility.HtmlDecode(e.Value), e.Type);
                }
                else
                {
                    yield return new NameValueType(e.Name, e.GetAttribute("value", string.Empty), e.Type);
                }
            }
        }

        /// <summary>
        /// Returns the <see cref="NameValueType" /> instances corresponding to the selected option elements that are contained in
        /// the provided select element.
        /// </summary>
        /// <param name="select">The select element to inspect.</param>
        /// <returns>A sequence of <see cref="NameValueType" /> instances for each selected option.</returns>
        private static IEnumerable<NameValueType> GetOptionKeyValueTuples(IHTMLSelectElement select)
        {
            IEnumerable<IDomElement> optionNodes = select
                .ChildElements
                .Concat(select
                    .ChildElements
                    .Where(e => e.NodeName == "OPTGROUP")
                    .SelectMany(e => e.ChildElements))
                .Where(e => e is IHTMLOptionElement);

            foreach (IDomElement option in optionNodes)
            {
                if (option.Selected && !option.Disabled)
                {
                    string value;
                    if (option.HasAttribute("value"))
                    {
                        value = option.GetAttribute("value", string.Empty);
                    }
                    else
                    {
                        value = StripAndCollapseWhitespace(WebUtility.HtmlDecode(option.InnerText));
                    }
                    yield return new NameValueType(select.Name, value, select.Type);
                }
            }
        }

        /// <summary>
        /// Determines whether an element is excluded from a form data set.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="e"></param>
        /// <param name="submitter"></param>
        /// <returns></returns>
        private static bool IsExcludedFromDataSet(IHTMLFormElement form, IDomElement e, IDomElement submitter)
        {
            var submittable = e as IFormSubmittableElement;
            if (submittable == null || submittable.Form != form)
            {
                return true;
            }

            return (e.GetAncestors().Any(a => a.NodeName == "DATALIST")) ||
                   (e.Disabled) ||
                   (IsButton(e) && e != submitter) ||
                   (e is IHTMLInputElement && e.Type == "checkbox" && !e.Checked) ||
                   (e is IHTMLInputElement && e.Type == "radio" && !e.Checked) ||
                   (e is IHTMLInputElement && e.Type != "image" && e.Name.IsNullOrEmpty()) ||
                   (e.NodeName == "OBJECT");
        }

        /// <summary>
        /// Determines whether the node can be a submitter
        /// </summary>
        /// <param name="e">The node to check.</param>
        /// <returns>True if the node is some kind of button. False otherwise.</returns>
        private static bool IsButton(IDomObject e)
        {
            return (e is IHTMLButtonElement) ||
                   (e is IHTMLInputElement && (
                       e.Type == "reset" ||
                       e.Type == "image" ||
                       e.Type == "submit" ||
                       e.Type == "button"
                       ));
        }

        /// <summary>
        /// Determines whether the provided element is a valid submitter of the provided form.
        /// </summary>
        /// <param name="form">The form to test.</param>
        /// <param name="e">The element to test.</param>
        /// <returns>True if the provided element is a valid submitter of the provided for. False otherwise.</returns>
        private static bool IsValidSubmitter(IHTMLFormElement form, IDomElement e)
        {
            var formElement = e as IFormSubmittableElement;
            if (formElement != null)
            {
                return IsButton(e) && formElement.Form == form;
            }

            return false;
        }

        /// <summary>
        /// Trim leading and trailing whitespace and collapse whitespace to a single space.
        /// </summary>
        /// <param name="input">The string to trim and collapse.</param>
        /// <returns>A new string.</returns>
        private static string StripAndCollapseWhitespace(string input)
        {
            return Regex.Replace(input, @"[ \t\n\f\r]+", " ").Trim();
        }
    }
}