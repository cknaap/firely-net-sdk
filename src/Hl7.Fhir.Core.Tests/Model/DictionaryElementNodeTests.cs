using FluentAssertions;
using Hl7.Fhir.ElementModel;
using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hl7.Fhir.ElementModel.Tests
{
    [TestClass]
    public class DictionaryElementNodeTests
    {
        [TestMethod]
        public void GetPatientChildren()
        {
            var patient = new Patient
            {
                Name = new List<Model.HumanName> {
                new Model.HumanName {
                    Given = new[] { "Fred", "Flippo" },
                    Family = "Flintstone"
                },
                new Model.HumanName {
                    Given = new[] { "Dave", "Derek" },
                    Family = "Dareall"
                }
            },
                BirthDate = "1654-03-23",
                Id = "patientFred"
            };
            var sut = patient.ToDictionaryTypedElement();
            sut.Children("name").Should().HaveCount(2);
            sut.Children().Should().HaveCount(4);
            sut.Children("birthDate").Should().HaveCount(1);
        }
    }
}
