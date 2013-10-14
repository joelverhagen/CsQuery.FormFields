using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsQuery.FormContent.Test
{
    [TestClass]
    public class FormAssocationTests
    {
        [TestMethod]
        public void ExcludeWhenNonSubmittableElement()
        {
            // ARRANGE
            CQ document = "<form><input name=a><img><a></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);
            Assert.AreEqual("a", nameValueTypes[0].Name);
        }

        [TestMethod]
        public void ExcludeWhenOutsideOfForm()
        {
            // ARRANGE
            CQ document = "<input name=a><form><input name=b></form><input name=c>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);
            Assert.AreEqual("b", nameValueTypes[0].Name);
        }

        [TestMethod]
        public void ExcludeWhenDifferentForm()
        {
            // ARRANGE
            CQ document = "<form id=form1><input name=a><input name=b form=form2></form><form id=form2></form>";
            IHTMLFormElement form = document["#form1"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);
            Assert.AreEqual("a", nameValueTypes[0].Name);
        }

        [TestMethod]
        public void IncludeWhenFormReassociated()
        {
            // ARRANGE
            CQ document = "<form id=form1></form><form id=form2><input name=a form=form1></form>";
            IHTMLFormElement form = document["#form1"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);
            Assert.AreEqual("a", nameValueTypes[0].Name);
        }

        [TestMethod]
        public void IncludeWhenFormReassociatedAndInside()
        {
            // ARRANGE
            CQ document = "<form id=form1><input name=a form=form1></form>";
            IHTMLFormElement form = document["#form1"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);
            Assert.AreEqual("a", nameValueTypes[0].Name);
        }

        [TestMethod]
        public void IncludeWhenInsideFieldset()
        {
            // ARRANGE
            CQ document = "<form><fieldset><input name=a></fieldset></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);
            Assert.AreEqual("a", nameValueTypes[0].Name);
        }

        [TestMethod]
        public void IncludeWhenInsideDisabledFieldsetLegend()
        {
            // ARRANGE
            CQ document = "<form><fieldset disabled><legend><input name=a></legend><input name=b></fieldset></form>";
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