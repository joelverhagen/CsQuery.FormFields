using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using CsQuery.ExtensionMethods.Internal;

namespace CsQuery.FormFields
{
    internal class FormFieldsParser
    {
        public IEnumerable<NameValueType> GetNameValueTypes(IHTMLFormElement form, IDomElement submitterNode, bool implicitSubmission)
        {
            if (submitterNode != null && !IsSubmitter(submitterNode, form))
            {
                throw new ArgumentException("The provided submitter node is not a button or does not belong to the provided form.");
            }

            if (implicitSubmission && submitterNode == null)
            {
                submitterNode = GetFirstValidSubmitterNode(form);
            }

            return GetKeyValuePairs(form, submitterNode).ToArray();
        }

        private IEnumerable<NameValueType> GetKeyValuePairs(IHTMLFormElement form, IDomElement submitter)
        {
            foreach (IDomElement e in GetSubmittableElements(form, submitter))
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
                    foreach (NameValueType pair in GetOptionKeyValueTuples(e))
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

        private static IEnumerable<NameValueType> GetOptionKeyValueTuples(IDomElement select)
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

        private static IEnumerable<IDomElement> GetSubmittableElements(IHTMLFormElement form, IDomElement submitter)
        {
            return form
                .Document
                .GetDescendentElements()
                .OfType<IFormSubmittableElement>()
                .Where(e => e.Form == form)
                .OfType<IDomElement>()
                .Where(e => !IsExcludedFromDataSet(e, submitter));
        }

        private static IDomElement GetFirstValidSubmitterNode(IHTMLFormElement form)
        {
            IDomElement firstSubmitter = form
                .Document
                .GetDescendentElements()
                .FirstOrDefault(e => IsSubmitter(e, form));
            if (firstSubmitter == null || firstSubmitter.Disabled)
            {
                return null;
            }
            return firstSubmitter;
        }

        private static bool IsExcludedFromDataSet(IDomElement e, IDomElement submitter)
        {
            return (e == null) ||
                   (e.GetAncestors().Any(a => a.NodeName == "DATALIST")) ||
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
        public static bool IsButton(IDomObject e)
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
        /// Determines whether the provided element is a valid submitter of the
        /// </summary>
        /// <param name="e"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        public static bool IsSubmitter(IDomElement e, IHTMLFormElement form)
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
        public static string StripAndCollapseWhitespace(string input)
        {
            return Regex.Replace(input, @"[ \t\n\f\r]+", " ").Trim();
        }
    }
}