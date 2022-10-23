using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Helpers.Custom.Api
{
    public class EnRegisterEdit
    {

        public int id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string photo { get; set; }
        public string email { get; set; }
        public string city { get; set; }

        public int? age { get; set; }
        public EnDDL gender { get; set; }

        public List<EnDDL> lookingRelations { get; set; }



        public EnDDL lookingGender { get; set; }


        public string eduction { get; set; }
        public string profession { get; set; }


        public EnDDL annualIncome { get; set; }


        public string aboutMe { get; set; }
        public string last_relationship { get; set; }


        public EnDDL alcoholConsumption { get; set; }

        public EnDDL smoke { get; set; }


        public EnDDL fetiches { get; set; }


        public EnDDL sexualOrientation { get; set; }

        public EnDDL relationshipStatus { get; set; }


        public EnDDL vaccine { get; set; }

        public EnDDL children { get; set; }



        public List<EnDDL> personalities { get; set; }
        public List<EnDDL> qualities { get; set; }


        public EnDDL myProfession { get; set; }


        public string height { get; set; }


        public EnDDL physicalType { get; set; }

        public EnDDL religion { get; set; }



        public List<EnDDL> hobbies { get; set; }


        public EnDDL sign { get; set; }

        public string zipcode { get; set; }
        public bool hideage { get; set; }
        public EnDDL gelocationBydistance { get; set; }
        public EnDDL whereamiknow { get; set; }
        public int likes { get; internal set; }
        public double rating { get; internal set; }
    }
    public class EnRegister
    {

        public int id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string photo { get; set; }
        public string email { get; set; }
        public string city { get; set; }

        public int? age { get; set; }
        public int? genderId { get; set; }

        public List<int> lookingRelations { get; set; }



        public int? lookingGenderId { get; set; }


        public string eduction { get; set; }
        public string profession { get; set; }


        public int? annualIncomeId { get; set; }


        public string aboutMe { get; set; }
        public string last_relationship { get; set; }


        public int? alcoholConsumptionId { get; set; }

        public int? smokeId { get; set; }


        public int? fetichesId { get; set; }


        public int? sexualOrientationId { get; set; }

        public int? relationshipStatusId { get; set; }


        public int? vaccineId { get; set; }

        public int? childrenId { get; set; }



        public List<int> personalities { get; set; }
        public List<int> qualities { get; set; }


        public int? myProfessionId { get; set; }


        public string height { get; set; }


        public int? physicalTypeId { get; set; }

        public int? religionId { get; set; }



        public List<int> hobbies { get; set; }


        public int? signId { get; set; }

        public string zipcode { get; set; }
        public bool hideage { get; set; }
        public int? gelocationBydistance { get; set; }
        public int? whereamiknow { get; set; }
    }

    public class EnProfile
    {

        public int id { get; set; }
        public string username { get; set; }
        public string name { get; set; }

        public string photo { get; set; }
        public string email { get; set; }

        public int? age { get; set; }
        public string gender { get; set; }

        public List<string> lookingRelations { get; set; }



        public string lookingGender { get; set; }


        public string eduction { get; set; }
        public string profession { get; set; }


        public string annualIncome { get; set; }


        public string aboutMe { get; set; }
        public string last_relationship { get; set; }


        public string alcoholConsumption { get; set; }

        public string smoke { get; set; }


        public string fetiches { get; set; }


        public string sexualOrientation { get; set; }

        public string relationshipStatus { get; set; }


        public string vaccine { get; set; }

        public string children { get; set; }



        public List<string> personalities { get; set; }
        public List<string> qualities { get; set; }


        public string myProfession { get; set; }


        public string height { get; set; }


        public string physicalType { get; set; }

        public string religion { get; set; }



        public List<string> hobbies { get; set; }


        public string sign { get; set; }

        public string zipcode { get; set; }
        public bool hideage { get; set; }
        public string gelocationBydistance { get; set; }
        public string whereamiknow { get; set; }
        public List<EnAlbum> albums { get; set; }
        public List<EnRatingList> ratings { get;  set; }
        public int likes { get; internal set; }
        public double rating { get; internal set; }
        public string looking_for { get;  set; }
        public string city { get; set; }
        public string lat { get; internal set; }
        public string lng { get; internal set; }
        public double? distance { get;  set; }
        public bool ismatch { get; internal set; }
    }
}
