using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsQuery.FormContent.Test
{
    [TestClass]
    public class HttpContentTests
    {
        [TestMethod]
        public void NoImplicitSubmitter()
        {
            // ARRANGE
            CQ document = "<form><input name=a value=foo></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            HttpContent httpContent = form.GetHttpContent(false);

            // ASSERT
            Assert.IsNotNull(httpContent);
            
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(httpContent.ReadAsStringAsync().Result);
            
            Assert.AreEqual(1, nameValueCollection.Count);
            Assert.IsTrue(nameValueCollection.AllKeys.Contains("a"));
            Assert.AreEqual("foo", nameValueCollection["a"]);
        }

        [TestMethod]
        public void ExplicitSubmitter()
        {
            // ARRANGE
            CQ document = "<form><input type=submit name=a value=foo></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();
            IHTMLInputElement submitter = document["input"].OfType<IHTMLInputElement>().First();

            // ACT
            HttpContent httpContent = form.GetHttpContent(submitter);

            // ASSERT
            Assert.IsNotNull(httpContent);

            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(httpContent.ReadAsStringAsync().Result);

            Assert.AreEqual(1, nameValueCollection.Count);
            Assert.IsTrue(nameValueCollection.AllKeys.Contains("a"));
            Assert.AreEqual("foo", nameValueCollection["a"]);
        }

        [TestMethod]
        public void ImplicitSubmitter()
        {
            // ARRANGE
            CQ document = "<form><input type=submit name=a value=foo></form>";
            IHTMLFormElement form = document["form"].OfType<IHTMLFormElement>().First();

            // ACT
            HttpContent httpContent = form.GetHttpContent(true);

            // ASSERT
            Assert.IsNotNull(httpContent);

            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(httpContent.ReadAsStringAsync().Result);

            Assert.AreEqual(1, nameValueCollection.Count);
            Assert.IsTrue(nameValueCollection.AllKeys.Contains("a"));
            Assert.AreEqual("foo", nameValueCollection["a"]);
        }
    }
}
