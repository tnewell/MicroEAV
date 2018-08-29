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

namespace EAV
{
    /// <summary>
    /// A Container represents a related group of data items intended to be collected/organized together.
    /// An example might be an address (street name, city, postal code, etc.).
    /// Containers can be organized in hierachical relationships.
    /// </summary>
    public class Container
    {
        public Container()
        {
        }

        public string Name { get; set; }
        public string DataName { get; set; }
        public string DisplayText { get; set; }
        public int Sequence { get; set; }
        public bool IsRepeating { get; set; }
    }
}
