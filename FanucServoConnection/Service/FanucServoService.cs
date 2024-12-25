using FanucServoConnection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanucServoConnection.Service
{
    public class FanucServoService:BaseService<FanucServo300>
    {
        public override List<FanucServo300> Insert(List<FanucServo300> entities)
        {
            DateTime now=DateTime.Now;
            foreach (var entity in entities) { 
                entity.CreateDate = now;
            }
            return base.Insert(entities);
        }
    }
}
