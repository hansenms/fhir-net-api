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
using Hl7.FhirPath;

namespace Hl7.Fhir.Model
{
    public partial class Instant : INullableValue<DateTimeOffset>
    {
        public static Instant FromLocalDateTime(int year, int month, int day,
                            int hour, int min, int sec, int millis = 0)
        {
            return new Instant( new DateTimeOffset(year, month, day, hour, min, sec, millis,
                            DateTimeOffset.Now.Offset) );
        }


        public static Instant FromDateTimeUtc(int year, int month, int day,
                                            int hour, int min, int sec, int millis=0)
        {
            return new Instant(new DateTimeOffset(year, month, day, hour, min, sec, millis,
                                   TimeSpan.Zero));
        }

        public static Instant Now()
        {
            return new Instant(DateTimeOffset.Now);
        }

        public Primitives.PartialDateTime? ToPartialDateTime() => Value != null ? (Primitives.PartialDateTime?)Primitives.PartialDateTime.FromDateTimeOffset(Value.Value) : null;


        public static bool IsValidValue(string value)
        {
            if (!DateTimeOffset.TryParse(value, out DateTimeOffset _))
                return false;

            //TODO: Implement useful validation functionality
            return true;
        }

        public static bool operator >(Instant a, Instant b)
        {
            var aValue = a?.Value;
            var bValue = b?.Value;

            if (aValue == null) return bValue == null;
            if (bValue == null) return false;

            return Primitives.PartialDateTime.FromDateTimeOffset(a.Value.Value) > Primitives.PartialDateTime.FromDateTimeOffset(b.Value.Value);
        }

        public static bool operator >=(Instant a, Instant b)
        {
            var aValue = a?.Value;
            var bValue = b?.Value;

            if (aValue == null) return bValue == null;
            if (bValue == null) return false;

            return Primitives.PartialDateTime.FromDateTimeOffset(a.Value.Value) >= Primitives.PartialDateTime.FromDateTimeOffset(b.Value.Value);
        }

        public static bool operator <(Instant a, Instant b)
        {
            var aValue = a?.Value;
            var bValue = b?.Value;

            if (aValue == null) return bValue == null;
            if (bValue == null) return false;

            return Primitives.PartialDateTime.FromDateTimeOffset(a.Value.Value) < Primitives.PartialDateTime.FromDateTimeOffset(b.Value.Value);
        }

        public static bool operator <=(Instant a, Instant b)
        {
            var aValue = a?.Value;
            var bValue = b?.Value;

            if (aValue == null) return bValue == null;
            if (bValue == null) return false;

            return Primitives.PartialDateTime.FromDateTimeOffset(a.Value.Value) <= Primitives.PartialDateTime.FromDateTimeOffset(b.Value.Value);
        }

        /// <summary>
        /// If you use this operator, you should check that a modifierExtension isn't changing the meaning
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Instant a, Instant b)
        {
            return Object.Equals(a, b);
        }

        /// <summary>
        /// If you use this operator, you should check that a modifierExtension isn't changing the meaning
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Instant a, Instant b)
        {
            return !Object.Equals(a, b);
        }

        public override bool Equals(object obj)
        {
            if (obj is Instant other)
            {
                var otherValue = other?.Value;

                if (Value == null) return otherValue == null;
                if (otherValue == null) return false;

                if (this.Value == otherValue) return true; // Default reference/string comparison works in most cases

                var left = Primitives.PartialDateTime.FromDateTimeOffset(Value.Value);
                var right = Primitives.PartialDateTime.FromDateTimeOffset(otherValue.Value);

                return left == right;
            }
            else
                return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
