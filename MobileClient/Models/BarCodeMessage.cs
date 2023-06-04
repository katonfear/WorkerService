using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MobileClient.Models
{
    internal class BarCodeMessage : ValueChangedMessage<string>
    {
        public BarCodeMessage(string value) : base(value) 
        { 
        }
    }
}
