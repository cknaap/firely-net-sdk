﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Hl7.Fhir.FhirPath;
using Hl7.Fhir.Specification.Navigation.FhirPath;
using Hl7.Fhir.Specification.Source;
using Hl7.Fhir.Model;
using Hl7.Fhir.Specification.Navigation;
using System.Linq;

namespace Hl7.Fhir.Specification.Tests
{
    [TestClass]

    public class StructureDefinitionWalkerTests
    {
        public static IResourceResolver CreateTestResolver()
        {
            return new CachedResolver(
                new SnapshotSource(
                new MultiResolver(
                    new DirectorySource(@"TestData\validation"),
                    new ZipSource("specification.zip"))));
        }

        [ClassInitialize]
        public static void SetupSource(TestContext t)
        {
            _source = CreateTestResolver();
        }

        private static IResourceResolver _source = null;

        [TestMethod]
        public void WalkIntoTypeMembers()
        {
            var nav = ElementDefinitionNavigator.ForSnapshot(_source.FindStructureDefinitionForCoreType(FHIRDefinedType.Observation));
            var walker = new StructureDefinitionSchemaWalker(nav, _source);

            // A member that is of a complex type.
            var elem = walker.Child("method");
            Assert.AreEqual("Observation.method", elem.Current.Path);

            // Move into the complex type
            elem = elem.Child("coding");
            Assert.AreEqual("CodeableConcept.coding", elem.Current.Path);

            // A primivite type
            elem = walker.Child("status");
            Assert.AreEqual("Observation.status", elem.Current.Path);

            // Try move to the special value member
            Assert.ThrowsException<StructureDefinitionSchemaWalkerException>(() => elem.Child("value"), "Primitives should not have a 'value' member");
            // STU3: Assert.AreEqual("code.extension", elem.Child("extension").Current.Path);
            Assert.AreEqual("string.extension", elem.Child("extension").Current.Path);

            // Move into a component
            elem = walker.Child("component");
            Assert.AreEqual("Observation.component", elem.Current.Path);
            Assert.IsTrue(elem.Current.Current.IsBackboneElement());

            // ...and deeper into .referenceRange
            elem = elem.Child("referenceRange");
            Assert.AreEqual("Observation.component.referenceRange", elem.Current.Path);
            elem = elem.Child("low");
            Assert.AreEqual("Observation.referenceRange.low", elem.Current.Path);

            // normal navigation into a Reference
            elem = walker.Child("performer");
            Assert.AreEqual("Observation.performer", elem.Current.Path);
            elem = elem.Child("display");
            Assert.AreEqual("Reference.display", elem.Current.Path);

            // should not walk into value[x] when unconstrained to a single type
            elem = walker.Child("value");
            Assert.ThrowsException<StructureDefinitionSchemaWalkerException>(() => elem.Child("system"));  // i.e. a Quantity

            // can't walk into an unknown child
            Assert.ThrowsException<StructureDefinitionSchemaWalkerException>(() => walker.Child("ewout"));
        }

        [TestMethod]
        public void WalkIntoChoice()            
        {
            var nav = ElementDefinitionNavigator.ForSnapshot(_source.FindStructureDefinitionForCoreType(FHIRDefinedType.Observation));
            var walker = new StructureDefinitionSchemaWalker(nav, _source);

            // If you filter on the type of a non-choice member, you'll arrive at that type.
            var elem = walker.Child("method");
            var typed = elem.OfType("CodeableConcept");
            Assert.AreEqual(1, typed.Count());
            Assert.AreEqual("CodeableConcept", typed.Single().Current.Path);

            // Now, walk across a choice type, disambiguating the choice.
            elem = walker.Child("value").OfType("Quantity").Child("system").Single();
            Assert.AreEqual("Quantity.system", elem.Current.Path);
        }

        [TestMethod]
        public void WalkAcrossReference()
        {
            var nav = ElementDefinitionNavigator.ForSnapshot(_source.FindStructureDefinitionForCoreType(FHIRDefinedType.Observation));
            var walker = new StructureDefinitionSchemaWalker(nav, _source);

            var elem = walker.Child("performer").Resolve().OfType("Practitioner").Child("name").Single();
            Assert.AreEqual("Practitioner.name", elem.Current.Path);
        }
    }
}


   