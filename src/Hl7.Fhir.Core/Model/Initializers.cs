﻿/*
  Copyright (c) 2011-2012, HL7, Inc
  All rights reserved.
  
  Redistribution and use in source and binary forms, with or without modification, 
  are permitted provided that the following conditions are met:
  
   * Redistributions of source code must retain the above copyright notice, this 
     list of conditions and the following disclaimer.
   * Redistributions in binary form must reproduce the above copyright notice, 
     this list of conditions and the following disclaimer in the documentation 
     and/or other materials provided with the distribution.
   * Neither the name of HL7 nor the names of its contributors may be used to 
     endorse or promote products derived from this software without specific 
     prior written permission.
  
  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
  ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
  IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
  INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
  NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
  WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
  POSSIBILITY OF SUCH DAMAGE.
  

*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hl7.Fhir.Model
{
    public partial class HumanName
    {
        public static HumanName ForFamily(string family)
        {
            var result = new HumanName()
            {
                Family = family
            };
            return result;
        }

        public HumanName WithGiven(string given)
        {
            if (this.GivenElement == null) this.GivenElement = new List<FhirString>();
            this.GivenElement.Add(new FhirString(given));

            return this;
        }

        public HumanName WithPrefix(string prefix)
        {
            if (this.PrefixElement == null) this.PrefixElement = new List<FhirString>();
            this.PrefixElement.Add(new FhirString(prefix));

            return this;
        }

        public HumanName WithSuffix(string prefix)
        {
            if (this.SuffixElement == null) this.SuffixElement = new List<FhirString>();
            this.SuffixElement.Add(new FhirString(prefix));

            return this;
        }

        public HumanName AndFamily(string family)
        {
            this.Family = family;

            return this;
        }

        public List<HumanName> AsList()
        {
            return new List<HumanName>() { this };
        }
    }

    [System.Diagnostics.DebuggerDisplay(@"\{{DebuggerDisplay(),nq}}")] // http://blogs.msdn.com/b/jaredpar/archive/2011/03/18/debuggerdisplay-attribute-best-practices.aspx
    public partial class Address
    {
        private string DebuggerDisplay()
        {
            if (!String.IsNullOrEmpty(Text))
                return String.Format("Text=\"{0}\"", Text);
            return string.Join(", ", string.Join(", ", Line.ToArray()),
                City, State, PostalCode, Country)
                + (Use.HasValue ? " Use=\"" + Use.Value + "\"" : "")
                + (Type.HasValue ? " Type=\"" + Type.Value + "\"" : "");
        }
    }
}
