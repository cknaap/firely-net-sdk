using BenchmarkDotNet.Attributes;
using Hl7.Fhir.ElementModel;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Hl7.Fhir.Utility;
using Hl7.FhirPath;
using System.IO;
using System.Text.Json;
using System.Linq;
using Hl7.Fhir.Specification;
using System;
using System.Collections.Generic;

namespace Firely.Sdk.Benchmarks
{
    [MemoryDiagnoser]
    public class FhirPathBenchmarks
    {
        internal Patient Patient;
        ITypedElement PatientNode;

        public ITypedElement PatientDictionaryNode { get; private set; }
        public ITypedElement PatientTypedElement { get; private set; }
        public ITypedElement CachedTypedElement { get; private set; }
        public Dictionary<string, Func<Base, IEnumerable<Base>>> Extractors { get; private set; }

        JsonSerializerOptions Options;
        public Dictionary<string, CompiledExpression> FhirPaths;

        public static void Debug()
        {
            var testClass = new FhirPathBenchmarks();
            testClass.BenchmarkSetup();
            //testClass.PocoElementNode();
            testClass.DictionaryElementNode();
            //testClass.PocoFunctions();
        }

        [GlobalSetup]
        public void BenchmarkSetup()
        {
            Options = new JsonSerializerOptions().ForFhir(typeof(Patient).Assembly);

            var filename = Path.Combine("TestData", "fp-test-patient.json");
            var data = File.ReadAllText(filename);
            Patient = JsonSerializer.Deserialize<Patient>(data, Options);

            var fpCompiler = new FhirPathCompiler();
            var fhirPathStatements = ModelInfo.SearchParameters
                .Where(sp => sp.Resource == "Patient")
                .Select(sp => sp.Expression);
            FhirPaths = fhirPathStatements.ToDictionary(st => st, st => fpCompiler.Compile(st));
            PatientNode = Patient.ToTypedElement();
            PatientDictionaryNode = Patient.ToDictionaryTypedElement();

            var sdsProvider = new PocoStructureDefinitionSummaryProvider();
            var jsonNode = FhirJsonNode.Parse(data);
            PatientTypedElement = jsonNode.ToTypedElement(sdsProvider);

            CachedTypedElement = PatientTypedElement.Cache();

            FhirPathFunctionsSetup();
        }

        private void FhirPathFunctionsSetup()
        {
            IEnumerable<Base> getResult(Base input) => input is object ? new[] { input } : new Base[] { };
            IEnumerable<Base> getResults(IEnumerable<Base> input) => input is null
                ? new Base[] { }
                : input.Where(elt => elt is not null).ToArray();

            Extractors = new Dictionary<string, Func<Base, IEnumerable<Base>>>()
            {
                {"Patient.active", b => getResult((b as Patient)?.ActiveElement) },
                {"Patient.address", b => getResults((b as Patient)?.Address) },
                {"Patient.address.city", b => getResults((b as Patient)?.Address.Select(a => a.CityElement)) },
                {"Patient.address.country", b => getResults((b as Patient)?.Address.Select(a => a.CountryElement)) },
                {"Patient.address.postalCode", b => getResults((b as Patient)?.Address.Select(a => a.PostalCodeElement) ) },
                {"Patient.address.state", b => getResults((b as Patient)?.Address.Select(a => a.StateElement))  },
                {"Patient.address.use", b => getResults((b as Patient)?.Address.Select(a => a.UseElement)) },
                {"Patient.birthDate", b => getResult((b as Patient)?.BirthDateElement ) },
                {"(Patient.deceased as dateTime)", b => getResult((b as Patient)?.Deceased as FhirDateTime ) },
                {"Patient.deceased.exists() and Patient.deceased != false", b => getResult(
                    new FhirBoolean((b as Patient)?.Deceased is object && ((b as Patient)?.Deceased as FhirBoolean)?.Value != false) ) },
                {"Patient.telecom.where(system='email')", b => getResults((b as Patient)?.Telecom.Where(t => t.SystemElement?.Value == ContactPoint.ContactPointSystem.Email)) },
                {"Patient.name.family", b => getResults((b as Patient)?.Name.Select(n => n.FamilyElement)) },
                {"Patient.gender", b => getResult((b as Patient)?.GenderElement ) },
                {"Patient.generalPractitioner", b => getResults((b as Patient)?.GeneralPractitioner) },
                {"Patient.name.given", b => getResults((b as Patient)?.Name.SelectMany(n => n.GivenElement)) },
                {"Patient.identifier", b => getResults((b as Patient)?.Identifier) },
                {"Patient.communication.language", b => getResults((b as Patient)?.Communication.Select(c => c.Language)) },
                {"Patient.link.other", b => getResults((b as Patient)?.Link.Select(l => l.Other)) },
                {"Patient.name", b => getResults((b as Patient)?.Name) },
                {"Patient.managingOrganization", b => getResult((b as Patient)?.ManagingOrganization ) },
                {"Patient.telecom.where(system='phone')", b => getResults((b as Patient)?.Telecom.Where(t => t.SystemElement?.Value == ContactPoint.ContactPointSystem.Phone)) },
                {"Patient.telecom", b => getResults((b as Patient)?.Telecom) },
            };
        }

        [Benchmark]
        public void PocoElementNode()
        {
            foreach (var path in FhirPaths)
            {
                _ = path.Value.Invoke(PatientNode, EvaluationContext.CreateDefault()).ToList();
            }
        }
        [Benchmark]
        public void DictionaryElementNode()
        {
            //foreach (var path in FhirPaths)
            //{
            //    _ = path.Value.Invoke(PatientDictionaryNode, EvaluationContext.CreateDefault()).ToList();
            //}
            var path = FhirPaths.Where(kvp => kvp.Key.Contains("city")).First();
            _ = path.Value.Invoke(PatientDictionaryNode, EvaluationContext.CreateDefault()).ToList();
        }

        [Benchmark]
        public void TypedElement()
        {
            foreach (var path in FhirPaths)
            {
                _ = path.Value.Invoke(PatientTypedElement, EvaluationContext.CreateDefault()).ToList();
            }
        }

        [Benchmark]
        public void CachedElement()
        {
            foreach (var path in FhirPaths)
            {
                _ = path.Value.Invoke(CachedTypedElement, EvaluationContext.CreateDefault()).ToList();
            }
        }

        [Benchmark]
        public void PocoFunctions()
        {
            foreach (var path in Extractors)
            {
                var x = path.Value(Patient);
            }
        }
    }
}
