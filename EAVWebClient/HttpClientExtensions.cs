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

using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;


namespace EAVModelClient
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PatchAsync<T>(this HttpClient client, string requestUri, T value, MediaTypeFormatter formatter)
        {
            // ConfigureAwait is used here to avoid deadlocks.
            // https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html
            return (await client.SendAsync(new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = new ObjectContent<T>(value, formatter) }).ConfigureAwait(false));
        }

        public static async Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            // ConfigureAwait is used here to avoid deadlocks.
            // https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html
            return (await PatchAsync<T>(client, requestUri, value, new JsonMediaTypeFormatter()).ConfigureAwait(false));
        }

        public static async Task<HttpResponseMessage> PatchAsXmlAsync<T>(this HttpClient client, string requestUri, T value)
        {
            // ConfigureAwait is used here to avoid deadlocks.
            // https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html
            return (await PatchAsync<T>(client, requestUri, value, new XmlMediaTypeFormatter()).ConfigureAwait(false));
        }
    }
}
