﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;


namespace Translation
{
    class WebApi
    {
        public class WebReader
        {
            protected string globalHost;
            protected CookieContainer globalCookie;

            public static class WebMethods
            {
                public static String OPTIONS = "OPTIONS";
                public static String GET = "GET";
                public static String HEAD = "HEAD";
                public static String POST = "POST";
                public static String PUT = "PUT";
                public static String PATCH = "PATCH";
                public static String DELETE = "DELETE";
                public static String TRACE = "TRACE";
                public static String CONNECT = "CONNECT";
            }

            ILog _Logger;

            public WebReader(string host, ILog logger)
            {
                _Logger = logger;
                globalCookie = new CookieContainer();
                globalHost = host;
            }

            public WebReader(CookieContainer cookieIn, string host, ILog logger)
            {
                _Logger = logger;
                globalCookie = cookieIn;
                globalHost = host;
            }

            public virtual string GetWebData(string url, string method, string dataIn)
            {
                string content = String.Empty;

                try
                {
                    WebRequest localRequest = WebRequest.Create(new Uri(url));
                    localRequest.Method = method;

                    ((HttpWebRequest)localRequest).UserAgent = "User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
                    ((HttpWebRequest)localRequest).Accept = "text/html, application/xhtml+xml, */*";
                    ((HttpWebRequest)localRequest).Host = globalHost;
                    ((HttpWebRequest)localRequest).ContentLength = dataIn.Length;
                    ((HttpWebRequest)localRequest).CookieContainer = globalCookie;
                    localRequest.ContentType = "application/x-www-form-urlencoded";

                    using (Stream dataStream = localRequest.GetRequestStream())
                    {
                        dataStream.Write(Encoding.Default.GetBytes(dataIn), 0, dataIn.Length);
                        dataStream.Close();

                        using (WebResponse localResponse = localRequest.GetResponse())
                        {
                            using (Stream ReceiveStream = localResponse.GetResponseStream())
                            {
                                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                                using (StreamReader readStream = new StreamReader(ReceiveStream, encode))
                                {
                                    content = readStream.ReadToEnd();

                                    readStream.Close();
                                }

                                ReceiveStream.Close();

                            }

                            localResponse.Close();
                        }
                    }

                }
                catch (Exception e)
                {
                    string logMsg = method + Environment.NewLine + url + Environment.NewLine + e.ToString();
                    _Logger?.WriteLog(logMsg);
                }

                return content;
            }

            public string GetWebData(string url, string method)
            {
                string content = string.Empty;
                try
                {
                    WebRequest localRequest = WebRequest.Create(new Uri(url));
                    localRequest.Method = method;

                    ((HttpWebRequest)localRequest).UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
                    ((HttpWebRequest)localRequest).Accept = "text/html, application/xhtml+xml, */*";
                    ((HttpWebRequest)localRequest).Host = globalHost;

                    ((HttpWebRequest)localRequest).CookieContainer = globalCookie;
                    localRequest.ContentType = "application/x-www-form-urlencoded";

                    using (WebResponse localResponse = localRequest.GetResponse())
                    {
                        using (Stream ReceiveStream = localResponse.GetResponseStream())
                        {
                            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

                            using (StreamReader readStream = new StreamReader(ReceiveStream, encode))
                            {
                                content = readStream.ReadToEnd();

                                readStream.Close();
                            }

                            ReceiveStream.Close();
                        }

                        localResponse.Close();
                    }
                }
                catch (Exception e)
                {
                    string logMsg = method + Environment.NewLine + url + Environment.NewLine + e.ToString();
                    _Logger?.WriteLog(logMsg);
                }

                return content;
            }

            public string GetWebDataAndSetCookie(string url, string method)
            {
                string content = string.Empty;
                HttpWebResponse WebResp;
                try
                {
                    WebRequest localRequest = WebRequest.Create(new Uri(url));
                    localRequest.Method = method;

                    ((HttpWebRequest)localRequest).UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
                    ((HttpWebRequest)localRequest).Accept = "text/html, application/xhtml+xml, */*";
                    ((HttpWebRequest)localRequest).Host = globalHost;

                    ((HttpWebRequest)localRequest).CookieContainer = globalCookie;
                    localRequest.ContentType = "application/x-www-form-urlencoded";

                    using (WebResponse localResponse = localRequest.GetResponse())
                    {
                        using (Stream ReceiveStream = localResponse.GetResponseStream())
                        {
                            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                            using (StreamReader readStream = new StreamReader(ReceiveStream, encode))
                            {

                                content = readStream.ReadToEnd();

                                WebResp = (HttpWebResponse)localResponse;

                                foreach (Cookie cookie in WebResp.Cookies)
                                    globalCookie.Add(cookie);

                                readStream.Close();
                            }

                            ReceiveStream.Close();
                        }

                        localResponse.Close();
                    }
                    

                }
                catch (Exception e)
                {
                    string logMsg = method + Environment.NewLine + url + Environment.NewLine + e.ToString();
                    _Logger?.WriteLog(logMsg);
                }

                return content;
            }
        }

