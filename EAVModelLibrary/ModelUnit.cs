// MicroEAV
//
// Copyright(C) 2017  Tim Newell

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.

using System;
using System.Runtime.Serialization;


namespace EAVModelLibrary
{
    [DataContract(IsReference = true)]
    public class ModelUnit : ModelObject, EAV.Model.IModelUnit
    {
        public ModelUnit()
        {
        }

        [DataMember(Name = "UnitID")]
        protected int? unitID;
        [IgnoreDataMember]
        public int? UnitID
        {
            get
            {
                return (unitID);
            }
            set
            {
                if (ObjectState == EAV.Model.ObjectState.New || (ObjectState == EAV.Model.ObjectState.Unmodified && unitID == null))
                {
                    unitID = value;
                }
                else
                {
                    throw (new InvalidOperationException("This property has already been set."));
                }
            }
        }

        [DataMember(Name = "SingularName")]
        protected string singularName;
        [IgnoreDataMember]
        public string SingularName
        {
            get
            {
                return (singularName);
            }
            set
            {
                if (singularName != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'SingularName' may not be modified when object in 'Deleted' state."));

                    singularName = value;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "SingularAbbreviation")]
        protected string singularAbbreviation;
        [IgnoreDataMember]
        public string SingularAbbreviation
        {
            get
            {
                return (singularAbbreviation);
            }
            set
            {
                if (singularAbbreviation != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'SingularAbbreviation' may not be modified when object in 'Deleted' state."));

                    singularAbbreviation = value;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "PluralName")]
        protected string pluralName;
        [IgnoreDataMember]
        public string PluralName
        {
            get
            {
                return (pluralName);
            }
            set
            {
                if (pluralName != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'PluralName' may not be modified when object in 'Deleted' state."));

                    pluralName = value;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "PluralAbbreviation")]
        protected string pluralAbbreviation;
        [IgnoreDataMember]
        public string PluralAbbreviation
        {
            get
            {
                return (pluralAbbreviation);
            }
            set
            {
                if (pluralAbbreviation != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'PluralAbbreviation' may not be modified when object in 'Deleted' state."));

                    pluralAbbreviation = value;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "Symbol")]
        protected string symbol;
        [IgnoreDataMember]
        public string Symbol
        {
            get
            {
                return (symbol);
            }
            set
            {
                if (symbol != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Symbol' may not be modified when object in 'Deleted' state."));

                    symbol = value;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "DisplayText")]
        protected string displayText;
        [IgnoreDataMember]
        public string DisplayText
        {
            get
            {
                return (displayText);
            }
            set
            {
                if (displayText != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'DisplayText' may not be modified when object in 'Deleted' state."));

                    displayText = value;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "Curated")]
        protected bool curated;
        [IgnoreDataMember]
        public bool Curated
        {
            get
            {
                return (curated);
            }
            set
            {
                if (curated != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Curated' may not be modified when object in 'Deleted' state."));

                    curated = value;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }

        public override void MarkUnmodified()
        {
            if (ObjectState == EAV.Model.ObjectState.Deleted)
                throw (new InvalidOperationException("Operation failed. Object in 'Deleted' state may not be marked unmodified."));

            if (UnitID == null)
                throw (new InvalidOperationException("Operation failed. Object with null 'UnitID' property may not be marked unmodified."));

            ObjectState = EAV.Model.ObjectState.Unmodified;
        }

        public override void MarkDeleted()
        {
            if (ObjectState == EAV.Model.ObjectState.New)
                throw (new InvalidOperationException("Operation failed. Object in 'New' state may not be marked deleted."));

            if (ObjectState != EAV.Model.ObjectState.Deleted)
            {
                ObjectState = EAV.Model.ObjectState.Deleted;
            }
        }
    }
}
