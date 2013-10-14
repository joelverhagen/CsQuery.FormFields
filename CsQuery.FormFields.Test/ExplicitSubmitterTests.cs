using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsQuery.FormContent.Test
{
    [TestClass]
    public class ExplicitSubmitterTests
    {
        [TestMethod]
        public void IncludeSubmitterResetInput()
        {
            // ARRANGE
            CQ document = "<form><input name=a type=reset value=foo></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();
            IHTMLInputElement submitter = document["input"].OfType<IHTMLInputElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(submitter).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);

            Assert.AreEqual("a", nameValueTypes[0].Name);
            Assert.AreEqual("foo", nameValueTypes[0].Value);
            Assert.AreEqual("reset", nameValueTypes[0].Type);
        }

        [TestMethod]
        public void IncludeSubmitterSubmitInput()
        {
            // ARRANGE
            CQ document = "<form><input name=a type=submit value=foo></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();
            IHTMLInputElement submitter = document["input"].OfType<IHTMLInputElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(submitter).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);

            Assert.AreEqual("a", nameValueTypes[0].Name);
            Assert.AreEqual("foo", nameValueTypes[0].Value);
            Assert.AreEqual("submit", nameValueTypes[0].Type);
        }

        [TestMethod]
        public void IncludeSubmitterButtonInput()
        {
            // ARRANGE
            CQ document = "<form><input name=a type=button value=foo></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();
            IHTMLInputElement submitter = document["input"].OfType<IHTMLInputElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(submitter).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);

            Assert.AreEqual("a", nameValueTypes[0].Name);
            Assert.AreEqual("foo", nameValueTypes[0].Value);
            Assert.AreEqual("button", nameValueTypes[0].Type);
        }

        [TestMethod]
        public void IncludeNamelessSubmitterImageInput()
        {
            // ARRANGE
            CQ document = "<form><input type=image></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();
            IHTMLInputElement submitter = document["input"].OfType<IHTMLInputElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(submitter).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(2, nameValueTypes.Length);

            Assert.AreEqual("x", nameValueTypes[0].Name);
            Assert.AreEqual("0", nameValueTypes[0].Value);
            Assert.AreEqual("image", nameValueTypes[0].Type);

            Assert.AreEqual("y", nameValueTypes[1].Name);
            Assert.AreEqual("0", nameValueTypes[1].Value);
            Assert.AreEqual("image", nameValueTypes[1].Type);
        }

        [TestMethod]
        public void IncludeSubmitterImageInput()
        {
            // ARRANGE
            CQ document = "<form><input type=image name=a></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();
            IHTMLInputElement submitter = document["input"].OfType<IHTMLInputElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(submitter).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(2, nameValueTypes.Length);

            Assert.AreEqual("a.x", nameValueTypes[0].Name);
            Assert.AreEqual("0", nameValueTypes[0].Value);
            Assert.AreEqual("image", nameValueTypes[0].Type);

            Assert.AreEqual("a.y", nameValueTypes[1].Name);
            Assert.AreEqual("0", nameValueTypes[1].Value);
            Assert.AreEqual("image", nameValueTypes[1].Type);
        }

        [TestMethod]
        public void IncludeSubmitterButton()
        {
            // ARRANGE
            CQ document = "<form><button name=a value=foo></button></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();
            IHTMLButtonElement submitter = document["button"].OfType<IHTMLButtonElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(submitter).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);

            Assert.AreEqual("a", nameValueTypes[0].Name);
            Assert.AreEqual("foo", nameValueTypes[0].Value);
            Assert.AreEqual("submit", nameValueTypes[0].Type);
        }

        [TestMethod]
        public void NotSubmitter()
        {
            // ARRANGE
            CQ document = "<form><input name=a></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();
            IHTMLInputElement submitter = document["input"].OfType<IHTMLInputElement>().First();

            // ACT
            try
            {
                form.GetNameValueTypes(submitter);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                // ASSERT
                Assert.AreEqual("The provided submitter node is not a button or does not belong to the provided form.", e.Message);
            }
        }

        [TestMethod]
        public void NotAssociated()
        {
            // ARRANGE
            CQ document = "<form></form><input name=a>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();
            IHTMLInputElement submitter = document["input"].OfType<IHTMLInputElement>().First();

            // ACT
            try
            {
                form.GetNameValueTypes(submitter);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                // ASSERT
                Assert.AreEqual("The provided submitter node is not a button or does not belong to the provided form.", e.Message);
            }
        }
    }
}