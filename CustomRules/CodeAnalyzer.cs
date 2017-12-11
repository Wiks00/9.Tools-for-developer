using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StyleCop;
using StyleCop.CSharp;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace CustomRules
{
    [SourceAnalyzer(typeof(CsParser))]
    public class CodeAnalyzer : SourceAnalyzer
    {
        public override void AnalyzeDocument(CodeDocument document)
        {
            CsDocument csharpDocument = (CsDocument) document;
            if (csharpDocument.RootElement != null && !csharpDocument.RootElement.Generated)
            {
                csharpDocument.WalkDocument(
                    new CodeWalkerElementVisitor<object>(this.VisitElement),
                    null,
                    null);
            }
        }

        //private bool VisitExpression(Expression expression, Expression parentexpression, Statement parentstatement, CsElement parentelement, object context)
        //{
        //    return true;
        //}

        //private bool VisitStatement(Statement statement, Expression parentExpression, Statement parentStatement,
        //    CsElement parentElement, object context)
        //{
        //    return true;
        //}

        readonly string idProperyName = "Id";
        readonly string nameProperyName = "Name";

        private bool VisitElement(CsElement element, CsElement parentElement, object context)
        {
            if (element.ElementType != ElementType.Class)
            {
                return true;
            }

            var elementClass = (Class) element;

            Type elementType = elementClass.GetType();

            if (elementClass.FullyQualifiedName.Replace("." + elementClass.Declaration.Name, string.Empty).EndsWith(".Entities"))
            {
                if (elementClass.ActualAccess != AccessModifierType.Public)
                {
                    AddViolation(element, "PublicEntityClass");
                }

                if (elementClass.Attributes.FirstOrDefault(attr => attr.Text == "[DataContract]") == null)
                {
                    AddViolation(element, "EntityClassAttribute", nameof(DataContractAttribute));
                }

                if (elementClass.ChildElements.Count(childElement =>
                        childElement.ElementType == ElementType.Property &&
                        childElement.AccessModifier == AccessModifierType.Public &&
                        ((childElement as Property).Declaration.Name == idProperyName || (childElement as Property).Declaration.Name == nameProperyName)) != 2)
                {
                    AddViolation(element, "EntityClassMandatoryProperties", nameProperyName, idProperyName);
                }

                return true;
            }

            if (elementClass.BaseClass == "Controller")
            {
                if (!elementClass.Declaration.Name.EndsWith("Controller"))
                {
                    AddViolation(element, "ControllerClassNaming", typeof(Controller));
                }

                if (elementClass.Attributes.FirstOrDefault(attr => attr.Text == "[Authorize]") == null)
                {
                    AddViolation(element, "ControllerClassAttribute", nameof(AuthorizeAttribute));
                }
            }

            return true;
        }
    }
}
