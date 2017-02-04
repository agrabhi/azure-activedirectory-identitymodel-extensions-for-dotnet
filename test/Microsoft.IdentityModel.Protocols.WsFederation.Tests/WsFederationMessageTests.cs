//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.IdentityModel.Tokens.Tests;
using Xunit;

namespace Microsoft.IdentityModel.Protocols.WsFederation.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public class WsFederationMessageTests
    {
        [Fact(DisplayName = "WsFederationMessageTests: Constructors")]
        public void Constructors()
        {
            WsFederationMessage wsFederationMessage = new WsFederationMessage();
            Assert.Equal(wsFederationMessage.IssuerAddress, string.Empty);

            wsFederationMessage = new WsFederationMessage { IssuerAddress = "http://www.got.jwt.com" };
            Assert.Equal(wsFederationMessage.IssuerAddress, "http://www.got.jwt.com");

            var expectedException = ExpectedException.ArgumentNullException("IDX10000:");
            try
            {
                wsFederationMessage = new WsFederationMessage((string)null);
                expectedException.ProcessNoException();
            }
            catch(Exception exception)
            {
                expectedException.ProcessException(exception);
            }
        }

        [Fact(DisplayName = "WsFederationMessageTests: Defaults")]
        public void Defaults()
        {
            WsFederationMessage wsFederationMessage = new WsFederationMessage();

            Assert.Equal(wsFederationMessage.IssuerAddress, string.Empty);
            Assert.Null(wsFederationMessage.Wa);
            Assert.Null(wsFederationMessage.Wauth);
            Assert.Null(wsFederationMessage.Wct);
            Assert.Null(wsFederationMessage.Wctx);
            Assert.Null(wsFederationMessage.Wencoding);
            Assert.Null(wsFederationMessage.Wfed);
            Assert.Null(wsFederationMessage.Wfresh);
            Assert.Null(wsFederationMessage.Whr);
            Assert.Null(wsFederationMessage.Wp);
            Assert.Null(wsFederationMessage.Wpseudo);
            Assert.Null(wsFederationMessage.Wpseudoptr);
            Assert.Null(wsFederationMessage.Wreply);
            Assert.Null(wsFederationMessage.Wreq);
            Assert.Null(wsFederationMessage.Wreqptr);
            Assert.Null(wsFederationMessage.Wres);
            Assert.Null(wsFederationMessage.Wresult);
            Assert.Null(wsFederationMessage.Wresultptr);
            Assert.Null(wsFederationMessage.Wtrealm);
        }

        [Fact(DisplayName = "WsFederationMessageTests: GetSets")]
        public void GetSets()
        {
            var errors = new List<string>();
            WsFederationMessage wsFederationMessage = new WsFederationMessage();

            Type type = typeof(WsFederationParameterNames);
            FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static);
            foreach( FieldInfo fieldInfo in fields)
            {
                TestUtilities.GetSet(wsFederationMessage, fieldInfo.Name, null, new object[]{ fieldInfo.Name, null, fieldInfo.Name + fieldInfo.Name }, errors );
            }
        }

        [Fact(DisplayName = "WsFederationMessageTests: Publics")]
        public void Publics()
        {
            string issuerAdderss = @"http://www.gotjwt.com";
            string wreply = @"http://www.relyingparty.com";
            string wct = Guid.NewGuid().ToString();
            WsFederationMessage wsFederationMessage = new WsFederationMessage
            {
                IssuerAddress = issuerAdderss,
                Wreply = wreply,
                Wct = wct,
            };

            wsFederationMessage.SetParameter("bob", null);
            wsFederationMessage.Parameters.Add("bob", null);
            string uriString = wsFederationMessage.BuildRedirectUrl();
            Uri uri = new Uri(uriString);

            WsFederationMessage wsFederationMessageReturned = WsFederationMessage.FromQueryString(uri.Query);
            wsFederationMessageReturned.IssuerAddress = issuerAdderss;
            wsFederationMessageReturned.Parameters.Add("bob", null);
            Assert.Equal(wsFederationMessageReturned.IssuerAddress, issuerAdderss );
            Assert.Equal(wsFederationMessageReturned.Wreply, wreply);
            Assert.Equal(wsFederationMessageReturned.Wct, wct);

            wsFederationMessageReturned = WsFederationMessage.FromUri(uri);
            wsFederationMessageReturned.IssuerAddress = issuerAdderss;
            wsFederationMessageReturned.Parameters.Add("bob", null);
            Assert.Equal(wsFederationMessageReturned.IssuerAddress, issuerAdderss);
            Assert.Equal(wsFederationMessageReturned.Wreply, wreply);
            Assert.Equal(wsFederationMessageReturned.Wct, wct);
        }
    }
}
