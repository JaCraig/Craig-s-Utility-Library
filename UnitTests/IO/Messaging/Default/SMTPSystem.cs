using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.IO.Messaging.Default
{
    public class SMTPSystem
    {
        [Fact]
        public void Creation()
        {
            Utilities.IO.Messaging.Default.SMTPSystem Temp = new Utilities.IO.Messaging.Default.SMTPSystem();
            Assert.NotNull(Temp);
            Assert.Equal(0, Temp.Formatters.Count());
            Assert.Equal(typeof(Utilities.IO.EmailMessage), Temp.MessageType);
            Assert.Equal("SMTP", Temp.Name);
        }

        [Fact]
        public void NullSend()
        {
            Utilities.IO.Messaging.Default.SMTPSystem Temp = new Utilities.IO.Messaging.Default.SMTPSystem();
            Assert.NotNull(Temp);
            Assert.DoesNotThrow(() => { Temp.Send(null, new Temp()).Wait(); });
        }


        [Serializable]
        [DataContract]
        protected class Temp
        {
            [DataMember(Name = "A", Order = 1)]
            public int A { get; set; }
        }
    }
}
