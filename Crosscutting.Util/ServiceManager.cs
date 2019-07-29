using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Crosscutting.Util
{
    public class ServiceManager
    {
        public string TokenAccess { get; set; }
        public ServiceManager(string path)
        {
            _path = path;
        }

        private string _path;

        public HttpResponseMessage CallServiceAsync(IEnumerable<DSParameter> parameters, ref CookieCollection cookies, string content, string tokenAccess = "",
            RequestType method = RequestType.POST, RequestBodyType bodyType = RequestBodyType.FormData)
        {

            var listformData = new List<KeyValuePair<string, string>>();
            FormUrlEncodedContent formdata = null;

            var baseAddress = new Uri(_path);

            var parametersUrl = string.Empty;
            var counterParameterQuery = 0;
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {


                if (parameters != null && parameters.Count() > 0)
                {
                    foreach (DSParameter parameter in parameters)
                    {
                        switch (parameter.Type)
                        {
                            case "Cookie":
                                cookieContainer.Add(baseAddress,
                                    new Cookie
                                    {
                                        Name = parameter.Name,
                                        Value = parameter.Value,
                                        Domain = baseAddress.Host
                                    });

                                break;
                            case "Form":

                                listformData.Add(new KeyValuePair<string, string>(parameter.Name, parameter.Value));
                                //reqparm = SetFormParametro(reqparm, parameter);
                                ;
                                break;
                            case "ViewState":
                                //reqparm = ExtractViewState(crawlerDataSource.Url, parameter, reqparm);
                                break;
                            case "Header":
                                client.DefaultRequestHeaders.TryAddWithoutValidation(parameter.Name, parameter.Value);
                                break;
                            case "QueryString":
                                bool last;
                                counterParameterQuery++;
                                last = counterParameterQuery == parameters.Where(s => s.Type == "QueryString").Count();
                                if (!last)
                                {
                                    parametersUrl += parameter.Name + "=" + parameter.Value + "&";
                                }
                                else
                                {
                                    parametersUrl += parameter.Name + "=" + parameter.Value;
                                }




                                break;
                            default:
                                break;
                        }
                    }
                }
                //add Query String
                _path = _path + (!string.IsNullOrEmpty(parametersUrl) ? "?" : string.Empty) + parametersUrl;

                if (listformData != null && listformData.Any())
                {
                    formdata = new FormUrlEncodedContent(listformData.ToArray());
                }




                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                if (!string.IsNullOrEmpty(tokenAccess))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + tokenAccess);
                }

                HttpResponseMessage result;

                switch (method)
                {
                    case RequestType.GET:

                        result = client.GetAsync(_path).Result;
                        break;

                    case RequestType.POST:

                        switch (bodyType)
                        {
                            case RequestBodyType.Raw:
                                result = client.PostAsync(_path, new StringContent(content, Encoding.UTF8)).Result;
                                break;
                            case RequestBodyType.FormData:
                                result = client.PostAsync(_path, formdata).Result;
                                break;

                            default:
                                result = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                break;
                        }
                        break;

                    case RequestType.PATCH:
                        var patchMethod = new HttpMethod("PATCH");
                        HttpContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");
                        var request = new HttpRequestMessage(patchMethod, new Uri(_path))
                        {
                            Content = httpContent
                        };
                        result = client.SendAsync(request).Result;
                        break;
                    case RequestType.PUT:
                        result =

                                client.PutAsync(_path,
                                    new StringContent(content, Encoding.UTF8)).Result;
                        break;
                    case RequestType.DELETE:
                        var deleteMethod = new HttpMethod("DELETE");
                        HttpContent contdelete = new StringContent(content, Encoding.UTF8, "application/json");
                        var requestdelete = new HttpRequestMessage(deleteMethod, new Uri(_path))
                        {
                            Content = contdelete
                        };
                        result = client.SendAsync(requestdelete).Result;
                        break;
                    default:
                        result = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        break;
                }

                cookies = cookieContainer.GetCookies(baseAddress);


                return result;
            }
        }

    }
}
