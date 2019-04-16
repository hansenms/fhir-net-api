using Hl7.Fhir.ElementModel;
using Hl7.Fhir.Specification;
using Hl7.Fhir.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hl7.Fhir.Serialization.Tests
{

    public class OutOfOrderNode : ISourceNode, IExceptionSource, IAnnotated
    {
        readonly ISourceNode _xmlPatientNode;
        private readonly SourceNode _idSourceNode;

        public OutOfOrderNode()
        {
            var xml = @"<Patient xmlns=""http://hl7.org/fhir""><name><family value=""Test"" /></name></Patient>";
            _xmlPatientNode = FhirXmlNode.Parse(xml, new FhirXmlParsingSettings { PermissiveParsing = false });
            _idSourceNode = SourceNode.Valued("id", "123");

            ExceptionHandler = (_xmlPatientNode as IExceptionSource)?.ExceptionHandler;
        }

        public string Name => _xmlPatientNode.Name;

        public string Text => _xmlPatientNode.Text;

        public string Location => _xmlPatientNode.Location;

        public ExceptionNotificationHandler ExceptionHandler { get; set; }

        public IEnumerable<object> Annotations(Type type)
        {
            return (_xmlPatientNode as IAnnotated)?.Annotations(type);
        }

        /// <summary>
        /// 'id' is returned after 'name'. That is invalid if it were all xml, but 'id' is a SourceNode, not xml.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<ISourceNode> Children(string name = null)
        {
            if (name == "name" || name is null)
                yield return _xmlPatientNode.Children("name").First();
            if (name == "id" || name is null)
                yield return _idSourceNode;
        }
    }

    [TestClass]
    public class HybridTreeParsingTests
    {
        [TestMethod]
        public void NonXmlNodeNeedsNotBeInOrder()
        {
            var oooNode = new OutOfOrderNode();
            var nodeErrors = oooNode.VisitAndCatch();
            Assert.AreEqual(0, nodeErrors.Count);
        }

        [TestMethod]
        public void NonXmlElementNeedsNotBeInOrder()
        {
            var oooNode = new OutOfOrderNode();
            var oooElt = oooNode.ToTypedElement(new PocoStructureDefinitionSummaryProvider(), "Patient");
            var eltErrors = oooElt.VisitAndCatch();
            Assert.AreEqual(0, eltErrors.Count);
        }
    }
}