using Hl7.Fhir.ElementModel;
using Hl7.Fhir.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hl7.Fhir.Model
{
    public partial class Patient : ILookup<string, Base>, ITypedElement
    {
        #region ITypedElement
        string ITypedElement.Name => throw new NotImplementedException();

        string ITypedElement.InstanceType => throw new NotImplementedException();

        object ITypedElement.Value => throw new NotImplementedException();

        string ITypedElement.Location => throw new NotImplementedException();

        IElementDefinitionSummary ITypedElement.Definition => throw new NotImplementedException();

        IEnumerable<ITypedElement> ITypedElement.Children(string name) => throw new NotImplementedException();

        #endregion ITypedElement

        #region ILookup
        IEnumerable<Base> ILookup<string, Base>.this[string key] => throw new NotImplementedException();

        int ILookup<string, Base>.Count => throw new NotImplementedException();

        bool ILookup<string, Base>.Contains(string key) => throw new NotImplementedException();
        IEnumerator<IGrouping<string, Base>> IEnumerable<IGrouping<string, Base>>.GetEnumerator() => throw new NotImplementedException();

        private bool TryGetValues(string key, out IEnumerable<Base> values)
        {
            switch (key)
            {
                case "identifier":
                    values = Identifier;
                    return Identifier?.Any() == true;
                case "active":
                    values = new[] { ActiveElement };
                    return ActiveElement is not null;
                case "name":
                    values = Name;
                    return Name?.Any() == true;
                case "telecom":
                    values = Telecom;
                    return Telecom?.Any() == true;
                case "gender":
                    values = new[] { GenderElement };
                    return GenderElement is not null;
                case "birthDate":
                    values = new[] { BirthDateElement };
                    return BirthDateElement is not null;
                case "deceased":
                    values = new[] { Deceased };
                    return Deceased is not null;
                case "address":
                    values = Address;
                    return Address?.Any() == true;
                case "maritalStatus":
                    values = new[] { MaritalStatus };
                    return MaritalStatus is not null;
                case "multipleBirth":
                    values = new[] { MultipleBirth };
                    return MultipleBirth is not null;
                case "photo":
                    values = Photo;
                    return Photo?.Any() == true;
                case "contact":
                    values = Contact;
                    return Contact?.Any() == true;
                case "communication":
                    values = Communication;
                    return Communication?.Any() == true;
                case "generalPractitioner":
                    values = GeneralPractitioner;
                    return GeneralPractitioner?.Any() == true;
                case "managingOrganization":
                    values = new[] { ManagingOrganization };
                    return ManagingOrganization is not null;
                case "link":
                    values = Link;
                    return Link?.Any() == true;
                default:
                    throw new NotImplementedException("Not implemented in base classes yet");
                    //return base.TryGetValues(key, out values);
            }

            #endregion ILookup
        }
    }
}
