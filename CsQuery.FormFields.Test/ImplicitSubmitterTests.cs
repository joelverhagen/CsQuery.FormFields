using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsQuery.FormFields.Test
{
    [TestClass]
    public class ImplicitSubmitterTests
    {
        [TestMethod]
        public void FirstInsideForm()
        {
            // ARRANGE
            CQ document = "<form><input type=submit value=foo name=a><input type=submit value=bar name=b></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(true).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);

            Assert.AreEqual("a", nameValueTypes[0].Name);
            Assert.AreEqual("foo", nameValueTypes[0].Value);
            Assert.AreEqual("submit", nameValueTypes[0].Type);
        }

        [TestMethod]
        public void FirstBeforeForm()
        {
            // ARRANGE
            CQ document = "<input type=submit value=foo name=a form=form1><form id=form1><input type=submit value=bar name=b></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(true).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(1, nameValueTypes.Length);

            Assert.AreEqual("a", nameValueTypes[0].Name);
            Assert.AreEqual("foo", nameValueTypes[0].Value);
            Assert.AreEqual("submit", nameValueTypes[0].Type);
        }

        [TestMethod]
        public void ExcludeDisabled()
        {
            // ARRANGE
            CQ document = "<form><input type=submit value=foo name=a disabled><input type=submit value=bar name=b></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(true).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(0, nameValueTypes.Length);
        }

        [TestMethod]
        public void ExcludeNoNameInput()
        {
            // ARRANGE
            CQ document = "<form><input type=submit value=foo><input type=submit value=bar name=b></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(true).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(0, nameValueTypes.Length);
        }


        [TestMethod]
        public void ExcludeNoNameButton()
        {
            // ARRANGE
            CQ document = "<form><button value=foo></button><input type=submit value=bar name=b></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(true).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(0, nameValueTypes.Length);
        }

        [TestMethod]
        public void SuccessfulNull()
        {
            // ARRANGE
            CQ document = "<form></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(true).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(0, nameValueTypes.Length);
        }
    }
}