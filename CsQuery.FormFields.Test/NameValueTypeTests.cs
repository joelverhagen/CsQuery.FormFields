using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsQuery.FormContent.Test
{
    [TestClass]
    public class NameValueTypeTests
    {
        [TestMethod]
        public void SelectOptionInOptgroup()
        {
            // ARRANGE
            CQ document = "<form><select name=a><optgroup><option value=foo selected>moo</option></optgroup></select></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);

            Assert.AreEqual("a", nameValueTypes[0].Name);
            Assert.AreEqual("foo", nameValueTypes[0].Value);
            Assert.AreEqual("select-one", nameValueTypes[0].Type);
        }

        [TestMethod]
        public void SelectOneOption()
        {
            // ARRANGE
            CQ document = "<form><select name=a><option value=foo selected>moo</option><option>bar</option></select></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);

            Assert.AreEqual("a", nameValueTypes[0].Name);
            Assert.AreEqual("foo", nameValueTypes[0].Value);
            Assert.AreEqual("select-one", nameValueTypes[0].Type);
        }

        [TestMethod]
        public void SelectOneOptionWithDisabled()
        {
            // ARRANGE
            CQ document = "<form><select name=a><option value=foo selected disabled>moo</option><option>bar</option></select></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(0, nameValueTypes.Length);
        }

        [TestMethod]
        public void SelectMultipleOptions()
        {
            // ARRANGE
            CQ document = "<form><select multiple name=a><option value=foo selected>moo</option><option selected>bar</option></select></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(2, nameValueTypes.Length);

            Assert.AreEqual("a", nameValueTypes[0].Name);
            Assert.AreEqual("foo", nameValueTypes[0].Value);
            Assert.AreEqual("select-multiple", nameValueTypes[0].Type);

            Assert.AreEqual("a", nameValueTypes[1].Name);
            Assert.AreEqual("bar", nameValueTypes[1].Value);
            Assert.AreEqual("select-multiple", nameValueTypes[1].Type);
        }

        [TestMethod]
        public void SelectMultipleOptionsWithDisabled()
        {
            // ARRANGE
            CQ document = "<form><select multiple name=a><option value=foo selected>moo</option><option selected disabled>bar</option></select></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);

            Assert.AreEqual("a", nameValueTypes[0].Name);
            Assert.AreEqual("foo", nameValueTypes[0].Value);
            Assert.AreEqual("select-multiple", nameValueTypes[0].Type);
        }

        [TestMethod]
        public void CheckboxCheckedValue()
        {
            // ARRANGE
            CQ document = "<form><input name=a type=checkbox checked value=foo></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);

            Assert.AreEqual("a", nameValueTypes[0].Name);
            Assert.AreEqual("foo", nameValueTypes[0].Value);
            Assert.AreEqual("checkbox", nameValueTypes[0].Type);
        }

        [TestMethod]
        public void CheckboxCheckedNoValue()
        {
            // ARRANGE
            CQ document = "<form><input name=a type=checkbox checked></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);

            Assert.AreEqual("a", nameValueTypes[0].Name);
            Assert.AreEqual("on", nameValueTypes[0].Value);
            Assert.AreEqual("checkbox", nameValueTypes[0].Type);
        }

        [TestMethod]
        public void RadioButtonCheckedValue()
        {
            // ARRANGE
            CQ document = "<form><input name=a type=radio checked value=foo></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);

            Assert.AreEqual("a", nameValueTypes[0].Name);
            Assert.AreEqual("foo", nameValueTypes[0].Value);
            Assert.AreEqual("radio", nameValueTypes[0].Type);
        }

        [TestMethod]
        public void RadioButtonCheckedNoValue()
        {
            // ARRANGE
            CQ document = "<form><input name=a type=radio checked></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);

            Assert.AreEqual("a", nameValueTypes[0].Name);
            Assert.AreEqual("on", nameValueTypes[0].Value);
            Assert.AreEqual("radio", nameValueTypes[0].Type);
        }

        [TestMethod]
        public void InputDefaultType()
        {
            // ARRANGE
            CQ document = "<form><input name=a value=foo></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);

            Assert.AreEqual("a", nameValueTypes[0].Name);
            Assert.AreEqual("foo", nameValueTypes[0].Value);
            Assert.AreEqual("text", nameValueTypes[0].Type);
        }

        [TestMethod]
        public void TextArea()
        {
            // ARRANGE
            CQ document = "<form><textarea name=a>Foo that bar. Also, ampersands: &amp;.</textarea></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);

            Assert.AreEqual("a", nameValueTypes[0].Name);
            Assert.AreEqual("Foo that bar. Also, ampersands: &.", nameValueTypes[0].Value);
            Assert.AreEqual("textarea", nameValueTypes[0].Type);
        }

        [TestMethod]
        public void InputOtherType()
        {
            // ARRANGE
            CQ document = "<form><input type=search name=a value=foo></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);

            Assert.AreEqual("a", nameValueTypes[0].Name);
            Assert.AreEqual("foo", nameValueTypes[0].Value);
            Assert.AreEqual("search", nameValueTypes[0].Type);
        }
    }
}