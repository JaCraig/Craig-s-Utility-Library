/*
Copyright (c) 2010 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

#region Usings
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Xml;
using Utilities.FileFormats;
#endregion

namespace Utilities.Exchange
{
    /// <summary>
    /// Utility class in charge of exchange server functions
    /// </summary>
    public static class Exchange
    {
        #region Static Public Functions
        /// <summary>
        /// Checks if a person/group of people are free for a specified time
        /// </summary>
        /// <param name="People">The people to check on</param>
        /// <param name="StartTime">The start time</param>
        /// <param name="EndTime">The end time</param>
        /// <param name="Interval">The interval in minutes at which to check (the minimum is 10 minutes)</param>
        /// <param name="UserName">The username of the person checking</param>
        /// <param name="Password">The password of the person checking</param>
        /// <param name="TimeZoneDifference">Time zone difference</param>
        /// <param name="Server">Server name</param>
        /// <returns>returns an array of bytes stating whether someone is free or not, a 1 means they are not free during that time and a 0 means they are free</returns>
        public static byte[] GetFreeBusyData(StringDictionary People, DateTime StartTime, DateTime EndTime, int Interval, string UserName, string Password,string Server,string TimeZoneDifference)
        {
            if (People.Count < 1)
                return null;
            if (StartTime >= EndTime)
                return null;
            string EmailAddresses = "";
            foreach (string Person in People.Values)
            {
                EmailAddresses += "&u=SMTP:" + Person;
            }
            string Url = Server + "/public/?cmd=freebusy&start=" + StartTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + TimeZoneDifference + "&end=" + EndTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + TimeZoneDifference+ "&interval=" + Interval.ToString() + EmailAddresses;

            System.Net.HttpWebRequest Request;
            System.Net.WebResponse Response = null;
            System.Net.CredentialCache MyCredentialCache;

            string strUserName = UserName;
            string strPassword = Password;
            string strDomain = "";
            //string strQuery = "";

            byte[] bytes = null;
            //System.IO.Stream RequestStream = null;
            System.IO.Stream ResponseStream = null;
            XmlDocument ResponseXmlDoc = null;
            XmlNodeList DisplayNameNodes = null;

            try
            {
                MyCredentialCache = new System.Net.CredentialCache();
                if (!string.IsNullOrEmpty(strUserName) && !string.IsNullOrEmpty(strPassword))
                {
                    MyCredentialCache.Add(new System.Uri(Url),
                                        "NTLM",
                                        new System.Net.NetworkCredential(strUserName, strPassword, strDomain)
                                        );
                }
                else
                {
                    MyCredentialCache.Add(new System.Uri(Url),
                                           "Negotiate",
                                           (System.Net.NetworkCredential)CredentialCache.DefaultCredentials);
                }

                Request = (System.Net.HttpWebRequest)HttpWebRequest.Create(Url);

                Request.Credentials = MyCredentialCache;

                Request.Method = "GET";
                Request.ContentType = "text/xml; encoding='utf-8'";
                Request.UserAgent = "Mozilla/4.0(compatible;MSIE 6.0; " +
                    "Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727; InfoPath.1)";
                Request.KeepAlive = true;
                Request.AllowAutoRedirect = false;

                Response = (HttpWebResponse)Request.GetResponse();
                ResponseStream = Response.GetResponseStream();

                ResponseXmlDoc = new XmlDocument();
                ResponseXmlDoc.Load(ResponseStream);

                DisplayNameNodes = ResponseXmlDoc.GetElementsByTagName("a:item");

                if (DisplayNameNodes.Count > 0)
                {
                    for (int i = 0; i < DisplayNameNodes.Count; i++)
                    {
                        XmlNodeList Nodes = DisplayNameNodes[i].ChildNodes;
                        foreach (XmlNode Node in Nodes)
                        {
                            if (Node.Name == "a:fbdata")
                            {
                                bytes = new byte[Node.InnerText.Length];
                                for (int x = 0; x < Node.InnerText.Length; ++x)
                                {
                                    bytes[x] = byte.Parse(Node.InnerText[x].ToString());
                                }
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Could not get free/busy data from the exchange server");
                }
                // Clean up.
                ResponseStream.Close();
                Response.Close();
                ResponseStream.Dispose();
                return bytes;

            }
            catch (Exception ex)
            {
                if (ResponseStream != null)
                {
                    ResponseStream.Close();
                }
                if (Response != null)
                {
                    Response.Close();
                }
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Checks if a person/group of people are free for a specified time
        /// </summary>
        /// <param name="People">The people to check on</param>
        /// <param name="StartTime">The start time</param>
        /// <param name="EndTime">The end time</param>
        /// <param name="Interval">The interval in minutes at which to check (the minimum is 10 minutes)</param>
        /// <param name="UserName">The username of the person checking</param>
        /// <param name="Password">The password of the person checking</param>
        /// <param name="Server">Server name</param>
        /// <param name="TimeZoneDifference">Time zone difference in hh:mm format</param>
        /// <returns>return true if they are free, false otherwise</returns>
        public static bool ArePeopleFree(StringDictionary People, DateTime StartTime, DateTime EndTime, int Interval, string UserName, string Password,string Server,string TimeZoneDifference)
        {
            try
            {
                byte[] FreeBusy = GetFreeBusyData(People, StartTime, EndTime, Interval, UserName, Password, Server, TimeZoneDifference);
                if (FreeBusy == null)
                    return false;

                for (int x = 0; x < FreeBusy.Length; ++x)
                {
                    if (FreeBusy[x] > 0)
                    {
                        return false;
                    }
                }
            }
            catch (Exception a)
            {
                throw a;
            }
            return true;
        }

        /// <summary>
        /// Checks to see if someone has access to a specific calendar
        /// </summary>
        /// <param name="UserName">User name</param>
        /// <param name="Password">Password</param>
        /// <param name="Directory">Directory to check</param>
        /// <param name="Server">Server location</param>
        /// <returns>true if they can, false otherwise</returns>
        public static bool HasPermissionToCalendar(string UserName, string Password, string Directory,string Server)
        {
            System.Net.WebResponse Response = null;
            try
            {
                System.Net.HttpWebRequest Request = null;
                Request = (System.Net.HttpWebRequest)HttpWebRequest.Create(Server + "exchange/" + Directory + "/Calendar/");

                System.Net.CredentialCache MyCredentialCache = new System.Net.CredentialCache();
                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                {
                    MyCredentialCache.Add(new System.Uri(Server + "exchange/" + Directory + "/Calendar/"),
                                       "NTLM",
                                       new System.Net.NetworkCredential(UserName, Password)
                                      );
                }
                else
                {
                    MyCredentialCache.Add(new System.Uri(Server + "exchange/" + Directory + "/Calendar/"),
                                           "Negotiate",
                                           (System.Net.NetworkCredential)CredentialCache.DefaultCredentials);
                }

                Request.Credentials = MyCredentialCache;

                Request.Method = "GET";
                Request.ContentType = "text/xml; encoding='utf-8'";
                Request.UserAgent = "Mozilla/4.0(compatible;MSIE 6.0; " +
                    "Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727; InfoPath.1)";
                Request.KeepAlive = true;
                Request.AllowAutoRedirect = false;

                Response = (HttpWebResponse)Request.GetResponse();
                Response.Close();
                return true;
            }
            catch
            {
                if (Response != null)
                {
                    Response.Close();
                }
                return false;
            }
        }

        /// <summary>
        /// Returns a user's contacts
        /// </summary>
        /// <param name="UserName">User name</param>
        /// <param name="Password">Password</param>
        /// <param name="Directory">Directory</param>
        /// <param name="Server">Server name</param>
        /// <returns></returns>
        public static List<VCard> GetContacts(string UserName, string Password, string Directory,string Server)
        {
            try
            {
                List<VCard> ReturnArray = new List<VCard>();
                string uri = string.Format("http://{0}/exchange/{1}/contacts/", Server, Directory);

                byte[] contents = System.Text.Encoding.UTF8.GetBytes(string.Format(
                   @"<?xml version=""1.0""?>
                    <g:searchrequest xmlns:g=""DAV:"">
                        <g:sql>
                            SELECT
                                ""urn:schemas:contacts:givenName"", ""urn:schemas:contacts:sn"",
                                ""urn:schemas:contacts:email1"",""urn:schemas:contacts:businesshomepage"",
                                ""urn:schemas:contacts:title"",""urn:schemas:contacts:telephoneNumber"",
                                ""urn:schemas:contacts:o""
                            FROM
                                Scope('SHALLOW TRAVERSAL OF ""http://{0}/exchange/{1}/contacts/""')
                        </g:sql>
                    </g:searchrequest>",
                    Server, Directory));



                HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
                System.Net.CredentialCache MyCredentialCache = new System.Net.CredentialCache();
                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                {
                    MyCredentialCache.Add(new System.Uri(uri),
                                       "NTLM",
                                       new System.Net.NetworkCredential(UserName, Password)
                                      );
                }
                else
                {
                    MyCredentialCache.Add(new System.Uri(uri),
                                           "Negotiate",
                                           (System.Net.NetworkCredential)CredentialCache.DefaultCredentials);
                }
                request.Credentials = MyCredentialCache;
                request.Method = "SEARCH";
                request.ContentLength = contents.Length;
                request.ContentType = "text/xml";

                using (System.IO.Stream requestStream = request.GetRequestStream())
                    requestStream.Write(contents, 0, contents.Length);


                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (System.IO.Stream responseStream = response.GetResponseStream())
                    {
                        XmlDocument document = new XmlDocument();
                        document.Load(responseStream);
                        foreach (XmlElement PropStat in document.GetElementsByTagName("a:propstat"))
                        {
                            if (PropStat.GetElementsByTagName("a:status")[0].InnerText.Contains("200 OK"))
                            {
                                foreach (XmlElement element in PropStat.GetElementsByTagName("a:prop"))
                                {
                                    VCard TempCard = new VCard();
                                    if (element["d:sn"] != null)
                                    {
                                        TempCard.LastName = element["d:sn"].InnerText;
                                    }
                                    if (element["d:givenName"] != null)
                                    {
                                        TempCard.FirstName = element["d:givenName"].InnerText;
                                    }
                                    if (element["d:email1"] != null)
                                    {
                                        TempCard.Email = element["d:email1"].InnerText.Replace("\"", "");
                                    }
                                    if (element["d:businesshomepage"] != null)
                                    {
                                        TempCard.Url = element["d:businesshomepage"].InnerText;
                                    }
                                    if (element["d:title"] != null)
                                    {
                                        TempCard.Title = element["d:title"].InnerText;
                                    }
                                    if (element["d:telephoneNumber"] != null)
                                    {
                                        TempCard.DirectDial = element["d:telephoneNumber"].InnerText;
                                    }
                                    if (element["d:o"] != null)
                                    {
                                        TempCard.Organization = element["d:o"].InnerText;
                                    }
                                    ReturnArray.Add(TempCard);
                                }
                            }
                        }
                    }
                }
                return ReturnArray;
            }
            catch
            {
                return new List<VCard>();
            }
        }

        /// <summary>
        /// Returns a user's email messages from their inbox
        /// </summary>
        /// <param name="UserName">User name</param>
        /// <param name="Password">Password</param>
        /// <param name="Directory">Directory</param>
        /// <param name="Server">Server using</param>
        /// <returns>a list of messages</returns>
        public static List<Utilities.Web.Email.Message> GetEmail(string UserName, string Password, string Directory, string Server)
        {
            try
            {
                List<Utilities.Web.Email.Message> ReturnArray = new List<Utilities.Web.Email.Message>();
                string uri = string.Format("http://{0}/exchange/{1}/inbox", Server, Directory);

                byte[] contents = System.Text.Encoding.UTF8.GetBytes(string.Format(
                   @"<?xml version=""1.0""?>
                    <g:searchrequest xmlns:g=""DAV:"">
                        <g:sql>
                            SELECT
                                ""urn:schemas:httpmail:subject"", ""urn:schemas:httpmail:to"",
                                ""urn:schemas:httpmail:from"",""urn:schemas:httpmail:htmldescription""
                            FROM
                                Scope('DEEP TRAVERSAL OF ""http://{0}/exchange/{1}/inbox""')
                            WHERE
                                ""DAV:contentclass"" = 'urn:content-classes:message'
                        </g:sql>
                    </g:searchrequest>",
                    Server, Directory));



                HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
                System.Net.CredentialCache MyCredentialCache = new System.Net.CredentialCache();
                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                {
                    MyCredentialCache.Add(new System.Uri(uri),
                                       "NTLM",
                                       new System.Net.NetworkCredential(UserName, Password)
                                      );
                }
                else
                {
                    MyCredentialCache.Add(new System.Uri(uri),
                                           "Negotiate",
                                           (System.Net.NetworkCredential)CredentialCache.DefaultCredentials);
                }
                request.Credentials = MyCredentialCache;
                request.Method = "SEARCH";
                request.ContentLength = contents.Length;
                request.ContentType = "text/xml";

                using (System.IO.Stream requestStream = request.GetRequestStream())
                    requestStream.Write(contents, 0, contents.Length);


                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (System.IO.Stream responseStream = response.GetResponseStream())
                    {
                        XmlDocument document = new XmlDocument();
                        document.Load(responseStream);
                        foreach (XmlElement element in document.GetElementsByTagName("a:prop"))
                        {
                            Utilities.Web.Email.Message TempMessage = new Utilities.Web.Email.Message();
                            if (element["d:subject"] != null)
                            {
                                TempMessage.Subject = element["d:subject"].InnerText;
                            }
                            if (element["d:htmldescription"] != null)
                            {
                                TempMessage.Body = element["d:htmldescription"].InnerText;
                            }
                            if (element["d:to"] != null)
                            {
                                TempMessage.To = element["d:to"].InnerText;
                            }
                            if (element["d:from"] != null)
                            {
                                TempMessage.From = element["d:from"].InnerText;
                            }
                            ReturnArray.Add(TempMessage);
                        }
                    }
                }
                return ReturnArray;
            }
            catch
            {
                return new List<Utilities.Web.Email.Message>();
            }
        }

        /// <summary>
        /// Returns a list of the user's calendar items.
        /// </summary>
        /// <param name="UserName">User name used for logging in</param>
        /// <param name="Password">Password for logging in</param>
        /// <param name="Directory">User's directory</param>
        /// <param name="StartDate">Date to start at</param>
        /// <param name="EndDate">Date to end at</param>
        /// <param name="Server">Server Name</param>
        /// <returns>A list of the user's calendar items in VCalendar format, between two date ranges</returns>
        public static List<VCalendar> GetCalendarItems(string UserName, string Password, string Server,string Directory,DateTime StartDate,DateTime EndDate)
        {
            try
            {
                List<VCalendar> ReturnArray = new List<VCalendar>();
                string uri = string.Format("http://{0}/exchange/{1}/calendar/", Server, Directory);

                string Query = "<?xml version=\"1.0\"?>"
                     + "<g:searchrequest xmlns:g=\"DAV:\">"
                     + "<g:sql>SELECT \"urn:schemas:calendar:location\", \"urn:schemas:httpmail:subject\", "
                     + "\"urn:schemas:httpmail:textdescription\", "
                     + "\"urn:schemas:calendar:dtstart\", \"urn:schemas:calendar:dtend\", "
                     + "\"urn:schemas:calendar:busystatus\", \"urn:schemas:calendar:instancetype\" "
                     + "FROM Scope('SHALLOW TRAVERSAL OF \"" + uri + "\"') "
                     + "WHERE ((\"urn:schemas:calendar:dtstart\" &gt;= '" + StartDate.ToString("yyyy/MM/dd hh:mm:ss") + "' "
                     + "AND \"urn:schemas:calendar:dtstart\" &lt;= '" + EndDate.ToString("yyyy/MM/dd hh:mm:ss") + "') "
                     + "OR (\"urn:schemas:calendar:dtend\" &gt;= '" + StartDate.ToString("yyyy/MM/dd hh:mm:ss") + "' "
                     + "AND \"urn:schemas:calendar:dtstart\" &lt;= '" + EndDate.ToString("yyyy/MM/dd hh:mm:ss") + "')) "
                     + "AND \"DAV:contentclass\" = 'urn:content-classes:appointment' "
                     + "AND NOT \"urn:schemas:calendar:instancetype\" = 1 "
                     + "ORDER BY \"urn:schemas:calendar:dtstart\" ASC"
                     + "</g:sql></g:searchrequest>";
                byte[] contents = System.Text.Encoding.UTF8.GetBytes(Query);



                HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
                System.Net.CredentialCache MyCredentialCache = new System.Net.CredentialCache();
                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                {
                    MyCredentialCache.Add(new System.Uri(uri),
                                       "NTLM",
                                       new System.Net.NetworkCredential(UserName, Password)
                                      );
                }
                else
                {
                    MyCredentialCache.Add(new System.Uri(uri),
                                           "Negotiate",
                                           (System.Net.NetworkCredential)CredentialCache.DefaultCredentials);
                }
                request.Credentials = MyCredentialCache;
                request.Method = "SEARCH";
                request.ContentLength = contents.Length;
                request.ContentType = "text/xml";

                using (System.IO.Stream requestStream = request.GetRequestStream())
                    requestStream.Write(contents, 0, contents.Length);

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (System.IO.Stream responseStream = response.GetResponseStream())
                    {
                        XmlDocument document = new XmlDocument();
                        document.Load(responseStream);
                        foreach (XmlElement element in document.GetElementsByTagName("a:prop"))
                        {
                            VCalendar Cal = new VCalendar();
                            if (element["e:textdescription"] != null)
                            {
                                Cal.Description = element["e:textdescription"].InnerText;
                            }
                            if (element["e:subject"] != null)
                            {
                                Cal.Subject = element["e:subject"].InnerText;
                            }
                            if (element["d:location"] != null)
                            {
                                Cal.Location = element["d:location"].InnerText;
                            }
                            if (element["d:dtstart"] != null)
                            {
                                Cal.StartTime = DateTime.Parse(element["d:dtstart"].InnerText);
                            }
                            if (element["d:dtend"] != null)
                            {
                                Cal.EndTime = DateTime.Parse(element["d:dtend"].InnerText);
                            }
                            ReturnArray.Add(Cal);
                        }
                    }
                }
                return ReturnArray;
            }
            catch
            {
                return new List<VCalendar>();
            }
        }

        /// <summary>
        /// Gets the GAL for the system
        /// </summary>
        /// <param name="UserName">User name using</param>
        /// <param name="Password">Password using</param>
        /// <param name="Server">Server Name</param>
        /// <returns>a StringDictionary containing the name
        /// and email address of everyone in the GAL</returns>
        public static StringDictionary GetGAL(string UserName, string Password,string Server)
        {
            try
            {
                StringDictionary ReturnArray = new StringDictionary();
                LDAP.Directory TempDirectory = new Utilities.LDAP.Directory("", UserName, Password, Server);
                List<LDAP.Entry> Entries = TempDirectory.FindActiveUsersAndGroups("mail=*");
                TempDirectory.Close();
                TempDirectory.Dispose();
                foreach (LDAP.Entry Entry in Entries)
                {
                    ReturnArray.Add(Entry.Name, Entry.Email);
                }
                return ReturnArray;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Checks the users' calendars and picks the next available times for all of the users
        /// </summary>
        /// <param name="People">People to check</param>
        /// <param name="StartTime">Original start time</param>
        /// <param name="EndTime">Originial end time</param>
        /// <param name="UserName">User name</param>
        /// <param name="Password">Password</param>
        /// <param name="Server">Server name</param>
        /// <param name="TimeZoneDifference">Time zone difference</param>
        /// <returns>If a time is found, it returns the new start time. If it can't find an available time, it will return a date of 1/1/1900.</returns>
        public static DateTime PickNextAvailableTime(StringDictionary People, DateTime StartTime, DateTime EndTime, string UserName, string Password,string Server,string TimeZoneDifference)
        {
            try
            {
                StartTime = StartTime.AddMinutes(15.0d);
                EndTime = EndTime.AddMinutes(15.0d);
                DateTime FinalEndTime = StartTime.AddMonths(3);
                DateTime TrueStartTime = StartTime;
                byte[] Data = GetFreeBusyData(People, StartTime, FinalEndTime, 15, UserName, Password, Server, TimeZoneDifference);
                if (Data.Length == 0)
                    return new DateTime(1900, 1, 1);
                int FinalCount = 0;
                while (StartTime < EndTime)
                {
                    StartTime = StartTime.AddMinutes(15.0d);
                    ++FinalCount;
                }
                StartTime = TrueStartTime;
                int Counter = 0;
                DateTime CurrentTime = StartTime;
                DateTime CurrentStartTime = StartTime;
                for (int y = 0; y < Data.Length; ++y)
                {
                    CurrentTime = CurrentTime.AddMinutes(15.0d);
                    if (Data[y] != 0)
                    {
                        Counter = 0;
                        CurrentStartTime = CurrentTime;
                    }
                    else if (CurrentTime.Hour < 8 || CurrentTime.Hour >= 19 || CurrentTime.DayOfWeek == DayOfWeek.Saturday || CurrentTime.DayOfWeek == DayOfWeek.Sunday)
                    {
                        Counter = 0;
                        CurrentStartTime = CurrentTime;
                    }
                    else
                    {
                        ++Counter;
                        if (Counter >= FinalCount)
                            return CurrentStartTime;
                    }
                }
                return new DateTime(1900, 1, 1);
            }
            catch
            {
                return new DateTime(1900, 1, 1);
            }
        }

        /// <summary>
        /// Picks the next available time in the past based on a set of people's calendars
        /// </summary>
        /// <param name="People">People to check on</param>
        /// <param name="StartTime">Start time</param>
        /// <param name="EndTime">End time</param>
        /// <param name="UserName">User name</param>
        /// <param name="Password">Password</param>
        /// <param name="Server">Server name</param>
        /// <param name="TimeZoneDifference">Time zone difference</param>
        /// <returns>If a time is found, it returns the new start time. If it can't find an available time, it will return a date of 1/1/1900.</returns>
        public static DateTime PickPreviousAvailableTime(StringDictionary People, DateTime StartTime, DateTime EndTime, string UserName, string Password,string Server, string TimeZoneDifference)
        {
            try
            {
                StartTime = StartTime.AddMinutes(-15.0d);
                EndTime = EndTime.AddMinutes(-15.0d);
                DateTime FinalEndTime = EndTime;
                DateTime FinalStartTime = StartTime.AddMonths(-3);
                DateTime TrueStartTime = StartTime;
                byte[] Data = GetFreeBusyData(People, FinalStartTime, FinalEndTime, 15, UserName, Password, Server, TimeZoneDifference);
                if (Data.Length == 0)
                    return new DateTime(1900, 1, 1);
                int FinalCount = 0;
                while (StartTime < EndTime)
                {
                    StartTime = StartTime.AddMinutes(15.0d);
                    ++FinalCount;
                }
                StartTime = TrueStartTime;
                int Counter = 0;
                DateTime CurrentTime = EndTime;
                DateTime CurrentEndTime = EndTime;
                for (int y = Data.Length - 1; y >= 0; --y)
                {
                    CurrentTime = CurrentTime.AddMinutes(-15.0d);
                    if (Data[y] != 0)
                    {
                        Counter = 0;
                        CurrentEndTime = CurrentTime;
                    }
                    else if (CurrentTime.Hour < 8 || CurrentTime.Hour >= 19 || CurrentTime.DayOfWeek == DayOfWeek.Saturday || CurrentTime.DayOfWeek == DayOfWeek.Sunday)
                    {
                        Counter = 0;
                        CurrentEndTime = CurrentTime;
                    }
                    else
                    {
                        ++Counter;
                        if (Counter >= FinalCount)
                        {
                            for (int x = 0; x < FinalCount; ++x)
                            {
                                CurrentEndTime = CurrentEndTime.AddMinutes(-15.0d);
                            }
                            return CurrentEndTime;
                        }
                    }
                }
                return new DateTime(1900, 1, 1);
            }
            catch
            {
                return new DateTime(1900, 1, 1);
            }
        }
        #endregion
    }
}