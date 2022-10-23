using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Helpers.Custom.Api
{
    public class EnDDL
    {
        public int id { get; set; }
        public string name { get; set; }
        public int type { get; set; }
    }
    public class EnRegisterDDL
    {
     public List<EnDDL>   Gender {get;set;}
      public List<EnDDL> LookingRelation { get; set; }
        public List<EnDDL> LookingGender { get; set; }
        public List<EnDDL> AnnualIncome { get; set; }
        public List<EnDDL> Alcohol { get; set; }
        public List<EnDDL> Smokes { get; set; }
        public List<EnDDL> SexualOrientation { get; set; }
        public List<EnDDL> RelationshipStatus { get; set; }
        public List<EnDDL> Vacine { get; set; }
        public List<EnDDL> Children { get; set; }
        public List<EnDDL> Personlity { get; set; }
        public List<EnDDL> Qualities { get; set; }
        public List<EnDDL> Myproffession { get; set; }
        public List<EnDDL> PhysicalType { get; set; }
        public List<EnDDL> Religon { get; set; }
        public List<EnDDL> Hobbies { get; set; }
        public List<EnDDL> Sign {get;set;}
        public List<EnDDL> Fetches { get;  set; }
    }
}