        public class DeepLWebReader
        {
            protected CookieContainer globalCookie;

            public static class WebMethods
            {
                public static String OPTIONS = "OPTIONS";
                public static String GET = "GET";
                public static String HEAD = "HEAD";
                public static String POST = "POST";
                public static String PUT = "PUT";
                public static String PATCH = "PATCH";
                public static String DELETE = "DELETE";
                public static String TRACE = "TRACE";
                public static String CONNECT = "CONNECT";
            }

            ILog _Logger;

            public DeepLWebReader(ILog logger)
            {
                _Logger = logger;
                globalCookie = new CookieContainer();
            }

            public string GetWebData(string url, string method, string dataIn)
            {
                string content = String.Empty;

                try
                {
                    WebRequest localRequest = GetConnection(url, method);

                    ((HttpWebRequest)localRequest).ContentLength = dataIn.Length;

                    using (Stream dataStream = localRequest.GetRequestStream())
                    {
                        dataStream.Write(Encoding.Default.GetBytes(dataIn), 0, dataIn.Length);
                        dataStream.Close();
                    }

                    using (WebResponse localResponse = localRequest.GetResponse())
                    {
                        using (Stream ReceiveStream = localResponse.GetResponseStream())
                        {
                            try
                            {
                                var cook = ((HttpWebResponse)localResponse).Cookies;
                                globalCookie.Add(cook);

                                if (cook.Count > 0)
                                {
                                    object[] CookArr = new object[cook.Count];
                                    cook.CopyTo(CookArr, 0);
                                    CookieCollection cookieCollection = new CookieCollection();
                                    Cookie tmpCook = ((Cookie)CookArr[0]);
                                    Cookie newCook1 = new Cookie(tmpCook.Name, tmpCook.Value, tmpCook.Path, "www2.deepl.com");
                                    Cookie newCook2 = new Cookie(tmpCook.Name, tmpCook.Value, tmpCook.Path, "www.deepl.com");

                                    globalCookie.Add(tmpCook);
                                    globalCookie.Add(newCook1);
                                    globalCookie.Add(newCook2);
                                }
                            }
                            catch { }

                            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                            using (StreamReader readStream = new StreamReader(ReceiveStream, encode))
                            {
                                content = readStream.ReadToEnd();

                                readStream.Close();
                            }

                            ReceiveStream.Close();
                        }

                        localResponse.Close();

                    }                    
                    
                }
                catch (Exception e)
                {
                    string logMsg = method + Environment.NewLine + url + Environment.NewLine + e.ToString();
                    _Logger?.WriteLog(logMsg);
                }

                return content;
            }

            private WebRequest GetConnection(string url, string method)
            {
                var tmpUri = new Uri(url);
                WebRequest localRequest = WebRequest.Create(tmpUri);

                localRequest.Method = method; //*/

                ((HttpWebRequest)localRequest).Headers.Add("Origin", @"https://www.deepl.com");
                ((HttpWebRequest)localRequest).Referer = @"https://www.deepl.com/translator";

                ((HttpWebRequest)localRequest).UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.140 Safari/537.36 Edge/17.17134";
                ((HttpWebRequest)localRequest).Headers.Add("Cache-Control", "no-cache");

                localRequest.ContentType = "text/plain";

                ((HttpWebRequest)localRequest).Accept = @"*/*";
                ((HttpWebRequest)localRequest).Headers.Add("Accept-Language", "ru,en-US;q=0.7,en;q=0.3");
                ((HttpWebRequest)localRequest).AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None;

                ((HttpWebRequest)localRequest).Host = tmpUri.Host;
                ((HttpWebRequest)localRequest).CookieContainer = globalCookie;

                ((HttpWebRequest)localRequest).KeepAlive = true;


                return localRequest;
            }
        }

        public static string GetQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return String.Join("&", properties.ToArray());
        }
    }
}
