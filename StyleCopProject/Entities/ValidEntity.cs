using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace StyleCopProject.Entities
{
    [DataContract]
    public class ValidEntity
    {
        public int Id { get; set; }

        public string Name{ get; set; }
    }
}