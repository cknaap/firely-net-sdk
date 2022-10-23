using FluentAssertions;
using Hl7.FhirPath.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hl7.FhirPath.R4.Tests.PocoTests
{
    [TestClass]
    public class FhirPathParserTests
    {
        [TestMethod]
        public void ParseFhirPath()
        {
            var parser = new FhirPathCompiler();
            var expr = parser.Parse("Patient.telecom.where(system='phone')"); // ("Patient.name.family");
            var dump = expr.Dump();
            expr.Should().NotBeNull();
        }
    }

    public class FpToPocoFunctionVisitor : ExpressionVisitor<StringBuilder>
    {
        private readonly StringBuilder _result = new StringBuilder();

        public override StringBuilder VisitConstant(ConstantExpression expression, SymbolTable scope) => throw new NotImplementedException();
        public override StringBuilder VisitFunctionCall(FunctionCallExpression expression, SymbolTable scope) => throw new NotImplementedException();
        public override StringBuilder VisitNewNodeListInit(NewNodeListInitExpression expression, SymbolTable scope) => throw new NotImplementedException();
        public override StringBuilder VisitVariableRef(VariableRefExpression expression, SymbolTable scope) => throw new NotImplementedException();
    }
}
