using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsQuery.FormFields.Test
{
    [TestClass]
    public class OrderTests
    {
        [TestMethod]
        public void AllowsEmptyForm()
        {
            // ARRANGE
            CQ document = "<form></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(0, nameValueTypes.Length);
        }

        [TestMethod]
        public void IncludeInCorrectOrder()
        {
            // ARRANGE
            CQ document = "<form><input name=a><p><input name=b><input name=c><span><input name=d></span></p><input name=e></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(5, nameValueTypes.Length);
            Assert.AreEqual("a", nameValueTypes[0].Name);
            Assert.AreEqual("b", nameValueTypes[1].Name);
            Assert.AreEqual("c", nameValueTypes[2].Name);
            Assert.AreEqual("d", nameValueTypes[3].Name);
            Assert.AreEqual("e", nameValueTypes[4].Name);
        }

        [TestMethod]
        public void IncludeInCorrectOrderWithFormReassociated()
        {
            // ARRANGE
            CQ document = "<input name=a form=form1><form id=form1><input name=b></form><p><input name=c form=form1><span><input name=d form=form1></span></p><input name=e form=form1>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            NameValueType[] nameValueTypes = form.GetNameValueTypes(false).ToArray();

            // ASSERT
            Assert.IsNotNull(nameValueTypes);
            Assert.AreEqual(5, nameValueTypes.Length);
            Assert.AreEqual("a", nameValueTypes[0].Name);
            Assert.AreEqual("b", nameValueTypes[1].Name);
            Assert.AreEqual("c", nameValueTypes[2].Name);
            Assert.AreEqual("d", nameValueTypes[3].Name);
            Assert.AreEqual("e", nameValueTypes[4].Name);
        }
    }
}