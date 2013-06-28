/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

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


using Utilities.Web.Google.Enums;
using Utilities.Web.Google.HelperClasses;
using Xunit;

namespace UnitTests.Web.Google.StaticMaps
{
    public class Map
    {
        [Fact]
        public void Create()
        {
            Utilities.Web.Google.StaticMaps.Map TestMap = new Utilities.Web.Google.StaticMaps.Map();
            TestMap.Center = new Address(){PhysicalAddress="123 Somewhere St. Detroit, MI"};
            TestMap.Format = ImageFormat.JPG;
            TestMap.Key = "12345678";
            TestMap.MapType = MapType.Hybrid;
            Markers MarkerSet=new Markers();
            MarkerSet.Color="0x00FF00";
            MarkerSet.Size=MarkerSize.Tiny;
            MarkerSet.MarkerList.Add(new LongLat(){Longitude=1.03123,Latitude=2.03123});
            MarkerSet.MarkerList.Add(new Address(){PhysicalAddress="123 Somewhere St. Detroit, MI"});
            TestMap.Markers.Add(MarkerSet);
            TestMap.Sensor = false;
            TestMap.UseHTTPS = true;
            Assert.Equal("https://maps.googleapis.com/maps/api/staticmap?sensor=false&center=123+Somewhere+St.+Detroit%2c+MI&zoom=12&size=100x100&scale=1&format=jpg&maptype=hybrid&markers=size%3atiny%7ccolor%3a0x00FF00%7c2.03123%2c1.03123%7c123+Somewhere+St.+Detroit%2c+MI&key=12345678", TestMap.ToString());
        }
    }
}
