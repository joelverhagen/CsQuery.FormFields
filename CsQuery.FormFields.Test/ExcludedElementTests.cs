using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsQuery.FormFields.Test
{
    [TestClass]
    public class ExcludedElementTests
    {
        [TestMethod]
        public void ExcludeDatalistChildren()
        {
            // ARRANGE
            CQ document = "<form><input name=a><datalist><input name=b></datalist></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);
            Assert.AreEqual("a", nameValueTypes[0].Name);
        }

        [TestMethod]
        public void ExcludeWithDisabledAttribute()
        {
            // ARRANGE
            CQ document = "<form><input name=a><input name=b disabled></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);
            Assert.AreEqual("a", nameValueTypes[0].Name);
        }

        [TestMethod]
        public void ExcludeWithDisabledFieldset()
        {
            // ARRANGE
            CQ document = "<form><input name=a><fieldset disabled><input name=b></fieldset></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);
            Assert.AreEqual("a", nameValueTypes[0].Name);
        }

        [TestMethod]
        public void ExcludeWhenNonSubmitterButton()
        {
            // ARRANGE
            CQ document = "<form><input name=a><button name=b></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);
            Assert.AreEqual("a", nameValueTypes[0].Name);
        }

        [TestMethod]
        public void ExcludeWhenNonSubmitterImageInput()
        {
            // ARRANGE
            CQ document = "<form><input name=a><input name=b type=image></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);
            Assert.AreEqual("a", nameValueTypes[0].Name);
        }

        [TestMethod]
        public void ExcludeWhenNonSubmitterResetInput()
        {
            // ARRANGE
            CQ document = "<form><input name=a><input name=b type=reset></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);
            Assert.AreEqual("a", nameValueTypes[0].Name);
        }

        [TestMethod]
        public void ExcludeWhenNonSubmitterSubmitInput()
        {
            // ARRANGE
            CQ document = "<form><input name=a><input name=b type=submit></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);
            Assert.AreEqual("a", nameValueTypes[0].Name);
        }

        [TestMethod]
        public void ExcludeWhenNonSubmitterButtonInput()
        {
            // ARRANGE
            CQ document = "<form><input name=a><input name=b type=button></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);
            Assert.AreEqual("a", nameValueTypes[0].Name);
        }

        [TestMethod]
        public void ExcludeUncheckedCheckbox()
        {
            // ARRANGE
            CQ document = "<form><input name=a><input name=b type=checkbox></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);
            Assert.AreEqual("a", nameValueTypes[0].Name);
        }

        [TestMethod]
        public void ExcludeUncheckedRadioButton()
        {
            // ARRANGE
            CQ document = "<form><input name=a><input name=b type=radio></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);
            Assert.AreEqual("a", nameValueTypes[0].Name);
        }

        [TestMethod]
        public void ExcludeNamelessElement()
        {
            // ARRANGE
            CQ document = "<form><input name=a><input></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);
            Assert.AreEqual("a", nameValueTypes[0].Name);
        }

        [TestMethod]
        public void ExcludeObjectElement()
        {
            // ARRANGE
            CQ document = "<form><input name=a><object name=b></object></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);
            Assert.AreEqual("a", nameValueTypes[0].Name);
        }
    }
}