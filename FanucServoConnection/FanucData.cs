using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanucServoConnection
{
    public class FanucData<T>
    {
        public short Ret {  get; set; }

        public T Data {  get; set; }

        public string Message {  get; set; }
    }
}
